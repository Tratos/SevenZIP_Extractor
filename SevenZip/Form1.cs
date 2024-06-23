using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;



namespace SevenZip
{
    public partial class Form1 : Form
    {
        public static string dataInput;
        public static string dataRead;
        public static string dataExe;
        public static string dataPath;
        public static string dataName;

        List<string> wordlist = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Logger.box = rtb1;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.InitialDirectory = ApplicationPath();
            d.Filter = "7-ZIP files (*.7z)|*.7z|All files (*.*)|*.*";
            if (d.ShowDialog() == DialogResult.OK)
            {
                dataInput = d.FileName;
                dataName = Path.GetFileName(d.FileName);

                Logger.Log("loaded 7-Zip file: " + d.FileName);
                Logger.Log("File: " + dataName);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.InitialDirectory = ApplicationPath();

            if (File.Exists("C:\\Program Files\\7-Zip\\7z.exe"))
            {
                d.InitialDirectory = "C:\\Program Files\\7-Zip\\";
            }

            d.Filter = "exe files (*.exe)|*.exe";
            if (d.ShowDialog() == DialogResult.OK)
            {
                dataExe = d.FileName;
                Logger.Log("set 7-Zip exe file: " + d.FileName);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    dataPath = fbd.SelectedPath;
                    Logger.Log("set Destination path: " + dataPath);
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.InitialDirectory = ApplicationPath();
            d.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (d.ShowDialog() == DialogResult.OK)
            {
                dataRead = d.FileName;
                Logger.Log("loaded Wordlist file: " + d.FileName);
            }

            string textLines = toolStripTextBox1.Text;
            int x = Int32.Parse(toolStripTextBox1.Text);
            int i = 0;
            wordlist.Clear();

            try
            {
                using (StreamReader sr = new StreamReader(dataRead))
                {
                    for (i = 0; i < x; i++)
                    {
                        if (sr.EndOfStream)
                        {
                            break;
                        }

                        wordlist.Add(sr.ReadLine());
                    }
                }
                Logger.Log("successfully read " + i + " lines...",Color.Green);
            }
            catch (Exception ex)
            {
             Logger.Log(ex.Message,Color.Red);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "running...";

            Logger.Log("start...", Color.Blue);
            Logger.Log("wordlist: " + wordlist.Count + " Items");

            foreach (var line in wordlist)
            {
                string s = dataName;
                s = s.Substring(0, s.Length - 3);
                string path = dataPath + "\\" + s + "\\";

                if (Directory.Exists(path) && CheckFiles(path))
                {
                    toolStripStatusLabel2.Text = "finish...";
                    Logger.Log("successfully!", Color.Green);
                    break;
                }
                else
                {
                    Logger.Log("extract 7-zip file: " + dataName + " with password: " + line, Color.Gray);
                    Helper.ExtractZip(dataExe, dataInput, dataPath, line);

                }
            }

            string s2 = dataName;
            s2 = s2.Substring(0, s2.Length - 3);
            string path2 = dataPath + "\\" + s2 + "\\";

            if (!Directory.Exists(path2) && !CheckFiles(path2))
            {
                Logger.Log("wordlist finish...");
                toolStripStatusLabel2.Text = "fail...";
            }
        }

        public static string ApplicationPath()
        {
          return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static bool CheckFiles(string path)
        {
            string[] dirs = System.IO.Directory.GetDirectories(path);
            string[] files = System.IO.Directory.GetFiles(path);

            if (dirs.Length == 0 && files.Length == 0)
                return false;
            else
                return true;
        }

    }
}
