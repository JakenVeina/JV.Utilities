using System;

using JV.Utilities.Wpf.Mvvm.Interfaces;

namespace JV.Utilities.Wpf.Mvvm
{
    /// <summary>
    /// See <see cref="IBooleanOptionVM{T}"/>.
    /// </summary>
    public class BooleanOptionVM<T> : ViewModelBase, IBooleanOptionVM<T>
    {
        /**********************************************************************/
        #region IBooleanOptionVM

        /// <summary>
        /// See <see cref="IBooleanOptionVM{T}.Value"/>.
        /// </summary>
        public T Value
        {
            get
            {
                AssertHasInitialized();
                return _value;
            }
            private set
            {
                _value = value;
            }
        }
        private T _value;

        /// <summary>
        /// See <see cref="IBooleanOptionVM{T}.IsSelected"/>.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                AssertHasInitialized();
                return _isSelected;
            }
            set
            {
                AssertHasInitialized();
                if (_isSelected == value)
                    return;

                _isSelected = value;

                RaisePropertyChanged(nameof(IsSelected));
            }
        }
        private bool _isSelected;

        /// <summary>
        /// See <see cref="IBooleanOptionVM{T}.Init(T)"/>.
        /// </summary>
        public void Init(T value)
        {
            if (_hasInitialized)
                throw new InvalidOperationException($"{nameof(Init)} has already been invoked.");

            Value = value;
            _hasInitialized = true;

        }
        private bool _hasInitialized = false;

        #endregion IBooleanOptionVM

        /**********************************************************************/
        #region Protected Methods

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if <see cref="Init(T)"/> has not yet been invoked.
        /// For use within class and subclass members that intend for the object to be fully initialized before being invoked.
        /// </summary>
        internal protected void AssertHasInitialized()
        {
            if (!_hasInitialized)
                throw new InvalidOperationException($"{nameof(Init)} has not been invoked.");
        }

        #endregion Protected Methods
    }
}
