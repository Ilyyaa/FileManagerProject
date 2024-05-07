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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Metadata.Ecma335;
using System.Reflection;
using FileManagerProject.SearchForm;
using System.Text;

namespace FileManagerProject.MainForm
{
    public partial class MainView : Form, IView
    {
        Presenter? presenter;
        SelectedPanel cPanel;

        public MainView()
        {
            InitializeComponent();
        }

        public void SetPresenter(Presenter p)
        {
            presenter = p;
        }


        private void listView1_Enter(object sender, EventArgs e)
        {
            cPanel = SelectedPanel.left;
            presenter.SetCurrentDirectory(cPanel);
        }

        private void listView2_Enter(object sender, EventArgs e)
        {
            cPanel = SelectedPanel.right;
            presenter.SetCurrentDirectory(cPanel);
        }

        public void ChangeViewMode(object sender, EventArgs e)
        {
            string mode = (sender as ToolStripMenuItem).Tag as string;
            if (cPanel == SelectedPanel.left)
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
            var listview = sender as System.Windows.Forms.ListView;
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

        private void ChangeDir(string path, SelectedPanel cPanel)
        {
            var newDir = new DirectoryInfo(path);
            presenter.SetDirectory(cPanel, newDir);
            presenter.SetCurrentDirectory(cPanel);
            if (cPanel == SelectedPanel.left)
            {
                presenter.SetListView(cPanel);
            }
            else
            {
                presenter.SetListView(cPanel);
            }
        }

        private void CopyFiles(object sender, EventArgs e)
        {
            //fileManagerFacade._effect = OperationEffect.copy;
            System.Windows.Forms.ListView listView;
            if (cPanel == SelectedPanel.left)
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
                items.Add(item.Text);
            }
            presenter.copy(items);


        }

        public void PasteFiles(object sender, EventArgs e)
        {
            presenter.paste();
            if (cPanel == SelectedPanel.left)
            {
                presenter.SetListView(cPanel);
            }
            else
            {
                presenter.SetListView(cPanel);
            }
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
            System.Windows.Forms.ListView listView;
            if (cPanel == SelectedPanel.left)
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
                items.Add(item.Text);
            }
            presenter.cut(items);
        }

        private void OpenSearchBox(object sender, EventArgs e)
        {
            try
            {
                var searchBox = new SearchView();
                SearchModel searchModel = new SearchModel();
                SearchPresenter SearchPresenter = new SearchPresenter(searchModel, searchBox);
                SearchPresenter.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DeleteFiles(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView listView;
            DialogResult result = MessageBox.Show("Вы действительно хотите удалить выбранные папки и файлы?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (cPanel == SelectedPanel.left)
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
                    items.Add(item.Text);
                }
                try
                {
                    presenter.delete(items);
                    presenter.SetListView(cPanel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        public void SetUpComboboxes(DriveInfo[] drives)
        {
            comboBox1.Items.AddRange(drives);
            comboBox2.Items.AddRange(drives);
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            presenter.SetUpDriveLists();

            imageList1.Images.Clear();
            imageList2.Images.Clear();
            presenter.ChangeDrive(SelectedPanel.left);
            presenter.ChangeDrive(SelectedPanel.right);
            presenter.SetListView(SelectedPanel.left);
            presenter.SetListView(SelectedPanel.right);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.ChangeDrive(SelectedPanel.left);
            
        }

        public DriveInfo selectedDrive1 { get { return (DriveInfo)comboBox1.SelectedItem; } }
        public DriveInfo selectedDrive2 { get { return (DriveInfo)comboBox2.SelectedItem; } }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.ChangeDrive(SelectedPanel.right);
        }

        public void PopulateListView(SelectedPanel selectedPanel)
        {
            throw new NotImplementedException();
        }

        public void AddItemToListView(SelectedPanel selectedPanel, ListViewItem item)
        {
            if (selectedPanel == SelectedPanel.left)
            {
                listView1.ListViewItemSorter = null;
                listView1.Items.Add(item);
            }

            else
            {
                listView2.ListViewItemSorter = null;
                listView2.Items.Add(item);
            }
                
        }

        public void AddImageToList(SelectedPanel selectedPanel, string str, Bitmap image)
        {
            if(selectedPanel == SelectedPanel.left)
            {
                listView1.SmallImageList.Images.Add(str, image);
                listView1.LargeImageList.Images.Add(str, image);
            }
            else
            {
                listView2.SmallImageList.Images.Add(str, image);
                listView2.LargeImageList.Images.Add(str, image);
            }
        }

        public void ImageListClear()
        {
            imageList1.Images.Clear();
            imageList2.Images.Clear();
        }

        public void ListViewItemsClear(SelectedPanel selectedPanel)
        {
            if (selectedPanel == SelectedPanel.left)
            {
                listView1.Items.Clear();
            }
            else
            {
                listView2.Items.Clear();
            }
        }

        private void ListView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                var listView = sender as System.Windows.Forms.ListView;
                if (e.Label == null)
                    return;
                if (!presenter.AllowChangeLabel(cPanel, e.Label))
                {
                    e.CancelEdit = true;

                    MessageBox.Show("Such file or directory already exists");
                }
                else
                {
                    string oldName = listView.Items[e.Item].Text;
                    presenter.ChangeFileName(oldName, cPanel, e.Label);
                    presenter.SetListView(SelectedPanel.left);
                    presenter.SetListView(SelectedPanel.right);
                }
            }
            catch(Exception ex)
            {
                e.CancelEdit = true;
                MessageBox.Show(ex.Message);
            }
        }

        private void listView1_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            //if (!presenter.AllowChangeLabel(cPanel, e.Label))
            //{
            //    e.CancelEdit = true;
            //    MessageBox.Show("Such file or directory already exists");
            //}      
        }

        public class ItemComparer : IComparer
        {
            //column used for comparison
            public int Column { get; set; }
            //Order of sorting
            public SortOrder Order { get; set; }
            public ItemComparer(int colIndex)
            {
                Column = colIndex;
                Order = SortOrder.None;
            }
            public int Compare(object a, object b)
            {
                int result = 0;
                ListViewItem itemA = a as ListViewItem;
                ListViewItem itemB = b as ListViewItem;
                // datetime comparison
                if (Column == 3)
                {
                    if (itemA.Text == "..")
                    {
                        return -1;
                    }
                    else if(itemB.Text == "..")
                    {
                        return 1;
                    }
                    else
                    {
                        DateTime x1, y1;
                        if(itemA.SubItems.Count == 4)
                        {
                            if (!DateTime.TryParse(itemA.SubItems[Column].Text, out x1))
                                x1 = DateTime.MinValue;
                            if (!DateTime.TryParse(itemB.SubItems[Column].Text, out y1))
                                y1 = DateTime.MinValue;
                            result = DateTime.Compare(x1, y1);
                        }
                    }
                    
                }
                else if (Column == 2)
                {
                    if (itemA.Text == "..")
                    {
                        
                        return -1;
                    }
                    else if (itemB.Text == "..")
                    {
                        
                        return 1;
                    }
                    else
                    {
                        decimal x2, y2;
                        var what = itemA.SubItems[Column].Text;
                        if (!Decimal.TryParse(itemA.SubItems[Column].Text, out x2))
                            x2 = Decimal.MinValue;
                        if (!Decimal.TryParse(itemB.SubItems[Column].Text, out y2))
                            y2 = Decimal.MinValue;
                        result = Decimal.Compare(x2, y2);
                    }
                    
                }
                else if(Column == 1 || Column == 0)
                {
                    if (itemA.Text == "..")
                    {
                        
                        return -1;
                    }
                    else if (itemB.Text == "..")
                    {
                        
                        return 1;
                    }
                    else
                    {
                        result = String.Compare(itemA.SubItems[Column].Text, itemB.SubItems[Column].Text);
                    }
                }
                // if sort order is descending.
                if (Order == SortOrder.Descending)
                    // Invert the value returned by Compare.
                    result *= -1;
                return result;
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ItemComparer sorter = listView1.ListViewItemSorter as ItemComparer;
            if (sorter == null)
            {
                sorter = new ItemComparer(e.Column);
                sorter.Order = SortOrder.Ascending;
                listView1.ListViewItemSorter = sorter;
            }
            // if clicked column is already the column that is being sorted
            if (e.Column == sorter.Column)
            {
                // Reverse the current sort direction
                if (sorter.Order == SortOrder.Ascending)
                    sorter.Order = SortOrder.Descending;
                else
                    sorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.Column = e.Column;
                sorter.Order = SortOrder.Ascending;
            }
            
            listView1.Sort();
        }

        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ItemComparer sorter = listView2.ListViewItemSorter as ItemComparer;
            if (sorter == null)
            {
                sorter = new ItemComparer(e.Column);
                sorter.Order = SortOrder.Ascending;
                listView2.ListViewItemSorter = sorter;
            }
            // if clicked column is already the column that is being sorted
            if (e.Column == sorter.Column)
            {
                // Reverse the current sort direction
                if (sorter.Order == SortOrder.Ascending)
                    sorter.Order = SortOrder.Descending;
                else
                    sorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.Column = e.Column;
                sorter.Order = SortOrder.Ascending;
            }

            listView2.Sort();
        }

        private void ListView2_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                var listView = sender as System.Windows.Forms.ListView;
                if (e.Label == null)
                    return;
                if (!presenter.AllowChangeLabel(cPanel, e.Label))
                {
                    e.CancelEdit = true;

                    MessageBox.Show("Such file or directory already exists");
                }
                else
                {
                    string oldName = listView.Items[e.Item].Text;
                    presenter.ChangeFileName(oldName, cPanel, e.Label);
                    presenter.SetListView(SelectedPanel.left);
                    presenter.SetListView(SelectedPanel.right);

                }
            }
            catch (Exception ex)
            {
                e.CancelEdit = true;
                MessageBox.Show(ex.Message);
            }
        }

        private void listView2_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {

        }
    }
    }


