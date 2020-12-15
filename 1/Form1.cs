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
using System.IO.Compression;

namespace _1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }
        private bool folder = false;
        private bool file = false;

        //СОЗДАНИЕ ФАЙЛА
        private void Create(object sender, EventArgs e)
        {
            if (folder == true && file == false)
            {
                string path = PATHBOX.Text;
                if (Directory.Exists(path) == false)
                {
                    MessageBox.Show("ERROR. Check the path or file");
                }
                else
                {
                    if (FILENAME.Text != null)
                    {
                        path += $@"\{FILENAME.Text}.txt";
                    }
                    else
                    {
                        path += @"\note.txt";
                    }
                    using (FileStream fstream = new FileStream($@"{path}", FileMode.Create)) { }
                    PATHBOX.Text = path;
                    folder = false;
                    file = true;

                }
            }
            else
                MessageBox.Show("Choose path");
        }

        //SAVE
        private void Writter(object sender, EventArgs e)
        {
            if (file == true && folder == false)
            {
                string path = PATHBOX.Text;
                if (File.Exists(path) == false)
                {
                    MessageBox.Show("ERROR. Check the path or file");
                }
                else
                {
                    using (FileStream fstream = new FileStream($@"{path}", FileMode.Open))
                    {
                        byte[] array = System.Text.Encoding.Default.GetBytes(text.Text);
                        fstream.Write(array, 0, array.Length);
                    }
                }
            }
            else
                MessageBox.Show("Choose file");
        }
        //ВЫБОР ФАЙЛА
        private void FileChoose(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string path = openFileDialog1.FileName;
            PATHBOX.Text = path;
            if (File.Exists(path) == false)
            {
                MessageBox.Show("ERROR. Check the path");
            }
            else
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    text.Text = sr.ReadToEnd();
                }
                FileInfo info = new FileInfo(PATHBOX.Text);
                FILENAME.Text = info.Name;
                file = true;
                folder = false;
            }
        }

        //МЕСТО ПОД ФАЙЛ
        private void FolderPick(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                PATHBOX.Text = FBD.SelectedPath;
                folder = true;
                file = false;
            }
        }

        //Удаление файла
        private void Delete(object sender, EventArgs e)
        {
            if (file == true)
            {
                File.Delete(PATHBOX.Text);
                file = false;
                PATHBOX.Clear();
                FILENAME.Clear();
                text.Text = "";
            }
            else MessageBox.Show("file not chosed");
        }
        //КОПИРОВАНИЕ
        private void Copy(object sender, EventArgs e)
        {
            if (file == true)
            {
                FolderBrowserDialog FBD = new FolderBrowserDialog();
                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(PATHBOX.Text, $@"{FBD.SelectedPath}\NewCopy.txt");
                }
            }
        }

        private void Rename(object sender, EventArgs e)
        {
            if (file == true)
            {
                FileInfo info = new FileInfo(PATHBOX.Text);
                string old = info.FullName;
                string dir = info.DirectoryName;
                dir += $@"\{FILENAME.Text}";
                if (old == dir)
                {
                    MessageBox.Show("Write new name");
                    return;
                }
                else
                {
                    File.Move(old, dir);
                    PATHBOX.Text = dir;
                }
            }
        }

        private void Move(object sender, EventArgs e)
        {
            if (file == true)
            {
                FolderBrowserDialog FBD = new FolderBrowserDialog();
                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    FileInfo info = new FileInfo(PATHBOX.Text);
                    string old = info.FullName;
                    string dir = FBD.SelectedPath;
                    dir += $@"\{info.Name}";
                    File.Move(old, dir);
                    PATHBOX.Text = dir;
                }
            }
            else
            {
                MessageBox.Show("Choose file");
                return;
            }

        }

    }
}
