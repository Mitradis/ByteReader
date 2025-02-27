using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ByteReader
{
    public partial class FormMain : Form
    {
        List<string> cache = new List<string>();
        string lastPath = null;

        public FormMain()
        {
            InitializeComponent();
        }

        void button1_Click(object sender, EventArgs e)
        {
            if (lastPath != null)
            {
                openFileDialog1.InitialDirectory = lastPath;
            }
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                lastPath = Path.GetDirectoryName(openFileDialog1.FileNames[0]);
                listView1.Items.Clear();
                cache.Clear();
                cache.AddRange(openFileDialog1.FileNames);
                funcGet();
            }
        }

        void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (string line in openFileDialog1.FileNames)
                {
                    if (File.Exists(line))
                    {
                        string textBytes = "";
                        byte[] bytesFile = File.ReadAllBytes(line);
                        int fileSize = bytesFile.Length;
                        for (int i = 0; i < fileSize; i++)
                        {
                            textBytes += bytesFile[i] + ", ";
                        }
                        File.WriteAllText(Path.Combine(Path.GetDirectoryName(line), Path.GetFileNameWithoutExtension(line) + ".txt"), textBytes);
                    }
                }
            }
        }

        void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (string line1 in openFileDialog1.FileNames)
                {
                    if (File.Exists(line1) && line1.Contains(".txt"))
                    {
                        List<string> cacheFile = new List<string>();
                        foreach (string line2 in File.ReadAllLines(line1))
                        {
                            cacheFile.AddRange(line2.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
                        }
                        List<byte> byteFile = new List<byte>();
                        foreach (string line2 in cacheFile)
                        {
                            if (!String.IsNullOrEmpty(line2))
                            {
                                byteFile.Add(Convert.ToByte(line2, 10));
                            }
                        }
                        File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(line1), Path.GetFileNameWithoutExtension(line1) + ".UNKNOWN"), byteFile.ToArray());
                    }
                }
            }
        }

        void funcGet()
        {
            foreach (string line in cache)
            {
                if (File.Exists(line))
                {
                    string[] row = { Path.GetFileName(line), getByte(line) };
                    var listViewItem = new ListViewItem(row);
                    listView1.Items.Add(listViewItem);
                }
            }
        }

        string getByte(string path)
        {
            if (File.Exists(path))
            {
                FileStream fs = File.OpenRead(path);
                fs.Seek(Convert.ToInt64(numericUpDown1.Value), SeekOrigin.Begin);
                String hex = String.Format("{0:X2}", fs.ReadByte());
                fs.Close();
                return hex;
            }
            else
            {
                return "";
            }
        }

        void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            funcGet();
        }

        void listView1_DragDrop(object sender, DragEventArgs e)
        {
            listView1.Items.Clear();
            cache.Clear();
            cache.AddRange((string[])e.Data.GetData(DataFormats.FileDrop));
            funcGet();
        }

        void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
        }
    }
}
