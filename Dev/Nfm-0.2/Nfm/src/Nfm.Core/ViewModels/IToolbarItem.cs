// <copyright file="IToolbarItem.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-28</date>
// </editor>
// <summary>General <see cref="IToolbar"/> element.</summary>

using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// General <see cref="IToolbar"/> element.
	/// </summary>
	public interface IToolbarItem : ICloneable
	{
		/// <summary>
		/// Gets or sets toolbar item icon.
		/// </summary>
		ImageSource Icon { get; set; }

		/// <summary>
		/// Gets or sets toolbar item text.
		/// </summary>
		string Text { get; set; }

		/// <summary>
		/// Gets or sets toolbar item tooltip.
		/// </summary>
		string Tooltip { get; set; }

		/// <summary>
		/// Gets all child toolbar items.
		/// </summary>
		ObservableCollection<IToolbarItem> Childs { get; }
	}
}