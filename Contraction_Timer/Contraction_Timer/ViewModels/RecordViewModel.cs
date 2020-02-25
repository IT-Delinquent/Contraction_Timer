using Contraction_Timer.Helpers;
using Contraction_Timer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contraction_Timer.ViewModels
{
    public class RecordViewModel : BaseViewModel
    {

        private readonly Stopwatch stopWatch;

        public RecordViewModel()
        {
            PainLevels = new List<int>
            {
                1,
                2,
                3
            };

            StartCommand = new Command(()  => Start(), () => !IsRunning);
            StopCommand = new Command(async () => await StopAsync(), () => IsRunning);

            Contraction = new Contraction();
            StartTime = Contraction.StartTime;
            stopWatch = new Stopwatch();
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        private bool _isRunning;

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


        private Contraction _contraction;
        public Contraction Contraction
        {
            get { return _contraction; }
            set 
            { 
                _contraction = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startTime;

        public DateTime? StartTime
        {
            get { return _startTime; }
            set 
            { 
                _startTime = Contraction.StartTime;
                OnPropertyChanged();
            }
        }


        private string _duration;
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

        private List<int> _painLevels;
        public List<int> PainLevels
        {
            get { return _painLevels; }
            set 
            {
                _painLevels = value;
                OnPropertyChanged();
            }
        }


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

        private void PainLevel(string level)
        {
            Contraction.PainLevel = int.Parse(level);
        }


        private async Task StopAsync()
        {
            IsRunning = false;
            Contraction.EndTime = DateTime.Now;
            Save();
            //save and clear the contraction
            Contraction = new Contraction{
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

        private void Save()
        {
            string fileData = string
                .Format("{0}^{1}^{2}",
                Contraction.StartTime,
                Contraction.EndTime,
                Contraction.PainLevel);

            string fileName = IOHelpers.GetUniqueFileName();

            IOHelpers.SaveData(fileName, fileData);
        }



    }
}