using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileManagerProject
{
    public class CreateFileModel : ICreateFileModel
    {
        public CreateFileModel()
        {

        }

        public string GetDirPath()
        {
            return Directory.GetCurrentDirectory();
        }
        public bool CreateFile(string p, string n)
        {
            if (File.Exists(p + @"\" + n))
            {
                return false;
            } 
            else
            {
                
                File.Create(p + @"\" + n).Close();
                return true;
            }
            
        }
        
    }
}
