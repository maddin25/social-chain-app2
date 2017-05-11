using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace PartyTimeline
{
	public abstract class EntryBehavior : Behavior<Entry>
	{
		public Entry AssociatedObject;

		public ICommand OnCompletedCommand
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(OnCompletedCommand), typeof(ICommand), typeof(EntryBehavior), null);

		protected override void OnAttachedTo(Entry bindable)
		{
			base.OnAttachedTo(bindable);
			AssociatedObject = bindable;
			bindable.BindingContextChanged += OnEntryBindingContextChanged;
			bindable.Completed += OnCompleted;
		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			base.OnDetachingFrom(bindable);
			bindable.BindingContextChanged -= OnEntryBindingContextChanged;
			bindable.Completed -= OnCompleted;
			AssociatedObject = null;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			BindingContext = AssociatedObject.BindingContext;
		}

		void OnCompleted(object sender, EventArgs e)
		{
			OnCompletedCommand?.Execute(e);
		}

		void OnEntryBindingContextChanged(object sender, EventArgs e)
		{
			this.OnBindingContextChanged();
		}
	}
}
