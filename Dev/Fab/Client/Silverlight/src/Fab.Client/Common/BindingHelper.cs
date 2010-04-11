// <copyright file="BindingHelper.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Thomas Claudius Huber">
// 	<url>http://www.thomasclaudiushuber.com/blog/2009/07/17/here-it-is-the-updatesourcetrigger-for-propertychanged-in-silverlight/</url>
// 	<date>2009-07-17</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-04-08</date>
// </editor>
// <summary>Supports a PropertyChanged-Trigger for DataBindings in Silverlight. Works just for TextBoxes.</summary>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Fab.Client.Common
{
	/// <summary>
	/// Supports a PropertyChanged-Trigger for DataBindings in Silverlight. Works just for TextBoxes.
	/// </summary>
	public class BindingHelper
	{
		/// <summary>
		/// Attached property "UpdateSourceOnChange" registration.
		/// </summary>
		public static readonly DependencyProperty UpdateSourceOnChangeProperty = DependencyProperty.RegisterAttached(
			"UpdateSourceOnChange",
			typeof (bool),
			typeof (BindingHelper),
			new PropertyMetadata(false, OnPropertyChanged));

		/// <summary>
		/// Gets <see cref="UpdateSourceOnChangeProperty"/> attached property value.
		/// </summary>
		/// <param name="obj">Object with attached property.</param>
		/// <returns>Current <see cref="UpdateSourceOnChangeProperty"/> value.</returns>
		public static bool GetUpdateSourceOnChange(DependencyObject obj)
		{
			return (bool) obj.GetValue(UpdateSourceOnChangeProperty);
		}

		/// <summary>
		/// Sets <see cref="UpdateSourceOnChangeProperty"/> attached property value.
		/// </summary>
		/// <param name="obj">Object with attached property.</param>
		/// <param name="value">New value for <see cref="UpdateSourceOnChangeProperty"/>.</param>
		public static void SetUpdateSourceOnChange(DependencyObject obj, bool value)
		{
			obj.SetValue(UpdateSourceOnChangeProperty, value);
		}

		/// <summary>
		/// Property changed callback for <see cref="UpdateSourceOnChangeProperty"/>.
		/// </summary>
		/// <param name="obj">Object with attached property.</param>
		/// <param name="e">Event data.</param>
		private static void OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var textBox = obj as TextBox;

			if (textBox == null)
			{
				return;
			}

			if ((bool) e.NewValue)
			{
				textBox.TextChanged += OnTextChanged;
			}
			else
			{
				textBox.TextChanged -= OnTextChanged;
			}
		}

		/// <summary>
		/// Handler for <see cref="TextBox.TextChanged"/> event.
		/// </summary>
		/// <param name="sender">TextBox control.</param>
		/// <param name="e">Event data.</param>
		private static void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = sender as TextBox;

			if (textBox == null)
			{
				return;
			}

			BindingExpression bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);

			if (bindingExpression != null)
			{
				bindingExpression.UpdateSource();
			}
		}
	}
}