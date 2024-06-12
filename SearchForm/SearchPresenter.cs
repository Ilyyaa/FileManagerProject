using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileManagerProject.MainForm;
using FileManagerProject.SearchForm;

namespace FileManagerProject
{
    public class SearchPresenter
    {
        ISearchModel model;
        ISearchView view;
        IView mainView;
        //IMainModel mainModel;
        public List<string> paths;

        public SearchPresenter(ISearchModel _model, ISearchView _view, IView _mainView)
        {
            model = _model;
            view = _view;
            mainView = _mainView;
            paths = new List<string>();
            view.SetPresenter(this);
        }

        public void ShowDialog()
        {
            view.ShowDialog();
        }

       
        public async void Search(string fileName, string dirPath, string inner_txt, DateTime leftTime, DateTime rightTime)
        {
            await Task.Factory.StartNew(() => {

                paths.Clear();
                try
                {
                    paths = model.Search(fileName, dirPath, inner_txt, leftTime, rightTime);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            view.ViewResults(paths);
            view.EnableSearchButton();
        }

        public void SendPathToMainView(string path)
        {
            mainView.ChangeDirToFoundFile(Path.GetDirectoryName(path), path);
        }
    }
}
