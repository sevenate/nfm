namespace Fab.Client.Shell.Controls
{
	public partial class Spinner
	{
        public Spinner()
        {
            InitializeComponent();
            Loaded += (s, e) => spinAnimation.Begin();
        }
    }
}