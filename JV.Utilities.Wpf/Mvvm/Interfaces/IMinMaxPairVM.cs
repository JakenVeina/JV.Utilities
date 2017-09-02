using System;

using JV.Utilities.Math;

namespace JV.Utilities.Wpf.Mvvm.Interfaces
{
    /// <summary>
    /// Encapsulates the properties and behaviors of View-layer (UI) interaction with a <see cref="MinMaxPair{T}"/> value.
    /// </summary>
    public interface IMinMaxPairVM<T> : IModelViewModel<MinMaxPair<T>> where T : IComparable<T>
    {
        /**********************************************************************/
        #region Properties (View Layer)

        /// <summary>
        /// The minimum value within the pair.
        /// </summary>
        T Minimum { get; set; }

        /// <summary>
        /// The maximum value within the pair.
        /// </summary>
        T Maximum { get; set; }

        #endregion Properties (View Layer)
    }
}
