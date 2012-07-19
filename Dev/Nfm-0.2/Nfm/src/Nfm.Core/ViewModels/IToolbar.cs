// <copyright file="IToolbar.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-26</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-26</date>
// </editor>
// <summary>General toolbar interface.</summary>

using System;
using System.Collections.ObjectModel;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// General toolbar interface.
	/// </summary>
	public interface IToolbar : ICloneable
	{
		/// <summary>
		/// Gets all child toolbar items.
		/// </summary>
		ObservableCollection<IToolbarItem> Childs { get; }
	}
}