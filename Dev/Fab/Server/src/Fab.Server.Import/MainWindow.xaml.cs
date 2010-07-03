﻿// <copyright file="MainWindow.xaml.cs" company="HD">
//  Copyright (c) 2009-2010 nReez. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff" email="78@nreez.com" date="2010-06-28" />
// <summary>Interaction logic for MainWindow.xaml</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using Fab.Server.Import.MoneyServiceReference;
using Fab.Server.Import.UserServiceReference;

namespace Fab.Server.Import
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		/// <summary>
		/// Maps old two old groups (revenueGroupId and expenseGroupId) to new one (categoryId).
		/// </summary>
		private readonly Dictionary<KeyValuePair<int, bool>, int> categoryMap = new Dictionary<KeyValuePair<int, bool>, int>();

		/// <summary>
		/// Proxy client for MoneyService.
		/// </summary>
		private readonly MoneyServiceClient moneyClient = new MoneyServiceClient();
		
		/// <summary>
		/// Proxy client for UserService.
		/// </summary>
		private readonly UserServiceClient userСlient = new UserServiceClient();

		/// <summary>
		/// Created account ID.
		/// </summary>
		private int accountId;

		/// <summary>
		/// Registered user ID.
		/// </summary>
		private Guid userId;

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Generate new unique user login name.
		/// </summary>
		/// <param name="sender">Generate button.</param>
		/// <param name="e">Routed event data.</param>
		private void GenerateButtonClick(object sender, RoutedEventArgs e)
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

		/// <summary>
		/// Start import data from old SQL database to new one via service calls.
		/// </summary>
		/// <param name="sender">Start button.</param>
		/// <param name="e">Routed event data.</param>
		private void StartButtonClick(object sender, RoutedEventArgs e)
		{
			DisableUI(false);

			Stopwatch stopWatch = Stopwatch.StartNew();

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
					List<ExpenseGroup> expenseGroups = context.TblExpenseGroups.OrderBy(expenseGroup => expenseGroup.Id).ToList();

					for (int i = 0; i < expenseGroups.Count; i++)
					{
						UpdateStatus("Creating " + i + " / " + expenseGroups.Count + " expense category...");
						int categoryId = moneyClient.CreateCategory(userId, expenseGroups[i].Name, 1); // withdrawal category
						categoryMap.Add(new KeyValuePair<int, bool>(expenseGroups[i].Id, true), categoryId);
					}

					UpdateStatus("Retrieving revenue category list...");
					List<RevenueGroup> revenueGroups = context.TblRevenueGroups.OrderBy(revenueGroup => revenueGroup.Id).ToList();

					for (int i = 0; i < revenueGroups.Count; i++)
					{
						UpdateStatus("Creating " + i + " / " + revenueGroups.Count + " revenue category...");
						int categoryId = moneyClient.CreateCategory(userId, revenueGroups[i].Name, 2); // deposit category
						categoryMap.Add(new KeyValuePair<int, bool>(revenueGroups[i].Id, false), categoryId);
					}

					ImportTransactions(context);

					stopWatch.Stop();
					UpdateStatus(
						string.Format(
						"Done in {0:00}:{1:00}:{2:00}.{3:00}",
												stopWatch.Elapsed.TotalHours,
												stopWatch.Elapsed.Minutes,
												stopWatch.Elapsed.Seconds,
												stopWatch.Elapsed.Milliseconds));
				}
			}
			catch (Exception exception)
			{
				stopWatch.Stop();
				UpdateStatus("Error");
				ShowError(exception);
			}

			DisableUI(true);
		}

		/// <summary>
		/// Enable or disable UI when long running operation is in progress.
		/// </summary>
		/// <param name="isEnabled"><c>True</c> will enable UI and <c>False</c> will disable.</param>
		private void DisableUI(bool isEnabled)
		{
			GenerateButton.IsEnabled = isEnabled;
			UsernameTextBox.IsEnabled = isEnabled;
			PasswordTextBox.IsEnabled = isEnabled;
			AccountTextBox.IsEnabled = isEnabled;
			StartButton.IsEnabled = isEnabled;
		}

		/// <summary>
		/// Load one expense data from old SQL database.
		/// </summary>
		/// <param name="context">EF context for database.</param>
		/// <param name="expenseMapper">EmitMapper that maps old expanse data object to new <see cref="TransactionData"/>.</param>
		/// <param name="skip">How many records to skip from the beginning of the whole "TblExpenseList"  table.</param>
		/// <returns>New transaction data instance filled with data from record in table "TblExpenseList" of the old SQL database.</returns>
		private static TransactionData GetExpense(
			ProfitAndExpenseEntities context,
		    ObjectsMapper<ExpenseList, TransactionData> expenseMapper,
			int skip)
		{
			return expenseMapper.Map(context.TblExpenseLists
			                         	.OrderBy(list => list.CreationDate)
			                         	.Skip(skip)
			                         	.First());
		}

		/// <summary>
		/// Load one revenue data from old SQL database.
		/// </summary>
		/// <param name="context">EF context for database.</param>
		/// <param name="revenueMapper">EmitMapper that maps old revenue data object to new <see cref="TransactionData"/>.</param>
		/// <param name="skip">How many records to skip from the beginning of the whole "TblRevenueList" table.</param>
		/// <returns>New transaction data instance filled with data from record in table "TblRevenueList" of the old SQL database.</returns>
		private static TransactionData GetRevenue(
			ProfitAndExpenseEntities context,
		    ObjectsMapper<RevenueList, TransactionData> revenueMapper,
			int skip)
		{
			return revenueMapper.Map(context.TblRevenueLists
			                         	.OrderBy(list => list.CreationDate)
			                         	.Skip(skip)
			                         	.First());
		}

		/// <summary>
		/// Save revenue data from old database as deposit to new database.
		/// </summary>
		/// <param name="context">EF context for old database.</param>
		/// <param name="revenuesCount">Total revenue count in old database.</param>
		/// <param name="revenueMapper">Revenue to transaction mapper.</param>
		/// <param name="revenueIndex">Current processed revenue index.</param>
		/// <param name="revenue">New loaded revenue data.</param>
		/// <returns>Updated current processed revenue index.</returns>
		private int SaveDeposit(
			ProfitAndExpenseEntities context,
			int revenuesCount,
		    ObjectsMapper<RevenueList, TransactionData> revenueMapper,
			int revenueIndex,
		    ref TransactionData revenue)
		{
			RevenueProgressTextBlock.Text = string.Format("{0} / {1}", revenueIndex + 1, revenuesCount);
			RevenueProgressBar.Value = revenueIndex + 1;
			RevenueProgressPercentTextBlock.Text = string.Format("{0:0.00}%", 100*RevenueProgressBar.Value/RevenueProgressBar.Maximum);
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

		/// <summary>
		/// Save expense data from old database as withdrawal to new database.
		/// </summary>
		/// <param name="context">EF context for old database.</param>
		/// <param name="expensesCount">Total expense count in old database.</param>
		/// <param name="expenseMapper">Expense to transaction mapper.</param>
		/// <param name="expenseIndex">Current processed expense index.</param>
		/// <param name="expense">New loaded expense data.</param>
		/// <returns>Updated current processed expense index.</returns>
		private int SaveWithdrawal(
			ProfitAndExpenseEntities context,
			int expensesCount,
		    ObjectsMapper<ExpenseList, TransactionData> expenseMapper,
			int expenseIndex,
		    ref TransactionData expense)
		{
			ExpenseProgressTextBlock.Text = string.Format("{0} / {1}", expenseIndex + 1, expensesCount);
			ExpenseProgressBar.Value = expenseIndex + 1;
			ExpenseProgressPercentTextBlock.Text = string.Format("{0:0.00}%", 100*ExpenseProgressBar.Value/ExpenseProgressBar.Maximum);
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

		/// <summary>
		/// Main method that import revenue and expense data from old database to new one.
		/// </summary>
		/// <param name="context">EF context for old database.</param>
		private void ImportTransactions(ProfitAndExpenseEntities context)
		{
			UpdateStatus("Estimating import data size...");
			int expensesCount = context.TblExpenseLists.Count();
			int revenuesCount = context.TblRevenueLists.Count();

			ExpenseProgressTextBlock.Text = "0 / " + expensesCount;
			RevenueProgressTextBlock.Text = "0 / " + revenuesCount;
			ExpenseProgressBar.Maximum = expensesCount;
			RevenueProgressBar.Maximum = revenuesCount;

			ObjectsMapper<ExpenseList, TransactionData> expenseMapper = ObjectMapperManager.DefaultInstance.GetMapper<ExpenseList, TransactionData>(
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

			ObjectsMapper<RevenueList, TransactionData> revenueMapper = ObjectMapperManager.DefaultInstance.GetMapper<RevenueList, TransactionData>(
					new DefaultMapConfig()
						.ConvertUsing<RevenueList, TransactionData>(rev => new TransactionData
						                                                   	{
						                                                   		Date = rev.CreationDate.ToUniversalTime(),
						                                                   		CategoryId = categoryMap[new KeyValuePair<int, bool>(rev.RevenueGroupId, false)],
						                                                   		Comment = string.Format("{0}\n{1}", rev.SourceName, rev.Memo),
						                                                   		Price = (decimal) rev.ExchangeRate,
						                                                   		Quantity = rev.AmountGRN/(decimal) rev.ExchangeRate,
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

		/// <summary>
		/// Display error message.
		/// </summary>
		/// <param name="exception">Exception to display.</param>
		private static void ShowError(Exception exception)
		{
			MessageBox.Show(exception.ToString(), "Import error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		/// <summary>
		/// Synchronously update status text on the screen.
		/// </summary>
		/// <param name="statusText">New status text.</param>
		private void UpdateStatus(string statusText)
		{
			StatusTextBlock.Text = statusText;
			Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
		}
	}
}