// <copyright file="IPanelHeader.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-16</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-16</date>
// </editor>
// <summary>Represent general <see cref="IPanel"/> header.</summary>

using System;
using System.Windows.Media;
using Caliburn.PresentationFramework.ApplicationModel;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Represent general <see cref="IPanel"/> header.
	/// </summary>
	public interface IPanelHeader : IPresenter, ICloneable
	{
		/// <summary>
		/// Gets or sets header icon.
		/// </summary>
		ImageSource Icon { get; set; }

		/// <summary>
		/// Gets or sets header text.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether header is visible.
		/// </summary>
		bool IsVisible { get; set; }
	}
}