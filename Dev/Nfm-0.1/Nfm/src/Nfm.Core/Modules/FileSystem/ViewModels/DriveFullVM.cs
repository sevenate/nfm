// <copyright file="DriveFullVM.cs" company="HD">
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
// <summary>Logical drive content view model.</summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Nfm.Core.Models.FileSystem;

namespace Nfm.Core.ViewModels.FileSystem
{
	/// <summary>
	/// Logical drive content view model.
	/// </summary>
	[DebuggerDisplay("{Name} ({Childs.Count})")]
	public class DriveFullVM : DriveVM, IViewModelWithChilds, IPanelContent
	{
		/// <summary>
		/// Child nodes view models.
		/// </summary>
		private ObservableCollection<IViewModel> childs = new ObservableCollection<IViewModel>();

		/// <summary>
		/// Selected child nodes view models.
		/// </summary>
		private readonly ObservableCollection<IViewModel> selectedChilds = new ObservableCollection<IViewModel>();

		/// <summary>
		/// Current child item with keyboard focus on.
		/// </summary>
		private int currentItemIndex = -1;

		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="DriveFullVM" /> class.
		/// </summary>
		/// <param name="model">Local file system data model.</param>
		public DriveFullVM(LocalFileSystemModule model)
			: base(model)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DriveFullVM"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="DriveFullVM"/> instance to copy data from.</param>
		protected DriveFullVM(DriveFullVM another)
			: base(another)
		{
			// Deep copy all childs
			var childsCopy = new ObservableCollection<IViewModel>();

			foreach (IViewModel child in another.childs)
			{
				childsCopy.Add((IViewModel) child.Clone());
			}

			childs = childsCopy;


			// Deep copy all selected childs
			var selectedChildsCopy = new ObservableCollection<IViewModel>();

			foreach (IViewModel child in another.selectedChilds)
			{
				selectedChildsCopy.Add((IViewModel)child.Clone());
			}

			selectedChilds = selectedChildsCopy;

			currentItemIndex = another.CurrentItemIndex;
		}

		#endregion

		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public override object Clone()
		{
			return new DriveFullVM(this);
		}

		#endregion

		#region Override of IViewModel

		/// <summary>
		/// Fetch data from corresponding folder model and its childs folder and files.
		/// </summary>
		public override void Refresh()
		{
			base.Refresh();

			IList<IViewModel> list = new List<IViewModel>();

			if (Drive.IsReady)
			{
				foreach (DirectoryInfo folder in Model.Folders(Drive.Name))
				{
					var vm = new FolderVM(Model)
					         {
					         	AbsolutePath = folder.FullName
					         };

					vm.Refresh();
					list.Add(vm);
				}

				foreach (FileInfo file in Model.Files(Drive.Name))
				{
					var vm = new FileVM(Model)
					         {
					         	AbsolutePath = file.FullName
					         };

					vm.Refresh();
					list.Add(vm);
				}
			}

			// Sorting by file extenshion and, then, by file name.
			// TODO: make it configurable
			IOrderedEnumerable<IViewModel> sortedList =
				list.OrderBy(vm => vm is FileVM)
					.ThenBy(
					vm => (vm is FileVM)
					      	? ((FileVM) vm).Extension.ToLowerInvariant()
					      	: string.Empty)
					.ThenBy(
					vm => (vm is FileVM)
					      	? ((FileVM) vm).Name.ToLowerInvariant()
					      	: string.Empty);
			// -- TODOEND --

			// TODO: remove this temporary "parent simulator" from childs collection
			// and make it separate in UI and code, but navigatable like always.
			IEnumerable<IViewModel> resultList = Enumerable.Empty<IViewModel>();

			var fullVM = NavigateOut();
			var parent = new ParentNodeVM(fullVM)
			             {
			             	AbsolutePath = ((IViewModel) fullVM).AbsolutePath
			             };

			resultList = Enumerable.Repeat((IViewModel)parent, 1);

			resultList = resultList.Concat(sortedList);
			// -- TODOEND --

			OnPropertyChanging("Childs");
			childs.Clear();
			childs = new ObservableCollection<IViewModel>(resultList);
			OnPropertyChanged("Childs");

			if (CurrentItemIndex == -1 && childs.Count > 0)
			{
				OnPropertyChanging("CurrentItemIndex");
				CurrentItemIndex = 0;
				OnPropertyChanged("CurrentItemIndex");
			}
		}

		#region Execute

		/// <summary>
		/// Checks if node support <see cref="IViewModel.Execute"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.Execute"/> action.</returns>
		public override bool SupportExecute()
		{
			return false;
		}

		/// <summary>
		/// "Execute" action.
		/// </summary>
		public override void Execute()
		{
			throw new NotSupportedException("Execute action is not supported for logical drive in full view model.");
		}

		#endregion

		#region Navigate Into

		/// <summary>
		/// Checks if node support <see cref="IViewModel.NavigateInto"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.NavigateInto"/> action.</returns>
		public override bool SupportNavigateInto()
		{
			return false;
		}

		/// <summary>
		/// "Navigate into" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for node in "navigated" mode.</returns>
		public override IPanelContent NavigateInto()
		{
			throw new NotSupportedException("NavigateInto action is not supported for logical drive in full view model.");
		}

		#endregion

		#region Navigate Out

		/// <summary>
		/// Checks if node support <see cref="IViewModel.SupportNavigateOut"/> action.
		/// </summary>
		/// <returns>True if node could handle <see cref="IViewModel.SupportNavigateOut"/> action.</returns>
		public override bool SupportNavigateOut()
		{
			return true;
		}

		/// <summary>
		/// "Navigate out" action.
		/// </summary>
		/// <returns>Corresponding (or default) content view model for parent node in "navigated" mode.</returns>
		public override IPanelContent NavigateOut()
		{
			var fullVM = new LocalFileSystemModuleFullVM(Model);
			fullVM.Refresh();

			for (int i = 0; i < fullVM.Childs.Count; i++)
			{
				var model = fullVM.Childs[i];

				if (model.AbsolutePath.StartsWith(AbsolutePath, StringComparison.InvariantCultureIgnoreCase))
				{
					fullVM.CurrentItemIndex = i;
				}
			}

			return fullVM;
		}

		#endregion

		#endregion

		#region Implementation of IViewModelWithChilds

		/// <summary>
		/// Gets childs view models.
		/// </summary>
		public ObservableCollection<IViewModel> Childs
		{
			[DebuggerStepThrough]
			get { return childs; }
		}

		/// <summary>
		/// Gets selected childs view models.
		/// </summary>
		public ObservableCollection<IViewModel> SelectedItems
		{
			[DebuggerStepThrough]
			get { return selectedChilds; }
		}

		/// <summary>
		/// Gets or sets current child item index.
		/// </summary>
		public int CurrentItemIndex
		{
			get { return currentItemIndex; }
			set
			{
				currentItemIndex = 0 <= value && value < childs.Count
									? value
									: -1;
			}
		}
		#endregion

		#region Implementation of IPanelContent

		/// <summary>
		/// Gets panel header: string text or complex content.
		/// </summary>
		public object Header
		{
			get { return Name; }
		}

		/// <summary>
		/// Gets or sets parent host panel.
		/// </summary>
		public IPanelContentHost Host { get; set; }

		#endregion
	}
}