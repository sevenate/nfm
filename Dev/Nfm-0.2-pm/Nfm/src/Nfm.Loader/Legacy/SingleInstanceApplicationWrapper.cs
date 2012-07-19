// <copyright file="SingleInstanceApplicationWrapper.cs" company="HD">
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
// <summary>Single-instance WPF application wrapper.</summary>

using Microsoft.VisualBasic.ApplicationServices;

namespace Nfm.Loader.Legacy
{
	/// <summary>
	/// Single-instance WPF application wrapper.
	/// </summary>
	public class SingleInstanceApplicationWrapper : WindowsFormsApplicationBase
	{
		/// <summary>
		/// Single-instance WPF application.
		/// </summary>
		private App app;

		/// <summary>
		/// Initializes a new instance of the <see cref="SingleInstanceApplicationWrapper" /> class.
		/// </summary>
		public SingleInstanceApplicationWrapper()
		{
			IsSingleInstance = true;
		}

		#region Overrides of WindowsFormsApplicationBase

		/// <summary>
		/// Run when the application starts.
		/// </summary>
		/// <param name="eventArgs">Contains the command-line arguments of the application and indicates whether the application startup should be canceled.</param>
		/// <returns>Indicates if the application should continue starting up.</returns>
		protected override bool OnStartup(StartupEventArgs eventArgs)
		{
			app = new App();
//			app.InitializeComponent();
			app.Run();
			return false;
		}

		/// <summary>
		/// Run when a subsequent instance of a single-instance application starts.
		/// </summary>
		/// <param name="eventArgs">Contains the command-line arguments of the subsequent application instance and indicates whether the first application instance should be brought to the foreground upon exiting the exception handler.</param>
		protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
		{
			app.RunNextInstance(eventArgs.CommandLine);
		}

		#endregion
	}
}