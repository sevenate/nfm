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
// <summary>Change node in panel to he's parent node command.</summary>

using Caliburn.Actions.Filters;
using Nfm.Core.ViewModels.FileSystem;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Change node in panel to he's parent node command.
	/// </summary>
	public class NavigateToParentNodeCommand
	{
		/// <summary>
		/// Change node in panel to he's parent node.
		/// </summary>
		/// <param name="panel">Specific panel with node.</param>
		[Preview("CanExecute")]
		public void Execute(FileSystemEntityNodeVM panel)
		{
			if (panel != null)
			{
				panel.NavigateToParent();
			}
		}

		/// <summary>
		/// Check if the current node in the panel have parent to make sense in navigation.
		/// </summary>
		/// <param name="panel">Specific panel with node.</param>
		/// <returns>True, if the node have parent node.</returns>
		public bool CanExecute(FileSystemEntityNodeVM panel)
		{
			// TODO: implement check for parent node availability
			return true;
		}
	}
}