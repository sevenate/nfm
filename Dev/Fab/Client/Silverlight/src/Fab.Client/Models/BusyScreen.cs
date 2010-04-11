using Caliburn.PresentationFramework.Screens;

namespace Fab.Client.Models
{
    public class BusyScreen : Screen
    {
        private string _message = "Loading...";

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }
    }
}