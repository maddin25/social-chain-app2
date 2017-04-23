using System;

using PartyTimeline.Services;

using Xamarin.Forms;

namespace PartyTimeline
{
	public class AddEventPageViewModel : Event
	{
		private readonly string AlertInvalidField = "Invalid field";

		public AddEventPageViewModel()
		{
			AddEventCommand = new Command(() =>
			{
				if (AllFieldsValid())
				{
					Id = new Random().Next();
					SetDateCreated(DateTime.Now);
					EventService.INSTANCE.AddNewEvent(this);
					Application.Current.MainPage.Navigation.PopAsync(true);
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
