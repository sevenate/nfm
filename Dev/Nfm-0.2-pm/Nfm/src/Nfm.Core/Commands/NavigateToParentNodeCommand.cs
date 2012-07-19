// <copyright file="NavigateToParentNodeCommand.cs" company="HD">
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
// <summary>Change node in panel to parent node command.</summary>

using Caliburn.Core.Metadata;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.Commands.Interfaces;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Change node in panel to parent node command.
	/// </summary>
	[Singleton(typeof(INavigateToParentNodeCommand))]
	public class NavigateToParentNodeCommand : INavigateToParentNodeCommand
	{
		/// <summary>
		/// Change node in panel to parent node.
		/// </summary>
		/// <param name="panel">Specific panel with node.</param>
		[Preview("CanExecute", AffectsTriggers = false)]
		public void Execute(IPanelContent panel)
		{
			// Todo: consider switching to single interface (IPanelContent or IViewModel)
			panel.Host.PanelContent = ((IViewModel) panel).NavigateOut();
		}

		/// <summary>
		/// Check if the current node in the panel have parent to make sense in navigation.
		/// </summary>
		/// <param name="panel">Specific panel with node.</param>
		/// <returns>True, if the node have parent node.</returns>
		public bool CanExecute(IPanelContent panel)
		{
			return panel != null && ((IViewModel) panel).SupportNavigateOut();
		}
	}
}