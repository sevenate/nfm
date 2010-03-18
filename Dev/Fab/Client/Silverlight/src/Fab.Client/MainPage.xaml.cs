using System;
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

//			var proxy = new UserServiceClient();
//			proxy.IsLoginAvailableCompleted += ProxyOnIsLoginAvailableCompleted;
//			proxy.IsLoginAvailableAsync("123456");

//			var proxy = new TransactionServiceClient();
//			proxy.GetAllTransactionsCompleted += ProxyOnGetAllTransactionsCompleted;
//			proxy.GetAllTransactionsAsync(userId: new Guid("6184b6dd-26d0-4d06-ba2c-95c850ccfebe"), accountId: 1);

			var proxy = new AccountServiceClient();
			proxy.GetAllAccountsCompleted += ProxyOnGetAllAccountsCompleted;
			proxy.GetAllAccountsAsync(userId: new Guid("6184b6dd-26d0-4d06-ba2c-95c850ccfebe"));
		}

		private void ProxyOnGetAllAccountsCompleted(object sender, GetAllAccountsCompletedEventArgs getAllAccountsCompletedEventArgs)
		{
			if (getAllAccountsCompletedEventArgs.Error != null)
			{
				ResultText.Text = "Error: " + getAllAccountsCompletedEventArgs.Error;
			}
			else
			{
				DataContext = getAllAccountsCompletedEventArgs.Result;
				ResultText.Text = "Ready";
			}

			TestButton.IsEnabled = true;
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

		private void ProxyOnGetAllTransactionsCompleted(object sender, GetAllTransactionsCompletedEventArgs getAllTransactionsCompletedEventArgs)
		{
			if (getAllTransactionsCompletedEventArgs.Error != null)
			{
				ResultText.Text = "Error: " + getAllTransactionsCompletedEventArgs.Error;
			}
			else
			{
				DataContext = getAllTransactionsCompletedEventArgs.Result;
				ResultText.Text = "Ready";
			}

			TestButton.IsEnabled = true;
		}
	}
}