using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileManagerProject.SearchForm;

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
            try
            {
                presenter.Search(textBox1.Text, textBox2.Text);
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
    }
}
