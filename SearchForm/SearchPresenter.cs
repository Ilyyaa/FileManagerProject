using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileManagerProject.SearchForm;

namespace FileManagerProject
{
    public class SearchPresenter
    {
        ISearchModel model;
        ISearchView view;
        public List<string> paths;

        public SearchPresenter(ISearchModel _model, ISearchView _view)
        {
            model = _model;
            view = _view;
            paths = new List<string>();
            view.SetPresenter(this);
        }

        public void ShowDialog()
        {
            view.ShowDialog();
        }

        public async void Search(string fileName, string dirPath)
        {
            await Task.Factory.StartNew(() => {

                paths.Clear();
                try
                {
                    paths = model.Search(fileName, dirPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            view.ViewResults(paths);
        }
    }
}
