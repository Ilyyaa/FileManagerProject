using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileManagerProject.MainForm;
using FileManagerProject.SearchForm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FileManagerProject
{
    public partial class SearchView : Form, ISearchView
    {
        SearchPresenter presenter;
        //FileTreeDialogue FileTreeDialogue;
        public SearchView()
        {
            //FileTreeDialogue = new FileTreeDialogue();
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            button2.Enabled = false;
            try
            {
                presenter.Search(textBox1.Text, textBox2.Text, textBox3.Text, dateTimePicker1.Value, dateTimePicker2.Value);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }



        }

        public void SetPresenter(SearchPresenter _presenter)
        {
            presenter = _presenter;
        }

        public void ViewResults(List<string> paths)
        {
            if (paths.Count == 0)
            {
                MessageBox.Show("Ничего не найдено!", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            listBox1.Items.AddRange(paths.ToArray());

        }

        void ISearchView.ShowDialog()
        {
            this.ShowDialog();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                //presenter.ChangeDir(Path.GetDirectoryName(listBox1.Items[index].ToString()), MainForm.SelectedPanel.left);
                //presenter.SetListView(MainForm.SelectedPanel.right);
                presenter.SendPathToMainView(listBox1.Items[index].ToString());
                this.Close();
            }
        }

        private void SearchView_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        public void EnableSearchButton()
        {
            button2.Enabled = true;
        }
    }
}
