using System.Net;
using System.Net.Browser;
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

			// Required for support HTTP response code 500 (Internal Server Error)
			// with SOAP Faults xml informarmation returned from WCF service in case of server side error.
			// Details: http://blogs.msdn.com/carlosfigueira/archive/2009/08/15/fault-support-in-silverlight-3.aspx
			// Note: should always return "true", unless the prefix has been previously registered.
			WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
		}

		private void TestButton_Click(object sender, RoutedEventArgs e)
		{
			TestButton.IsEnabled = false;
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

			TestButton.IsEnabled = true;
		}
	}
}