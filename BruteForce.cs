using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


// TODO:
// GPU Acceleration - ILGPU?


namespace JoaatBruteForcer
{
	internal static class CBruteForceMgr
	{
#pragma warning disable CS8618
		// Main Form
		private static MainForm _mainForm;
#pragma warning restore CS8618
		// Regex for finding hexadecimal numbers in the hash list
		private static Regex _hexRegex = new Regex(@"0x[A-F0-9]+", RegexOptions.IgnoreCase);

		// HashSet of all unique hashes to brute force
		private static HashSet<uint> _hashesToBruteForce = new HashSet<uint>();
		// List of string to hash while in string hash mode
		private static List<string> _stringsToHash = new List<string>();
		// StringBuilder of matched strings found while brute forcing, or the results of string hash mode
		private static StringBuilder _matchedStrings = new StringBuilder();
		// Keeps track of the words each thread is on
		private static string[] _threadDictionaryData = Array.Empty<string>();
		// Array of all dictionaries being used
		private static sDictionary[] _dictionaries = Array.Empty<sDictionary>();


		// Current state of the brute force
		private static eBruteForceState _state = eBruteForceState.Inactive;
		// The number of dictionaries being used in the brute force
		private static int _dictionaryCount = 0;
		// Boolean telling the brute force to abort
		private static bool _abort = false;
		// Boolean telling the brute force to pause
		private static bool _pause = false;
		// ManualResetEvent for pausing a brute force
		private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
		// Time the brute force was started
		private static long _startTime = 0;
		// Time the brute force ended
		private static long _endTime = 0;
		// TimeSpan of when the brute force completed
		private static TimeSpan _completionTime;
		// Brute force completion percentage
		private static float _percentCompleted = 0.0f;
		// Brute force format (tbFormat)
		private static string _format = "";
		// Output format (tbOutput)
		private static string _outputFormat = "";


		public static void SetMainForm(MainForm form)
		{
			_mainForm = form;
		}


		// Parses the hash list and validates the hashes
		public static bool ParseHashList(string[] ar)
		{
			if (Settings.bStringHashMode)
			{
				_stringsToHash.Clear();
				for (int i = 0; i < ar.Length; i++)
				{
					_stringsToHash.Add(ar[i]);
				}
				return true;
			}

			if (ar.Length == 0)
			{
				CMessageBox.Error("Cannot start brute force, hash list is empty.");
				return false;
			}

			_hashesToBruteForce.Clear();
			List<string> invalidhashes = new List<string>();
			for (int i = 0; i < ar.Length; i++)
			{
				string str = ar[i].Trim();
				if (string.IsNullOrWhiteSpace(str)) continue;

				// Parse hex string
				if (_hexRegex.IsMatch(str))
				{
					string value = _hexRegex.Match(str).Value;
					uint hash = Convert.ToUInt32(value, 16);
					_hashesToBruteForce.Add(hash);
				}
				// Parse unsigned
				else if (uint.TryParse(str, out uint uint_value))
				{
					_hashesToBruteForce.Add(uint_value);
				}
				// Parse signed
				else if (int.TryParse(str, out int int_value))
				{
					_hashesToBruteForce.Add((uint)int_value);
				}
				else
				{
					invalidhashes.Add(str);
				}
			}

			if (invalidhashes.Count > 0)
			{
				string temp = "";
				// If there are a lot of invalid hashes, limit the max shown to 20.
				// Anything greater, nothing will be shown except the number
				if (invalidhashes.Count < 21)
				{
					Array.ForEach(invalidhashes.ToArray(), line =>
					{
						temp += line + "\n";
					});
				}

				var r = CMessageBox.Warn($"{invalidhashes.Count} invalid hashes were found. Invalid 32-bit number(s). Would you like to skip these hashes and continue the brute force?\n\n{temp}", MessageBoxButtons.YesNoCancel);
				if (r == DialogResult.Yes)
				{
					if (invalidhashes.Count == ar.Length)
					{
						CMessageBox.Error($"No valid hashes to brute force");
						return false;
					}

					return true;
				}
				return false;
			}

			return true;
		}


		// Allocates and assigns dictionaries to "_dictionaries"
		private static bool AssignDictionaries()
		{
			string[] fileNames = _mainForm.Invoke(_mainForm.GetFileNamesFromFormat);
			_dictionaryCount = fileNames.Length;
			_dictionaries = new sDictionary[_dictionaryCount];
			Dictionary<string, string[]> dictDatas = new();

			for (int i = 0; i < fileNames.Length; i++)
			{
				string name = fileNames[i];
				string path = Path.Combine(Environment.CurrentDirectory, $"list.{name}.txt");

				try
				{
					sDictionary dictionary = new();
					if (dictDatas.ContainsKey(name))
					{
						dictionary.Name = name;
						// No need to re-read this file if we've already read it previously.
						// We already checked if this file exists in validateBruteForceFormat()
						dictionary.Data = dictDatas[name];
					}
					else
					{
						dictionary.Name = name;
						dictionary.Data = File.ReadAllLines(path);
						dictDatas.Add(dictionary.Name, dictionary.Data);
					}

					_mainForm.Invoke(() => _mainForm.SetDictionaryStringCase(ref dictionary, i));
					_dictionaries[i] = dictionary;
				}
				catch (Exception e)
				{
					CMessageBox.Error($"Failed to create and assign dictionary, failed to read file '{name}':\n\n{e.Message}");
					_mainForm.Invoke(() => _mainForm.SetComponentsEnabled(true));
					return false;
				}
			}

			_threadDictionaryData = new string[_dictionaryCount];
			return true;
		}


		// Converts a string to uppercase or lowercase depending on the format of the brute force (_format)
		private static void SetStringCase(ref string str, ref sDictionary dictionary)
		{
			if (dictionary.Uppercase)
			{
				str = str.ToUpperInvariant(); // Faster than ToUpper()
			}
			else if (dictionary.Lowercase)
			{
				str = str.ToLowerInvariant(); // Faster than ToLower()
			}
			else if (dictionary.FirstLetterLowercase)
			{
				if (char.IsUpper(str[0]))
				{
					str = string.Concat(char.ToLower(str[0]), str.Substring(1));
				}
			}
		}


		// Calculates a Jenkins One-At-A-Time hash for a given string
		private static uint joaat(string key)
		{
			uint hash = 0;

			for (int i = 0; i < key.Length; i++)
			{
				hash += key[i];
				hash += (hash << 10);
				hash ^= (hash >> 6);
			}

			hash += (hash << 3);
			hash ^= (hash >> 11);
			hash += (hash << 15);

			return hash;
		}


		// Adds a matched or generated string to the output
		private static void AddStringToOutput(string key, uint hash)
		{
			string result = "";

			switch (Settings.OutputMode)
			{
				case eOutputMode.kBoth:
					if (Settings.bUnsignedIntegers)
					{
						result = string.Concat(key, " = ", "0x", hash.ToString("X8"), " (", (hash).ToString(), ")");
					}
					else
					{
						result = string.Concat(key, " = ", "0x", hash.ToString("X8"), " (", ((int)hash).ToString(), ")");
					}
					break;
				case eOutputMode.kHexadecimal:
					result = string.Concat(key, " = ", "0x", hash.ToString("X8"));
					break;
				case eOutputMode.kDecimal:
					if (Settings.bUnsignedIntegers)
					{
						result = string.Concat(key, " = ", (hash).ToString());
					}
					else
					{
						result = string.Concat(key, " = ", ((int)hash).ToString());
					}
					break;
				case eOutputMode.kString:
					result = key;
					break;
				case eOutputMode.kCustom:
					if (_outputFormat == string.Empty)
					{
						_outputFormat = _mainForm.GetOutputFormat();
					}

					// if string.Format errors, then it means there was a misspelling with one of the options
					// {0} = {hash}, {1} = {hex}, {2} = {dec}
					if (Settings.bUnsignedIntegers)
					{
						result = string.Format(_outputFormat, key, "0x" + hash.ToString("X8"), (hash).ToString());
					}
					else
					{
						result = string.Format(_outputFormat, key, "0x" + hash.ToString("X8"), ((int)hash).ToString());
					}
				break;
			}

			// We're only appending tbOutput here because we want to let the
			// user know that we found a hash now, instead of telling them at the end.
			//
			// Don't append text here if we're in string hash mode, it will be appended when its done generating hashes
			if (!Settings.bStringHashMode && Settings.bRealTimeUIUpdates)
			{
				_mainForm.Invoke(() => _mainForm.tbOutput.AppendText(result + "\r\n"));
			}
			_matchedStrings.AppendLine(result);
		}


		// Checks if the given string is a match to a hash in the hash list
		private static void CheckString(string key)
		{
			if (Settings.bForceUppercase)
			{
				key = key.ToUpperInvariant();
			}

			if (Settings.bForceLowercase)
			{
				key = key.ToLowerInvariant();
			}

			uint hash = joaat(key);
			if (_hashesToBruteForce.Contains(hash))
			{
				AddStringToOutput(key, hash);
			}
		}


		// Calculate the JOAAT hashes in the hash list for string hash mode
		public static void GenerateJoaatHashes()
		{
			_matchedStrings.Clear();

			for (int i = 0; i < _stringsToHash.Count; i++)
			{
				string str = _stringsToHash[i];

				if (Settings.bForceUppercase)
				{
					str = str.ToUpperInvariant();
				}

				if (Settings.bForceLowercase)
				{
					str = str.ToLowerInvariant();
				}

				uint hash = joaat(str);
				AddStringToOutput(str, hash);
			}

			// Fine because this function is not called cross-threaded
			_mainForm.tbOutput.Text = _matchedStrings.ToString();
			CompleteBruteForce();
		}


		private static int __count = 0; // For EST Time Remaining
		private static int __index = 0; // For EST Time Remaining
		private static DateTime __start; // For EST Time Remaining
		private static bool __update = false; // For EST Time Remaining
		// Updates the percent and est time text labels
		private static void UpdateTextLabels()
		{
			while (true)
			{
				if (_abort || _percentCompleted == 100.0f) { break; }

				if (!_pause)
				{
					//_mainForm.lblPercent.Text = fPercentCompleted.ToString("n2") + '%';
					_mainForm.lblPercent.Invoke(() => _mainForm.lblPercent.Text = _percentCompleted.ToString("n2") + '%');

					if (__update)
					{
						TimeSpan timeRemaining = TimeSpan.FromTicks(DateTime.Now.Subtract(__start).Ticks * (__count - (__index + 1)) / (__index + 1));
						_mainForm.lblPercent.Invoke(() => _mainForm.lblTimeRemaining.Text = "EST Time Remaining: " + timeRemaining.ToString(@"hh\:mm\:ss"));
						__update = false;
					}
				}
			}
		}


		// Worker thread function that combines strings to be brute forced
		private static void WorkerThreadFunction(object? threadIdx)
		{
#pragma warning disable 8605
			//if (threadIdx == null) throw new ArgumentNullException(nameof(threadIdx));
			int idx = (int)threadIdx;
#pragma warning restore 8605

			sDictionary dictionary = _dictionaries[idx];
			ReadOnlySpan<string> data = dictionary.Data;
			int len = data.Length;

			for (int i = 0; i < len; i++)
			{
				if (_abort) { break; }
				if (_pause) { _manualResetEvent.Reset(); _manualResetEvent.WaitOne(); _pause = false; }

				string line = data[i];
				SetStringCase(ref line, ref dictionary);
				_threadDictionaryData[idx] = line;
				line = string.Format(_format, _threadDictionaryData);
				CheckString(line);

				if (idx != _dictionaryCount - 1)
				{
					WorkerThreadFunction(idx + 1);
				}
			}
		}


		// Start the brute force
		public static void StartBruteForce()
		{
			if (!AssignDictionaries())
			{
				return;
			}

			_percentCompleted = 0.0f;
			_startTime = Stopwatch.GetTimestamp();
			_format = _mainForm.Invoke(_mainForm.GetBruteForceFormat);
			_outputFormat = _mainForm.Invoke(_mainForm.GetOutputFormat);
			_state = eBruteForceState.Running;

			// This is kinda lame as fuck
			if (_dictionaryCount > 1 && Settings.bRealTimeUIUpdates)
			{
				Thread thread = new(UpdateTextLabels);
				thread.Start(); // Don't join
			}

			// 0 is this thread
			sDictionary dictionary = _dictionaries[0];
			ReadOnlySpan<string> data = dictionary.Data;
			int len = data.Length;

			__start = DateTime.Now;
			__count = len;
			for (int i = 0; i < len; i++)
			{
				if (_abort) { break; }
				if (_pause) { _manualResetEvent.Reset(); _manualResetEvent.WaitOne(); _pause = false; }

				string line = data[i];
				SetStringCase(ref line, ref dictionary);
				_threadDictionaryData[0] = line;
				// This is faster than StringBuilder apparently
				line = string.Format(_format, _threadDictionaryData); // TODO: Any faster way to do this instead of string.Format? This is too slow
				CheckString(line);

				// I feel like this is pretty inefficient and could be done better
				if (_dictionaryCount > 1)
				{
					Thread thread = new Thread(WorkerThreadFunction);
					thread.Start(1); // 0 is this thread, so 1 goes here.
					thread.Join();
				}

				if (Settings.bRealTimeUIUpdates)
				{
					__index = i;
					__update = true;
					// Cross-threaded operations here but I don't care (needs to be moved)
					_percentCompleted = (i + 1.0f) * _mainForm.bruteForceprogressBar.Maximum / (float)len;
					_mainForm.bruteForceprogressBar.Value = (int)_percentCompleted;
				}
			}

			if (!_abort)
			{
				_mainForm.lblPercent.Invoke(() => _mainForm.lblPercent.Text = "100.00% (" + Stopwatch.GetElapsedTime(_startTime).ToString(@"hh\:mm\:ss\.ff") + ")");
			}
			else
			{
				_mainForm.lblPercent.Invoke(() => _mainForm.lblPercent.Text += " (" + Stopwatch.GetElapsedTime(_startTime).ToString(@"hh\:mm\:ss\.ff") + ") (Aborted)");
			}
			_mainForm.lblPercent.Invoke(() => _mainForm.lblTimeRemaining.Text = "EST Time Remaining: 00:00:00");
			
			_percentCompleted = 100.0f;

			CompleteBruteForce();
		}


		// Abort/Cancel the brute force
		public static void Abort()
		{
			if (_pause)
			{
				Resume();
			}
			_abort = true;
		}


		// Pause the brute force
		public static void Pause()
		{
			_state = eBruteForceState.Paused;
			_pause = true;
		}


		// Resume the brute force
		public static void Resume()
		{
			if (_pause)
			{
				_state = eBruteForceState.Running;
				_manualResetEvent.Set();
			}
		}


		// Cleans up data and completes the brute force
		private static void CompleteBruteForce()
		{
			_mainForm.Invoke(() => _mainForm.SetComponentsEnabled(true));
			_mainForm.Invoke(_mainForm.SetMiscComponents);

			if (!Settings.bRealTimeUIUpdates)
			{
				_mainForm.Invoke(() =>
				{
					_mainForm.tbOutput.Text = _matchedStrings.ToString();
					_mainForm.bruteForceprogressBar.Value = 100;
				});
			}

			if (_state == eBruteForceState.Running || Settings.bStringHashMode)
			{
				ClearArrays();
			}

			SetCompletionTimestamps();
			ResetVariables();
		}


		// Sets "_endTime" and "_completionTime" timestamps
		private static void SetCompletionTimestamps()
		{
			_endTime = Stopwatch.GetTimestamp();
			_completionTime = Stopwatch.GetElapsedTime(_startTime, _endTime);
		}


		// Clears all arrays of all data
		private static void ClearArrays()
		{
			_hashesToBruteForce.Clear();
			_matchedStrings.Clear();
			_stringsToHash.Clear();
			Array.Clear(_threadDictionaryData, 0, _dictionaryCount);
			Array.Clear(_dictionaries, 0, _dictionaryCount);
		}


		// Resets (some) brute force variables back to their default values
		private static void ResetVariables()
		{
			_abort = false;
			_pause = false;
			_state = eBruteForceState.Inactive;
			_dictionaryCount = 0;
			_format = "";
			_outputFormat = "";
		}


		// Checks if the brute force state is "eBruteForceState.Inactive"
		public static bool IsInactive()
		{
			return _state == eBruteForceState.Inactive;
		}


		// Checks if the brute force state is "eBruteForceState.Running"
		public static bool IsRunning()
		{
			return _state == eBruteForceState.Running;
		}


		// Checks if the brute force state is "eBruteForceState.Paused"
		public static bool IsPaused()
		{
			return _state == eBruteForceState.Paused;
		}
	}
}
