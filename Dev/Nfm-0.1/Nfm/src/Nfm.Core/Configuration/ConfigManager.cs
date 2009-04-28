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

			TabContainer subTabContainer1 = GetEnterTopTabContainer();
			TabContainer subTabContainer2 = GetDisksTopTabSubContainer();
			TabContainer topTabContainer = GetTopTabContainer(subTabContainer1, subTabContainer2);

			TabContainer mainTabContainer = GetMainTabContainer(workStackContainer, enterPanel, topTabContainer);

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
			IViewModel driveC = RootNode.Inst.GetNode(@"\LocalFileSystem\C:\");
			driveC.Refresh();

			var driveCPanel = new PanelBase
			                  {
			                  	PanelContent = (IPanelContent) driveC,
			                  	IsSelected = true
			                  };

			IViewModel driveD = RootNode.Inst.GetNode(@"\LocalFileSystem\D:\");
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
			workLeftTabContainer.Childs.Add(driveCPanel);
			return workLeftTabContainer;
		}

		/// <summary>
		/// Get Work Middle Tab Container.
		/// </summary>
		/// <returns>Work Middle Tab Container.</returns>
		private static TabContainer GetWorkMiddleTabContainer()
		{
			IViewModel workMiddle = RootNode.Inst.GetNode(@"\LocalFileSystem\D:\Downloads");
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
			IViewModel workRight1 = RootNode.Inst.GetNode(@"\LocalFileSystem\D:\Games");
			workRight1.Refresh();

			var workRightPanel1 = new PanelBase
			                      {
			                      	PanelContent = (IPanelContent) workRight1
			                      };

			IViewModel workRight2 = RootNode.Inst.GetNode(@"\LocalFileSystem\D:\Music");
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
			IViewModel enter = RootNode.Inst.GetNode(@"\LocalFileSystem\D:\Videos");
			enter.Refresh();

			return new PanelBase
			       {
			       	PanelContent = (IPanelContent) enter,
			       	IsSelected = true
			       };
		}

		/// <summary>
		/// Get Enter Top Tab Container.
		/// </summary>
		/// <returns>Enter Top Tab Container.</returns>
		private static TabContainer GetEnterTopTabContainer()
		{
			IViewModel topGames = RootNode.Inst.GetNode(@"\LocalFileSystem\D:\Games");
			topGames.Refresh();

			var topGamesPanel = new PanelBase
			                    {
			                    	PanelContent = (IPanelContent) topGames
			                    };

			IViewModel topMusic = RootNode.Inst.GetNode(@"\LocalFileSystem\D:\Music");
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
			IViewModel topDriveC = RootNode.Inst.GetNode(@"\LocalFileSystem\C:\");
			topDriveC.Refresh();

			var topDriveCpanel = new PanelBase
			                     {
			                     	PanelContent = (IPanelContent) topDriveC
			                     };

			IViewModel topDriveD = RootNode.Inst.GetNode(@"\LocalFileSystem\D:\");
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
		/// <param name="workStackContainer">Work Stack Container.</param>
		/// <param name="enterPanel">Rnter Panel.</param>
		/// <param name="topTabContainer">Top Tab Container.</param>
		/// <returns>Main Tab Container.</returns>
		private static TabContainer GetMainTabContainer(IPanel workStackContainer, IPanel enterPanel, IPanel topTabContainer)
		{
			var mainTabContainer = new TabContainer
			                       {
			                       	Header = "Main Window",
			                       };
			mainTabContainer.Childs.Add(workStackContainer);
			mainTabContainer.Childs.Add(enterPanel);
			mainTabContainer.Childs.Add(topTabContainer);
			return mainTabContainer;
		}

		#endregion
	}
}