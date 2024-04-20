using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileManagerProject
{
    internal class SearchFacade
    {
        public List<string> paths;
        public SearchFacade()
        {
            paths = new List<string>();
        }
        public void Search(string fileName, string dirPath)
        {
            paths.Clear();
            //try
            //{
                //MessageBox.Show(dirPath);
                if (Directory.Exists(dirPath))
                {
                    var filePaths = Directory.EnumerateFiles(dirPath, fileName, new EnumerationOptions
                    {
                        IgnoreInaccessible = true,
                        RecurseSubdirectories = true
                    });
                var dirPaths = Directory.EnumerateFiles(dirPath, fileName, new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                });
                    paths.AddRange(filePaths); paths.AddRange(dirPaths);
                }
                
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}  
        }
    }
}
