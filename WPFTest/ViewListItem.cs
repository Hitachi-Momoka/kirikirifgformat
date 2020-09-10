using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTest
{
    class ViewListItem : INotifyPropertyChanged
    {
        private string _str;
        public string Str
        {
            get => _str;
            set
            {
                _str = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Str"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
