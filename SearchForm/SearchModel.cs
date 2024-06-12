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

        public List<string> Search(string fileName, string dirPath, string inner_txt, DateTime leftTime,
            DateTime rightTime)
        {

            if (Directory.Exists(dirPath))
            {
                List<string> filePaths = Directory.EnumerateFiles(dirPath, fileName, new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                }).ToList();
                foreach (var filePath in filePaths.ToList())
                {
                    FileInfo file = new FileInfo(filePath);
                    if (file.LastWriteTime < leftTime && file.LastWriteTime > rightTime)
                    {
                        filePaths.Remove(filePath);
                    }
                }

                if (inner_txt != null && inner_txt != "")
                {
                    foreach (var filePath in filePaths.ToList())
                    {
                        if (Path.GetExtension(filePath) != ".txt")
                            filePaths.Remove(filePath);
                        else
                        {
                            string fileContents = File.ReadAllText(filePath);
                            if (!fileContents.Contains(inner_txt))
                            {
                                filePaths.Remove(filePath);
                            }
                        }
                    }
                }
                else
                {
                    var dirPaths = Directory.EnumerateDirectories(dirPath, fileName, new EnumerationOptions
                    {
                        IgnoreInaccessible = true,
                        RecurseSubdirectories = true
                    });
                    paths.AddRange(dirPaths);
                }

                paths.AddRange(filePaths);
            }

            return paths;
        }

    }
}
