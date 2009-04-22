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

using System.Collections.ObjectModel;
using System.Diagnostics;
using Nfm.Core.Models.FileSystem;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// Logical drive node view model.
	/// </summary>
	public class LogicalDriveNodeVM : NodePanelBase
	{
		#region Implementation of IPanel

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public override object Clone()
		{
			return new LogicalDriveNodeVM(this);
		}

		#endregion

		#region Private

		/// <summary>
		/// Child nodes view models.
		/// </summary>
		private readonly ObservableCollection<FileSystemEntityNodeVM> childs =
			new ObservableCollection<FileSystemEntityNodeVM>();

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

		/// <summary>
		/// Initializes a new instance of the <see cref="LogicalDriveNodeVM"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="LogicalDriveNodeVM"/> instance to copy data from.</param>
		protected LogicalDriveNodeVM(LogicalDriveNodeVM another)
			: base(another)
		{
			NodeModel = another.NodeModel;

			// Deep copy all childs
			var childsCopy = new ObservableCollection<FileSystemEntityNodeVM>();

			foreach (FileSystemEntityNodeVM child in another.childs)
			{
				childsCopy.Add((FileSystemEntityNodeVM) child.Clone());
			}

			childs = childsCopy;
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