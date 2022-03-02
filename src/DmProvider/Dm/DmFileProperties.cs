using System;
using System.Collections.Generic;
using System.IO;

namespace Dm
{
	public class DmFileProperties
	{
		private static char[] m_KeyValSpliter = new char[1] { '=' };

		private Dictionary<string, string> m_KeyValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private string m_filename = "";

		private void SetFilename(string filename)
		{
			m_filename = filename;
		}

		public string GetFilename()
		{
			return m_filename;
		}

		private void AddProperty(string key, string value)
		{
			m_KeyValues.Add(key, value);
		}

		private bool ParseStr(string line)
		{
			string[] array = line.Split(m_KeyValSpliter);
			if (array.Length == 2)
			{
				AddProperty(array[0], array[1]);
				return true;
			}
			return false;
		}

		private void LoadFile()
		{
			try
			{
				using StreamReader streamReader = new StreamReader(m_filename);
				string line;
				while ((line = streamReader.ReadLine()) != null)
				{
					ParseStr(line);
				}
			}
			catch (Exception)
			{
			}
		}

		public string[] GetProperty(string propertyName, string startWith, string endWith, string spliter)
		{
			string property = GetProperty(propertyName);
			if (property == null)
			{
				return null;
			}
			try
			{
				property = property.Trim();
				if (!property.StartsWith(startWith) || !property.EndsWith(endWith))
				{
					return null;
				}
				int num = property.IndexOf(startWith) + startWith.Length;
				int num2 = property.LastIndexOf(endWith) - 1;
				string[] array = property.Substring(num, num2 - num + 1).Split(spliter.ToCharArray());
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].Trim();
				}
				return array;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public string GetProperty(string propertyName)
		{
			try
			{
				if (!m_KeyValues.ContainsKey(propertyName))
				{
					return null;
				}
				return m_KeyValues[propertyName];
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public DmFileProperties(string filename)
		{
			SetFilename(filename);
			LoadFile();
		}

		public DmFileProperties()
		{
			OperatingSystem oSVersion = Environment.OSVersion;
			string text = "unknown";
			SetFilename((!oSVersion.VersionString.Contains("Windows")) ? "/etc/dm_svc.conf" : (Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\dm_svc.conf"));
			LoadFile();
		}
	}
}
