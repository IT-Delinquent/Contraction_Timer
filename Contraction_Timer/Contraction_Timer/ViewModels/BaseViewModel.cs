using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Contraction_Timer.ViewModels
{
    /// <summary>
    /// The base ViewModel used throughout the application
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged

        /// <summary>
        /// The public event for property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The protected method for calling the property changed event handler
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PropertyChanged
    }
}
