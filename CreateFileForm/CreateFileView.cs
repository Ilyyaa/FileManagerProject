using FileManagerProject.MainForm;
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
    public partial class CreateFileView : Form, ICreateFileView
    {
        CreateFilePresenter presenter;

        public void SetPresenter(CreateFilePresenter _presenter)
        {
            presenter = _presenter;
        }

        public CreateFileView()
        {
            InitializeComponent();
        }

        void ICreateFileView.ShowDialog()
        {
            this.ShowDialog();
        }
        string name { get => textBox1.Text;  }
        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                presenter.CreateFile(name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        void Close()
        {
            this.Close();
        }
    }
}
