using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

/* Sadly, I had to put all these functions in one class, because C# does not support multiple inheritance. Using
* interface was not an option either, because each functionality needs to define properties
*/
namespace PartyTimeline
{
	/// <summary>
	/// UIB inding helper. Provides three functionalities
	/// - Refreshing implementation with abstract function to define custom refreshing rountine
	/// - OnPropertyChanged notifier methods
	/// - SelectedItem property with a abstract function to define custom behavior on select
	/// </summary>
	public abstract class UIBindingHelper<T> : INotifyPropertyChanged
	{
		private T _selectedItem;
		protected ListView RefreshableListView;
		public event PropertyChangedEventHandler PropertyChanged;

		public T SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				if (_selectedItem != null)
				{
					OnSelect(ref _selectedItem);
					SelectedItem = default(T);
					OnPropertyChanged(nameof(SelectedItem));
				}
			}
		}

		public Command RefreshListCommand { get; set; }

		public UIBindingHelper(ListView refreshableListView)
		{
			RefreshableListView = refreshableListView;
			RefreshListCommand = new Command(async () =>
			{
				Debug.WriteLine("Refreshing list");
				//refreshableListView.IsRefreshing = true;
				await OnRefreshTriggered();
				//refreshableListView.IsRefreshing = false;
				Debug.WriteLine("Finished refreshing list");
			});
		}

		public virtual void OnAppearing()
		{
			
		}
		
		public virtual void OnDisappearing()
		{
			
		}
		
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected abstract void OnSelect(ref T element);

		protected abstract Task OnRefreshTriggered();
	}
}
