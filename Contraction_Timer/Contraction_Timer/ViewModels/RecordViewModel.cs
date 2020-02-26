﻿using Contraction_Timer.Helpers;
using Contraction_Timer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contraction_Timer.ViewModels
{
    /// <summary>
    /// Class for recording new contractions
    /// </summary>
    public class RecordViewModel : BaseViewModel
    {
        #region Private backing fields

        /// <summary>
        /// Holds the private stopwatch
        /// </summary>
        private readonly Stopwatch stopWatch;

        /// <summary>
        /// Holds whether the timer is running
        /// </summary>
        private bool _isRunning;

        /// <summary>
        /// Holds the private contraction
        /// </summary>
        private Contraction _contraction;

        /// <summary>
        /// Holds the start time for the current contraction
        /// </summary>
        private DateTime? _startTime;

        /// <summary>
        /// Holds the duration of the contraction
        /// </summary>
        private string _duration;

        /// <summary>
        /// Holds the private list of possible pain levels
        /// </summary>
        private List<int> _painLevels;

        #endregion Private backing fields

        #region Public properties

        /// <summary>
        /// Command for starting a new contaction timer
        /// </summary>
        public ICommand StartCommand { get; }

        /// <summary>
        /// Command for stopping a contraction
        /// </summary>
        public ICommand StopCommand { get; }

        /// <summary>
        /// Accessor and modifier for whether the timer is running
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged();
                ((Command)StartCommand).ChangeCanExecute();
                ((Command)StopCommand).ChangeCanExecute();
            }
        }

        /// <summary>
        /// Accessor and modifier for the contraction
        /// </summary>
        public Contraction Contraction
        {
            get { return _contraction; }
            set
            {
                _contraction = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Accessor and modifier for the start time of the contraction
        /// </summary>
        public DateTime? StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = Contraction.StartTime;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Accessor and modifier for the contractions duration
        /// </summary>
        public string Duration
        {
            get
            {
                if (string.IsNullOrEmpty(_duration))
                {
                    _duration = "00:00";
                }
                return _duration;
            }
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Accessor and modifier for the possible pain levels
        /// </summary>
        public List<int> PainLevels
        {
            get { return _painLevels; }
            set
            {
                _painLevels = value;
                OnPropertyChanged();
            }
        }

        #endregion Public properties

        #region Private methods

        /// <summary>
        /// Starts the timer to tick once per second
        /// </summary>
        private void SecondTimerAsync()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                TimeSpan ts = stopWatch.Elapsed;

                if (ts.TotalSeconds >= 600)
                {
                    //ten minutes has passed, trigger the stop
                    IsRunning = false;
                    StopCommand.Execute(null);

                    Application
                    .Current
                    .MainPage
                    .DisplayAlert("Max Contraction Time",
                    "The maximum time for a contraction has been reach",
                    "OK");

                    return IsRunning;
                }

                Duration = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
                return IsRunning;
            });
        }

        /// <summary>
        /// Start a new contractions
        /// </summary>
        private void Start()
        {
            IsRunning = true;
            stopWatch.Start();

            SecondTimerAsync();

            if (Contraction == null)
            {
                Contraction = new Contraction
                {
                    StartTime = DateTime.Now
                };
                return;
            }
            else
            {
                Contraction.StartTime = DateTime.Now;
            }
            StartTime = Contraction.StartTime;
        }

        /// <summary>
        /// Update the pain level for the contraction
        /// </summary>
        /// <param name="level">A string of the contraction pain level</param>
        private void PainLevel(string level)
        {
            Contraction.PainLevel = int.Parse(level);
        }

        /// <summary>
        /// Stop the contraction, save the entry and reset the values
        /// </summary>
        /// <returns></returns>
        private async Task StopAsync()
        {
            IsRunning = false;

            //Build and save the current contraction
            Contraction.EndTime = DateTime.Now;
            TimeSpan? ts = Contraction.EndTime - Contraction.StartTime;
            Contraction.Duration = ts.ToString();
            Save();

            //Clear the contraction
            Contraction = new Contraction
            {
                StartTime = null
            };
            StartTime = Contraction.StartTime;
            stopWatch.Stop();
            stopWatch.Reset();
            PainLevel("1");
            Duration = null;

            await Application
                .Current
                .MainPage
                .DisplayAlert("Saved",
                "Your contraction has been saved",
                "OK");
        }

        /// <summary>
        /// Save the contraction data
        /// </summary>
        private void Save()
        {
            string fileData = string
                .Format("{0}^{1}^{2}^{3}",
                Contraction.StartTime,
                Contraction.EndTime,
                Contraction.PainLevel,
                Contraction.Duration);

            string fileName = IOHelpers.GetUniqueFileName();

            IOHelpers.SaveData(fileName, fileData);
        }

        #endregion Private methods

        /// <summary>
        /// The constructor for the class
        /// </summary>
        public RecordViewModel()
        {
            PainLevels = new List<int>
            {
                1,
                2,
                3
            };

            StartCommand = new Command(() => Start(), () => !IsRunning);
            StopCommand = new Command(async () => await StopAsync(), () => IsRunning);

            Contraction = new Contraction();
            StartTime = Contraction.StartTime;
            stopWatch = new Stopwatch();
        }
    }
}