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
// <summary>Base <see cref="INodePanel"/> implementation.</summary>

using System;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Base <see cref="IPanel"/> implementation.
	/// </summary>
	public class PanelBase : NotificationBase, IPanelContentHost
	{
		#region Implementation of IDisposable

		/// <summary>
		/// Forced object distruction.
		/// </summary>
		/// <param name="disposing">"True" for manual calls.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Release managed resources.
			}

			// Release unmanaged resources.
			// Set large fields to null.
			// Call Dispose on your base class.
			base.Dispose(disposing);
		}

		// The derived class does not have a Finalize method
		// or a Dispose method with parameters because it inherits
		// them from the base class.

		#endregion

		#region ICloneable

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
		/// Indicating whether a panel is selected.
		/// </summary>
		private bool isSelected;

		/// <summary>
		/// Gets panel header: string text or complex content.
		/// </summary>
		public object Header
		{
			get { return panelContent.Header; }
		}

		/// <summary>
		/// Gets a value indicating whether a panel can be closed.
		/// </summary>
		public bool CanClose { get; protected set; }

		/// <summary>
		/// Gets or sets a value indicating whether a panel is selected.
		/// </summary>
		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				OnPropertyChanging("IsSelected");
				isSelected = value;
				OnPropertyChanged("IsSelected");
			}
		}

		/// <summary>
		/// Gets or sets parent <see cref="IPanel"/>.
		/// </summary>
		public IPanel Parent { get; set; }

		/// <summary>
		/// Request close action for panel.
		/// </summary>
		public void RequestClose()
		{
			if (!CanClose)
			{
				throw new Exception("This panel can not be closed.");
			}

			OnEvent(Closing, this);
			//Dispose(true);
			OnEvent(Closed, this);
		}

		/// <summary>
		/// Fire when panel is intended to close.
		/// </summary>
		public event EventHandler<EventArgs> Closing;

		/// <summary>
		/// Fire when panel is closed.
		/// </summary>
		public event EventHandler<EventArgs> Closed;

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
				OnPropertyChanging("PanelContent");
				OnPropertyChanging("Header");

				panelContent = value;
				panelContent.Host = this;

				OnPropertyChanged("Header");
				OnPropertyChanged("PanelContent");
			}
		}

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelBase"/> class.
		/// </summary>
		public PanelBase()
		{
			// Note: for future use
			CanClose = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelBase"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="PanelBase"/> instance to copy data from.</param>
		protected PanelBase(PanelBase another)
		{
			CanClose = another.CanClose;
			isSelected = another.isSelected;

			PanelContent = (IPanelContent) another.PanelContent.Clone();

			// Detach from parent panel
			//	Parent = null;

			// Remove original subscribiters
			//	Closing = null;	// -= Closing;
			//	Closed = null;	// -= Closed;
		}

		#endregion
	}
}