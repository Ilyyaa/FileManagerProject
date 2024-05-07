using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject.SearchForm
{
    public class SearchModel : ISearchModel
    {
        List<string> paths;
        public SearchModel()
        {
            paths = new List<string>();
        }
        public List<string> Search(string fileName, string dirPath)
        {

            if (Directory.Exists(dirPath))
            {
                var filePaths =  Directory.EnumerateFiles(dirPath, fileName, new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                });
                var dirPaths = Directory.EnumerateDirectories(dirPath, fileName, new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                });
                paths.AddRange(filePaths); paths.AddRange(dirPaths);
            }
            return paths;
        }
    }
}
