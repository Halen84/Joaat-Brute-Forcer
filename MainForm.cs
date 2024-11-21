using System.Text;
using System.Text.RegularExpressions;

namespace JoaatBruteForcer
{
	public partial class MainForm : Form
	{
		// Regex pattern for finding dictionaries in the brute force format
		public Regex RegexPattern = new Regex(@"{[^{}]*}");
		// Can't remember why I needed this
		private bool _initialized = false;

		public MainForm()
		{
			InitializeComponent();
			Settings.Load();

			// Options
			cbStringHashMode.Checked = Settings.bStringHashMode;
			cbUnsigned.Checked = Settings.bUnsignedIntegers;
			cbRealTimeUpdates.Checked = Settings.bRealTimeUIUpdates;
			// Output
			rbStringOnly.Checked = Settings.OutputMode == eOutputMode.kString;
			rbHex.Checked = Settings.OutputMode == eOutputMode.kHexadecimal;
			rbDec.Checked = Settings.OutputMode == eOutputMode.kDecimal;
			rbHexAndDec.Checked = Settings.OutputMode == eOutputMode.kBoth;
			rbCustom.Checked = Settings.OutputMode == eOutputMode.kCustom;
			tbCustomOutput.Text = Settings.pCustomOutputFormat;
			// Brute Force Format
			tbFormat.Text = Path.GetFileName(Settings.pBruteForceFormat);

			openFileDialog1 = new()
			{
				DefaultExt = "txt",
				RestoreDirectory = true,
				Title = "Select Dictionary File",
				Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*", // "txt files (*.txt)|*.txt",
				CheckFileExists = true,
				CheckPathExists = true,
				Multiselect = false
			};

			_initialized = true;
		}


		// Enables/Disables certain UI components
		public void SetComponentsEnabled(bool flag)
		{
			// Don't need to set components if we're in string hash mode
			if (cbStringHashMode.Checked) { return; }

			gbHashOptions.Enabled = flag;
			//gbOutput.Enabled = flag;

			btnSaveToFile.Enabled = flag;
			tbHashList.ReadOnly = !flag;
			tbHashList.BackColor = Color.White;
			tbFormat.Enabled = flag;
		}


		// Gets the name of a dictionary
		private string trimDictionaryName(string name)
		{
			name = name.TrimStart('{').TrimEnd('}');
			if (name.Contains('*'))
			{
				name = name.Substring(0, name.IndexOf('*'));
			}
			return name;
		}


		// Checks if the custom output format is valid
		private bool validateOutputFormat()
		{
			if (!rbCustom.Checked) return true;

			string text = tbCustomOutput.Text;
			if (string.IsNullOrWhiteSpace(text))
			{
				CMessageBox.Error("Invalid custom output, format is empty");
				return false;
			}

			if (!text.Contains("{hash}") && !text.Contains("{hex}") && !text.Contains("{dec}"))
			{
				CMessageBox.Error("Invalid custom output, format was not valid. Make sure it contains at least one of:\n\n{hash}\n{hex}\n{dec}");
				return false;
			}

			return true;
		}


		// Checks if the brute force format is valid
		private bool validateBruteForceFormat()
		{
			string text = Settings.pBruteForceFormat;

			if (string.IsNullOrWhiteSpace(text))
			{
				CMessageBox.Error("Invalid brute forcer format. The format is empty.");
				return false;
			}

			MatchCollection matches = RegexPattern.Matches(text);
			int dictCount = matches.Count;
			if (dictCount == 0)
			{
				CMessageBox.Error("Invalid format. The format needs at least 1 dictionary.");
				return false;
			}

			if (text.Split('{').Length - 1 != dictCount)
			{
				CMessageBox.Error("Invalid format. One or more '{' were not closed.");
				return false;
			}

			foreach (Match match in matches)
			{
				string name = trimDictionaryName(match.Value);
				string path = Path.Combine(Environment.CurrentDirectory, $"list.{name}.txt");

				if (!File.Exists(path))
				{
					CMessageBox.Error($"Invalid format. Unable to find file 'list.{name}.txt'");
					return false;
				}
			}

			return true;
		}


		// Creates a format string from the custom output format
		public string GetOutputFormat()
		{
			string text = tbCustomOutput.Text;
			text = text.Replace("{hash}", "{0}");
			text = text.Replace("{hex}", "{1}");
			text = text.Replace("{dec}", "{2}");
			return text;
		}


		// Creates a format string from the brute force format
		public string GetBruteForceFormat()
		{
			int idx = 0;

			string result = RegexPattern.Replace(Settings.pBruteForceFormat, match =>
			{
				string s = $"{{{idx++.ToString()}}}";
				return s;
			});

			return result;
		}


		// Sets a dictionary's string case variables from the brute force format
		public void SetDictionaryStringCase(ref sDictionary dictionary, int index)
		{
			var matches = RegexPattern.Matches(Settings.pBruteForceFormat);
			string fmt = matches.ElementAt(index).Value; // "index" should be safe because GetFileNamesFromFormat() goes in order
			string value = fmt.TrimStart('{').TrimEnd('}');

			if (value.EndsWith("*U"))
			{
				dictionary.Uppercase = true;
			}
			else if (value.EndsWith("*L"))
			{
				dictionary.Lowercase = true;
			}
			else if (value.EndsWith("*F"))
			{
				dictionary.FirstLetterLowercase = true;
			}
		}


		// Gets the names of dictionaries in the brute force format
		public string[] GetFileNamesFromFormat()
		{
			List<string> names = new List<string>();

			foreach (Match match in RegexPattern.Matches(Settings.pBruteForceFormat))
			{
				string value = trimDictionaryName(match.Value);
				names.Add(value);
			}

			return names.ToArray();
		}


		// Regenerates the hashes in the hash list (string hash mode)
		private void regenerateHashes(bool regen)
		{
			if (_initialized && regen && cbStringHashMode.Checked)
			{
				if (!validateOutputFormat()) return;
				if (!CBruteForceMgr.ParseHashList(tbHashList.Lines)) return;
				CBruteForceMgr.SetMainForm(this);
				CBruteForceMgr.GenerateJoaatHashes();
			}
		}


		// Sets some misc UI data
		public void SetMiscComponents()
		{
			btnStartPauseResume.Text = "Start";
			int len = tbOutput.Lines.Length;
			if (len > 0)
			{
				len--; // Don't count new line
			}
			lblOutput.Text = $"Output ({len.ToString()})";
		}


		// Event Functions //


		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (CBruteForceMgr.IsRunning() || CBruteForceMgr.IsPaused())
			{
				var r = CMessageBox.Warn("WARNING: A brute force is currently ongoing. Are you sure you want to close the application?", MessageBoxButtons.YesNoCancel);
				if (r != DialogResult.Yes)
				{
					e.Cancel = true;
				}
				else
				{
					CBruteForceMgr.Abort();
				}
			}

			Settings.pCustomOutputFormat = tbCustomOutput.Text;
			Settings.Save();
		}

		private void tbHashList_TextChanged(object sender, EventArgs e)
		{
			int len = tbHashList.Lines.Length;
			if (len == 0)
			{
				len = 1;
			}
			lblHashList.Text = $"Hash List ({len.ToString()})";
			if (!cbStringHashMode.Checked) return;
			if (!_initialized) return;

			if (!validateOutputFormat()) return;
			if (!CBruteForceMgr.ParseHashList(tbHashList.Lines)) return;
			CBruteForceMgr.SetMainForm(this);
			CBruteForceMgr.GenerateJoaatHashes();
		}

		private void btnSaveToFile_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(tbOutput.Text))
			{
				CMessageBox.Error("Cannot save empty output to file");
				return;
			}

			string filename = DateTime.Now.ToString().Replace("/", ".").Replace(":", ".");
			filename = filename.Substring(0, filename.Length - 3) + ".txt";
			string filepath;
			if (Settings.bStringHashMode)
			{
				filepath = Environment.CurrentDirectory + "\\generated_hashes " + filename;
			}
			else
			{
				filepath = Environment.CurrentDirectory + "\\brute_force " + filename;
			}

			try
			{
				File.WriteAllLines(filepath, tbOutput.Lines);
				CMessageBox.Info($"Successfully created file at '{filepath}'");
			}
			catch (Exception ex)
			{
				CMessageBox.Error($"Failed to save hashes to file.\n\n{ex.Message}");
			}
		}

		private async void btnStartPauseResume_Click(object sender, EventArgs e)
		{
			if (CBruteForceMgr.IsInactive())
			{
				if (!validateOutputFormat()) return;
				if (!validateBruteForceFormat()) return;
				if (!CBruteForceMgr.ParseHashList(tbHashList.Lines)) return;
				CBruteForceMgr.SetMainForm(this);

				SetComponentsEnabled(false);
				tbOutput.Clear();
				bruteForceprogressBar.Value = 0;
				btnStartPauseResume.Text = "Pause";
				if (Settings.bRealTimeUIUpdates)
				{
					lblPercent.Text = "0.00%";
					lblTimeRemaining.Text = "EST Time Remaining: Calculating...";
				}
				else
				{
					lblPercent.Text = "";
					lblTimeRemaining.Text = "Brute Force In Progress...";
				}

				if (cbStringHashMode.Checked)
				{
					CBruteForceMgr.GenerateJoaatHashes();
				}
				else
				{
					// doing "await" like this appears to be faster than using it on a variable?
					await Task.Run(CBruteForceMgr.StartBruteForce);
				}
			}
			else if (CBruteForceMgr.IsRunning())
			{
				btnStartPauseResume.Text = "Resume";
				this.Text = "JOAAT Brute Forcer by Tuffy [PAUSED]";
				gbOutput.Enabled = true;
				CBruteForceMgr.Pause();
			}
			else if (CBruteForceMgr.IsPaused())
			{
				btnStartPauseResume.Text = "Pause";
				this.Text = "JOAAT Brute Forcer by Tuffy";
				gbOutput.Enabled = false;
				CBruteForceMgr.Resume();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (CBruteForceMgr.IsInactive()) return;

			if (CMessageBox.Question("Are you sure you want to cancel the brute force? It cannot be resumed after this point!", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
			{
				CBruteForceMgr.Abort();
				SetComponentsEnabled(true);
				bruteForceprogressBar.Value = 0;
			}
		}

		private void tbFormat_TextChanged(object sender, EventArgs e)
		{
			Settings.pBruteForceFormat = tbFormat.Text;
		}

		#region Hash Options Group

		private void cbStringHashMode_CheckedChanged(object sender, EventArgs e)
		{
			bool ischecked = cbStringHashMode.Checked;
			Settings.bStringHashMode = ischecked;
			// No reason to have this option enabled if we're just hashing strings
			rbStringOnly.Enabled = !ischecked;
			if (rbStringOnly.Checked)
			{
				rbStringOnly.Checked = false;
				rbHex.Checked = true;
			}
		}

		private void cbRealTimeUpdates_CheckedChanged(object sender, EventArgs e)
		=> Settings.bRealTimeUIUpdates = cbRealTimeUpdates.Checked;

		private void cbUnsigned_CheckedChanged(object sender, EventArgs e)
		=> Settings.bUnsignedIntegers = cbUnsigned.Checked;

		private void cbUppercase_CheckedChanged(object sender, EventArgs e)
		{
			if (cbLowercase.Checked && !Settings.bForceUppercase)
			{
				cbLowercase.Checked = false;
				Settings.bForceLowercase = false;
			}

			Settings.bForceUppercase = cbUppercase.Checked;
		}

		private void cbLowercase_CheckedChanged(object sender, EventArgs e)
		{
			if (cbUppercase.Checked && !Settings.bForceLowercase)
			{
				cbUppercase.Checked = false;
				Settings.bForceUppercase = false;
			}

			Settings.bForceLowercase = cbLowercase.Checked;
		}

		#endregion

		#region Output Group

		private void rbStringOnly_CheckedChanged(object sender, EventArgs e)
		=> Settings.OutputMode = eOutputMode.kString;

		private void rbHex_CheckedChanged(object sender, EventArgs e)
		{
			Settings.OutputMode = eOutputMode.kHexadecimal;
			regenerateHashes(rbHex.Checked);
		}

		private void rbDec_CheckedChanged(object sender, EventArgs e)
		{
			Settings.OutputMode = eOutputMode.kDecimal;
			regenerateHashes(rbDec.Checked);
		}

		private void rbHexAndDec_CheckedChanged(object sender, EventArgs e)
		{
			Settings.OutputMode = eOutputMode.kBoth;
			regenerateHashes(rbHexAndDec.Checked);
		}

		private void rbCustom_CheckedChanged(object sender, EventArgs e)
		{
			Settings.OutputMode = eOutputMode.kCustom;
			bool ischecked = rbCustom.Checked;
			tbCustomOutput.Enabled = ischecked;
			regenerateHashes(ischecked);
		}

		#endregion
	}
}
