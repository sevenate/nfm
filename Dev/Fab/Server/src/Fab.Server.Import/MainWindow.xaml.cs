using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using Fab.Server.Import.MoneyServiceReference;
using Fab.Server.Import.UserServiceReference;

namespace Fab.Server.Import
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly UserServiceClient userСlient = new UserServiceClient();
		private readonly MoneyServiceClient moneyClient = new MoneyServiceClient();
		private Guid userId;
		private int accountId;

		private readonly Dictionary<KeyValuePair<int, bool>, int> categoryMap = new Dictionary<KeyValuePair<int, bool>, int>();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void GenerateButton_Click(object sender, RoutedEventArgs e)
		{
			DisableUI(false);

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

			DisableUI(true);
		}

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			DisableUI(false);

			var stopWatch = Stopwatch.StartNew();

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
					UpdateStatus("Retrieving expense category list...");
					var expenseGroups = context.TblExpenseGroups.OrderBy(expenseGroup => expenseGroup.Id).ToList();

					for (int i = 0; i < expenseGroups.Count; i++)
					{
						UpdateStatus("Creating " + i + " / " + expenseGroups.Count + " expense category...");
						var categoryId = moneyClient.CreateCategory(userId, expenseGroups[i].Name, 1); // withdrawal category
						categoryMap.Add(new KeyValuePair<int, bool>(expenseGroups[i].Id, true), categoryId);
					}

					UpdateStatus("Retrieving revenue category list...");
					var revenueGroups = context.TblRevenueGroups.OrderBy(revenueGroup => revenueGroup.Id).ToList();

					for (int i = 0; i < revenueGroups.Count; i++)
					{
						UpdateStatus("Creating " + i + " / " + revenueGroups.Count + " revenue category...");
						var categoryId = moneyClient.CreateCategory(userId, revenueGroups[i].Name, 2); // deposit category
						categoryMap.Add(new KeyValuePair<int, bool>(revenueGroups[i].Id, false), categoryId);
					}

					ImportTransactions(context);
				}
			}
			catch (Exception exception)
			{
				stopWatch.Stop();
				ShowError(exception);
			}

			stopWatch.Stop();
			UpdateStatus(string.Format("Done in {0:00}:{1:00}:{2:00}.{3:00}", stopWatch.Elapsed.TotalHours, stopWatch.Elapsed.Minutes, stopWatch.Elapsed.Seconds, stopWatch.Elapsed.Milliseconds));

			DisableUI(true);
		}

		private void DisableUI(bool isEnabled)
		{
			GenerateButton.IsEnabled = isEnabled;
			UsernameTextBox.IsEnabled = isEnabled;
			PasswordTextBox.IsEnabled = isEnabled;
			AccountTextBox.IsEnabled = isEnabled;
			StartButton.IsEnabled = isEnabled;
		}

		private static TransactionData GetExpense(ProfitAndExpenseEntities context, ObjectsMapper<ExpenseList, TransactionData> expenseMapper, int skip)
		{
			return expenseMapper.Map(context.TblExpenseLists
											.OrderBy(list => list.CreationDate)
											.Skip(skip)
											.First());
		}

		private static TransactionData GetRevenue(ProfitAndExpenseEntities context, ObjectsMapper<RevenueList, TransactionData> revenueMapper, int skip)
		{
			return revenueMapper.Map(context.TblRevenueLists
											.OrderBy(list => list.CreationDate)
											.Skip(skip)
											.First());
		}

		private int SaveDeposit(ProfitAndExpenseEntities context, int revenuesCount, ObjectsMapper<RevenueList, TransactionData> revenueMapper, int revenueIndex, ref TransactionData revenue)
		{
			RevenueProgressTextBlock.Text = string.Format("{0} / {1}", revenueIndex + 1, revenuesCount);
			RevenueProgressBar.Value = revenueIndex + 1;
			RevenueProgressPercentTextBlock.Text = string.Format("{0:0.00}%", 100 * RevenueProgressBar.Value / RevenueProgressBar.Maximum);
			UpdateStatus("Importing revenue...");

			moneyClient.Deposit(userId, accountId, revenue.Date, revenue.Price, revenue.Quantity, revenue.Comment, revenue.CategoryId);

			if (revenueIndex < revenuesCount)
			{
				// read next revenue
				revenueIndex++;

				revenue = revenueIndex < revenuesCount
				          	? GetRevenue(context, revenueMapper, revenueIndex)
				          	: null;
			}

			return revenueIndex;
		}

		private int SaveWithdrawal(ProfitAndExpenseEntities context, int expensesCount, ObjectsMapper<ExpenseList, TransactionData> expenseMapper, int expenseIndex, ref TransactionData expense)
		{
			ExpenseProgressTextBlock.Text = string.Format("{0} / {1}", expenseIndex + 1, expensesCount);
			ExpenseProgressBar.Value = expenseIndex + 1;
			ExpenseProgressPercentTextBlock.Text = string.Format("{0:0.00}%", 100 * ExpenseProgressBar.Value / ExpenseProgressBar.Maximum);
			UpdateStatus("Importing expense...");
					
			moneyClient.Withdrawal(userId, accountId, expense.Date, expense.Price, expense.Quantity, expense.Comment, expense.CategoryId);
					
			if (expenseIndex < expensesCount)
			{
				// read next expense
				expenseIndex++;

				expense = expenseIndex < expensesCount
				          	? GetExpense(context, expenseMapper, expenseIndex)
				          	: null;
			}

			return expenseIndex;
		}

		private void ImportTransactions(ProfitAndExpenseEntities context)
		{
			UpdateStatus("Estimating import data size...");
			var expensesCount = context.TblExpenseLists.Count();
			var revenuesCount = context.TblRevenueLists.Count();

			ExpenseProgressTextBlock.Text = "0 / " + expensesCount;
			RevenueProgressTextBlock.Text = "0 / " + revenuesCount;
			ExpenseProgressBar.Maximum = expensesCount;
			RevenueProgressBar.Maximum = revenuesCount;

			var expenseMapper = ObjectMapperManager.DefaultInstance.GetMapper<ExpenseList, TransactionData>(
										new DefaultMapConfig()
										.ConvertUsing<ExpenseList, TransactionData>(exp => new TransactionData
										{
											Date = exp.CreationDate.ToUniversalTime(),
											CategoryId = categoryMap[new KeyValuePair<int, bool>(exp.ExpenseGroupId, true)],
											Comment = string.Format("{0}\n{1}", exp.Name, exp.Memo),
											Price = exp.Cost,
											Quantity = 1.0m,
											IsWithdrawal = true
										}));

			var revenueMapper = ObjectMapperManager.DefaultInstance.GetMapper<RevenueList, TransactionData>(
										new DefaultMapConfig()
										.ConvertUsing<RevenueList, TransactionData>(rev => new TransactionData
										{
											Date = rev.CreationDate.ToUniversalTime(),
											CategoryId = categoryMap[new KeyValuePair<int, bool>(rev.RevenueGroupId, false)],
											Comment = string.Format("{0}\n{1}", rev.SourceName, rev.Memo),
											Price = (decimal)rev.ExchangeRate,
											Quantity = rev.AmountGRN / (decimal)rev.ExchangeRate,
											IsWithdrawal = false
										}));

			int expenseIndex = 0;
			int revenueIndex = 0;

			// Load first expense and first revenue
			TransactionData expense = GetExpense(context, expenseMapper, expenseIndex);
			TransactionData revenue = GetRevenue(context, revenueMapper, revenueIndex);

			do
			{
				if (expense != null && revenue != null)
				{
					if (expense.Date < revenue.Date)
					{
						expenseIndex = SaveWithdrawal(context, expensesCount, expenseMapper, expenseIndex, ref expense);
					}
					else
					{
						revenueIndex = SaveDeposit(context, revenuesCount, revenueMapper, revenueIndex, ref revenue);
					}
				}
				else if (expense != null)
				{
					expenseIndex = SaveWithdrawal(context, expensesCount, expenseMapper, expenseIndex, ref expense);
				}
				else if (revenue != null)
				{
					revenueIndex = SaveDeposit(context, revenuesCount, revenueMapper, revenueIndex, ref revenue);
				}
			} while (expenseIndex < expensesCount || revenueIndex < revenuesCount);
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