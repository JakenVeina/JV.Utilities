using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

using JV.Utilities.Wpf.Collections.Interfaces;

namespace JV.Utilities.Wpf.Collections
{
    /// <summary>
    /// Implements <see cref="ICollectionFactory"/> for the concrete classes defined in the <see cref="Collections"/> namespace.
    /// </summary>
    public class CollectionFactory : ICollectionFactory
    {
        /**********************************************************************/
        #region Static Fields

        /// <summary>
        /// Contains a default instance of <see cref="CollectionFactory"/>.
        /// </summary>
        public static readonly CollectionFactory Default = new CollectionFactory();

        #endregion Static Fields

        /**********************************************************************/
        #region Methods

        /// <summary>
        /// See <see cref="ICollectionFactory.CreateObservableCollection{T}()"/>.
        /// Creates objects of type <see cref="ObservableCollection{T}"/>.
        /// </summary>
        public IObservableCollection<T> CreateObservableCollection<T>()
            => new ObservableCollection<T>();

        /// <summary>
        /// See <see cref="ICollectionFactory.CreateObservableCollection{T}(List{T})"/>.
        /// Creates objects of type <see cref="ObservableCollection{T}"/>.
        /// </summary>
        public IObservableCollection<T> CreateObservableCollection<T>(List<T> list)
            => new ObservableCollection<T>(list);

        /// <summary>
        /// See <see cref="ICollectionFactory.CreateObservableCollection{T}(IEnumerable{T})"/>.
        /// Creates objects of type <see cref="ObservableCollection{T}"/>.
        /// </summary>
        public IObservableCollection<T> CreateObservableCollection<T>(IEnumerable<T> collection)
            => new ObservableCollection<T>(collection);

        /// <summary>
        /// See <see cref="ICollectionFactory.CreateReadOnlyObservableCollection{T}(IObservableCollection{T})"/>.
        /// Creates objects of type <see cref="ReadOnlyObservableCollection{T}"/>.
        /// </summary>
        public IReadOnlyObservableCollection<T> CreateReadOnlyObservableCollection<T>(IObservableCollection<T> source)
            => new ReadOnlyObservableCollection<T>(source);

        /// <summary>
        /// See <see cref="ICollectionFactory.CreateCollectionView{T}(IEnumerable{T})"/>.
        /// Utilizes <see cref="CollectionViewSource"/> to create new <see cref="ICollectionView"/> objects.
        /// </summary>
        public ICollectionView CreateCollectionView<T>(IEnumerable<T> source)
            => new CollectionViewSource() { Source = source }.View;

        #endregion Methods
    }
}
