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
using System.Windows.Controls;
using System.Windows.Threading;
using Caliburn.Actions;
using Caliburn.Commands;
using Caliburn.Core;
using Caliburn.RoutedUIMessaging;
using Nfm.Core.Models.FileSystem;
using Nfm.Core.ViewModels;
using Nfm.Core.ViewModels.FileSystem;
using Nfm.Loader.Legacy;
using Nfm.Loader.Views;

namespace Nfm.Loader
{
	/// <summary>
	/// WPF Application.
	/// </summary>
	public partial class App
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Windows.Application" /> class.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// More than one instance of the <see cref="T:System.Windows.Application" /> class is created per <see cref="T:System.AppDomain" />.
		/// </exception>
		public App()
		{
			//Note: Using an external container via adapter.  The usage pattern is the same for all supported containers.

//			var adapter = new StructureMapAdapter();

			CaliburnApplication
				.ConfigureCore( /*adapter*/)
				.WithRoutedUIMessaging()
				.WithActions()
				.WithCommands()
				.StartApplication();

//			adapter.Register(typeof(INode), typeof(FileSystemEntityNode));

			//Note: Retrieve one of Caliburn's services.

//			var controller = adapter.Resolve<IRoutedMessageController>();

			//Note: Customize the default behavior of button elements.

//			controller.SetupDefaults(
//				new GenericInteractionDefaults<Button>(
//					"MouseEnter",
//					(b, v) => b.DataContext = v,
//					b => b.DataContext
//					)
//				);
		}

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

		/// <summary>
		/// Show new application window.
		/// </summary>
		public void ShowNewWindow()
		{
			#region Work Left Tab Container

			var driveCPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"C:\",
				});
			driveCPanel.RefreshDetails();
			driveCPanel.RefreshChilds();

			var driveDPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"D:\",
				});
			driveDPanel.RefreshDetails();
			driveDPanel.RefreshChilds();

			var workLeftTabContainer = new TabContainer
			{
				Header = "Left Tab Container"
			};
			workLeftTabContainer.Childs.Add(driveDPanel);
			workLeftTabContainer.Childs.Add(driveCPanel);

			#endregion

			#region Work Middle Tab

			var workMiddlePanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"D:\Downloads",
				});
			workMiddlePanel.RefreshDetails();
			workMiddlePanel.RefreshChilds();

			var workMiddleTabContainer = new TabContainer
			{
				Header = "Middle Tab Container"
			};
			workMiddleTabContainer.Childs.Add(workMiddlePanel);

			#endregion

			#region Work Right Vertical Stack Container

			var workRightPanel1 = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"D:\Games",
				});
			workRightPanel1.RefreshDetails();
			workRightPanel1.RefreshChilds();

			var workRightPanel2 = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"D:\Music",
				});
			workRightPanel2.RefreshDetails();
			workRightPanel2.RefreshChilds();


			var workRightPanel1TabContainer = new TabContainer
			{
				Header = "Work Right Top Tab Container"
			};
			workRightPanel1TabContainer.Childs.Add(workRightPanel1);

			var workRightPanel2TabContainer = new TabContainer
			{
				Header = "Work Right Bottom Tab Container"
			};
			workRightPanel2TabContainer.Childs.Add(workRightPanel2);


			var workRightStackContainer = new StackContainer
			{
				Header = "Right Stack Container",
				Orientation = Orientation.Vertical
			};
//			workRightStackContainer.Childs.Add(workRightPanel1);
//			workRightStackContainer.Childs.Add(workRightPanel2);
			workRightStackContainer.Childs.Add(workRightPanel1TabContainer);
			workRightStackContainer.Childs.Add(workRightPanel2TabContainer);

			#endregion

			#region Work Horizontal Stack Container

			var workStackContainer = new StackContainer
			{
				Header = "Work Stack Container"
			};
			workStackContainer.Childs.Add(workLeftTabContainer);
//			workStackContainer.Childs.Add(workMiddlePanel);
			workStackContainer.Childs.Add(workMiddleTabContainer);
			workStackContainer.Childs.Add(workRightStackContainer);

			#endregion

			#region Enter Panel

			var enterPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"D:\Images",
				});
			enterPanel.RefreshDetails();
			enterPanel.RefreshChilds();

			#endregion

			#region Top Tab Container

			var topGamesPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"D:\Games",
				});
			topGamesPanel.RefreshDetails();
			topGamesPanel.RefreshChilds();

			var topMusicPanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"D:\Music",
				});
			topMusicPanel.RefreshDetails();
			topMusicPanel.RefreshChilds();

			var subTabContainer1 = new TabContainer
			{
				Header = "Enter Container"
			};
			subTabContainer1.Childs.Add(topGamesPanel);
			subTabContainer1.Childs.Add(topMusicPanel);
			subTabContainer1.SelectedTab = topMusicPanel;

			var topDriveCpanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"C:\",
				});
			topDriveCpanel.RefreshDetails();
			topDriveCpanel.RefreshChilds();

			var topDriveDpanel = new FileSystemEntityNodeVM(
				new FileSystemEntityNode
				{
					AbsoluteName = @"D:\",
				});
			topDriveDpanel.RefreshDetails();
			topDriveDpanel.RefreshChilds();

			var subTabContainer2 = new TabContainer
			{
				Header = "DisksContainer"
			};

			subTabContainer2.Childs.Add(topDriveCpanel);
			subTabContainer2.Childs.Add(topDriveDpanel);
			subTabContainer2.SelectedTab = topDriveDpanel;

			var topTabContainer = new TabContainer
			{
				Header = "Top Tab Container"
			};
			topTabContainer.Childs.Add(subTabContainer1);
			topTabContainer.Childs.Add(subTabContainer2);
			topTabContainer.SelectedTab = subTabContainer1;

			#endregion

			#region Main Tab Container

			var mainTabContainer = new TabContainer
			{
				Header = "Main Window"
			};
			mainTabContainer.Childs.Add(workStackContainer);
			mainTabContainer.Childs.Add(enterPanel);
			mainTabContainer.Childs.Add(topTabContainer);
			mainTabContainer.SelectedTab = enterPanel;

			#endregion

			var newWindow = new MainWindow
			{
				DataContext = mainTabContainer //workLeftTabContainer
//				Content = mainTabContainer //workLeftTabContainer
			};

			mainTabContainer.Closed += (sender, e) => newWindow.Close();

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
	}
}