using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnimeViewer.Support
{
    class Files
    {
        public static void WriteFile(string path,string content)
        {
            try
            {
                FileInfo file = new FileInfo(path);
                if (File.Exists(path))
                    file.Attributes &= ~FileAttributes.Hidden;

                File.WriteAllText(path, content);
                file.Attributes |= FileAttributes.Hidden;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message,"Write file exception");
            }
        }
        public static string ReadFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return null;
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "read file exception");
                return null;
            }
        }
    }
}
