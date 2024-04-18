using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    internal class AllDrives
    {
        public DriveInfo[] DrivesArray { get; set; }
        public AllDrives()
        {
            DrivesArray = DriveInfo.GetDrives();
        }
    }
}
