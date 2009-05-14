// <copyright file="ConfigManager.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-20</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-20</date>
// </editor>
// <summary>General configuration settings manager.</summary>

using System.Windows.Controls;
using Nfm.Core.Models;
using Nfm.Core.Models.FileSystem;
using Nfm.Core.ViewModels;
using Nfm.Core.ViewModels.FileSystem;

namespace Nfm.Core.Configuration
{
	/// <summary>
	/// General configuration settings manager.
	/// </summary>
	public static class ConfigManager
	{
		/// <summary>
		/// Gets panels layout.
		/// </summary>
		/// <returns>Panels layout.</returns>
		public static IPanel GetLayout()
		{
			LoadModules();
			return GetDefaultLayout();
		}

		/// <summary>
		/// Gets default panels layout.
		/// </summary>
		/// <returns>Default panels layout.</returns>
		private static IPanel GetDefaultLayout()
		{
			TabContainer workLeftTabContainer = GetWorkLeftTabContainer();
			TabContainer workMiddleTabContainer = GetWorkMiddleTabContainer();
			StackContainer workRightStackContainer = GetWorkRightStackContainer();
			StackContainer workStackContainer = GetWorkStackContainer(
				workLeftTabContainer, workMiddleTabContainer, workRightStackContainer);

			PanelBase enterPanel = GetEnterPanel();
			enterPanel.IsSelected = true;

			TabContainer subTabContainer1 = GetEnterTopTabContainer();
			TabContainer subTabContainer2 = GetDisksTopTabSubContainer();
			TabContainer topTabContainer = GetTopTabContainer(subTabContainer1, subTabContainer2);

//			workStackContainer.IsSelected = true;
			TabContainer mainTabContainer = GetMainTabContainer(enterPanel, workStackContainer, topTabContainer);

			return mainTabContainer; //workLeftTabContainer
		}

		/// <summary>
		/// Load root node modules.
		/// </summary>
		private static void LoadModules()
		{
			var firstModule = new LocalFileSystemModule();
			RootNode.Inst.RegisterNode(firstModule, new LocalFileSystemModuleVM(firstModule));
		}

		#region Debug layout

		/// <summary>
		/// Get Work Left Tab Container.
		/// </summary>
		/// <returns>Work Left Tab Container.</returns>
		private static TabContainer GetWorkLeftTabContainer()
		{
			IViewModel music = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Music");
			music.Refresh();

			var musicPanel = new PanelBase
			                  {
			                  	PanelContent = (IPanelContent) music,
			                  	IsSelected = true
			                  };

			IViewModel driveD = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\C:\");
			driveD.Refresh();

			var driveDPanel = new PanelBase
			                  {
			                  	PanelContent = (IPanelContent) driveD
			                  };

			var workLeftTabContainer = new TabContainer
			                           {
			                           	Header = "Left Tab Container"
			                           };
			workLeftTabContainer.Childs.Add(driveDPanel);
			workLeftTabContainer.Childs.Add(musicPanel);
			return workLeftTabContainer;
		}

		/// <summary>
		/// Get Work Middle Tab Container.
		/// </summary>
		/// <returns>Work Middle Tab Container.</returns>
		private static TabContainer GetWorkMiddleTabContainer()
		{
			IViewModel workMiddle = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Downloads");
			workMiddle.Refresh();

			var workMiddlePanel = new PanelBase
			                      {
			                      	PanelContent = (IPanelContent) workMiddle
			                      };

			var workMiddleTabContainer = new TabContainer
			                             {
			                             	Header = "Middle Tab Container"
			                             };
			workMiddleTabContainer.Childs.Add(workMiddlePanel);
			return workMiddleTabContainer;
		}

		/// <summary>
		/// Get Work Right Stack Container.
		/// </summary>
		/// <returns>Work Right Stack Container.</returns>
		private static StackContainer GetWorkRightStackContainer()
		{
			IViewModel workRight1 = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Games");
			workRight1.Refresh();

			var workRightPanel1 = new PanelBase
			                      {
			                      	PanelContent = (IPanelContent) workRight1
			                      };

			IViewModel workRight2 = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\");
			workRight2.Refresh();

			var workRightPanel2 = new PanelBase
			                      {
			                      	PanelContent = (IPanelContent) workRight2
			                      };

			var workRightPanel1TabContainer = new TabContainer
			                                  {
			                                  	Header = "Work Right Top Tab Container"
			                                  };
			workRightPanel1TabContainer.Childs.Add(workRightPanel1);

			var workRightPanel2TabContainer = new TabContainer
			                                  {
			                                  	Header = "Work Right Bottom Tab Container"
			                                  };
			workRightPanel2TabContainer.Childs.Add(workRightPanel2);

			var workRightStackContainer = new StackContainer
			                              {
			                              	Header = "Right Stack Container",
			                              	Orientation = Orientation.Vertical
			                              };
			//			workRightStackContainer.Childs.Add(workRightPanel1);
			//			workRightStackContainer.Childs.Add(workRightPanel2);
			workRightStackContainer.Childs.Add(workRightPanel1TabContainer);
			workRightStackContainer.Childs.Add(workRightPanel2TabContainer);
			return workRightStackContainer;
		}

		/// <summary>
		/// Get Work Stack Container.
		/// </summary>
		/// <param name="workLeftTabContainer">Work left tab container.</param>
		/// <param name="workMiddleTabContainer">Work middle tab container.</param>
		/// <param name="workRightStackContainer">Work right stack container.</param>
		/// <returns>Work Stack Container.</returns>
		private static StackContainer GetWorkStackContainer(
			IPanel workLeftTabContainer, IPanel workMiddleTabContainer, IPanel workRightStackContainer)
		{
			var workStackContainer = new StackContainer
			                         {
			                         	Header = "Work Stack Container"
			                         };
			workStackContainer.Childs.Add(workLeftTabContainer);
			//			workStackContainer.Childs.Add(workMiddlePanel);
			workStackContainer.Childs.Add(workMiddleTabContainer);
			workStackContainer.Childs.Add(workRightStackContainer);
			return workStackContainer;
		}

		/// <summary>
		/// Get Enter Panel.
		/// </summary>
		/// <returns>Enter Panel.</returns>
		private static PanelBase GetEnterPanel()
		{
			IViewModel enter = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Videos");
			enter.Refresh();

			if (enter is IViewModelWithChilds)
			{
				var enterWithChilds = (IViewModelWithChilds) enter;

				if (enterWithChilds.Childs.Count > 5)
				{
					enterWithChilds.SelectedItems.Add(enterWithChilds.Childs[1]);
					enterWithChilds.SelectedItems.Add(enterWithChilds.Childs[2]);
					enterWithChilds.SelectedItems.Add(enterWithChilds.Childs[3]);

					enterWithChilds.CurrentItemIndex = 4;
				}
			}

			return new PanelBase
			       {
			       	PanelContent = (IPanelContent) enter
			       };
		}

		/// <summary>
		/// Get Enter Top Tab Container.
		/// </summary>
		/// <returns>Enter Top Tab Container.</returns>
		private static TabContainer GetEnterTopTabContainer()
		{
			IViewModel topGames = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Games");
			topGames.Refresh();

			var topGamesPanel = new PanelBase
			                    {
			                    	PanelContent = (IPanelContent) topGames
			                    };

			IViewModel topMusic = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\Music");
			topMusic.Refresh();

			var topMusicPanel = new PanelBase
			                    {
			                    	PanelContent = (IPanelContent) topMusic,
			                    	IsSelected = true
			                    };

			var enterTopTabContainer = new TabContainer
			                           {
			                           	Header = "Enter Container"
			                           };
			enterTopTabContainer.Childs.Add(topGamesPanel);
			enterTopTabContainer.Childs.Add(topMusicPanel);
			return enterTopTabContainer;
		}

		/// <summary>
		/// Get Disks Top Tab Sub Container.
		/// </summary>
		/// <returns>Disks Top Tab Sub Container.</returns>
		private static TabContainer GetDisksTopTabSubContainer()
		{
			IViewModel topDriveC = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\C:\");
			topDriveC.Refresh();

			var topDriveCpanel = new PanelBase
			                     {
			                     	PanelContent = (IPanelContent) topDriveC
			                     };

			IViewModel topDriveD = RootNode.Inst.GetNode(@"\{78888951-2516-4e63-AC97-90E9D54351D8}\D:\");
			topDriveD.Refresh();

			var topDriveDpanel = new PanelBase
			                     {
			                     	PanelContent = (IPanelContent) topDriveD,
			                     	IsSelected = true
			                     };

			var subTabContainer2 = new TabContainer
			                       {
			                       	Header = "Disks Container",
			                       	IsSelected = true
			                       };

			subTabContainer2.Childs.Add(topDriveCpanel);
			subTabContainer2.Childs.Add(topDriveDpanel);
			return subTabContainer2;
		}

		/// <summary>
		/// Get Top Tab Container.
		/// </summary>
		/// <param name="subTabContainer1">Sub container 1.</param>
		/// <param name="subTabContainer2">Sub container 2.</param>
		/// <returns>Top Tab Container.</returns>
		private static TabContainer GetTopTabContainer(IPanel subTabContainer1, IPanel subTabContainer2)
		{
			var topTabContainer = new TabContainer
			                      {
			                      	Header = "Top Tab Container",
			                      };
			topTabContainer.Childs.Add(subTabContainer1);
			topTabContainer.Childs.Add(subTabContainer2);
			return topTabContainer;
		}

		/// <summary>
		/// Get Main Tab Container.
		/// </summary>
		/// <param name="firstPanel">Work Stack Container.</param>
		/// <param name="secondPanel">Rnter Panel.</param>
		/// <param name="thirdPanel">Top Tab Container.</param>
		/// <returns>Main Tab Container.</returns>
		private static TabContainer GetMainTabContainer(IPanel firstPanel, IPanel secondPanel, IPanel thirdPanel)
		{
			var mainTabContainer = new TabContainer
			                       {
			                       	Header = "Main Window",
			                       };
			mainTabContainer.Childs.Add(firstPanel);
			mainTabContainer.Childs.Add(secondPanel);
			mainTabContainer.Childs.Add(thirdPanel);
			return mainTabContainer;
		}

		#endregion
	}
}