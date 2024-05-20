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
    public partial class CreateFolderView : Form, ICreateFolderView
    {
        CreateFolderPresenter presenter;
        public CreateFolderView()
        {
            InitializeComponent();
        }

        public void SetPresenter(CreateFolderPresenter _presenter)
        {
            presenter = _presenter;
        }

        void ICreateFolderView.ShowDialog()
        {
            this.ShowDialog();
        }

        string name { get => textBox1.Text; }
        private void button1_Click(object sender, EventArgs e)
        {
            presenter.CreateFolder(name);
        }
    }
}
