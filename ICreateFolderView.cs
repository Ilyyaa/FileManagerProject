using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public interface ICreateFolderView
    {
        void Close();
        public void SetPresenter(CreateFolderPresenter _presenter);
        void ShowDialog();
    }
}
