using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Fab.Server.Import.MoneyServiceReference;
using Fab.Server.Import.UserServiceReference;

namespace Fab.Server.Import
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private UserServiceClient userСlient = new UserServiceClient();
		private MoneyServiceClient moneyClient = new MoneyServiceClient();
		private Guid userId;
		private int accountId;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void GenerateButton_Click(object sender, RoutedEventArgs e)
		{
			GenerateButton.IsEnabled = false;

			try
			{
				UpdateStatus("Generating unique user login...");
				UsernameTextBox.Text = userСlient.GenerateUniqueLogin();
			
				UpdateStatus("Ready");
			}
			catch (Exception exception)
			{
				ShowError(exception);
			}
	
			GenerateButton.IsEnabled = true;
		}

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			StartButton.IsEnabled = false;

			try
			{
				UpdateStatus("Registering user \"" + UsernameTextBox.Text.Trim() + "\"...");
				userСlient.Register(UsernameTextBox.Text.Trim(), PasswordTextBox.Text.Trim());
				userId = userСlient.GetUserId(UsernameTextBox.Text.Trim());

				UpdateStatus("Creating account \"" + AccountTextBox.Text.Trim() + "\"...");
				moneyClient.CreateAccount(userId, AccountTextBox.Text.Trim(), 1); // UAH account by default
				accountId = moneyClient.GetAllAccounts(userId)[0].Id;

				using (var context = new ProfitAndExpenseEntities())
				{
					UpdateStatus("Retrieving revenue category list...");
					var revenueGroups = context.TblRevenueGroups.OrderBy(revenueGroup => revenueGroup.Id).ToList();

					for (int i = 0; i < revenueGroups.Count; i++)
					{
						UpdateStatus("Creating " + i + " / " + revenueGroups.Count + " revenue category...");
						moneyClient.CreateCategory(userId, revenueGroups[i].Name);
					}

					UpdateStatus("Retrieving expense category list...");
					var expenseGroups = context.TblExpenseGroups.OrderBy(expenseGroup => expenseGroup.Id).ToList();

					for (int i = 0; i < expenseGroups.Count; i++)
					{
						UpdateStatus("Creating " + i + " / " + expenseGroups.Count + " expense category...");
						moneyClient.CreateCategory(userId, expenseGroups[i].Name);
					}
				}


				UpdateStatus("Ready");
			}
			catch (Exception exception)
			{
				ShowError(exception);
			}
			
			StartButton.IsEnabled = true;
		}

		private static void ShowError(Exception exception)
		{
			MessageBox.Show(exception.ToString(), "Import error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void UpdateStatus(string statusText)
		{
			StatusTextBlock.Text = statusText;
			Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
		}
	}
}