using System;
using System.Collections.Generic;

using JV.Utilities.Math;
using JV.Utilities.Observation;
using JV.Utilities.Wpf.Mvvm.Interfaces;

namespace JV.Utilities.Wpf.Mvvm
{
    /// <summary>
    /// See <see cref="IMinMaxPairVM{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MinMaxPairVM<T> : ModelViewModelBase<MinMaxPair<T>>, IMinMaxPairVM<T> where T : IComparable<T>
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="MinMaxPairVM{T}"/> object.
        /// </summary>
        public MinMaxPairVM()
        {
            ModelChanged += OnModelChanged;
        }

        #endregion Constructors

        /**********************************************************************/
        #region IMinMaxPairVM

        /// <summary>
        /// See <see cref="IMinMaxPairVM{T}.Minimum"/>
        /// </summary>
        public T Minimum
        {
            get { return _minimum; }
            set
            {
                if (EqualityComparer<T>.Default.Equals(_minimum, value))
                    return;

                _minimum = value;

                if(!string.IsNullOrEmpty(this[nameof(Maximum)]))
                {
                    this[nameof(Maximum)] = null;
                    RaisePropertyChanged(nameof(Maximum)); // Force error info on Maximum to update
                }

                if (Comparer<T>.Default.Compare(value, _maximum) > 0)
                {
                    this[nameof(Minimum)] = $"Cannot be greater than {nameof(Maximum)}";
                }
                else
                {
                    this[nameof(Minimum)] = null;

                    Model = new MinMaxPair<T>(_minimum, _maximum);
                }

                RaisePropertyChanged(nameof(Minimum));
            }
        }
        private T _minimum;

        /// <summary>
        /// See <see cref="IMinMaxPairVM{T}.Maximum"/>
        /// </summary>
        public T Maximum
        {
            get { return _maximum; }
            set
            {
                if (EqualityComparer<T>.Default.Equals(_maximum, value))
                    return;

                _maximum = value;

                if (!string.IsNullOrEmpty(this[nameof(Minimum)]))
                {
                    this[nameof(Minimum)] = null;
                    RaisePropertyChanged(nameof(Minimum)); // Force error info on Maximum to update
                }

                if (Comparer<T>.Default.Compare(value, _minimum) < 0)
                {
                    this[nameof(Maximum)] = $"Cannot be less than {nameof(Minimum)}";
                }
                else
                {
                    this[nameof(Maximum)] = null;

                    Model = new MinMaxPair<T>(_minimum, _maximum);
                }

                RaisePropertyChanged(nameof(Maximum));
            }
        }
        private T _maximum;

        #endregion IMinMaxPairVM

        /**********************************************************************/
        #region Private Methods

        private void OnModelChanged(object sender, PropertyChangedEventArgs<MinMaxPair<T>> e)
        {
            Minimum = e.NewValue.Min;
            Maximum = e.NewValue.Max;
        }

        #endregion ModelViewModelBase Overrides
    }
}
