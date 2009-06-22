// <copyright file="SwapStackContainerOrientationCommand.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-10</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-10</date>
// </editor>
// <summary>Swap stack container orientation.</summary>

using System;
using System.Windows.Controls;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Swap stack container orientation.
	/// </summary>
	public class SwapStackContainerOrientationCommand
	{
		/// <summary>
		/// Swap panel's parent stack container orientation.
		/// </summary>
		/// <param name="panel">Any stack container child panel.</param>
		[Preview("CanExecute", AffectsTriggers = false)]
		public void Execute(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException("panel");
			}

			// Todo: consider using only 1 and 2 level up the panel's hierarchy
			// to search nearest StackContainer instead of seaching up the root.
			Swap(panel);
		}

		/// <summary>
		/// Check if the panel's parent is <see cref="IPanelContainer"/>.
		/// </summary>
		/// <param name="panel">Any stack container child panel.</param>
		/// <returns>True, if panel's parent is <see cref="IPanelContainer"/>.</returns>
		public bool CanExecute(IPanel panel)
		{
			return panel != null && panel.Parent is IPanelContainer; //is StackContainer;
		}

		/// <summary>
		/// Swap the <see cref="StackContainer.Orientation"/> property of the nearest parent <see cref="IPanelContainer"/>.
		/// </summary>
		/// <param name="panel">
		/// Any child panel to start recursively searching
		/// up the root of panel's tree to find nearest <see cref="StackContainer"/>.
		/// </param>
		private static void Swap(IPanel panel)
		{
			if (panel == null)
			{
				// Root panel is reached, exit from recurse
				return;
			}

			var stackContainer = panel.Parent as StackContainer;

			if (stackContainer != null)
			{
				stackContainer.Orientation = stackContainer.Orientation == Orientation.Horizontal
				                             	? Orientation.Vertical
				                             	: Orientation.Horizontal;
			}
			else
			{
				// Recursively move up the panel's tree
				Swap(panel.Parent);
			}
		}
	}
}