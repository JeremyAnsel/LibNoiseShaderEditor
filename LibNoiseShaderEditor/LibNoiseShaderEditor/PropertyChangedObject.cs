using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibNoiseShaderEditor
{
    public abstract class PropertyChangedObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _changed;

        [Browsable(false)]
        public int Changed
        {
            get => _changed;

            set
            {
                _changed++;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Changed)));
            }
        }

        public void RaiseAndSetIfChanged<T>(ref T property, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(property, newValue))
            {
                return;
            }

            property = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Changed = 0;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Changed = 0;
        }
    }
}
