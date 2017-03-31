using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace INRTracker.Models
{
    /// <summary>
    /// Base class for models that implements the INotifyPropertyChanged interface.
    /// </summary>
    public class BindableBase : INotifyPropertyChanged
    {
        #region Public Fields

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        

        #endregion

        #region Methods

        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
