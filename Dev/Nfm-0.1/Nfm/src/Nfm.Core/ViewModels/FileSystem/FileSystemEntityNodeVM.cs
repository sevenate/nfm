// <copyright file="FileSystemEntityNodeVM.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-26</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-26</date>
// </editor>
// <summary>Node view model.</summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Nfm.Core.Models.FileSystem;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// Node view model.
	/// </summary>
	[DebuggerDisplay(
			@"Name = {Name}"
		+ @", Ext = {Extension}"
		+ @", Childs = {Childs.Count}"
		+ @", Model = {NodeModel.DisplayName}"
		+ @", Parent = {Parent.Header}")]
	public class FileSystemEntityNodeVM : NotificationBase, IPanel
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
		/// Gets or sets a value indicating whether a panel is selected.
		/// </summary>
		public bool IsSelected { get; set; }

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

		#endregion

		#region Private

		/// <summary>
		/// Child nodes view models.
		/// </summary>
		private ObservableCollection<FileSystemEntityNodeVM> childs = new ObservableCollection<FileSystemEntityNodeVM>();

		/// <summary>
		/// Gets or sets corresponding node model.
		/// </summary>
		private FileSystemEntityNode NodeModel { get; set; }

		#endregion

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="FileSystemEntityNodeVM" /> class.
		/// </summary>
		/// <param name="node">Node model.</param>
		public FileSystemEntityNodeVM(FileSystemEntityNode node)
		{
			NodeModel = node;
			RefreshDetails();
		}

		#endregion

		#region Binding Properties

		/// <summary>
		/// Gets or sets a name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating file extension.
		/// </summary>
		public string Extension { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a file has an extension.
		/// </summary>
		public bool? HasExtension { get; set; }

		/// <summary>
		/// Gets or sets a value indicating file create date.
		/// </summary>
		public DateTime? DateCreated { get; set; }

		/// <summary>
		/// Gets or sets a value indicating file last modified date.
		/// </summary>
		public DateTime? DateModified { get; set; }

		/// <summary>
		/// Gets or sets a value indicating file size.
		/// </summary>
		public long? Size { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "Archive" attribute is set.
		/// </summary>
		public bool? IsArchive { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "Compressed" attribute is set.
		/// </summary>
		public bool? IsCompressed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "Device" attribute is set.
		/// </summary>
		public bool? IsDevice { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "Directory" attribute is set.
		/// </summary>
		public bool? IsDirectory { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "Directory" attribute is NOT set.
		/// </summary>
		/// TODO: remove this duplicate property
		/// and use data template selector to differentiate files and dirs view.
		public bool? IsFile { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "Encrypted" attribute is set.
		/// </summary>
		public bool? IsEncrypted { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "Hidden" attribute is set.
		/// </summary>
		public bool? IsHidden { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "NotContentIndexed" attribute is set.
		/// </summary>
		public bool? IsNotContentIndexed { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "Offline" attribute is set.
		/// </summary>
		public bool? IsOffline { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "ReadOnly" attribute is set.
		/// </summary>
		public bool? IsReadOnly { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "ReparsePoint" attribute is set.
		/// </summary>
		public bool? IsReparsePoint { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "SparseFile" attribute is set.
		/// </summary>
		public bool? IsSparseFile { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "System" attribute is set.
		/// </summary>
		public bool? IsSystem { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether file "Temporary" attribute is set.
		/// </summary>
		public bool? IsTemporary { get; set; }

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
		/// Fetch data from corresponding file system element node model.
		/// </summary>
		public void RefreshDetails()
		{
			NodeModel.RefreshDetails();

			if (!DateCreated.HasValue || DateCreated != NodeModel.DetailsInfo.CreationTime)
			{
				OnPropertyChanging("DateCreated");
				DateCreated = NodeModel.DetailsInfo.CreationTime;
				OnPropertyChanged("DateCreated");
			}

			if (!DateModified.HasValue || DateModified != NodeModel.DetailsInfo.LastWriteTime)
			{
				OnPropertyChanging("DateModified");
				DateModified = NodeModel.DetailsInfo.LastWriteTime;
				OnPropertyChanged("DateModified");
			}

			if (!IsDirectory.HasValue
			    || IsDirectory != ((NodeModel.DetailsInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory))
			{
				OnPropertyChanging("IsDirectory");
				OnPropertyChanging("IsFile");
				IsDirectory = (NodeModel.DetailsInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
				IsFile = !IsDirectory;
				OnPropertyChanged("IsFile");
				OnPropertyChanged("IsDirectory");
			}

			if (!IsArchive.HasValue
			    || IsArchive.Value != ((NodeModel.DetailsInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive))
			{
				OnPropertyChanging("IsArchive");
				IsArchive = (NodeModel.DetailsInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive;
				OnPropertyChanged("IsArchive");
			}

			if (!IsHidden.HasValue
			    || IsHidden.Value != ((NodeModel.DetailsInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden))
			{
				OnPropertyChanging("IsHidden");
				IsHidden = (NodeModel.DetailsInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
				OnPropertyChanged("IsHidden");
			}

			if (!IsReadOnly.HasValue
			    || IsReadOnly.Value != ((NodeModel.DetailsInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly))
			{
				OnPropertyChanging("IsReadOnly");
				IsReadOnly = (NodeModel.DetailsInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
				OnPropertyChanged("IsReadOnly");
			}

			if (!IsSystem.HasValue
			    || IsSystem.Value != ((NodeModel.DetailsInfo.Attributes & FileAttributes.System) == FileAttributes.System))
			{
				OnPropertyChanging("IsSystem");
				IsSystem = (NodeModel.DetailsInfo.Attributes & FileAttributes.System) == FileAttributes.System;
				OnPropertyChanged("IsSystem");
			}

			if (IsDirectory.Value)
			{
				if (Name == null || !Name.Equals(NodeModel.DetailsInfo.Name))
				{
					OnPropertyChanging("Name");
					Name = NodeModel.DetailsInfo.Name;
					OnPropertyChanged("Name");
				}

				if (Extension == null)
				{
					OnPropertyChanging("Extension");
					OnPropertyChanging("HasExtension");
					Extension = string.Empty;
					HasExtension = false;
					OnPropertyChanged("HasExtension");
					OnPropertyChanged("Extension");
				}
			}
			else
			{
				if (Name == null || !Name.Equals(Path.GetFileNameWithoutExtension(NodeModel.DetailsInfo.Name)))
				{
					OnPropertyChanging("Name");
					Name = Path.GetFileNameWithoutExtension(NodeModel.DetailsInfo.Name);
					OnPropertyChanged("Name");
				}

				if (Extension == null || !Extension.Equals(NodeModel.DetailsInfo.Extension.TrimStart('.')))
				{
					OnPropertyChanging("Extension");
					OnPropertyChanging("HasExtension");
					Extension = NodeModel.DetailsInfo.Extension.TrimStart('.'); //.ToUpperInvariant();
					HasExtension = !string.IsNullOrEmpty(Extension);
					OnPropertyChanged("HasExtension");
					OnPropertyChanged("Extension");
				}

				if (!Size.HasValue || Size.Value != ((FileInfo) NodeModel.DetailsInfo).Length)
				{
					OnPropertyChanging("Size");
					Size = ((FileInfo) NodeModel.DetailsInfo).Length;
					OnPropertyChanged("Size");
				}
			}

			OnPropertyChanging("Header");
			Header = Name;
			OnPropertyChanged("Header");

			OnPropertyChanging("Childs");
			OnPropertyChanged("Childs");
		}

		/// <summary>
		/// Change current node model to new value.
		/// </summary>
		/// <param name="newFileSystemEntityNodeVm">New node view model.</param>
		public void ChangeNode(FileSystemEntityNodeVM newFileSystemEntityNodeVm)
		{
			// TODO: if possible, switch to using common IPanel interface instead of casting
			ChangeNode2(newFileSystemEntityNodeVm.NodeModel);
		}

		public void ChangeNode2(FileSystemEntityNode newFileSystemEntityNode)
		{
			if (newFileSystemEntityNode != null)
			{
				NodeModel = newFileSystemEntityNode;
				RefreshDetails();
				RefreshChilds();
			}
		}

		#region NavigateToParent

		/// <summary>
		/// Gets hotkey for "NavigateToParent" action.
		/// </summary>
		public Key NavigateToParentHotKey { get { return Key.Back; } }

		/// <summary>
		/// Gets hotkey modifiers for "NavigateToParent" action.
		/// </summary>
		public ModifierKeys NavigateToParentHotKeyModifiers { get { return ModifierKeys.None; } }

		/// <summary>
		/// Change current node model to it parent value if available.
		/// </summary>
		public void NavigateToParent()
		{
			if (NodeModel.Parent != null)
			{
				ChangeNode2((FileSystemEntityNode)NodeModel.Parent);
			}
		}

		#endregion

		#region RefreshChilds

		/// <summary>
		/// Gets hotkey for "RefreshChilds" action.
		/// </summary>
		public Key RefreshChildsHotKey { get { return Key.R; } }

		/// <summary>
		/// Gets hotkey modifiers for "RefreshChilds" action.
		/// </summary>
		public ModifierKeys RefreshChildsHotKeyModifiers { get { return ModifierKeys.Control; } }

		/// <summary>
		/// Refresh childs view models.
		/// </summary>
		public void RefreshChilds()
		{
			IList<FileSystemEntityNodeVM> list = new List<FileSystemEntityNodeVM>();

			foreach (FileSystemEntityNode node in NodeModel.Childs)
			{
				list.Add(new FileSystemEntityNodeVM(node));
			}

			// Sorting by file extenshion and, then, by file name.
			// TODO: make it configurable!
			IOrderedEnumerable<FileSystemEntityNodeVM> sortedList =
				list.OrderBy(vm => vm.IsFile)
				.ThenBy(vm => vm.Extension.ToLowerInvariant())
				.ThenBy(vm => vm.Name.ToLowerInvariant());
			// -- TODOEND --

			// TODO: remove this temporary "parent simulator" from childs collection
			// and make it separate in UI and code, but navigatable like always.
			IEnumerable<FileSystemEntityNodeVM> resultList = Enumerable.Empty<FileSystemEntityNodeVM>();

			if (NodeModel.Parent != null)
			{
				var parentVm = new FileSystemEntityNodeVM((FileSystemEntityNode) NodeModel.Parent);
				resultList = Enumerable.Repeat(parentVm, 1);
			}

			resultList = resultList.Concat(sortedList);
			// -- TODOEND --

			OnPropertyChanging("Childs");
			childs.Clear();
			childs = new ObservableCollection<FileSystemEntityNodeVM>(resultList);
			OnPropertyChanged("Childs");
		}

		#endregion

		#endregion
	}
}