using System.Collections.Generic;

using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class AddEventPage : ContentPage
	{
		public AddEventPage()
		{
			InitializeComponent();
			BindingContext = new AddEventPageViewModel();
		}
	}
}
