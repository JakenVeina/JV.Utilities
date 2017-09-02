using System;

namespace JV.Utilities.Wpf.Mvvm.Interfaces
{
    /// <summary>
    /// Encapsulates the properties and behaviors of View-layer (UI) interaction with a single value of type <typeparam name="T"/>,
    /// allowing the user to specify this value as either selected or not selected or use by the application.
    /// </summary>
    public interface IBooleanOptionVM<T> : IViewModel
    {
        /**********************************************************************/
        #region Properties (View Layer)

        /// <summary>
        /// The data value associated with the option.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if <see cref="Init(T)"/> has not been called.</exception>
        T Value { get; }

        /// <summary>
        /// Flag specifying whether <see cref="Value"/> should be considered value for use by the application.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if <see cref="Init(T)"/> has not been called.</exception>
        bool IsSelected { get; set; }

        #endregion Properties (View Layer)

        /**********************************************************************/
        #region Methods (ViewModel Layer)

        /// <summary>
        /// Completes initialization of the <see cref="IBooleanOptionVM{T}"/> instance by setting the <see cref="Value"/> property.
        /// This method should be called immediately after creation of a new <see cref="IBooleanOptionVM{T}"/> instance, before it is consumed by the View layer,
        /// as <see cref="Value"/> does not implemnent change-notification logic.
        /// </summary>
        /// <param name="value">The value to use for <see cref="Value"/> </param>
        /// <exception cref="InvalidOperationException">Throws if <see cref="Init(T)"/> has already been called.</exception>
        void Init(T value);

        #endregion Methods (ViewModel Layer)
    }
}
