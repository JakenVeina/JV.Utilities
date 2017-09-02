using System.Collections.Specialized;
using System.Linq;

using JV.Utilities.Wpf.Collections;
using JV.Utilities.Wpf.Collections.Interfaces;
using JV.Utilities.Wpf.Mvvm.Interfaces;

namespace JV.Utilities.Wpf.Mvvm
{
    /// <summary>
    /// See <see cref="IStringCollectionVM"/>.
    /// </summary>
    public class StringCollectionVM : ViewModelBase, IStringCollectionVM
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="StringCollectionVM"/> object.
        /// </summary>
        public StringCollectionVM()
        {
            // Not worth doing dependency injection for a single collection that's publicly exposed anyway.
            Strings = new ObservableCollection<string>();
            Strings.CollectionChanged += OnStringsChanged;
        }

        #endregion Constructors

        /**********************************************************************/
        #region IStringCollectionVM

        /// <summary>
        /// See <see cref="IStringCollectionVM.CsvText"/>.
        /// </summary>
        public string CsvText
        {
            get { return _csvText; }
            set
            {
                if (_csvText == value)
                    return;

                _csvText = value;

                if(!_isStringDataChanging)
                {
                    _isStringDataChanging = true;

                    if (string.IsNullOrEmpty(_csvText))
                        Strings.Clear();

                    else
                    {
                        var newStrings = _csvText.Split(',')
                                                 .Select(x => x.Trim())
                                                 .Where(x => (x != string.Empty))
                                                 .ToArray();

                        var i = 0;
                        for (; ((i < Strings.Count) && (i < newStrings.Length)); ++i)
                            Strings[i] = newStrings[i];
                        for (; i < newStrings.Length; ++i)
                            Strings.Add(newStrings[i]);
                        while (Strings.Count > newStrings.Length)
                            Strings.RemoveAt(Strings.Count - 1);
                    }

                    _isStringDataChanging = false;
                }

                RaisePropertyChanged(nameof(CsvText));
            }
        }
        private string _csvText;

        /// <summary>
        /// See <see cref="IStringCollectionVM.Strings"/>
        /// </summary>
        public IObservableCollection<string> Strings { get; }

        #endregion IStringCollectionVM

        /**********************************************************************/
        #region Private Methods

        private void OnStringsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(!_isStringDataChanging)
            {
                _isStringDataChanging = true;

                CsvText = string.Join(", ", Strings);

                _isStringDataChanging = false;
            }
        }

        #endregion Private Methods

        /**********************************************************************/
        #region Private Fields

        private bool _isStringDataChanging = false;

        #endregion Private Fields
    }
}
