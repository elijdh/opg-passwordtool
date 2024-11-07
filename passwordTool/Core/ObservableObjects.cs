using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace passwordTool.Core
{
    internal class ObservableObjects
    {

        /* InotfiyProperty Changed
    * interface used in wpf to help manage data binding between user interface and underlying model
    */

        internal class ObservableObject : INotifyPropertyChanged // inherits from InotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged; // implemented event/actual interface

            // the onproperty change method
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                // callerMmemberName = an attribute
                //node check the actual event -> if its not null we want to invoke it-> the senders can be this & even args= want to create a new proprty change event and pass in name to it
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
