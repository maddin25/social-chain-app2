using System;

using PartyTimeline.Services;

using Xamarin.Forms;

namespace PartyTimeline
{
	public class AddEventPageViewModel : Event
	{
		private bool saveSuccessful = false;
		private readonly string AlertInvalidField = "Invalid field";
		Object _lockObject = new Object();

		public AddEventPageViewModel()
		{
			AddEventCommand = new Command(() =>
			{
				lock(_lockObject)
				{
					if (!saveSuccessful)
					{
						if (AllFieldsValid())
						{
							Id = new Random().Next();
							SetDate(DateTime.Now);
							EventService.INSTANCE.AddNewEvent(new Event(this));
							saveSuccessful = true;
							Application.Current.MainPage.Navigation.PopAsync(true);
						}
					}
				}
			});
		}

		public Command AddEventCommand { get; set; }

		private bool AllFieldsValid()
		{
			if (string.IsNullOrWhiteSpace(Name))
			{
				Application.Current.MainPage.DisplayAlert(AlertInvalidField, "The name field is empty", "Ok");
				return false;
			}
			return true;
		}
	}
}
