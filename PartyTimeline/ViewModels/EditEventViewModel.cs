using System;
using System.ComponentModel;
using SQLite;

using Xamarin.Forms;

namespace PartyTimeline.ViewModels
{
	public class EditEventViewModel : Event, INotifyPropertyChanged
	{
		private bool saveSuccessful = false;
		private readonly string AlertInvalidField = "Invalid field";
		Object _lockObject = new Object();

		private DateTime _eventStartDate;
		private TimeSpan _eventStartTime;
		private DateTime _eventEndDate;
		private TimeSpan _eventEndTime;

		[Ignore]
		public DateTime UiEventStartDate
		{
			get { return _eventStartDate; }
			set
			{
				_eventStartDate = value;
				OnPropertyChanged(nameof(UiEventStartDate));
			}
		}
		[Ignore]
		public TimeSpan UiEventStartTime
		{
			get { return _eventStartTime; }
			set
			{
				_eventStartTime = value;
				OnPropertyChanged(nameof(UiEventStartTime));
			}
		}
		[Ignore]
		public DateTime UiEventEndDate
		{
			get { return _eventEndDate; }
			set
			{
				_eventEndDate = value;
				OnPropertyChanged(nameof(UiEventEndDate));
			}
		}
		[Ignore]
		public TimeSpan UiEventEndTime
		{
			get { return _eventEndTime; }
			set
			{
				_eventEndTime = value;
				OnPropertyChanged(nameof(UiEventEndTime));
			}
		}

		[Ignore]
		public Command AddEventCommand { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public EditEventViewModel()
		{
			SetStartDateTime();
			SetEndDateTime();

			AddEventCommand = new Command(() =>
			{
				lock (_lockObject)
				{
					if (!saveSuccessful)
					{
						if (AllFieldsValid())
						{
							Id = new Random().Next();
							SetDateCreated(DateTime.Now);
							EventService.INSTANCE.AddNewEvent(this);
							saveSuccessful = true;
							Application.Current.MainPage.Navigation.PopAsync(true);
						}
					}
				}
			});
		}

		private bool AllFieldsValid()
		{
			StartDateTime = UiEventStartDate.Date.Add(UiEventStartTime);
			EndDateTime = UiEventEndDate.Date.Add(UiEventEndTime);
			if (string.IsNullOrWhiteSpace(Name))
			{
				Application.Current.MainPage.DisplayAlert(AlertInvalidField, "The name field is empty", "Ok");
				return false;
			}
			else if (StartDateTime > EndDateTime)
			{
				Application.Current.MainPage.DisplayAlert(AlertInvalidField, "The event can not end before it started", "Ok");
				SetEndDateTime();
				return false;
			}
			return true;
		}

		private void SetStartDateTime()
		{
			UiEventStartDate = DateTime.Now;
			UiEventStartTime = UiEventStartDate.TimeOfDay;
		}

		private void SetEndDateTime()
		{
			UiEventEndDate = UiEventStartDate.AddHours(2);
			UiEventEndTime = UiEventEndDate.TimeOfDay;
		}
	}
}
