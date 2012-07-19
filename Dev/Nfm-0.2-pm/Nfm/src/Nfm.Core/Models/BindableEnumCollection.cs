// <copyright file="BindableEnumCollection.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-07-06</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-07-06</date>
// </editor>
// <summary>Represent bindable enums collection.</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Caliburn.Core;
using Caliburn.PresentationFramework;

namespace Nfm.Core.Models
{
	/// <summary>
	/// Represent bindable enums collection.
	/// </summary>
	/// <typeparam name="T">Enum type.</typeparam>
	public class BindableEnumCollection<T> : BindableCollection<BindableEnum> where T : struct
	{
		#region .Ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="BindableEnumCollection{T}"/> class.
		/// </summary>
		public BindableEnumCollection()
		{
			Type type = typeof (T);

			if (!type.IsEnum)
			{
				throw new ArgumentException("This class only supports Enum types.");
			}

			FieldInfo[] fields = typeof (T).GetFields(BindingFlags.Static | BindingFlags.Public);

			foreach (FieldInfo field in fields)
			{
				DescriptionAttribute att = field.GetCustomAttributes(typeof (DescriptionAttribute), false)
					.OfType<DescriptionAttribute>().FirstOrDefault();

				var bindableEnum = new BindableEnum
				                   {
				                   	Value = field.GetValue(null),
				                   	UnderlyingValue = (int) field.GetValue(null),
				                   	DisplayName = att != null
				                   	              	? att.Description
				                   	              	: field.Name
				                   };

				Add(bindableEnum);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BindableEnumCollection{T}"/> class.
		/// </summary>
		/// <param name="values">Allowed enum values from all possible.</param>
		public BindableEnumCollection(params T[] values)
			: this()
		{
			// Remove all NOT specified in params enum values as "not allowed"
			IEnumerable<BindableEnum> toRemove = from bindableEnum in this
			                                     where !values.Contains((T) bindableEnum.Value)
			                                     select bindableEnum;

			toRemove.ToList().Apply(x => Remove(x));
		}

		#endregion
	}
}