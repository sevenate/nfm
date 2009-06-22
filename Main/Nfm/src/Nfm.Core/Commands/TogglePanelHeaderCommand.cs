// <copyright file="TogglePanelHeaderCommand.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-16</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-16</date>
// </editor>
// <summary>Hide or show <see cref="IPanel.Header" /> command.</summary>

using System;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Hide or show <see cref="IPanel.Header" /> command.
	/// </summary>
	public class TogglePanelHeaderCommand
	{
		/// <summary>
		/// Toggle header visibility of specific panel.
		/// </summary>
		/// <param name="panel">Specific panel.</param>
		[Preview("CanExecute")]
		public void Execute(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException("panel");
			}

//			panel.Header.IsVisible = !panel.Header.IsVisible;
			ToggleHeaderVisibility(panel);
		}

		/// <summary>
		/// Check if the panel has header at all.
		/// </summary>
		/// <param name="panel">Specific panel.</param>
		/// <returns>True, if panel has header.</returns>
		public bool CanExecute(IPanel panel)
		{
			return panel != null;// && panel.Header != null;
		}

		/// <summary>
		/// Toggle the <see cref="IPanelHeader.IsVisible"/> property of the nearest parent <see cref="TabContainer"/>.
		/// </summary>
		/// <param name="panel">
		/// Any child panel to start recursively searching
		/// up the root of panel's tree to find nearest <see cref="TabContainer"/>.
		/// </param>
		private static void ToggleHeaderVisibility(IPanel panel)
		{
			if (panel == null)
			{
				// Root panel is reached, exit from recurse
				return;
			}

			var tabContainer = panel as TabContainer;

			if (tabContainer != null && tabContainer.Header != null)
			{
				tabContainer.Header.IsVisible = !tabContainer.Header.IsVisible;
			}
			else
			{
				// Recursively move up the panel's tree
				ToggleHeaderVisibility(panel.Parent);
			}
		}
	}
}