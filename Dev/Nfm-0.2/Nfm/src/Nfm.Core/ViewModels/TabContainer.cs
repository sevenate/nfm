// <copyright file="TabContainer.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-23</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-23</date>
// </editor>
// <summary>TabControl-based <see cref="IPanel"/> container.</summary>

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// TabControl-based <see cref="IPanel"/> container.
	/// </summary>
	public class TabContainer : PanelContainerBase
	{
		#region Binding Properties

		/// <summary>
		/// Operational toolbar.
		/// </summary>
		private IToolbar toolbar;

		/// <summary>
		/// Gets or sets orientation for stacked panels.
		/// </summary>
		public IToolbar Toolbar
		{
			get { return toolbar; }
			set
			{
				OnPropertyChanging("Toolbar");
				toolbar = value;
				OnPropertyChanged("Toolbar");
			}
		}

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="TabContainer"/> class.
		/// </summary>
		public TabContainer()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TabContainer"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="TabContainer"/> instance to copy data from.</param>
		protected TabContainer(TabContainer another)
			: base(another)
		{
			if (another.Toolbar != null)
			{
				Toolbar = (IToolbar) another.Toolbar.Clone();
			}
		}

		#endregion

		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public override object Clone()
		{
			return new TabContainer(this);
		}

		#endregion
	}
}