// <copyright file="LocalFileSystemModuleVM.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-15</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-15</date>
// </editor>
// <summary>Local file system module view model.</summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Nfm.Core.Models.FileSystem;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// Local file system module view model.
	/// </summary>
	public class LocalFileSystemModuleVM : NotificationBase, IPanel
	{
		#region Implementation of IPanel

		/// <summary>
		/// Gets panel header: string text or complex content.
		/// </summary>
		public object Header { get; private set; }

		/// <summary>
		/// Gets a value indicating whether a panel can be closed.
		/// </summary>
		public bool CanClose
		{
			get
			{
				// Note: for future use
				return true;
			}
		}

		/// <summary>
		/// Indicating whether a panel is selected.
		/// </summary>
		private bool isSelected;

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
				throw new Exception("Some child panels can not be closed.");
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

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public LocalFileSystemModuleVM CloneDeep()
		{
			var result = (LocalFileSystemModuleVM)MemberwiseClone();

			// Detach from parent panel
			result.Parent = null;

			// Remove original subscribiters
			result.Closing -= Closing;
			result.Closed -= Closed;

			// Deep copy all childs
			var childsCopy = new ObservableCollection<LogicalDriveNodeVM>();

			foreach (var child in childs)
			{
				LogicalDriveNodeVM newChild = child.CloneDeep();
				childsCopy.Add(newChild);
			}

			result.childs = childsCopy;

			// Note: Model stay the same as original

			return result;
		}

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		IPanel IPanel.CloneDeep()
		{
			return CloneDeep();
		}

		#endregion

		#region Private

		/// <summary>
		/// Child nodes view models.
		/// </summary>
		private ObservableCollection<LogicalDriveNodeVM> childs = new ObservableCollection<LogicalDriveNodeVM>();

		/// <summary>
		/// Gets or sets corresponding node model.
		/// </summary>
		private LocalFileSystemModule NodeModel { get; set; }

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalFileSystemModuleVM" /> class.
		/// </summary>
		public LocalFileSystemModuleVM()
		{
			NodeModel = new LocalFileSystemModule();
		}

		#endregion

		#region Binding Properties

		/// <summary>
		/// Gets or sets a name.
		/// </summary>
		public string Name { get { return NodeModel.DisplayName; } }


		/// <summary>
		/// Gets childs node view models.
		/// </summary>
		public ObservableCollection<LogicalDriveNodeVM> Childs
		{
			[DebuggerStepThrough]
			get { return childs; }
		}

		#endregion

		#region Actions

		/// <summary>
		/// Refresh childs view models.
		/// </summary>
		public void RefreshChilds()
		{
//			var list = new List<IPanel>();
//
//			foreach (var node in NodeModel.Childs)
//			{
//				list.Add(new LogicalDriveNodeVM(this, node));
//			}
//
//			// Sorting by file extenshion and, then, by file name.
//			// TODO: make it configurable
//			var sortedList =
//				list.OrderBy(vm => vm.Name.ToLowerInvariant());
//			// -- TODOEND --
//
//			// TODO: remove this temporary "parent simulator" from childs collection
//			// and make it separate in UI and code, but navigatable like always.
//			var resultList = Enumerable.Empty<IPanel>();
//
//			if (NodeModel.Parent != null)
//			{
//				resultList = Enumerable.Repeat(this, 1);
//
//			}
//
//			resultList = resultList.Concat(sortedList);
//			// -- TODOEND --
//
//			OnPropertyChanging("Childs");
//			childs.Clear();
//			childs = new ObservableCollection<FileSystemEntityNodeVM>(resultList);
//			OnPropertyChanged("Childs");
		}

		#endregion
	}
}