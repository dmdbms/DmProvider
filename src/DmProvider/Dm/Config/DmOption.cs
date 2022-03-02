using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dm.Config
{
	internal class DmOption
	{
		private static Dictionary<string, EPGroup> epGroupMap;

		private static object epGroupMapLock = new object();

		internal string Keyword { get; private set; }

		internal string[] Synonym { get; private set; }

		internal Type BaseType { get; private set; }

		internal object Defaultvalue { get; private set; }

		internal long? Maxvalue { get; private set; }

		internal long? Minvalue { get; private set; }

		internal DmOption(string key, Type basetype, object defaultvalue = null, string syn = null, long? maxvalue = null, long? minvalue = 0L)
		{
			Keyword = key;
			Synonym = syn?.Split(',');
			Defaultvalue = defaultvalue;
			BaseType = basetype;
			Maxvalue = maxvalue;
			Minvalue = minvalue;
		}

		internal bool HasKey(string key)
		{
			if (string.Compare(key, Keyword, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			if (Synonym != null)
			{
				string[] synonym = Synonym;
				foreach (string strB in synonym)
				{
					if (string.Compare(key, strB, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		internal object ValidateValue(object value)
		{
			try
			{
				if (value == null)
				{
					value = Defaultvalue;
				}
				if (BaseType == typeof(bool))
				{
					if (value.GetType() == typeof(string) && (string.Compare("yes", value.ToString(), StringComparison.OrdinalIgnoreCase) == 0 || string.Compare("y", value.ToString(), StringComparison.OrdinalIgnoreCase) == 0))
					{
						return true;
					}
					if (value.GetType() == typeof(string) && (string.Compare("no", value.ToString(), StringComparison.OrdinalIgnoreCase) == 0 || string.Compare("n", value.ToString(), StringComparison.OrdinalIgnoreCase) == 0))
					{
						return false;
					}
					if (bool.TryParse(value.ToString(), out var result))
					{
						return result;
					}
					if (int.TryParse(value.ToString(), out var result2))
					{
						return result2 > 0;
					}
				}
				else
				{
					if (BaseType == typeof(long) && long.TryParse(value.ToString(), out var result3))
					{
						if (Maxvalue.HasValue && result3 > Maxvalue.Value)
						{
							return Defaultvalue;
						}
						if (Minvalue.HasValue && result3 < Minvalue.Value)
						{
							return Defaultvalue;
						}
						if (result3 > long.MaxValue || result3 < long.MinValue)
						{
							throw new ArgumentException("over flow");
						}
						return result3;
					}
					if (BaseType == typeof(string))
					{
						if (value.ToString()!.Trim().Equals(string.Empty))
						{
							return Defaultvalue;
						}
						if (value.ToString()!.Trim().Length > 128)
						{
							DmError.ThrowDmException(DmErrorDefinition.ECNET_NAME_TOO_LONG);
						}
						if (string.Compare("encoding", Keyword, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare("utf-8", value.ToString(), StringComparison.OrdinalIgnoreCase) != 0 && string.Compare("gb18030", value.ToString(), StringComparison.OrdinalIgnoreCase) != 0 && string.Compare("euc-kr", value.ToString(), StringComparison.OrdinalIgnoreCase) != 0)
						{
							return Defaultvalue;
						}
						if (string.Compare("dm_svc_conf", Keyword, StringComparison.OrdinalIgnoreCase) == 0)
						{
							try
							{
								new StreamReader(value.ToString()!.Trim());
							}
							catch (Exception)
							{
								return Defaultvalue;
							}
						}
						return value.ToString()!.Trim();
					}
					if (BaseType == typeof(string[]))
					{
						if (value is string[])
						{
							return value;
						}
						if (value.ToString()!.Length == 0)
						{
							return Defaultvalue;
						}
						return value.ToString()!.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					}
					if (BaseType == typeof(EPGroup))
					{
						if (value is string[])
						{
							string[] servers = value as string[];
							return ParseEPGroup(servers);
						}
						if (value.ToString()!.Length == 0)
						{
							return Defaultvalue;
						}
						return ParseEPGroup(value.ToString()!.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries));
					}
					if (BaseType.BaseType == typeof(Enum))
					{
						if (long.TryParse(value.ToString(), out var result4))
						{
							Array values = Enum.GetValues(BaseType);
							for (int i = 0; i < values.Length; i++)
							{
								if (values.GetValue(i)!.Equals(Enum.ToObject(BaseType, result4)))
								{
									return values.GetValue(i);
								}
							}
							return Defaultvalue;
						}
						object obj = Enum.Parse(BaseType, value.ToString()!.Trim(), ignoreCase: true);
						if (obj != null)
						{
							return obj;
						}
					}
				}
				return Defaultvalue;
			}
			catch (Exception)
			{
				return Defaultvalue;
			}
		}

		private EPGroup ParseEPGroup(string[] servers)
		{
			if (epGroupMap == null)
			{
				lock (epGroupMapLock)
				{
					if (epGroupMap == null)
					{
						epGroupMap = new Dictionary<string, EPGroup>(4);
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder(32);
			string[] array = servers;
			foreach (string value in array)
			{
				stringBuilder.Append(value);
			}
			string key = Keyword + stringBuilder.ToString();
			EPGroup value2 = null;
			epGroupMap.TryGetValue(key, out value2);
			if (value2 == null)
			{
				List<EP> list = new List<EP>(servers.Length);
				EP eP = null;
				array = servers;
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
					if (array2.Length == 1)
					{
						eP = new EP(array2[0], DmOptionHelper.portDef);
						list.Add(eP);
					}
					else if (array2.Length == 2)
					{
						int port = DmOptionHelper.portDef;
						try
						{
							port = Convert.ToInt32(array2[1]);
						}
						catch (Exception)
						{
						}
						eP = new EP(array2[0], port);
						list.Add(eP);
					}
				}
				lock (epGroupMapLock)
				{
					epGroupMap.TryGetValue(key, out value2);
					if (value2 == null)
					{
						value2 = new EPGroup(list);
						epGroupMap[key] = value2;
						return value2;
					}
					return value2;
				}
			}
			return value2;
		}
	}
}
