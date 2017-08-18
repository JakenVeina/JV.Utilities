using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using JV.Utilities.Wpf.Collections.Interfaces;

namespace JV.Utilities.Wpf.Collections
{
    /// <summary>
    /// Implements <see cref="IReadOnlyObservableCollection{T}"/> by wrapping an existing <see cref="IObservableCollection{T}"/> object, 
    /// and re-exposing only the members that are required by <see cref="IReadOnlyObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of items to be contained in the collection.</typeparam>
    public class ReadOnlyObservableCollection<T> : IReadOnlyObservableCollection<T>
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// See <see cref="ICollectionFactory.CreateReadOnlyObservableCollection{T}(IObservableCollection{T})"/>.
        /// </summary>
        public ReadOnlyObservableCollection(IObservableCollection<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            _source = source;
        }

        #endregion Constructors

        /**********************************************************************/
        #region INotifyCollectionChanged

        /// <summary>
        /// See <see cref="INotifyCollectionChanged.CollectionChanged"/>.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _source.CollectionChanged += value; }
            remove { _source.CollectionChanged -= value; }
        }

        #endregion INotifyCollectionChanged

        /**********************************************************************/
        #region INotifyPropertyChanged

        /// <summary>
        /// See <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _source.PropertyChanged += value; }
            remove { _source.PropertyChanged -= value; }
        }

        #endregion INotifyPropertyChanged

        /**********************************************************************/
        #region IReadOnlyList

        /// <summary>
        /// See <see cref="IReadOnlyList{T}.this[int]"/>.
        /// </summary>
        public T this[int index]
            => _source[index];

        #endregion IReadOnlyList

        /**********************************************************************/
        #region IReadOnlyCollection

        /// <summary>
        /// See <see cref="IReadOnlyCollection{T}.Count"/>.
        /// </summary>
        public int Count
            => _source.Count;

        #endregion IReadOnlyCollection

        /**********************************************************************/
        #region IEnumerable

        /// <summary>
        /// See <see cref="IEnumerable{T}.GetEnumerator"/>.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
            => _source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)_source).GetEnumerator();

        #endregion IEnumerable

        /**********************************************************************/
        #region Private Fields

        private readonly IObservableCollection<T> _source;

        #endregion Private Fields
    }
}
