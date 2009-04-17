// <copyright file="IDropTargetAdvisor.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Pavan Podila">
//	<url>http://pavanpodila.spaces.live.com/blog/cns!9C9E888164859398!199.entry</url>
// 	<date>2006-11-20</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-16</date>
// </editor>
// <summary>Provide common interface to target drop area.</summary>

using System.Windows;

namespace Nfm.Core.Controls.DragDrop
{
	/// <summary>
	/// Provide common interface to target drop area.
	/// </summary>
	public interface IDropTargetAdvisor
	{
		/// <summary>
		/// Gets or sets target drop area element.
		/// </summary>
		UIElement TargetUI { get; set; }

		/// <summary>
		/// Checks to see whether the data is available and valid.
		/// </summary>
		/// <param name="obj">Format-independent mechanism for transferring data.</param>
		/// <returns>"True" if the data is in and valid; otherwise, "False".</returns>
		bool IsValidDataObject(IDataObject obj);

		/// <summary>
		/// Accept transfered data object on target side.
		/// </summary>
		/// <param name="obj">Format-independent mechanism for transferring data.</param>
		/// <param name="finalEffects">Final effects of a drag-and-drop operation.</param>
		/// <param name="dropPoint">Specific coordinates where object was dropped.</param>
		void OnDropAccepted(IDataObject obj, DragDropEffects finalEffects, Point dropPoint);
	}
}