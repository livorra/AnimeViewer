using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeViewer.ExtendedControls
{
    class ExtendedTextBox : System.Windows.Controls.TextBox
    {
        public string Utf8Text
        {
            get
            {
                return EscapeToAscii(base.Text);
            }
        }
        string StringFold(string input, Func<char, string> proc)
        {
            return string.Concat(input.Select(proc).ToArray());
        }

        string FoldProc(char input)
        {
            if (input >= 128)
            {
                return string.Format(@"\u{0:x4}", (int)input);
            }
            return input.ToString();
        }

        public string EscapeToAscii(string input)
        {
            return StringFold(input, FoldProc);
        }
    }
}
