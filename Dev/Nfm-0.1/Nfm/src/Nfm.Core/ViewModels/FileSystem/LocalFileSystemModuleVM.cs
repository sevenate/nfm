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

using System.Collections.ObjectModel;
using System.Diagnostics;
using Nfm.Core.Models;
using Nfm.Core.Models.FileSystem;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// Local file system module view model.
	/// </summary>
	public class LocalFileSystemModuleVM : PanelBase
	{
		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public override object Clone()
		{
			return new LocalFileSystemModuleVM(this);
		}

		#endregion

		#region Private

		/// <summary>
		/// Child nodes view models.
		/// </summary>
		private readonly ObservableCollection<LogicalDriveNodeVM> childs = new ObservableCollection<LogicalDriveNodeVM>();

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
			NodeModel = (LocalFileSystemModule) RootNode.Inst.GetModule("Local File System");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalFileSystemModuleVM"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="LocalFileSystemModuleVM"/> instance to copy data from.</param>
		protected LocalFileSystemModuleVM(LocalFileSystemModuleVM another)
			: base(another)
		{
			NodeModel = another.NodeModel;

			// Deep copy all childs
			var childsCopy = new ObservableCollection<LogicalDriveNodeVM>();

			foreach (LogicalDriveNodeVM child in another.childs)
			{
				childsCopy.Add((LogicalDriveNodeVM) child.Clone());
			}

			childs = childsCopy;
		}

		#endregion

		#region Binding Properties

		/// <summary>
		/// Gets a name.
		/// </summary>
		public string Name
		{
			get { return NodeModel.DisplayName; }
		}

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