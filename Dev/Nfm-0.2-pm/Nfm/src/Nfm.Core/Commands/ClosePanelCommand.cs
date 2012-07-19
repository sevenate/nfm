// <copyright file="ClosePanelCommand.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-01</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-01</date>
// </editor>
// <summary>Close panel command.</summary>

using System;
using Caliburn.Core.Metadata;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.Commands.Interfaces;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Close panel command.
	/// </summary>
	[Singleton(typeof(IClosePanelCommand))]
	public class ClosePanelCommand : IClosePanelCommand
	{
		/// <summary>
		/// Close specific panel.
		/// </summary>
		/// <param name="panel">Panel to close.</param>
		[Preview("CanExecute", AffectsTriggers = false)]
		public void Execute(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException("panel");
			}

			panel.Shutdown();
		}

		/// <summary>
		/// Check if the panel can be closed.
		/// </summary>
		/// <param name="panel">Panel to close.</param>
		/// <returns>True, if panel can be closed.</returns>
		public bool CanExecute(IPanel panel)
		{
			return panel != null;
		}
	}
}