using System.ComponentModel;

namespace JV.Utilities.Wpf.Mvvm.Interfaces
{
    /// <summary>
    /// Encapsulates common functionality used by View Models.
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged, IDataErrorInfo { }
}
