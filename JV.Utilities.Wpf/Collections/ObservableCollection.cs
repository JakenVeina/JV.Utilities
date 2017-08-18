using System.Collections.Generic;

using JV.Utilities.Wpf.Collections.Interfaces;

namespace JV.Utilities.Wpf.Collections
{
    /// <summary>
    /// Extension of <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>, which implements <see cref="IObservableCollection{T}"/>,
    /// allowing for complete abstraction/dependency injection in Unit Testing.
    /// </summary>
    public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>, IObservableCollection<T>, IReadOnlyObservableCollection<T>
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// See <see cref="ICollectionFactory.CreateObservableCollection{T}()"/>.
        /// See <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.
        /// </summary>
        public ObservableCollection()
            : base() { }

        /// <summary>
        /// See <see cref="ICollectionFactory.CreateObservableCollection{T}(List{T})"/>.
        /// See <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.
        /// </summary>
        public ObservableCollection(List<T> list)
            : base(list) { }

        /// <summary>
        /// See <see cref="ICollectionFactory.CreateObservableCollection{T}(IEnumerable{T})"/>.
        /// See <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>.
        /// </summary>
        public ObservableCollection(IEnumerable<T> collection)
            : base(collection) { }

        #endregion Constructors
    }
}
