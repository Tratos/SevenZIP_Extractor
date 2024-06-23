using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace SevenZip
{
    public static class Helper
    {
        public static void ExtractZip(string pZipPath, string zipPath, string extractpath, string passwords)
        {
            try
            {
                ProcessStartInfo processExtract = new ProcessStartInfo();
                processExtract.FileName = pZipPath;
                string commandLineString = "x -o" + extractpath + " \"" + zipPath + @"""" + " -p" + passwords;
                processExtract.Arguments = commandLineString;

                processExtract.WindowStyle = ProcessWindowStyle.Hidden;
                Process x = Process.Start(processExtract);
                x.WaitForExit();
            }
            catch (System.Exception ex)
            {
                Logger.Log("Error: " + ex, Color.Red);
            }

        }
    }
}
