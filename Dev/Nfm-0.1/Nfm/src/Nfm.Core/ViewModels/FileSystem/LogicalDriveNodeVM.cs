// <copyright file="LogicalDriveNodeVM.cs" company="HD">
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
// <summary>Logical drive node view model.</summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Nfm.Core.Models.FileSystem;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// Logical drive node view model.
	/// </summary>
	public class LogicalDriveNodeVM : NotificationBase, IPanel
	{
		#region Implementation of IPanel

		/// <summary>
		/// Indicating whether a panel is selected.
		/// </summary>
		private bool isSelected;

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
		IPanel IPanel.CloneDeep()
		{
			return CloneDeep();
		}

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public LogicalDriveNodeVM CloneDeep()
		{
			var result = (LogicalDriveNodeVM) MemberwiseClone();

			// Detach from parent panel
			result.Parent = null;

			// Remove original subscribiters
			result.Closing -= Closing;
			result.Closed -= Closed;

			// Deep copy all childs
			var childsCopy = new ObservableCollection<FileSystemEntityNodeVM>();

			foreach (FileSystemEntityNodeVM child in childs)
			{
				FileSystemEntityNodeVM newChild = child.CloneDeep();
				childsCopy.Add(newChild);
			}

			result.childs = childsCopy;

			// Note: Model stay the same as original

			return result;
		}

		#endregion

		#region Private

		/// <summary>
		/// Child nodes view models.
		/// </summary>
		private ObservableCollection<FileSystemEntityNodeVM> childs = new ObservableCollection<FileSystemEntityNodeVM>();

		/// <summary>
		/// Gets or sets corresponding node model.
		/// </summary>
		private LogicalDriveNode NodeModel { get; set; }

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="LogicalDriveNodeVM" /> class.
		/// </summary>
		/// <param name="node">Corresponding logical drive node model.</param>
		public LogicalDriveNodeVM(LogicalDriveNode node)
		{
			NodeModel = node;
			RefreshDetails();
		}

		#endregion

		#region Binding Properties

		/// <summary>
		/// Gets the name of a drive.
		/// </summary>
		public string Name
		{
			get { return NodeModel.DriveInfo.Name; }
		}

		/// <summary>
		/// Gets the total size of the drive, in bytes.
		/// </summary>
		public long TotalSize
		{
			get { return NodeModel.DriveInfo.TotalSize; }
		}

		/// <summary>
		/// Gets the total amount of free space available on a drive.
		/// </summary>
		public long TotalFreeSpace
		{
			get { return NodeModel.DriveInfo.TotalFreeSpace; }
		}

		/// <summary>
		/// Gets or sets the volume label of a drive.
		/// </summary>
		public string VolumeLabel
		{
			get { return NodeModel.DriveInfo.VolumeLabel; }
			set { NodeModel.DriveInfo.VolumeLabel = value; }
		}

		/// <summary>
		/// Gets the amount of available free space on a drive.
		/// </summary>
		public long AvailableFreeSpace
		{
			get { return NodeModel.DriveInfo.AvailableFreeSpace; }
		}

		/// <summary>
		/// Gets a value indicating whether a drive is ready.
		/// </summary>
		public bool IsReady
		{
			get { return NodeModel.DriveInfo.IsReady; }
		}

		/// <summary>
		/// Gets the name of the file system, such as NTFS or FAT32.
		/// </summary>
		public string DriveFormat
		{
			get { return NodeModel.DriveInfo.DriveFormat; }
		}

		/// <summary>
		/// Gets the drive type.
		/// </summary>
		public string DriveType
		{
			get { return NodeModel.DriveInfo.DriveType.ToString(); }
		}

		/// <summary>
		/// Gets childs node view models.
		/// </summary>
		public ObservableCollection<FileSystemEntityNodeVM> Childs
		{
			[DebuggerStepThrough]
			get { return childs; }
		}

		#endregion

		#region Actions

		/// <summary>
		/// Fetch data from corresponding logical drive node model.
		/// </summary>
		public void RefreshDetails()
		{
			NodeModel.RefreshDetails();

			OnPropertyChanging("Name");
			OnPropertyChanged("Name");

			OnPropertyChanging("TotalSize");
			OnPropertyChanged("TotalSize");

			OnPropertyChanging("TotalFreeSpace");
			OnPropertyChanged("TotalFreeSpace");

			OnPropertyChanging("VolumeLabel");
			OnPropertyChanged("VolumeLabel");

			OnPropertyChanging("AvailableFreeSpace");
			OnPropertyChanged("AvailableFreeSpace");

			OnPropertyChanging("IsReady");
			OnPropertyChanged("IsReady");

			OnPropertyChanging("DriveFormat");
			OnPropertyChanged("DriveFormat");

			OnPropertyChanging("DriveType");
			OnPropertyChanged("DriveType");
		}

		/// <summary>
		/// Change current node vide model to new value.
		/// </summary>
		/// <param name="nodeViewModel">New node view model.</param>
		public void ChangeNode(LogicalDriveNodeVM nodeViewModel)
		{
			// TODO: if possible, switch to using common IPanel interface instead of casting
//			ChangeNode2(nodeViewModel.NodeModel);
		}

		/// <summary>
		/// Change current node nodeModel to new value.
		/// </summary>
		/// <param name="nodeModel">New node  model.</param>
		public void ChangeNode2(LogicalDriveNode nodeModel)
		{
//			if (nodeModel != null)
//			{
//				NodeModel = nodeModel;
//				RefreshDetails();
//				RefreshChilds();
//			}
		}

		/// <summary>
		/// Change current node model to it parent value if available.
		/// </summary>
		public void NavigateToParent()
		{
//			if (NodeModel.Parent != null)
//			{
//				ChangeNode2((LocalFileSystemModuleVM)NodeModel.Parent);
//			}
		}

		/// <summary>
		/// Refresh childs view models.
		/// </summary>
		public void RefreshChilds()
		{
//			IList<FileSystemEntityNodeVM> list = new List<FileSystemEntityNodeVM>();
//
//			foreach (FileSystemEntityNode node in NodeModel.Childs)
//			{
//				list.Add(new FileSystemEntityNodeVM(node));
//			}
//
//			// Sorting by file extenshion and, then, by file name.
//			// TODO: make it configurable
//			IOrderedEnumerable<FileSystemEntityNodeVM> sortedList =
//				list.OrderBy(vm => vm.IsFile)
//				.ThenBy(vm => vm.Extension.ToLowerInvariant())
//				.ThenBy(vm => vm.Name.ToLowerInvariant());
//			// -- TODOEND --
//
//			// TODO: remove this temporary "parent simulator" from childs collection
//			// and make it separate in UI and code, but navigatable like always.
//			IEnumerable<FileSystemEntityNodeVM> resultList = Enumerable.Empty<FileSystemEntityNodeVM>();
//
//			NodeModel.RefreshParent();
//
//			if (NodeModel.Parent != null)
//			{
//				var parentVm = new FileSystemEntityNodeVM((FileSystemEntityNode)NodeModel.Parent);
//				resultList = Enumerable.Repeat(parentVm, 1);
//
//				Parent = parentVm;
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