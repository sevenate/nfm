// <copyright file="WrapPanelWithTabContainerCommand.cs" company="HD">
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
// <summary>Wrap ane single panel with <see cref="TabContainer"/> .</summary>

using System;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Wrap <see cref="IPanel"/> with <see cref="TabContainer"/> as parent.
	/// </summary>
	public class WrapPanelWithTabContainerCommand
	{
		/// <summary>
		/// Wrap <see cref="IPanel"/> with <see cref="TabContainer"/>.
		/// </summary>
		/// <param name="panel">Any panel to wrap.</param>
		[Preview("CanExecute", AffectsTriggers = false)]
		public void Execute(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException("panel");
			}

			if (panel.Parent is IPanelContainer)
			{
				var parentContainer = panel.Parent as IPanelContainer;
				var panelIndex = parentContainer.Childs.IndexOf(panel);
				parentContainer.Childs.Remove(panel);

				var tabContainer = new TabContainer
				{
					Header = new PanelHeader
					         {
								 Text = "Tab Container"
					         },
					IsSelected = true
				};

				tabContainer.Childs.Add(panel);
				parentContainer.Childs.Insert(panelIndex, tabContainer);
			}
		}

		/// <summary>
		/// Check if the panel parent is stack container.
		/// </summary>
		/// <param name="panel">Any stack container child panel.</param>
		/// <returns>True, if panel's parent is stack container.</returns>
		public bool CanExecute(IPanel panel)
		{
			return panel != null && panel.Parent is IPanelContainer;	// TabContainer;	// != null;
		}
	}
}