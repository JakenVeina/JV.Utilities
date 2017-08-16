using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace JV.Utilities.Wpf.Collections.Interfaces
{
    /// <summary>
    /// Represents a list collection which notifies subscribers any items its items change, and which cannot be modified by subscribers.
    /// </summary>
    public interface IReadOnlyObservableCollection<T> : IReadOnlyList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
    }
}
