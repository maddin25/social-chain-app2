using System.Collections.Generic;

using Xamarin.Forms;

using PartyTimeline.ViewModels;

namespace PartyTimeline
{
	public partial class AddEventPage : ContentPage
	{
		public AddEventPage()
		{
			InitializeComponent();
			BindingContext = new EditEventViewModel();
		}
	}
}
