using System;
using System.ComponentModel;

namespace JV.Utilities.Wpf.Mvvm.Interfaces
{
    /// <summary>
    /// Represents a <see cref="IViewModel"/> that represents data for a specific model object.
    /// </summary>
    /// <typeparam name="TModel">The type of model object whose data is represented by the ViewModel.</typeparam>
    public interface IModelViewModel<TModel> : IViewModel
    {
        /**********************************************************************/
        #region Properties

        /// <summary>
        /// The TModel object whose data is currently represented within the ViewModel.
        /// This property is intended for VM to VM interaction only, not for use in the View layer.
        /// As such, change notification for this property is provided by the dedicated event <see cref="ModelChanged"/>,
        /// not by <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        TModel Model { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region Events

        /// <summary>
        /// Occurs after the value of <see cref="Model"/> has changed.
        /// </summary>
        event EventHandler ModelChanged;

        #endregion Events
    }
}
