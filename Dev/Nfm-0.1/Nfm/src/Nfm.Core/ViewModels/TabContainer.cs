// <copyright file="TabContainer.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-23</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-23</date>
// </editor>
// <summary>TabControl-based <see cref="IPanel"/> container.</summary>

using System.Diagnostics;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// TabControl-based <see cref="IPanel"/> container.
	/// </summary>
	public class TabContainer : PanelContainerBase
	{
		private IPanel selectedTab;

		/// <summary>
		/// Gets or sets selected tab panel.
		/// </summary>
		public IPanel SelectedTab
		{
			get { return selectedTab; }
			set
			{
//				OnPropertyChanging("SelectedTab");

//				var tabHeader = (selectedTab != null && selectedTab.Header != null)
//									? selectedTab.Header
//									: "null.";
				
//				Debug.WriteLine(Header + ", old selected: " + tabHeader);
				
				selectedTab = value;

//				tabHeader = (selectedTab != null && selectedTab.Header != null)
//									? selectedTab.Header
//									: "null.";

//				Debug.WriteLine(Header + ", new selected: " + tabHeader);

//				OnPropertyChanged("SelectedTab");
			}
		}
	}
}