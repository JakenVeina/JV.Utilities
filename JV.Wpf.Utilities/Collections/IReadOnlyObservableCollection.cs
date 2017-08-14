using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace JV.Wpf.Utilities.Collections
{
    /// <summary>
    /// Represents a list collection which notifies subscribers any items its items change, and which cannot be modified by subscribers.
    /// </summary>
    public interface IReadOnlyObservableCollection<T> : IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
    }
}
