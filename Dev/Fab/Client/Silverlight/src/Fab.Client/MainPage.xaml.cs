using System;
using System.ComponentModel;
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
		private Guid userId = new Guid("7F06BFA6-B675-483C-9BF3-F59B88230382");

		private int accountId = 5;

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
			//			proxy.GetAllTransactionsAsync(userId: userId, accountId: 1);

			var proxy = new TransactionServiceClient();
			proxy.GetAllTransactionsCompleted += ProxyOnGetAllTransactionsCompleted;
			proxy.GetAllTransactionsAsync(userId: userId, accountId: accountId);
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
				transactionsGrid.ItemsSource = getAllTransactionsCompletedEventArgs.Result;
				ResultText.Text = "Ready";
			}

			TestButton.IsEnabled = true;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			SaveButton.IsEnabled = false;
			ResultText.Text = "Saving...";

			var proxy = new TransactionServiceClient();
			
			if (DepositRadioButton.IsChecked.HasValue && DepositRadioButton.IsChecked.Value)
			{
				proxy.DepositCompleted += OnSavingCompleted;
				proxy.DepositAsync(userId: userId,
								   accountId: accountId,//AccountComboBox.SelectedValue,
								   price: decimal.Parse(PriceTextBox.Text.Trim()),
								   quantity: decimal.Parse(QuantityTextBox.Text.Trim()),
								   comment: CommentTextBox.Text.Trim(),
								   categoryId: null//(int)CategoryComboBox.SelectedValue
								  );
			}
			else
			{
				proxy.WithdrawalCompleted += OnSavingCompleted;
				proxy.WithdrawalAsync(userId: userId,
								   accountId: accountId,//AccountComboBox.SelectedValue,
								   price: decimal.Parse(PriceTextBox.Text.Trim()),
								   quantity: decimal.Parse(QuantityTextBox.Text.Trim()),
								   comment: CommentTextBox.Text.Trim(),
								   categoryId: null//(int)CategoryComboBox.SelectedValue
								  );
			}
		}

		private void OnSavingCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
		{
			if (asyncCompletedEventArgs.Error != null)
			{
				ResultText.Text = "Error: " + asyncCompletedEventArgs.Error;
			}
			else
			{
				ResultText.Text = "Ready";
			}

			SaveButton.IsEnabled = true;
		}
	}
}