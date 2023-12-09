namespace JoaatBruteForcer
{
	internal static class CMessageBox
	{
		public static DialogResult Error(string text, MessageBoxButtons buttons = MessageBoxButtons.OKCancel)
		{
			return MessageBox.Show(text, "JOAAT Brute Forcer", buttons, MessageBoxIcon.Error);
		}

		public static DialogResult Warn(string text, MessageBoxButtons buttons = MessageBoxButtons.OKCancel)
		{
			return MessageBox.Show(text, "JOAAT Brute Forcer", buttons, MessageBoxIcon.Warning);
		}

		public static DialogResult Info(string text, MessageBoxButtons buttons = MessageBoxButtons.OKCancel)
		{
			return MessageBox.Show(text, "JOAAT Brute Forcer", buttons, MessageBoxIcon.Information);
		}

		public static DialogResult Question(string text, MessageBoxButtons buttons = MessageBoxButtons.OKCancel)
		{
			return MessageBox.Show(text, "JOAAT Brute Forcer", buttons, MessageBoxIcon.Question);
		}
	}
}
