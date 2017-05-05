using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace PartyTimeline
{
	public class SortableObservableCollection<T> : ObservableCollection<T> where T : BaseModel
	{
		public SortableObservableCollection(IEnumerable<T> collection) :
			base(collection)
		{ }

		public SortableObservableCollection() : base() { }

		public void Sort<TKey>(Func<T, TKey> keySelector)
		{
			Sort(Items.OrderBy(keySelector));
		}

		public void Sort<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
		{
			Sort(Items.OrderBy(keySelector, comparer));
		}

		public void SortDescending<TKey>(Func<T, TKey> keySelector)
		{
			Sort(Items.OrderByDescending(keySelector));
		}

		public void SortDescending<TKey>(Func<T, TKey> keySelector,
			IComparer<TKey> comparer)
		{
			Sort(Items.OrderByDescending(keySelector, comparer));
		}

		public void Sort(IEnumerable<T> sortedItems)
		{
			List<T> sortedItemsList = sortedItems.ToList();
			for (int i = 0; i < sortedItemsList.Count; i++)
			{
				int oldIndex = IndexOf(sortedItemsList[i]);
				Items[i] = sortedItemsList[i];
				if (i != oldIndex)
				{
					OnCollectionChanged(new NotifyCollectionChangedEventArgs(
						NotifyCollectionChangedAction.Move,
						sortedItemsList[i],
						i,
						oldIndex
					));
				}
			}
		}

		public void Update(int index, T update)
		{
			this[index].Update(update);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
				NotifyCollectionChangedAction.Replace,
				update,
				update,
				index
			));
		}

		public void Replace(int index, T newT)
		{
			BaseModel oldT = this[index];
			this[index] = newT;
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(
				NotifyCollectionChangedAction.Replace,
				newT,
				oldT,
				index
			));
		}
	}
}