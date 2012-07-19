// <copyright file="ToolbarItemBase.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-28</date>
// </editor>
// <summary>Base <see cref="IToolbarItem"/> implementation.</summary>

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Base <see cref="IToolbarItem"/> implementation.
	/// </summary>
	[DebuggerDisplay("{Text}"
	                 + " {Childs.Count}")]
	public class ToolbarItemBase : NotificationBase, IToolbarItem
	{
		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolbarItemBase"/> class.
		/// </summary>
		public ToolbarItemBase()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ToolbarItemBase"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="ToolbarItemBase"/> instance to copy data from.</param>
		protected ToolbarItemBase(ToolbarItemBase another)
		{
			Icon = another.Icon;
			Text = another.Text;
			Tooltip = another.Tooltip;

			// Deep copy all childs
			var childsCopy = new ObservableCollection<IToolbarItem>();

			foreach (IToolbarItem child in another.Childs)
			{
				var newChild = (IToolbarItem) child.Clone();
				childsCopy.Add(newChild);
			}

			childs = childsCopy;
		}

		#endregion

		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public object Clone()
		{
			return new ToolbarItemBase(this);
		}

		#endregion

		#region Implementation of IToolbar

		/// <summary>
		/// Child toolbar items.
		/// </summary>
		private ObservableCollection<IToolbarItem> childs;

		/// <summary>
		/// Toolbar icon.
		/// </summary>
		private ImageSource icon;

		/// <summary>
		/// Toolbar text.
		/// </summary>
		private string text;

		/// <summary>
		/// Toolbar tooltip.
		/// </summary>
		private string tooltip;

		/// <summary>
		/// Gets all child toolbar items.
		/// </summary>
		public ObservableCollection<IToolbarItem> Childs
		{
			get
			{
				if (childs == null)
				{
					OnPropertyChanging("Childs");
					childs = new ObservableCollection<IToolbarItem>();
					OnPropertyChanged("Childs");
				}

				return childs;
			}
		}

		/// <summary>
		/// Gets or sets toolbar icon.
		/// </summary>
		public ImageSource Icon
		{
			get { return icon; }
			set
			{
				OnPropertyChanging("Icon");
				icon = value;
				HasIcon = icon != null;
				OnPropertyChanged("Icon");
			}
		}

		/// <summary>
		/// Gets or sets toolbar text.
		/// </summary>
		public string Text
		{
			get { return text; }
			set
			{
				OnPropertyChanging("Text");
				text = value;
				HasText = !string.IsNullOrEmpty(text);
				OnPropertyChanged("Text");
			}
		}

		/// <summary>
		/// Gets or sets toolbar item tooltip.
		/// </summary>
		public string Tooltip
		{
			get { return tooltip; }
			set
			{
				OnPropertyChanging("Tooltip");
				tooltip = value;
				HasTooltip = !string.IsNullOrEmpty(tooltip);
				OnPropertyChanged("Tooltip");
			}
		}

		#endregion

		#region Additional Binding Properties

		/// <summary>
		/// Flag indicating whether toolbar has icon.
		/// </summary>
		private bool hasIcon;

		/// <summary>
		/// Flag indicating whether toolbar has text.
		/// </summary>
		private bool hasText;

		/// <summary>
		/// Flag indicating whether toolbar has tooltip.
		/// </summary>
		private bool hasTooltip;

		/// <summary>
		/// Gets or sets a value indicating whether toolbar has icon.
		/// </summary>
		public bool HasIcon
		{
			get { return hasIcon; }
			set
			{
				OnPropertyChanging("HasIcon");
				hasIcon = value;
				OnPropertyChanged("HasIcon");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether toolbar has text.
		/// </summary>
		public bool HasText
		{
			get { return hasText; }
			set
			{
				OnPropertyChanging("HasText");
				hasText = value;
				OnPropertyChanged("HasText");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether toolbar has tooltip.
		/// </summary>
		public bool HasTooltip
		{
			get { return hasTooltip; }
			set
			{
				OnPropertyChanging("HasTooltip");
				hasTooltip = value;
				OnPropertyChanged("HasTooltip");
			}
		}

		#endregion
	}
}