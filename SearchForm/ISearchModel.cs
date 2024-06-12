using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public interface ISearchModel
    {
        public List<string> Search(string fileName, string dirPath, string inner_txt, DateTime leftTime, DateTime rightTime);
        
    }
}
