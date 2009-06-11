// <copyright file="IPanel.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-29</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-29</date>
// </editor>
// <summary>Represent general info panel.</summary>

using System;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Represent general info panel.
	/// </summary>
	public interface IPanel : ICloneable
	{
		/// <summary>
		/// Gets panel header: string text or complex content.
		/// </summary>
		object Header { get; }

		/// <summary>
		/// Gets a value indicating whether a panel can be closed.
		/// </summary>
		bool CanClose { get; }

		/// <summary>
		/// Gets or sets a value indicating whether a panel is selected.
		/// </summary>
		bool IsSelected { get; set; }

		/// <summary>
		/// Gets or sets parent <see cref="IPanel"/>.
		/// </summary>
		IPanel Parent { get; set; }

		/// <summary>
		/// Request close action for panel.
		/// </summary>
		void RequestClose();

		/// <summary>
		/// Rased before panel is closed.
		/// </summary>
		event EventHandler<EventArgs> Closing;

		/// <summary>
		/// Rased after panel is closed.
		/// </summary>
		event EventHandler<EventArgs> Closed;

		/// <summary>
		/// Rased before panel is selected.
		/// </summary>
		event Action<IPanel> SelectionChanging;

		/// <summary>
		/// Rased after panel is selected.
		/// </summary>
		event Action<IPanel> SelectionChanged;
	}
}