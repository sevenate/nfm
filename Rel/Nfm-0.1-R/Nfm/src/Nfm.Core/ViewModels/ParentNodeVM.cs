// <copyright file="ParentNodeVM.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-29</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-29</date>
// </editor>
// <summary>Parent view model.</summary>

using System;
using System.Diagnostics;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Parent view model.
	/// </summary>
	[DebuggerDisplay("{ParentContent}")]
	public class ParentNodeVM : NotificationBase, IViewModel
	{
		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public object Clone()
		{
			return new ParentNodeVM(this);
		}

		#endregion

		#region Implementation of IViewModel

		/// <summary>
		/// Flag value indicating whether view model is selected.
		/// </summary>
		private bool isSelected;

		/// <summary>
		/// Gets or sets absolute path.
		/// </summary>
		public string AbsolutePath { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether view model is selected.
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
		/// Fetch data from corresponding file system element view model.
		/// </summary>
		public void Refresh()
		{
			// Nothing to do here - its dummy view model.
		}

		#region Execute

		/// <summary>
		/// Checks if node support <see cref="IViewModel.Execute"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.Execute"/> action.</returns>
		public bool SupportExecute()
		{
			return false;
		}

		/// <summary>
		/// "Execute" action.
		/// </summary>
		public void Execute()
		{
			throw new NotSupportedException("Execute action is not supported for parent link view model.");
		}

		#endregion

		#region Navigate Into

		/// <summary>
		/// Checks if node support <see cref="IViewModel.NavigateInto"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.NavigateInto"/> action.</returns>
		public bool SupportNavigateInto()
		{
			return true;
		}

		/// <summary>
		/// "Navigate into" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for node in "navigated" mode.</returns>
		public IPanelContent NavigateInto()
		{
			return ParentContent;
		}

		#endregion

		#region Navigate Out

		/// <summary>
		/// Checks if node support <see cref="IViewModel.SupportNavigateOut"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.SupportNavigateOut"/> action.</returns>
		public bool SupportNavigateOut()
		{
			return false;
		}

		/// <summary>
		/// "Navigate out" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for parent node in "navigated" mode.</returns>
		public IPanelContent NavigateOut()
		{
			throw new NotSupportedException("NavigateOut action is not supported for parent link view model.");
		}

		#endregion

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ParentNodeVM" /> class.
		/// </summary>
		/// <param name="parentContent">Parent content view model.</param>
		public ParentNodeVM(IPanelContent parentContent)
		{
			ParentContent = parentContent;
			AbsolutePath = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ParentNodeVM"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="ParentNodeVM"/> instance to copy data from.</param>
		protected ParentNodeVM(ParentNodeVM another)
		{
			AbsolutePath = another.AbsolutePath;
			isSelected = another.IsSelected;
			ParentContent = (IPanelContent) another.ParentContent.Clone();
		}

		#endregion

		#region Private Data

		/// <summary>
		/// Gets or sets corresponding node model.
		/// </summary>
		private IPanelContent ParentContent { get; set; }

		#endregion
	}
}