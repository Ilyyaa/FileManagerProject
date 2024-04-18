using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Collections;
using System.Collections.Specialized;
using static System.Collections.Specialized.BitVector32;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using System.Windows.Forms;

namespace FileManagerProject
{
    public partial class Form1 : Form
    {
        AllDrives AllDrives = new AllDrives();
        DirectoryInfo LeftDirectory, RightDirectory;
        CurrPanel cPanel;
        FileManagerFacade fileManagerFacade;

        public Form1()
        {
            InitializeComponent();
            fileManagerFacade = new FileManagerFacade();
            comboBox1.Items.AddRange(AllDrives.DrivesArray);
            comboBox2.Items.AddRange(AllDrives.DrivesArray);
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = 0;

            LeftDirectory = AllDrives.DrivesArray[0].RootDirectory;
            RightDirectory = AllDrives.DrivesArray[0].RootDirectory;

            PopulateListView(LeftDirectory, listView1);
            PopulateListView(RightDirectory, listView2);

            textBox1.Text = LeftDirectory.FullName;
            textBox2.Text = RightDirectory.FullName;

        }


        public void PopulateListView(DirectoryInfo directory, ListView listview)
        {
            imageList1.Images.Clear();
            imageList2.Images.Clear();
            listview.Items.Clear();

            if (directory.Parent != null)
            {
                ListViewItem item = new ListViewItem("..");
                item.Tag = "ArrowUp";
                item.ImageKey = "arrowup";
                listview.Items.Add(item);
                imageList1.Images.Add("arrowup", Properties.Resources.ArrowUp);
                imageList2.Images.Add("arrowup", Properties.Resources.ArrowUp);
            }

            if (directory.GetDirectories().Length > 0)
            {
                imageList1.Images.Add("folder", Properties.Resources.folder);
                imageList2.Images.Add("folder", Properties.Resources.folder);

                foreach (var dir in directory.GetDirectories())
                {
                    ListViewItem item = new ListViewItem(dir.Name);
                    item.Tag = "Dir";
                    item.SubItems.Add("");
                    item.SubItems.Add("<DIR>");
                    item.SubItems.Add(dir.CreationTime.ToShortDateString());
                    item.ImageKey = "folder";
                    listview.Items.Add(item);
                }
            }

            foreach (var file in directory.GetFiles())
            {
                imageList1.Images.Add(file.FullName, Icon.ExtractAssociatedIcon(file.FullName).ToBitmap());
                imageList2.Images.Add(file.FullName, Icon.ExtractAssociatedIcon(file.FullName).ToBitmap());
                ListViewItem item = new ListViewItem(file.Name);
                item.Tag = "File";
                item.ImageKey = file.FullName;
                // Добавить элемент в ListView
                listview.Items.Add(item);
                item.SubItems.Add(file.Extension);
                item.SubItems.Add(file.Length.ToString() + " bytes");
                item.SubItems.Add(file.CreationTime.ToShortDateString());
            }
            listview.SmallImageList = imageList1;
            listview.LargeImageList = imageList2;

        }

        private void listView1_Enter(object sender, EventArgs e)
        {
            cPanel = CurrPanel.left;
            Directory.SetCurrentDirectory(LeftDirectory.FullName);
        }

        private void listView2_Enter(object sender, EventArgs e)
        {
            cPanel = CurrPanel.right;
            Directory.SetCurrentDirectory(RightDirectory.FullName);
        }

        public void ChangeViewMode(object sender, EventArgs e)
        {
            string mode = (sender as ToolStripMenuItem).Tag as string;
            if (cPanel == CurrPanel.left)
            {
                listView1.View = (View)Enum.Parse(typeof(View), mode);
            }
            else
            {
                listView2.View = (View)Enum.Parse(typeof(View), mode);
            }
        }

        private void listViewItemActivate(object sender, EventArgs e)
        {
            var listview = sender as ListView;
            try
            {
                if ((string)listview.FocusedItem.Tag == "File")
                {
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(listview.FocusedItem.Text)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
                else
                {
                    ChangeDir(listview.FocusedItem.Text, cPanel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ChangeDir(string path, CurrPanel cPanel)
        {
            var newDir = new DirectoryInfo(path);
            Directory.SetCurrentDirectory(path);
            if (cPanel == CurrPanel.left)
            {
                LeftDirectory = newDir;
                PopulateListView(newDir, listView1);
            }
            else
            {
                RightDirectory = newDir;
                PopulateListView(newDir, listView2);
            }
        }

        private void CopyFiles(object sender, EventArgs e)
        {
            //fileManagerFacade._effect = OperationEffect.copy;
            ListView listView;
            if (cPanel == CurrPanel.left)
            {
                listView = listView1;
            }
            else
            {
                listView = listView2;
            }
            var items = new List<string>();
            foreach (ListViewItem item in listView.SelectedItems)
            {
                items.Add(Directory.GetCurrentDirectory() + "\\" + item.Text);
            }
            fileManagerFacade.copy(items);


        }

        public void PasteFiles(object sender, EventArgs e)
        {
            string sourcePath;
            sourcePath = Directory.GetCurrentDirectory();

            fileManagerFacade.paste(sourcePath);
            PopulateListView(LeftDirectory, listView1);
            PopulateListView(RightDirectory, listView2);
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                    contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void listView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                    contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void CutFiles(object sender, EventArgs e)
        {
            ListView listView;
            if (cPanel == CurrPanel.left)
            {
                listView = listView1;
            }
            else
            {
                listView = listView2;
            }
            var items = new List<string>();
            foreach (ListViewItem item in listView.SelectedItems)
            {
                items.Add(Directory.GetCurrentDirectory() + "\\" + item.Text);
            }
            fileManagerFacade.cut(items);
        }

        public void DeleteFiles(object sender, EventArgs e)
        {
            ListView listView;
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить выбранные папки и файлы?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                if (cPanel == CurrPanel.left)
                {
                    listView = listView1;
                }
                else
                {
                    listView = listView2;
                }
                var items = new List<string>();
                foreach (ListViewItem item in listView.SelectedItems)
                {
                    items.Add(Directory.GetCurrentDirectory() + "\\" + item.Text);
                }
                try
                {
                    fileManagerFacade.delete(items);
                    PopulateListView(LeftDirectory, listView1);
                    PopulateListView(RightDirectory, listView2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}

