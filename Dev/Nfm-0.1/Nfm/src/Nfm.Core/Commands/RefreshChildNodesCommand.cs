// <copyright file="RefreshChildNodesCommand.cs" company="HD">
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
// <summary>Refresh child nodes command.</summary>

using Caliburn.Actions.Filters;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Refresh child nodes command.
	/// </summary>
	public class RefreshChildNodesCommand
	{
		/// <summary>
		/// Refresh child nodes collection in specific parent node.
		/// </summary>
		/// <param name="node">Specific parent node.</param>
		[Preview("CanExecute")]
		public void Execute(IViewModel node)
		{
			node.Refresh();
		}

		/// <summary>
		/// Check if the child nodes collection can be refreshed.
		/// </summary>
		/// <param name="node">Specific parent node.</param>
		/// <returns>True, if the child nodes collection can be refreshed.</returns>
		public bool CanExecute(IViewModel node)
		{
			// TODO: implement check for child refreshability sence
			return node != null;
		}
	}
}