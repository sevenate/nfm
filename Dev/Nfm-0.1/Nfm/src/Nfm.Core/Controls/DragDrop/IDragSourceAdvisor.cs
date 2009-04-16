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
// <summary>Provide common interface to draggable source item.</summary>

using System.Windows;

namespace Nfm.Core.Controls.DragDrop
{
	/// <summary>
	/// Provide common interface to draggable source item.
	/// </summary>
	public interface IDragSourceAdvisor
	{
		/// <summary>
		/// Gets or sets draggable source item element.
		/// </summary>
		UIElement SourceUI { get; set; }

		/// <summary>
		/// Gets the effects of a drag-and-drop operation.
		/// </summary>
		DragDropEffects SupportedEffects { get; }

		/// <summary>
		/// Fetch data object from drag element.
		/// </summary>
		/// <param name="dragElement">Drag element.</param>
		/// <returns>Format-independent data transfer.</returns>
		DataObject GetDataObject(UIElement dragElement);

		/// <summary>
		/// Get additional visual element,
		/// placed over draggable source item element to make drag-and-drop operation distinct.
		/// </summary>
		/// <param name="dragElement">Main drag element.</param>
		/// <returns>Additional visual feedback element.</returns>
		UIElement GetVisualFeedback(UIElement dragElement);
		
		/// <summary>
		/// Reject transfered data object on source side.
		/// </summary>
		/// <param name="dragElement">Drag element.</param>
		/// <param name="finalEffects">Final effects of a drag-and-drop operation.</param>
		void OnDropStarted(UIElement dragElement, DragDropEffects finalEffects);
		
		/// <summary>
		/// Check, if draggable element has specific data to drag.
		/// </summary>
		/// <param name="element">Specific draggable element.</param>
		/// <returns>"True" if element could be drag; otherwise, "False".</returns>
		bool IsDraggable(UIElement element);
	}
}