// <copyright file="PanelContainerBase.cs" company="HD">
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
// <summary>Base <see cref="IPanelContainer"/> with childs close notifications.</summary>

using System.Diagnostics;
using Caliburn.PresentationFramework.ApplicationModel;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Base <see cref="IPanelContainer"/> with childs close notifications.
	/// </summary>
	[DebuggerDisplay("{GetType().Name}:"
					 + " {Header.Text}"
					 + " {IsActive}"
					 + " {Presenters.Count}"
					 + " Parent={Parent != null ? Parent.Header.Text : null}")]
	public class PanelContainerBase : MultiPresenterManager, IPanelContainer
	{
		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelContainerBase"/> class.
		/// </summary>
		public PanelContainerBase()
		{
			Header = new PanelHeader();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelContainerBase"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="PanelContainerBase"/> instance to copy data from.</param>
		protected PanelContainerBase(PanelContainerBase another)
		{
			if (another.Header != null)
			{
				Header = (IPanelHeader)another.Header.Clone();
			}

			// Deep copy all childs
			foreach (IPresenter child in another.Presenters)
			{
				if (child is IPanel)
				{
					Presenters.Add((IPanel)((IPanel)child).Clone());
				}
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
			return new PanelContainerBase(this);
		}

		#endregion

		#region Implementation of IPanel

		/// <summary>
		/// Gets or sets panel header.
		/// </summary>
		public IPanelHeader Header { get; set; }

		/// <summary>
		/// Gets or sets parent <see cref="IPanel"/>.
		/// </summary>
		public IPanel Parent { get; set; }

		#endregion
	}
}