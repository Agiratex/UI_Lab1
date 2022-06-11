using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClassLibrary;

namespace App
{
    public class VMfunction
    {
        public VMf Function { get; set; }
        public string Name { get; set; }
    }

    public class VMfConv : INotifyPropertyChanged
    {
        private VMfunction selected_func;
        public event PropertyChangingEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangingEventArgs(prop));
        }
        public ObservableCollection<VMfunction> Functions { get; set; }

        public VMfConv()
        {
            Functions = new ObservableCollection<VMfunction>
            {
                new VMfunction { Function = VMf.vmdLn, Name = "dLn" },
                new VMfunction { Function = VMf.vmdLGamma, Name = "dLGamma"},
                new VMfunction { Function = VMf.vmsLn, Name = "sLn" },
                new VMfunction { Function = VMf.vmsLGamma, Name = "sLGamma"}
            };
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                ((INotifyPropertyChanged)Functions).PropertyChanged += value;
            }

            remove
            {
                ((INotifyPropertyChanged)Functions).PropertyChanged -= value;
            }
        }

        public VMfunction SelectedFunc
        {
            get => selected_func;
            set
            {
                selected_func = value;
                OnPropertyChanged(nameof(SelectedFunc));
            }
        }

    }
}
