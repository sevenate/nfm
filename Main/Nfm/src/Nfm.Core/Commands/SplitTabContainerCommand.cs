// <copyright file="SplitTabContainerCommand.cs" company="HD">
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
// <summary>
//  Split <see cref="TabContainer"/> horizontally or vertically
//  dependent of parent <see cref="StackContainer"/> orientation.
// </summary>

using System;
using System.Windows.Controls;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Split <see cref="TabContainer"/> horizontally or vertically to two <see cref="TabContainer"/>s.
	/// </summary>
	public class SplitTabContainerCommand
	{
		/// <summary>
		/// Change <see cref="TabContainer"/> parent panel to <see cref="StackContainer"/>
		/// and add to it current <see cref="TabContainer"/> and one it clone.
		/// </summary>
		/// <param name="panel">Tab container child panel.</param>
		[Preview("CanExecute", AffectsTriggers = false)]
		public void Execute(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException("panel");
			}

			var tabContainer = panel.Parent as TabContainer;

			if (tabContainer != null)
			{
				if (tabContainer.Parent is IPanelContainer)
				{
					var stackContainer = new StackContainer
					                     {
											Header = new PanelHeader
											{
												Text = "Stack Container"
											},
					                     	Orientation = FindOptimalStackContainerOrientation(tabContainer)
					                     	              ?? Orientation.Horizontal,
											IsSelected = true
					                     };

					var parentContainer = tabContainer.Parent as IPanelContainer;
					int tabContainerIndex = parentContainer.Childs.IndexOf(tabContainer);
					var newTabContainer = (TabContainer) tabContainer.Clone();

					parentContainer.Childs.Remove(tabContainer);

					stackContainer.Childs.Add(tabContainer);
					stackContainer.Childs.Add(newTabContainer);

					parentContainer.Childs.Insert(tabContainerIndex, stackContainer);
				}
			}
		}

		/// <summary>
		/// Check if the panel parent is <see cref="TabContainer"/>.
		/// </summary>
		/// <param name="panel">Any tab container child panel.</param>
		/// <returns>True, if panel's parent is <see cref="TabContainer"/>.</returns>
		public bool CanExecute(IPanel panel)
		{
			return panel != null && panel.Parent is TabContainer; // != null;
		}

		/// <summary>
		/// Find the nearest <see cref="StackContainer"/> up the panel's tree to determine
		/// optimal orientation property for the new stack container.
		/// </summary>
		/// <param name="panel">Any child panel to start searching.</param>
		/// <returns>Optimal <see cref="Orientation"/> or null if no any stack panel found up the tree.</returns>
		private static Orientation? FindOptimalStackContainerOrientation(IPanel panel)
		{
			if (panel == null)
			{
				// Root panel is reached, but no stack container found
				return null;
			}

			var stackContainer = panel.Parent as StackContainer;

			return stackContainer != null
			       	? stackContainer.Orientation == Orientation.Horizontal
			       	  	? Orientation.Vertical
			       	  	: Orientation.Horizontal
			       	: FindOptimalStackContainerOrientation(panel.Parent);
		}
	}
}