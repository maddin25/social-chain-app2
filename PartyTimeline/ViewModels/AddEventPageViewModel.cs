using System;

using PartyTimeline.Services;

using Xamarin.Forms;

namespace PartyTimeline
{
	public class AddEventPageViewModel : Event
	{

		public AddEventPageViewModel()
		{
			AddEventCommand = new Command(() =>
			{
				if (AllFieldsValid())
				{
					DateCreated = DateTime.Now;
					EventService.INSTANCE.AddNewEvent(this);
					Application.Current.MainPage.Navigation.PopAsync(true);
				}
			});
		}

		public Command AddEventCommand { get; set; }

		private bool AllFieldsValid()
		{
			return true;
		}
	}
}
