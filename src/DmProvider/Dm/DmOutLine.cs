using System;
using System.Collections.Generic;
using System.Reflection;

namespace Dm
{
	public class DmOutLine
	{
		public static void OutLine()
		{
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			DocEntry docEntry = new DocEntry();
			Type[] array = types;
			foreach (Type type in array)
			{
				object[] customAttributes = type.GetCustomAttributes(inherit: true);
				for (int j = 0; j < customAttributes.Length; j++)
				{
					Attribute attribute = (Attribute)customAttributes[j];
					if (attribute is ShowClassAttribute)
					{
						List<string> list = new List<string>();
						list.Add(((ShowClassAttribute)attribute).Describe);
						list.Add(((ShowClassAttribute)attribute).Syntax);
						list.Add(((ShowClassAttribute)attribute).Note);
						docEntry.Class.Add(type, list);
					}
					else if (attribute is ShowDelegateAttribute)
					{
						List<string> list2 = new List<string>();
						list2.Add(((ShowDelegateAttribute)attribute).Describe);
						list2.Add(((ShowDelegateAttribute)attribute).Syntax);
						list2.Add(((ShowDelegateAttribute)attribute).Parameter);
						list2.Add(((ShowDelegateAttribute)attribute).Note);
						docEntry.Delegate.Add(type, list2);
						DocDelegate docDelegate = new DocDelegate();
						docDelegate.DeleName = type.FullName;
						docDelegate.Syntax = ((ShowDelegateAttribute)attribute).Syntax;
						docDelegate.Parameter = ((ShowDelegateAttribute)attribute).Parameter;
						docDelegate.Note = ((ShowDelegateAttribute)attribute).Note;
						docDelegate.ToHtml();
					}
					else if (attribute is ShowEnumAttribute)
					{
						List<string> list3 = new List<string>();
						list3.Add(((ShowEnumAttribute)attribute).Describe);
						list3.Add(((ShowEnumAttribute)attribute).Syntax);
						list3.Add(((ShowEnumAttribute)attribute).Member);
						docEntry.Enum.Add(type, list3);
						DocEnum docEnum = new DocEnum();
						docEnum.EnumName = type.FullName;
						docEnum.Syntax = ((ShowEnumAttribute)attribute).Syntax;
						docEnum.Member = ((ShowEnumAttribute)attribute).Member;
						docEnum.ToHtml();
					}
				}
			}
			docEntry.ToHtml();
			foreach (KeyValuePair<Type, List<string>> item in docEntry.Class)
			{
				DocClass docClass = new DocClass();
				Type key = item.Key;
				MethodInfo[] methods = key.GetMethods();
				foreach (MethodInfo methodInfo in methods)
				{
					int num = 0;
					object[] customAttributes = methodInfo.GetCustomAttributes(inherit: true);
					for (int j = 0; j < customAttributes.Length; j++)
					{
						Attribute attribute2 = (Attribute)customAttributes[j];
						if (!(attribute2 is ShowMethodAttribute))
						{
							continue;
						}
						DocMethod docMethod = new DocMethod();
						if (docClass.Name == null)
						{
							docClass.Name = key.FullName;
							if (!docEntry.Class.ContainsKey(key))
							{
								continue;
							}
							List<string> list4 = docEntry.Class[key];
							docClass.Syntax = list4[1];
							docClass.Note = list4[2];
						}
						List<string> list5 = new List<string>();
						list5.Add(methodInfo.Name + num);
						list5.Add(((ShowMethodAttribute)attribute2).Describe);
						list5.Add(((ShowMethodAttribute)attribute2).Syntax);
						list5.Add(((ShowMethodAttribute)attribute2).ExceptionInfo);
						list5.Add(((ShowMethodAttribute)attribute2).Note);
						docClass.Method.Add(methodInfo, list5);
						docMethod.MethodName = methodInfo.Name + num;
						docMethod.Syntax = ((ShowMethodAttribute)attribute2).Syntax;
						docMethod.ExceptionInfo = ((ShowMethodAttribute)attribute2).ExceptionInfo;
						docMethod.Note = ((ShowMethodAttribute)attribute2).Note;
						docMethod.ToHtml();
					}
					num++;
				}
				if (docClass.Name != null)
				{
					docClass.ToHtml();
				}
			}
		}
	}
}
