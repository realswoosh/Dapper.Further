using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace Dapper.Futher
{
	public static class SqlMapperExtensions
	{
		public static void UpdateFields<T>(this IDbConnection connection, dynamic param, dynamic where, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
		{
			Type type = typeof(T);

			PropertyDescriptorCollection paramCollection = TypeDescriptor.GetProperties(param);
			PropertyDescriptorCollection whereCollection = TypeDescriptor.GetProperties(where);

			var attribute = type.GetCustomAttributes(true)
								.Where(x => x is Contrib.Extensions.TableAttribute).FirstOrDefault();

			Contrib.Extensions.TableAttribute tableAttribute = attribute as Contrib.Extensions.TableAttribute;

			if (attribute == null ||
				tableAttribute == null ||
				paramCollection.Count == 0 ||
				whereCollection.Count == 0)
			{
				throw new Exception();
			}

			StringBuilder sb = new StringBuilder($"update {tableAttribute.Name} set", 256);

			List<string> paramColumnList = new List<string>();
			List<string> whereColumnList = new List<string>();

			foreach (PropertyDescriptor prop in paramCollection)
			{
				Console.WriteLine(prop.Name);
				object val = prop.GetValue(param);
				string col = "";
				if (prop.PropertyType == typeof(Int32) ||
					prop.PropertyType == typeof(Int16))
					col = $"{prop.Name}= {val}";
				else
					col = $"{prop.Name}='{val}'";

				paramColumnList.Add(col);
			}

			sb.AppendFormat(" {0} where ", string.Join(",", paramColumnList));

			foreach (PropertyDescriptor prop in whereCollection)
			{
				Console.WriteLine(prop.Name);
				object val = prop.GetValue(where);
				string col = "";
				if (prop.PropertyType == typeof(Int32) ||
					prop.PropertyType == typeof(Int16))
					col = $"{prop.Name}= {val}";
				else
					col = $"{prop.Name}='{val}'";

				whereColumnList.Add(col);
			}

			sb.Append(string.Join(" and ", whereColumnList));

			connection.Execute(sb.ToString(), null, transaction, commandTimeout);
		}
	}
}
