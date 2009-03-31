// <copyright file="StackContainer.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-10</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-10</date>
// </editor>
// <summary>Horizontally or vertically stacked <see cref="IPanel"/> container.</summary>

using System.Windows.Controls;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Horizontally or vertically stacked <see cref="IPanel"/> container.
	/// </summary>
	public class StackContainer : PanelContainerBase
	{
		/// <summary>
		/// Orientation for stacked panels.
		/// </summary>
		private Orientation orientation;

		/// <summary>
		/// Gets or sets orientation for stacked panels.
		/// </summary>
		public Orientation Orientation
		{
			get { return orientation; }
			set
			{
				OnPropertyChanging("Orientation");
				orientation = value;
				OnPropertyChanged("Orientation");
			}
		}
	}
}