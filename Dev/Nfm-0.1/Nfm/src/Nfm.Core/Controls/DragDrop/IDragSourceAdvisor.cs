// <copyright file="IDragSourceAdvisor.cs" company="HD">
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
// <summary>Provide common interface to drag item.</summary>

using System.Windows;

namespace Nfm.Core.Controls.DragDrop
{
	/// <summary>
	/// Provide common interface to drag item.
	/// </summary>
	public interface IDragSourceAdvisor
	{
		/// <summary>
		/// Gets the effects of a drag-and-drop operation.
		/// </summary>
		DragDropEffects SupportedEffects { get; }

		/// <summary>
		/// Fetch data object from drag element.
		/// </summary>
		/// <param name="dragElement">Drag element.</param>
		/// <returns>Format-independent data transfer.</returns>
		IDataObject GetDataObject(UIElement dragElement);

		/// <summary>
		/// Get additional visual element,
		/// placed over drag item element to make drag-and-drop operation distinct.
		/// </summary>
		/// <param name="dragElement">Drag element.</param>
		/// <returns>Additional visual feedback element.</returns>
		UIElement GetVisualFeedback(UIElement dragElement);

		/// <summary>
		/// Check, if element has specific data to drag.
		/// </summary>
		/// <param name="element">Candidate to drag.</param>
		/// <returns>"True" if element could be drag; otherwise, "False".</returns>
		bool IsDraggable(UIElement element);

		/// <summary>
		/// Confirm acceptance of successfully drop transfered data object to target area on source side.
		/// </summary>
		/// <param name="dragElement">Drag element.</param>
		/// <param name="finalEffects">Final effects of a drag-and-drop operation.</param>
		void OnDropConfirmed(UIElement dragElement, DragDropEffects finalEffects);
	}
}