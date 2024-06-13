using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public class CreateFilePresenter
    {
        ICreateFileModel model;
        ICreateFileView view;
        public CreateFilePresenter(ICreateFileModel _model, ICreateFileView _view)
        {
            model = _model;
            view = _view;
            view.SetPresenter(this);
        }

        public void ShowDialog()
        {
            view.ShowDialog();
        }

        public void CreateFile(string name)
        {
                string path = model.GetDirPath();
                
                    if (!model.CreateFile(path, name))
                    {
                        
                        throw new Exception("Файл с таким именем уже существует");
                    }
                
            
                view.Close();
            
        }  
    }
}
