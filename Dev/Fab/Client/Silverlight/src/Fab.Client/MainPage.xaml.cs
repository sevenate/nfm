using System.Windows;
using Fab.Client.ApiServiceReference;

namespace Fab.Client
{
	/// <summary>
	/// The main page.
	/// </summary>
	public partial class MainPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MainPage"/> class.
		/// </summary>
		public MainPage()
		{
			InitializeComponent();
		}

		private void TestButton_Click(object sender, RoutedEventArgs e)
		{
			ResultText.Text = "Connecting...";

			var proxy = new UserServiceClient();
			proxy.IsLoginAvailableCompleted += ProxyOnIsLoginAvailableCompleted;
			proxy.IsLoginAvailableAsync("123456");
		}
		
		private void ProxyOnIsLoginAvailableCompleted(
			object sender, IsLoginAvailableCompletedEventArgs isLoginAvailableCompletedEventArgs)
		{
			if (isLoginAvailableCompletedEventArgs.Error != null)
			{
				ResultText.Text = "Error: " + isLoginAvailableCompletedEventArgs.Error;
			}
			else
			{
				ResultText.Text = "Done. Connection is OK";
			}
		}
	}
}