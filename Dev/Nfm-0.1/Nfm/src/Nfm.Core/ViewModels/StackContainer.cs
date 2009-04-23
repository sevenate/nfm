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

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="StackContainer"/> class.
		/// </summary>
		public StackContainer()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="StackContainer"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="StackContainer"/> instance to copy data from.</param>
		protected StackContainer(StackContainer another)
			: base(another)
		{
			orientation = another.Orientation;
		}

		#endregion

		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public override object Clone()
		{
			return new StackContainer(this);
		}

		#endregion
	}
}