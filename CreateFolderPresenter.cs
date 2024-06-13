using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public class CreateFolderPresenter
    {
        ICreateFolderModel model;
        ICreateFolderView view;
        public CreateFolderPresenter(ICreateFolderModel _model, ICreateFolderView _view)
        {
            model = _model;
            view = _view;
            view.SetPresenter(this);
        }

       


        public void ShowDialog()
        {
            view.ShowDialog();
        }

        public void CreateFolder(string name)
        {

                string path = model.GetDirPath();

                    if (!model.CreateFolder(path, name))
                    {
                        throw new Exception("Файл с таким именем уже существует");
                    }
                


                //else
                //model.CreateFolder(path, name);
                view.Close();
           
        }
    }
}
