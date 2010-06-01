// <copyright file="AutoCompleteComboBox.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Syed Mehroz Alam">
// 	<url>http://www.codeproject.com/KB/silverlight/AutoComplete_ComboBox.aspx</url>
// 	<date>2010-02-17</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-06-02</date>
// </editor>
// <summary>"Editable" combobox with autocomplete functionality.</summary>

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Reflection;
using System.Windows.Data;

namespace Fab.Client.Controls
{
	/// <summary>
	/// "Editable" combobox with autocomplete functionality.
	/// </summary>
    public class AutoCompleteComboBox : AutoCompleteBox
    {
        bool isUpdatingDPs;

        #region SelectedItemBinding

        /// <summary>
        /// SelectedItemBinding Dependency Property
        /// </summary>
        public static readonly DependencyProperty SelectedItemBindingProperty =
            DependencyProperty.Register("SelectedItemBinding",
                    typeof(object),
                    typeof(AutoCompleteComboBox),
                    new PropertyMetadata(new PropertyChangedCallback(OnSelectedItemBindingChanged))
                    );

        /// <summary>
        /// Gets or sets the SelectedItemBinding property.
        /// </summary>
        public object SelectedItemBinding
        {
            get { return GetValue(SelectedItemBindingProperty); }
            set { SetValue(SelectedItemBindingProperty, value); }
        }

        /// <summary>
        /// Handles changes to the SelectedItemBinding property.
        /// </summary>
        private static void OnSelectedItemBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoCompleteComboBox)d).OnSelectedItemBindingChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the SelectedItemBinding property.
        /// </summary>
        protected virtual void OnSelectedItemBindingChanged(DependencyPropertyChangedEventArgs e)
        {
            SetSelectemItemUsingSelectedItemBindingDP();
        }

        public void SetSelectemItemUsingSelectedItemBindingDP()
        {
            if (!this.isUpdatingDPs)
                SetValue(SelectedItemProperty, GetValue(SelectedItemBindingProperty));
        }

        #endregion

        #region SelectedValue

        /// <summary>
        /// SelectedValue Dependency Property
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue",
                    typeof(object),
                    typeof(AutoCompleteComboBox),
                    new PropertyMetadata(new PropertyChangedCallback(OnSelectedValueChanged))
                    );

        /// <summary>
        /// Gets or sets the SelectedValue property.
        /// </summary>
        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        /// <summary>
        /// Handles changes to the SelectedValue property.
        /// </summary>
        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoCompleteComboBox)d).OnSelectedValueChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the SelectedValue property.
        /// </summary>
        protected virtual void OnSelectedValueChanged(DependencyPropertyChangedEventArgs e)
        {
            SetSelectemItemUsingSelectedValueDP();
        }

        //selects the item whose value is given in SelectedValueDP
        public void SetSelectemItemUsingSelectedValueDP()
        {
            if (!this.isUpdatingDPs)
            {
                if (this.ItemsSource != null)
                {
                    /// if selectedValue is empty, remove the current selection
                    if (this.SelectedValue == null)
                    {
                        this.SelectedItem = null;
                    }

                    /// if there is no selected item, 
                    /// select the one given by SelectedValueProperty
                    else if (this.SelectedItem == null)
                    {
                        object selectedValue = GetValue(SelectedValueProperty);
                        string propertyPath = this.SelectedValuePath;
                        if (selectedValue != null && !(string.IsNullOrEmpty(propertyPath)))
                        {
                            /// loop through each item in the item source 
                            /// and see if its <SelectedValuePathProperty> == SelectedValue
                            foreach (object item in this.ItemsSource)
                            {
                                PropertyInfo propertyInfo = item.GetType().GetProperty(propertyPath);
                                if (propertyInfo.GetValue(item, null).Equals(selectedValue))
                                    this.SelectedItem = item;
                            }
                        }
                    }

                }
            }
        }


        #endregion

        #region SelectedValuePath

        /// <summary>
        /// SelectedValuePath Dependency Property
        /// </summary>
        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath",
                    typeof(string),
                    typeof(AutoCompleteComboBox),
                    null
                    );

        /// <summary>
        /// Gets or sets the SelectedValuePath property.
        /// </summary>
        public string SelectedValuePath
        {
            get { return GetValue(SelectedValuePathProperty) as string; }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        #endregion


        public AutoCompleteComboBox() : base()
        {
            SetCustomFilter();
            this.DefaultStyleKey = typeof(AutoCompleteComboBox);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ToggleButton toggle = (ToggleButton)GetTemplateChild("DropDownToggle");
            if (toggle != null)
            {
                //toggle.Click -= DropDownToggle_Click;
                toggle.Click += DropDownToggle_Click;
            }
        }

        private void DropDownToggle_Click(object sender, RoutedEventArgs e)
        {
            //this.Focus();
            FrameworkElement fe = sender as FrameworkElement;
            AutoCompleteBox acb = null;
            while (fe != null && acb == null)
            {
                fe = VisualTreeHelper.GetParent(fe) as FrameworkElement;
                acb = fe as AutoCompleteBox;
            }
            if (acb != null)
            {
                acb.IsDropDownOpen = !acb.IsDropDownOpen;
            }
        }

        protected virtual void SetCustomFilter()
        {
            //custom logic: how to autocomplete 
            this.ItemFilter = (prefix, item) =>
            {
                //return all items for empty prefix
                if (string.IsNullOrEmpty(prefix))
                    return true;

                //return all items if a record is already selected
                if (this.SelectedItem != null)
                    if (this.SelectedItem.ToString() == prefix)
                        return true;

                //else return items that contains prefex
                return item.ToString().ToLower().Contains(prefix.ToLower());
            };
        }

        //highlighting logic
        protected override void OnPopulated(PopulatedEventArgs e)
        {
            base.OnPopulated(e);
            ListBox listBox = GetTemplateChild("Selector") as ListBox;
            if (listBox != null)
            {
                //highlight the selected item, if any
                if (this.ItemsSource!=null && this.SelectedItem != null)
                {
                    listBox.SelectedItem = this.SelectedItem;
                    listBox.Dispatcher.BeginInvoke(delegate
                    {
                        listBox.UpdateLayout();
                        listBox.ScrollIntoView(listBox.SelectedItem);
                        //listBox.UpdateLayout();
                    });
                }
            }
        }

        protected override void OnDropDownClosed(RoutedPropertyChangedEventArgs<bool> e)
        {
            base.OnDropDownClosed(e);
            UpdateCustomDPs();
        }

        private void UpdateCustomDPs()
        {
            //flag to ensure that that we dont reselect the selected item
            this.isUpdatingDPs = true;

            //if a new item is selected or the user blanked out the selection, update the DP
            if (this.SelectedItem != null || this.Text == string.Empty)
            {
                //update the SelectedItemBinding DP
                SetValue(SelectedItemBindingProperty, GetValue(SelectedItemProperty));

                //update the SelectedValue DP 
                string propertyPath = this.SelectedValuePath;
                if (!string.IsNullOrEmpty(propertyPath))
                {
                    if (this.SelectedItem != null)
                    {
                        PropertyInfo propertyInfo = this.SelectedItem.GetType().GetProperty(propertyPath);
                        
                        //get property from selected item
                        object propertyValue = propertyInfo.GetValue(this.SelectedItem, null);

                        //update the datacontext
                        this.SelectedValue = propertyValue;
                    }
                    else //user blanked out the selection, so we need to set the default value
                    {                        
                        //get the binding for selectedvalue property
                        BindingExpression bindingExpression = this.GetBindingExpression(SelectedValueProperty);
                        Binding dataBinding = bindingExpression.ParentBinding;
                        
                        //get the dataitem (typically the datacontext)
                        object dataItem = bindingExpression.DataItem;

                        //get the property of that dataitem that's bound to selectedValue property
                        string propertyPathForSelectedValue = dataBinding.Path.Path;
                        
                        //get the default value for that property
                        Type propertyTypeForSelectedValue = dataItem.GetType().GetProperty(propertyPathForSelectedValue).PropertyType;
                        object defaultObj = null;
                        if (propertyTypeForSelectedValue.IsValueType) //get default for value types only
                            defaultObj = Activator.CreateInstance(propertyTypeForSelectedValue);

                        //update the Selected Value property
                        this.SelectedValue = defaultObj;
                    }
                }
            }
            else
            {
                //revert to the orginally selected one
                if (this.GetBindingExpression(SelectedItemBindingProperty) != null)
                {
                    SetSelectemItemUsingSelectedItemBindingDP();
                }

                else if (this.GetBindingExpression(SelectedValueProperty) != null)
                {
                    SetSelectemItemUsingSelectedValueDP();
                }
            }

            this.isUpdatingDPs = false;
        }
    }
}