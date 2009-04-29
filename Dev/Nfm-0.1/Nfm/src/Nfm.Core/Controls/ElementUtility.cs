// <copyright file="ElementUtility.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-29</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-29</date>
// </editor>
// <summary>Element helper class.</summary>

using System;
using System.Windows;

namespace Nfm.Core.Controls
{
	/// <summary>
	/// Element helper class.
	/// </summary>
	public static class ElementUtility
	{
		/// <summary>
		/// Adds a routed event handler for a specified routed event,
		/// adding the handler to the handler collection on the current element.
		/// </summary>
		/// <param name="d">The <see cref="UIElement"/> or <see cref="ContentElement"/> or <see cref="UIElement3D"/>.</param>
		/// <param name="routedEvent">An identifier for the routed event to be handled.</param>
		/// <param name="handler">A reference to the handler implementation.</param>
		public static void AddHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
		{
			if (d is UIElement)
			{
				var element = (UIElement) d;
				element.AddHandler(routedEvent, handler);
			}
			else if (d is ContentElement)
			{
				var element = (ContentElement) d;
				element.AddHandler(routedEvent, handler);
			}
			else if (d is UIElement3D)
			{
				var element = (UIElement3D) d;
				element.AddHandler(routedEvent, handler);
			}
		}

		/// <summary>
		/// Removes the specified routed event handler from this element.
		/// </summary>
		/// <param name="d">The <see cref="UIElement"/> or <see cref="ContentElement"/> or <see cref="UIElement3D"/>.</param>
		/// <param name="routedEvent">An identifier for the routed event to be handled.</param>
		/// <param name="handler">A reference to the handler implementation.</param>
		public static void RemoveHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
		{
			if (d is UIElement)
			{
				var element = (UIElement) d;
				element.RemoveHandler(routedEvent, handler);
			}
			else if (d is ContentElement)
			{
				var element = (ContentElement) d;
				element.RemoveHandler(routedEvent, handler);
			}
			else if (d is UIElement3D)
			{
				var element = (UIElement3D) d;
				element.RemoveHandler(routedEvent, handler);
			}
		}
	}
}