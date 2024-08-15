using System.Reflection;

namespace JoaatBruteForcer
{
	internal enum eOutputMode
	{
		kString, // Only show string, no numbers
		kHexadecimal, // Show hexadecimal output
		kDecimal, // Show decimal output
		kBoth, // Show both hexadecimal and decimal output
		kCustom, // Custom output
	}

	internal static class Settings
	{
		public const int MAX_DICTIONARY_COUNT = 10;

		public static bool bStringHashMode = false;
		public static bool bUnsignedIntegers = false;
		public static bool bRealTimeUIUpdates = true;
		public static eOutputMode OutputMode = eOutputMode.kBoth;
		public static string pCustomOutputFormat = string.Empty;
		public static string pBruteForceFormat = string.Empty;

		private static string SettingsFile = Environment.CurrentDirectory + "\\JoaatBruteForcer.settings.txt";
		public static void Load()
		{
			if (!File.Exists(SettingsFile))
			{
				CMessageBox.Error($"\"JoaatBruteForcer.settings.txt\" does not exist.", MessageBoxButtons.OK);
				return;
			}

			string[] data;
			try
			{
				data = File.ReadAllLines(SettingsFile);
			}
			catch (Exception e)
			{
				CMessageBox.Error($"Failed to read settings file.\n\n{e.Message}", MessageBoxButtons.OK);
				return;
			}

			foreach (string line in data)
			{
				if (string.IsNullOrWhiteSpace(line)) { continue; }

				int idx = line.IndexOf('=');
				string varname = "";
				string value = "";
				if (idx != -1)
				{
					varname = line.Substring(0, idx);
					value = line.Substring(idx + 1);
				}

				var field = GetFieldByName(varname);
				if (field != null && !field.IsLiteral)
				{
					Type fieldType = field.FieldType;
					if (fieldType == typeof(bool))
					{
						if (bool.TryParse(value, out bool b))
						{
							field.SetValue(null, b);
						}
					}
					else if (fieldType == typeof(int))
					{
						if (int.TryParse(value, out int i))
						{
							field.SetValue(null, i);
						}
					}
					else if (fieldType == typeof(string))
					{
						field.SetValue(null, value);
					}
					else if (fieldType == typeof(eOutputMode))
					{
						field.SetValue(null, Enum.Parse(typeof(eOutputMode), value));
					}
				}
			}
		}

		public static void Save()
		{
			if (!File.Exists(SettingsFile))
			{
				CMessageBox.Error($"Cannot save settings: \"JoaatBruteForcer.settings.txt\" does not exist.", MessageBoxButtons.OK);
				return;
			}

			string data = string.Empty;

			Type type = typeof(Settings);
			FieldInfo[] fieldsArray = type.GetFields();
			for (int i = 0; i < fieldsArray.Length; i++)
			{
				FieldInfo field = fieldsArray[i];
				if (field.IsPublic && field.IsStatic && !field.IsLiteral)
				{
					AppendSettingString(field.Name, field.GetValue(null), ref data);
				}
			}

			try
			{
				File.WriteAllText(SettingsFile, data);
			}
			catch (Exception e)
			{
				CMessageBox.Error($"Failed to save settings.\n\n{e.Message}", MessageBoxButtons.OK);
			}
		}

		private static void AppendSettingString(string variable, object? value, ref string data)
		{
			data += variable + '=' + value?.ToString() + "\r\n";
		}

		private static FieldInfo? GetFieldByName(string name)
		{
			return typeof(Settings).GetFields().FirstOrDefault(f => f.Name == name);
		}
	}
}
