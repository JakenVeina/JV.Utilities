using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using JV.Utilities.Wpf.Mvvm.Interfaces;

namespace JV.Utilities.Wpf.Mvvm
{
    /// <summary>
    /// See <see cref="IViewModel"/>.
    /// </summary>
    public abstract class ViewModelBase : IViewModel
    {
        /**********************************************************************/
        #region INotifyPropertyChanged

        /// <summary>
        /// See <see cref="INotifyPropertyChanged.PropertyChanged"/>.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The public name of the property that is changing.</param>
        internal protected void RaisePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion INotifyPropertyChanged

        /**********************************************************************/
        #region IDataErrorInfo

        /// <summary>
        /// See <see cref="IDataErrorInfo"/>
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                string errorMessage = null;
                _errorMessages.TryGetValue(columnName ?? string.Empty, out errorMessage);
                return errorMessage ?? string.Empty;
            }
            internal protected set
            {
                _errorMessages[columnName ?? string.Empty] = value;

                Error = !string.IsNullOrEmpty(value) ? value : _errorMessages.Values.FirstOrDefault(x => !string.IsNullOrEmpty(x)) ?? string.Empty;
            }
        }

        /// <summary>
        /// See <see cref="IDataErrorInfo.Error"/>.
        /// </summary>
        public string Error { get; private set; } = string.Empty;

        #endregion IDataErrorInfo

        /**********************************************************************/
        #region Private Fields

        private Dictionary<string, string> _errorMessages = new Dictionary<string, string>();

        #endregion Private Fields
    }
}
