// <copyright file="RootNodeVM.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-27</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-27</date>
// </editor>
// <summary>Root node view model.</summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Nfm.Core.Models;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Root node view model.
	/// </summary>
	public sealed class RootNodeVM : NotificationBase, IViewModel, IPanelContent
	{
		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public object Clone()
		{
			return new RootNodeVM(this);
		}

		#endregion

		#region Implementation of IPanelContent

		/// <summary>
		/// Gets or sets parent host panel.
		/// </summary>
		public IPanelContentHost Host { get; set; }

		#endregion

		#region Implementation of IViewModel

		/// <summary>
		/// Gets or sets absolute path.
		/// </summary>
		public string AbsolutePath
		{
			get { return Model.Key; }
			set { throw new NotSupportedException("Absolute path is read only for root node."); }
		}

		/// <summary>
		/// Fetch data from corresponding file system element view model.
		/// </summary>
		public void Refresh()
		{
			IList<IViewModel> list = new List<IViewModel>();

			foreach (var module in Model.Childs)
			{
				module.Value.Refresh();
				list.Add(module.Value);
			}

			OnPropertyChanging("Childs");
			childs.Clear();
			childs = new ObservableCollection<IViewModel>(list);
			OnPropertyChanged("Childs");
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
			throw new NotSupportedException("Execute action is not supported for root modules view model.");
		}

		#endregion

		#region Navigate Into

		/// <summary>
		/// Checks if node support <see cref="IViewModel.NavigateInto"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.NavigateInto"/> action.</returns>
		public bool SupportNavigateInto()
		{
			return false;
		}

		/// <summary>
		/// "Navigate into" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for node in "navigated" mode.</returns>
		public IPanelContent NavigateInto()
		{
			throw new NotSupportedException("NavigateInto action is not supported for root modules view model.");
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
			throw new NotSupportedException("NavigateOut action is not supported for root modules view model.");
		}

		#endregion

		#endregion

		/// <summary>
		/// Child nodes view models.
		/// </summary>
		private ObservableCollection<IViewModel> childs = new ObservableCollection<IViewModel>();

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="RootNodeVM" /> class.
		/// </summary>
		/// <param name="model">Local file system data model.</param>
		public RootNodeVM(RootNode model)
		{
			Model = model;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RootNodeVM"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="RootNodeVM"/> instance to copy data from.</param>
		private RootNodeVM(RootNodeVM another)
		{
			Model = another.Model;

			// Deep copy all childs
			var childsCopy = new ObservableCollection<IViewModel>();

			foreach (IViewModel child in another.childs)
			{
				childsCopy.Add((IViewModel) child.Clone());
			}

			childs = childsCopy;
		}

		#endregion

		#region Model Data

		/// <summary>
		/// Gets or sets corresponding node model.
		/// </summary>
		private RootNode Model { get; set; }

		#endregion

		#region Binding Properties

		/// <summary>
		/// Gets childs view models.
		/// </summary>
		public ObservableCollection<IViewModel> Childs
		{
			[DebuggerStepThrough]
			get { return childs; }
		}

		#endregion
	}
}