using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public interface ICreateFileView
    {
        void Close();
        public void SetPresenter(CreateFilePresenter presenter);
        void ShowDialog();
    }
}
