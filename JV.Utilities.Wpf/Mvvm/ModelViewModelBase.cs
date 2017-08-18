using System;
using System.Collections.Generic;

using JV.Utilities.Observation;
using JV.Utilities.Wpf.Mvvm.Interfaces;

namespace JV.Utilities.Wpf.Mvvm
{
    /// <summary>
    /// See <see cref="IModelViewModel{TModel}"/>.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class ModelViewModelBase<TModel> : ViewModelBase, IModelViewModel<TModel>
    {
        /**********************************************************************/
        #region IModelViewModel

        /// <summary>
        /// See <see cref="IModelViewModel{TModel}.Model"/>.
        /// </summary>
        public TModel Model
        {
            get { return _model; }
            set
            {
                if (EqualityComparer<TModel>.Default.Equals(_model, value))
                    return;
                var oldValue = _model;
                _model = value;

                OnModelChanged(oldValue, _model);
                ModelChanged?.Invoke(this, new PropertyChangedEventArgs<TModel>(oldValue, _model));
            }
        }
        private TModel _model;

        /// <summary>
        /// See <see cref="IModelViewModel{TModel}.ModelChanged"/>.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<TModel>> ModelChanged;

        #endregion IModelViewModel

        /**********************************************************************/
        #region Protected Methods

        /// <summary>
        /// Invokes after the value of <see cref="Model"/> has changed, just before <see cref="ModelChanged"/> occurs.
        /// </summary>
        /// <param name="oldValue">The old value of <see cref="Model"/>.</param>
        /// <param name="newValue">The new value of <see cref="Model"/>.</param>
        internal protected abstract void OnModelChanged(TModel oldValue, TModel newValue);

        #endregion Protected Methods
    }
}
