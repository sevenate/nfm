// <copyright file="DublicateSelectedPanelCommand.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-10</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-10</date>
// </editor>
// <summary>Dublicate selected panel command.</summary>

using System;
using Caliburn.PresentationFramework.Filters;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Commands
{
	/// <summary>
	/// Dublicate selected panel command.
	/// </summary>
	public class DublicateSelectedPanelCommand : ToolbarItemBase
	{
		/// <summary>
		/// Panel to duplicate.
		/// </summary>
		private IPanel panel;

		/// <summary>
		/// Initializes a new instance of the <see cref="DublicateSelectedPanelCommand"/> class.
		/// </summary>
		public DublicateSelectedPanelCommand()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DublicateSelectedPanelCommand"/> class.
		/// </summary>
		public DublicateSelectedPanelCommand(IPanel panel)
		{
			this.panel = panel;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DublicateSelectedPanelCommand"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="DublicateSelectedPanelCommand"/> instance to copy data from.</param>
		protected DublicateSelectedPanelCommand(DublicateSelectedPanelCommand another)
			: base(another)
		{
			panel = another.panel;
		}

		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public object Clone()
		{
			return new DublicateSelectedPanelCommand(this);
		}

		#endregion

		/// <summary>
		/// Dublicate panel in the parent container.
		/// </summary>
		/// <param name="panel">Panel to dublicate.</param>
		[Preview("CanExecute", AffectsTriggers = false)]
		public void Execute(IPanel panel)
		{
			if (panel == null)
			{
				throw new ArgumentNullException("panel");
			}

			var container = panel.Parent as IPanelContainer;

			if (container != null)
			{
				// Insert after current panel
				int targetIndex = container.Childs.IndexOf(panel);
				container.Childs.Insert(targetIndex + 1, (IPanel) panel.Clone());
			}
		}

		/// <summary>
		/// Check if the panel can be dublicated.
		/// </summary>
		/// <param name="panel">Panel to dublicate.</param>
		/// <returns>True, if at panel can be dublicated in the parent container.</returns>
		public bool CanExecute(IPanel panel)
		{
			return panel != null && panel.Parent is IPanelContainer;
		}
	}
}