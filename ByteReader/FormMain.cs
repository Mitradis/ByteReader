using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ByteReader
{
    public partial class FormMain : Form
    {
        List<string> cache = new List<string>();
        string lastFolder = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lastFolder != null)
            {
                if (Directory.Exists(lastFolder))
                {
                    openFileDialog1.InitialDirectory = lastFolder;
                }
            }
            else
            {
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                lastFolder = Path.GetDirectoryName(openFileDialog1.FileName);
                listView2.Items.Clear();
                cache.Clear();
                cache.AddRange(openFileDialog1.FileNames);
                funcGet();
            }
        }

        private void funcGet()
        {
            foreach (string line in cache)
            {
                if (File.Exists(line))
                {
                    string[] row = { Path.GetFileName(line), getByte(line) };
                    var listViewItem = new ListViewItem(row);
                    listView2.Items.Add(listViewItem);
                }
            }
        }

        private string getByte(string path)
        {
            if (File.Exists(path))
            {
                FileStream fs = File.OpenRead(path);
                fs.Seek(Convert.ToInt64(numericUpDown1.Value), SeekOrigin.Begin);
                String hex = string.Format("{0:X2}", fs.ReadByte());
                fs.Close();
                return hex;
            }
            else
            {
                return "";
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            funcGet();
        }

        private void listView2_DragDrop(object sender, DragEventArgs e)
        {
            listView2.Items.Clear();
            cache.Clear();
            cache.AddRange((string[])e.Data.GetData(DataFormats.FileDrop));
            funcGet();
        }

        private void listView2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.All;
            }
        }
    }
}
