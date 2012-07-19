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
// <summary>NFM Application.</summary>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Core;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.ApplicationModel;
using Microsoft.Practices.ServiceLocation;
using Nfm.Core.Configuration;
using Nfm.Core.Models;
using Nfm.Core.Presenters.Interfaces;
using Nfm.Core.ViewModels;
using Nfm.Core.Views;
using Nfm.Loader.Legacy;

namespace Nfm.Loader
{
	/// <summary>
	/// NFM Application.
	/// </summary>
	public partial class App
	{
		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="App" /> class.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// More than one instance of the <see cref="T:System.Windows.Application" />
		/// class is created per <see cref="T:System.AppDomain" />.
		/// </exception>
		public App()
		{
			InitializeComponent();
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
			//base.OnExit(e);

			//TODO: Add specific application finalization logic here.
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.SessionEnding" /> event.
		/// Occurs when the user ends the Windows session by logging off or shutting down the operating system.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.SessionEndingCancelEventArgs" /> that contains the event data.</param>
		protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
		{
			//base.OnSessionEnding(e);

			//TODO: Handle OS shutdown here.
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.Activated" /> event.
		/// Occurs when an application becomes the foreground application.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnActivated(System.EventArgs e)
		{
			//base.OnActivated(e);

			//TODO: Add specific activation logic here.
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Application.Deactivated" /> event.
		/// Occurs when an application stops being the foreground application.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnDeactivated(System.EventArgs e)
		{
			//base.OnDeactivated(e);

			//TODO: Add specific deactivation logic here.
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
/*
			IPanel rootLayoutPanel = ConfigManager.GetLayout();

			var newWindow = new ShellView
			                {
			                	DataContext = rootLayoutPanel
			                };

			rootLayoutPanel.WasShutdown += (sender, e) => newWindow.Close();

			// Select one of remained opened application windows as "main" window.
			newWindow.Closed += delegate
			                    {
			                    	if (ShellView == null && Windows.Count > 0)
			                    	{
			                    		ShellView = Windows[0];
			                    		// ReSharper disable PossibleNullReferenceException
			                    		ShellView.Show();
			                    		// ReSharper restore PossibleNullReferenceException
			                    		ShellView.Activate();
			                    	}
			                    };

			if (ShellView == null)
			{
				ShellView = newWindow;
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
*/
		}

		#endregion

		#region Overrides of CaliburnApplication

		/// <summary>
		/// Creates the container.
		/// </summary>
		/// <returns/>
		protected override IServiceLocator CreateContainer()
		{
			// Todo: consider using "Autofac" or "StructureMap" IoC frameworks.
			// Todo: write adapter for "Autofac" IoC and send it to Caliburn dev team.
			return new SimpleContainer();
		}

		/// <summary>
		/// Selects the assemblies which Caliburn will be able to inspect for components, views, etc.
		/// </summary>
		/// <returns/>
		protected override Assembly[] SelectAssemblies()
		{
			// TODO: Make core assembly file name auto determined
			return new[] { Assembly.LoadFrom("Nfm.Core.dll") };
		}

		/// <summary>
		/// Creates the root application model.
		/// </summary>
		/// <returns/>
		protected override object CreateRootModel()
		{
			return Container.GetInstance<IShellPresenter>();
		}

		/// <summary>
		/// Executes the shutdown model.
		/// </summary>
		/// <param name="subordinate">The subordinate.</param><param name="completed">The completed.</param>
		protected override void ExecuteShutdownModel(ISubordinate subordinate, Action completed)
		{
			var shell = Container.GetInstance<IShellPresenter>();
			var dialogPresenter = Container.GetInstance<IQuestionPresenter>();
			var question = (Question)subordinate;

			dialogPresenter.Setup(question, completed);
			shell.ShowDialog(dialogPresenter);
		}

		/// <summary>
		/// Called when shutdown attempted.
		/// </summary>
		/// <param name="rootModel">The root model.</param><param name="e">The <see cref="T:System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
//		protected override void OnShutdownAttempted(IPresenter rootModel, CancelEventArgs e)
//		{
//			;
//		}

		#endregion
	}
}