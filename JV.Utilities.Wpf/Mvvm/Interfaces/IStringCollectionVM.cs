using System;

using JV.Utilities.Wpf.Collections.Interfaces;

namespace JV.Utilities.Wpf.Mvvm.Interfaces
{
    /// <summary>
    /// Encapsulates properties and functionality for View-layer (UI) interaction with a collection of <see cref="String"/> objects.
    /// </summary>
    public interface IStringCollectionVM : IViewModel
    {
        /**********************************************************************/
        #region Properties

        /// <summary>
        /// The single-string representation of <see cref="Strings"/>, as a comma-separated list.
        /// This property is automatically synchronized with the values in <see cref="Strings"/>.
        /// </summary>
        string CsvText { get; set; }

        /// <summary>
        /// The collection of strings to be manipulated by the user.
        /// </summary>
        IObservableCollection<string> Strings { get; }

        #endregion Properties
    }
}
