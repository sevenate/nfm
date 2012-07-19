// <copyright file="DublicateSelectedPanelCommand.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-10</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-10</date>
// </editor>
// <summary>Dublicate selected panel command.</summary>

using System;
using Caliburn.Core.Metadata;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.Commands.Interfaces;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Dublicate selected panel command.
	/// </summary>
	[Singleton(typeof(IDublicateSelectedPanelCommand))]
	public class DublicateSelectedPanelCommand
	{
		/// <summary>
		/// Dublicate panel in the parent container.
		/// </summary>
		/// <param name="panel">Panel to dublicate.</param>
		[Preview("CanExecute", AffectsTriggers = false)]
		public void Execute(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException("panel");
			}

			var container = panel.Parent as IPanelContainer;

			if (container != null)
			{
				// Insert after current panel
/*
				int targetIndex = container.Presenters.IndexOf(panel);
				container.Presenters.Insert(targetIndex + 1, (IPanel)panel.Clone());
*/
			}
		}

		/// <summary>
		/// Check if the panel can be dublicated.
		/// </summary>
		/// <param name="panel">Panel to dublicate.</param>
		/// <returns>True, if at panel can be dublicated in the parent container.</returns>
		public bool CanExecute(IPanel panel)
		{
			return panel != null && panel.Parent is IPanelContainer;
		}
	}
}