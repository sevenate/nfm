// <copyright file="PanelBase.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-22</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-22</date>
// </editor>
// <summary>Base <see cref="IPanel"/> implementation.</summary>

using System.Diagnostics;
using Caliburn.PresentationFramework.ApplicationModel;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Base <see cref="IPanel"/> implementation.
	/// </summary>
	[DebuggerDisplay("{GetType().Name}:"
					+ " {Header.Text}"
					+ " Parent={Parent.Header.Text}")]
	public class PanelBase : Presenter, IPanelContentHost
	{
		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelBase"/> class.
		/// </summary>
		public PanelBase()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelBase"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="PanelBase"/> instance to copy data from.</param>
		protected PanelBase(PanelBase another)
		{
			if (another.PanelContent != null)
			{
				PanelContent = (IPanelContent) another.PanelContent.Clone();
			}
		}

		#endregion

		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public virtual object Clone()
		{
			return new PanelBase(this);
		}

		#endregion

		#region Implementation of IPanel

		/// <summary>
		/// Gets panel header.
		/// </summary>
		public IPanelHeader Header
		{
			get { return panelContent.Header; }
		}

		/// <summary>
		/// Gets or sets parent <see cref="IPanel"/>.
		/// </summary>
		public IPanel Parent { get; set; }

		#endregion

		#region Implementation of IPanelContentHost

		/// <summary>
		/// Panel content.
		/// </summary>
		private IPanelContent panelContent;

		/// <summary>
		/// Gets or sets panel content.
		/// </summary>
		public IPanelContent PanelContent
		{
			get { return panelContent; }
			set
			{
				panelContent = value;
				panelContent.Host = this;
				NotifyOfPropertyChange("Header");
				NotifyOfPropertyChange("PanelContent");
			}
		}

		#endregion
	}
}