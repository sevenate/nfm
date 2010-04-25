// <copyright file="TestItem.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-02</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-02</date>
// </editor>
// <summary>Money tracker module.</summary>

using System;
using System.Collections.Generic;
using Caliburn.PresentationFramework.Filters;
using Caliburn.PresentationFramework.RoutedMessaging;
using Caliburn.ShellFramework.Results;
using Fab.Client.Main.ViewModels;
using Fab.Client.Models;

namespace Fab.Client.Main
{
	/// <summary>
	/// Money tracker module.
	/// </summary>
	public class Part : IPart
	{
		#region Implementation of IPart

		public string DisplayName
		{
			get { return "Money"; }
		}

		[Rescue("OpenFailed")]
		public IEnumerable<IResult> Enter()
		{
			yield return Show.Child(new MoneyTrackerViewModel()).In<IShell>();
		}

		public bool OpenFailed(Exception exception)
		{
			Show.NotBusy().Execute();
			Show.MessageBox("There was a problem opeding the module:" + exception, "Error").Execute();

			return true;
		}

		#endregion
	}
}