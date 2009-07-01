// <copyright file="App.xaml.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-08</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-08</date>
// </editor>
// <summary>WPF Application.</summary>

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Nfm.Core.Configuration;
using Nfm.Core.ViewModels;
using Nfm.Core.Views;
using Nfm.Loader.Legacy;

namespace Nfm.Loader
{
	/// <summary>
	/// WPF Application.
	/// </summary>
	public partial class App
	{
		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="App" /> class.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// More than one instance of the <see cref="T:System.Windows.Application" /> class is created per <see cref="T:System.AppDomain" />.
		/// </exception>
		public App()
		{
			ConfigManager.InitializeCaliburn();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Open new application window.
		/// </summary>
		/// <param name="commandLine">Command line arguments.</param>
		public void RunNextInstance(ReadOnlyCollection<string> commandLine)
		{
			if (Windows.Count == 0)
			{
				//TODO: Add specific application initialization logic here (+handle command line args).

				ShowNewWindow();
				return;
			}

			ShowNewWindow();
		}

		#endregion

		#region Overrides of Application

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
		/// Occurs when the System.Windows.Application.Run() method of the System.Windows.Application object is called.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			RunNextInstance(new ReadOnlyCollection<string>(e.Args));
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
		/// Occurs just before an application shuts down, and cannot be canceled.
		/// </summary>
		/// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.</param>
		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			//TODO: Add specific application finalization logic here.
			;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.SessionEnding" /> event.
		/// Occurs when the user ends the Windows session by logging off or shutting down the operating system.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.SessionEndingCancelEventArgs" /> that contains the event data.</param>
		protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
		{
			base.OnSessionEnding(e);

			//TODO: Handle OS shutdown here.
			;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.Activated" /> event.
		/// Occurs when an application becomes the foreground application.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);

			//TODO: Add specific activation logic here.
			;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.Deactivated" /> event.
		/// Occurs when an application stops being the foreground application.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnDeactivated(EventArgs e)
		{
			base.OnDeactivated(e);

			//TODO: Add specific deactivation logic here.
			;
		}

		#endregion

		#region Private Implementation

		/// <summary>
		/// Occurs when an exception is thrown by an application but not handled.
		/// </summary>
		/// <param name="sender">Exception sender.</param>
		/// <param name="e">Exception data.</param>
		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			//TODO: Log exception here.

#if !DEBUG
			e.Handled =
				MessageBox.Show(
					e.Exception.ToString(), "Unhandled exception occur. Continue?", MessageBoxButton.YesNo, MessageBoxImage.Error)
				== MessageBoxResult.Yes;
#endif
		}

		/// <summary>
		/// Show new application window.
		/// </summary>
		private void ShowNewWindow()
		{
			IPanel rootLayoutPanel = ConfigManager.GetLayout();

			var newWindow = new MainWindow
			                {
			                	DataContext = rootLayoutPanel
			                };

			rootLayoutPanel.Closed += (sender, e) => newWindow.Close();

			// Select one of remained opened application windows as "main" window.
			newWindow.Closed += delegate
			                    {
			                    	if (MainWindow == null && Windows.Count > 0)
			                    	{
			                    		MainWindow = Windows[0];
			                    		// ReSharper disable PossibleNullReferenceException
			                    		MainWindow.Show();
			                    		// ReSharper restore PossibleNullReferenceException
			                    		MainWindow.Activate();
			                    	}
			                    };

			if (MainWindow == null)
			{
				MainWindow = newWindow;
			}

			// Center window on primary display monitor.
			newWindow.Width = SystemParameters.WorkArea.Width*2/3;
			newWindow.Height = SystemParameters.WorkArea.Height*2/3;
			newWindow.MinWidth = newWindow.Width/3;
			newWindow.MinHeight = newWindow.Height/3;
			newWindow.Left = (SystemParameters.WorkArea.Width - newWindow.Width)/2;
			newWindow.Top = (SystemParameters.WorkArea.Height - newWindow.Height)/2;

			// Workaround: recalculate device independent working area
			// to allow maximize window correctly.
			newWindow.SourceInitialized += (sender, e) => MultimonitorMaximizer.RecalculateMaxSize((Window) sender);

			newWindow.Show();
			newWindow.Activate();
		}

		#endregion
	}
}