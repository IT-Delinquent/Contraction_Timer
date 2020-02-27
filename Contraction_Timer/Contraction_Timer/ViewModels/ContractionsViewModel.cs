using Contraction_Timer.Helpers;
using Contraction_Timer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Contraction_Timer.ViewModels
{
    /// <summary>
    /// Class for displaying the contractions in a list
    /// </summary>
    public class ContractionsViewModel : BaseViewModel
    {
        #region Private backing fields

        /// <summary>
        /// Holds the list of contractions
        /// </summary>
        private ObservableCollection<Contraction> _contractions = new ObservableCollection<Contraction>();

        /// <summary>
        /// Holds the duration of a contraction
        /// </summary>
        private string _duration;

        /// <summary>
        /// Holds whether the page is refreshing
        /// </summary>
        private bool _isRefreshing;

        #endregion Private backing fields

        #region Public properties

        /// <summary>
        /// Accessor and modifier for the contractions
        /// </summary>
        public ObservableCollection<Contraction> Contractions
        {
            get { return _contractions; }
            set
            {
                _contractions = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Accessor and modifier for the duration of a contraction
        /// </summary>
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
            }
        }

        /// <summary>
        /// Accessor and modifier for if the page is refreshing
        /// </summary>
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command to delete a contraction
        /// </summary>
        public ICommand DeleteCommand { get; }

        /// <summary>
        /// Command to load the contractions
        /// </summary>
        public ICommand LoadContractionsCommand { get; set; }

        /// <summary>
        /// Command to delete all the contractions
        /// </summary>
        public ICommand DeleteAllCommand { get; set; }

        #endregion Public properties

        #region Private methods

        /// <summary>
        /// Deletes a contraction file
        /// </summary>
        /// <param name="contraction">The contraction object to delete</param>
        /// <returns>A task to delete and remove the contraction object</returns>
        private async Task DeleteNoteAsync(Contraction contraction)
        {
            //Confirm the user wants to delete the note
            bool prompResult = await Application
                .Current
                .MainPage
                .DisplayAlert("Are you sure?", "Do you want to delete this contraction entry", "Delete", "Cancel");

            if (prompResult != true)
            {
                return;
            }

            if (!IOHelpers.FileExists(contraction.Filename))
            {
                await Application
                    .Current
                    .MainPage
                    .DisplayAlert("Error",
                    "Couldn't find the note... Please report this bug",
                    "OK");
                return;
            }

            try
            {
                IOHelpers.DeleteFile(contraction.Filename);
            }
            catch
            {
                await Application
                    .Current
                    .MainPage
                    .DisplayAlert("Error", "Failed to delete the contraction, please report this", "OK");
            }

            Contractions.Remove(contraction);
        }

        /// <summary>
        /// Loads the contractions from disk
        /// </summary>
        private void LoadContractions()
        {
            IsRefreshing = true;
            List<string> files = IOHelpers.EnumeratAllFiles();

            Contractions?.Clear();

            foreach (var file in files)
            {
                string fileData = IOHelpers.ReadAllFileText(file);

                string[] fileParts = fileData.Split('^');

                string startTime = fileParts[0];
                string endTime = fileParts[1];
                string painLevel = fileParts[2];
                string duration = fileParts[3];

                TimeSpan ts = new TimeSpan();
                ts = DateTime.Parse(endTime) - DateTime.Parse(startTime);
                string durationString = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);

                Contraction _tempContraction = new Contraction
                {
                    Filename = file,
                    StartTime = DateTime.Parse(startTime),
                    EndTime = DateTime.Parse(endTime),
                    PainLevel = painLevel,
                    Duration = durationString
                };

                Contractions.Add(_tempContraction);
            }

            Contractions = new ObservableCollection<Contraction>(Contractions.OrderByDescending(x => x.StartTime));
            IsRefreshing = false;
        }

        /// <summary>
        /// Deletes all the contractions and clears the list
        /// </summary>
        /// <returns>A task to delete all the contractions and clears the list</returns>
        private async Task DeleteAllContractions()
        {
            await Application
                .Current
                .MainPage
                .DisplayAlert("Are you sure?",
                "This will permanently delete ALL your contractions", "OK");

            string promptResult = await Application
                .Current
                .MainPage
                .DisplayPromptAsync("Confirmation", "Please enter 'DELETE ALL MY CONTRACTIONS' to continue");

            if (promptResult == null)
            {
                return;
            }

            if (promptResult != "DELETE ALL MY CONTRACTIONS")
            {
                await Application
                    .Current
                    .MainPage
                    .DisplayAlert("Incorrect", "Yours contractions have NOT been deleted", "OK");
                return;
            }

            IOHelpers.DeleteAllNotes();
            Contractions.Clear();

            await Application
                .Current
                .MainPage
                .DisplayAlert("Deleted", "All your contractions have now been deleted", "OK");
        }

        #endregion Priavte methods

        /// <summary>
        /// The constructor for the class
        /// </summary>
        public ContractionsViewModel()
        {
            LoadContractionsCommand = new Command(() => LoadContractions());
            LoadContractionsCommand.Execute(null);

            DeleteCommand = new Command<Contraction>(async (x) => await DeleteNoteAsync(x));
            DeleteAllCommand = new Command(async () => await DeleteAllContractions());
        }
    }
}