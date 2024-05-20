using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public interface ICreateFileModel
    {
        bool CreateFile(string p, string n);
        public string GetDirPath();
    }
}
