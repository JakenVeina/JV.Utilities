using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace JV.Utilities.Wpf.Collections.Interfaces
{
    /// <summary>
    /// Represents a list collection which notifies subscribers any it changes.
    /// </summary>
    public interface IObservableCollection<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Moves an item in the collection from one location to another.
        /// </summary>
        /// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws if either <paramref name="oldIndex"/> or <paramref name="newIndex"/> is not within the current bounds of the collection.</exception>
        void Move(int oldIndex, int newIndex);
    }
}
