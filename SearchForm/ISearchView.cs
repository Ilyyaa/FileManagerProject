using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public interface ISearchView
    {
        public void SetPresenter(SearchPresenter _presenter);
        public void ShowDialog();
        public void ViewResults(List<string> paths);
        public void EnableSearchButton();
    }
}
