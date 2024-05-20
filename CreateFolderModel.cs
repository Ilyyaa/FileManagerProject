using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public class CreateFolderModel : ICreateFolderModel
    {
        public CreateFolderModel()
        {

        }

        public string GetDirPath()
        {
            return Directory.GetCurrentDirectory();
        }
        public bool CreateFolder(string p, string n)
        {
            if (Directory.Exists(p + @"\" + n))
                return false;
            else
            {
                Directory.CreateDirectory(p + @"\" + n);
                return true;
            }

        }
    }
}
