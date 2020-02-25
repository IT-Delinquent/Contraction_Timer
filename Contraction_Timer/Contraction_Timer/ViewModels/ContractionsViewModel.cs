using Contraction_Timer.Helpers;
using Contraction_Timer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace Contraction_Timer.ViewModels
{
    public class ContractionsViewModel : BaseViewModel
    {
		private ObservableCollection<Contraction> _contractions = new ObservableCollection<Contraction>();

		public ObservableCollection<Contraction> Contractions
		{
			get { return _contractions; }
			set 
			{
				_contractions = value;
				OnPropertyChanged();
			}
		}

		private string _duration;

		public string Duration
		{
			get { return _duration; }
			set 
			{ 
				_duration = value; 

			}
		}


		private bool _isRefreshing;

		public bool IsRefreshing
		{
			get { return _isRefreshing; }
			set 
			{
				_isRefreshing = value;
				OnPropertyChanged();
			}
		}


		public ContractionsViewModel()
		{
			LoadContractionsCommand = new Command(() => LoadContractions());
			LoadContractionsCommand.Execute(null);

			DeleteCommand = new Command<string>(async (x) => await DeleteNoteAsync(x));
		}

		public ICommand DeleteCommand { get; }


		private async Task DeleteNoteAsync(string filename)
		{
			if (!IOHelpers.FileExists(filename))
			{
				await Application
					.Current
					.MainPage
					.DisplayAlert("Error",
					"Couldn't find the note... Please report this bug", 
					"OK");
				return;
			}
			IOHelpers.DeleteFile(filename);
		}

		public ICommand LoadContractionsCommand { get; }

		public void LoadContractions()
		{
			IsRefreshing = true;
			List<string> files = IOHelpers.EnumeratAllFiles();

			Contractions?.Clear();

			foreach(var file in files)
			{
				string fileData = IOHelpers.ReadAllFileText(file);

				string[] fileParts = fileData.Split('^');

				string startTime = fileParts[0];
				string endTime = fileParts[1];
				string painLevel = fileParts[2];

				Contraction _tempContraction = new Contraction
				{
					Filename = file,
					StartTime = DateTime.Parse(startTime),
					EndTime = DateTime.Parse(endTime),
					PainLevel = int.Parse(painLevel)
				};

				Contractions.Add(_tempContraction);
			}

			Contractions = new ObservableCollection<Contraction>(Contractions.OrderByDescending(x => x.StartTime));
			IsRefreshing = false;
		}

	}
}