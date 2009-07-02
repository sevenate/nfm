// <copyright file="PanelHeader.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-16</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-06-16</date>
// </editor>
// <summary>Common <see cref="IPanel"/> header.</summary>

using System.Diagnostics;
using System.Windows.Media;

namespace Nfm.Core.ViewModels
{
	/// <summary>
	/// Common <see cref="IPanel"/> header.
	/// </summary>
	[DebuggerDisplay("{Text}")]
	public class PanelHeader : NotificationBase, IPanelHeader
	{
		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelHeader"/> class.
		/// </summary>
		public PanelHeader()
		{
			isVisible = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelHeader"/> class.
		/// </summary>
		/// <param name="another">Another <see cref="PanelHeader"/> instance to copy data from.</param>
		protected PanelHeader(PanelHeader another)
		{
			Icon = another.Icon;
			Text = another.Text;
			IsVisible = another.IsVisible;
		}

		#endregion

		#region Implementation of ICloneable

		/// <summary>
		/// Creates a new object that is a deep copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a deep copy of this instance.</returns>
		public object Clone()
		{
			return new PanelHeader(this);
		}

		#endregion

		#region Implementation of IPanelHeader

		/// <summary>
		/// Header icon.
		/// </summary>
		private ImageSource icon;

		/// <summary>
		/// Header text.
		/// </summary>
		private string text;

		/// <summary>
		/// Header visibility flag.
		/// </summary>
		private bool isVisible;

		/// <summary>
		/// Gets or sets header icon.
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
		/// Gets or sets header text.
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
		/// Gets or sets a value indicating whether header is visible.
		/// </summary>
		public bool IsVisible
		{
			get { return isVisible; }
			set
			{
				OnPropertyChanging("IsVisible");
				OnPropertyChanging("IsHidden");
				isVisible = value;
				OnPropertyChanged("IsHidden");
				OnPropertyChanged("IsVisible");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether header is hidden.
		/// </summary>
		public bool IsHidden
		{
			get { return !isVisible; }
		}

		#endregion

		#region Additional Binding Properties

		/// <summary>
		/// Flag indicating whether header has icon.
		/// </summary>
		private bool hasIcon;

		/// <summary>
		/// Flag indicating whether header has text.
		/// </summary>
		private bool hasText;

		/// <summary>
		/// Gets or sets a value indicating whether header has icon or not.
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
		/// Gets or sets a value indicating whether header has text or not.
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

		#endregion
	}
}