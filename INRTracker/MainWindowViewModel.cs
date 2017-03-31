using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using INRTracker.Models;
using INRTracker.Services;

namespace INRTracker
{
    /// <summary>
    /// ViewModel for the main window of the application.
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        #region Private Fields

        private IINREntryRepository _repo;
        private ObservableCollection<INREntry> _entries;
        private INREntry _selectedEntry;

        #endregion

        #region Public Fields

        public RelayCommand AddEntryCommand { get; private set; }
        public RelayCommand RemoveEntryCommand { get; private set; }

        #endregion

        #region Properties

        public INREntry SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                SetProperty(ref _selectedEntry, value);
            }
        }

        public ObservableCollection<INREntry> Entries
        {
            get { return _entries; }
            set
            {
                SetProperty(ref _entries, value);
            }
        }

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            string connString = ConfigurationManager.ConnectionStrings["main"].ConnectionString;
            _repo = new INREntryRepositorySqlite(connString);

            AddEntryCommand = new RelayCommand(OnAddEntry);
            RemoveEntryCommand = new RelayCommand(OnRemoveEntry);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load all INR Entries. 
        /// </summary>
        public void LoadEntries()
        {
            // TODO load entries from live database
            Entries = new ObservableCollection<INREntry>();
            for (int i = 0; i < 5; i++) _entries.Add(INREntry.GenerateRandomEntry());
        }

        /// <summary>
        /// Event handling for when the Add button is pressed.
        /// </summary>
        private void OnAddEntry()
        {
            Debug.WriteLine("Add Command");
        }

        /// <summary>
        /// Event handling for when the Remove button is pressed.
        /// </summary>
        private void OnRemoveEntry()
        {
            Debug.WriteLine("Remove Command");
        }

        #endregion
    }
}
