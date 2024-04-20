using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManagerProject
{
    public partial class SearchForm : Form
    {
        SearchFacade SearchFacade;
        //FileTreeDialogue FileTreeDialogue;
        public SearchForm()
        {
            SearchFacade = new SearchFacade();
            //FileTreeDialogue = new FileTreeDialogue();
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //try
            //{
                SearchFacade.Search(textBox1.Text, textBox2.Text);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            if (SearchFacade.paths.Count == 0)
            {
                MessageBox.Show("Ничего не найдено!", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            listBox1.Items.AddRange(SearchFacade.paths.ToArray());
        }

        
    }
}
