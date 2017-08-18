using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace JV.Utilities.Wpf.Collections.Interfaces
{
    /// <summary>
    /// Describes an object which creates concrete instances of objects implementing interfaces within this namespace.
    /// </summary>
    public interface ICollectionFactory
    {
        /// <summary>
        /// Creates a new, empty, instance of a <see cref="IObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of items to be contained in the collection.</typeparam>
        /// <returns>The newly created <see cref="IObservableCollection{T}"/> object.</returns>
        IObservableCollection<T> CreateObservableCollection<T>();

        /// <summary>
        /// Creates a new, empty, instance of a <see cref="IObservableCollection{T}"/>, with items copied from a given <see cref="List{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The type of items to be contained in the collection.</typeparam>
        /// <param name="list">The object containing items to be copied into the new <see cref="IObservableCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Throws for <paramref name="list"/>.</exception>
        /// <returns>The newly created <see cref="IObservableCollection{T}"/> object.</returns>
        IObservableCollection<T> CreateObservableCollection<T>(List<T> list);

        /// <summary>
        /// Creates a new, empty, instance of a <see cref="IObservableCollection{T}"/>, with items copied from a given <see cref="IEnumerable{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The type of items to be contained in the collection.</typeparam>
        /// <param name="collection">The enumerable object containing items to be copied into the new <see cref="IObservableCollection{T}"/>.</param>
        /// <exception cref="ArgumentNullException">Throws for <paramref name="collection"/>.</exception>
        /// <returns>The newly created <see cref="IObservableCollection{T}"/> object.</returns>
        IObservableCollection<T> CreateObservableCollection<T>(IEnumerable<T> collection);

        /// <summary>
        /// Creates a new, <see cref="IReadOnlyObservableCollection{T}"/> object, providing a read-only view of the state of a given <see cref="IObservableCollection{T}"/> object.
        /// </summary>
        /// <typeparam name="T">The type of items contained in <paramref name="source"/>.</typeparam>
        /// <param name="source">The collection whose read-only state is to be exposed.</param>
        /// <exception cref="ArgumentNullException">Throws for <paramref name="source"/>.</exception>
        /// <returns>The newly created <see cref="IReadOnlyObservableCollection{T}"/> object.</returns>
        IReadOnlyObservableCollection<T> CreateReadOnlyObservableCollection<T>(IObservableCollection<T> source);

        /// <summary>
        /// Creates a new <see cref="ICollectionView"/> object, using the given <see cref="IEnumerable{T}"/> as <see cref="ICollectionView.SourceCollection"/>.
        /// The <see cref="ICollectionView"/> provides a filterable/sortable/groupable view of the current state of the source collection,
        /// allowing it to be manipulated by the View layer without affecting the source.
        /// If <paramref name="source"/> implements <see cref="IList"/> or <see cref="IList{T}"/>, any changes (addition, removal, or replacement of items)
        /// made to the <see cref="ICollectionView"/> will automatically be applied to <paramref name="source"/>.
        /// If <paramref name="source"/> implements <see cref="INotifyCollectionChanged"/>, the <see cref="ICollectionView"/> object will automatically update itsef
        /// if any changes are made to <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">The type of items to be viewed.</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        ICollectionView CreateCollectionView<T>(IEnumerable<T> source);
    }
}
