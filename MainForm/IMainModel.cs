using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject.MainForm
{
    public interface IMainModel
    {
        public DriveInfo[] GetDrives();
        public void SetDirectory(SelectedPanel panel, DirectoryInfo rootDirectory);
        public bool IsRootDirectory(SelectedPanel panel);
        public DirectoryInfo[] GetDirectories(SelectedPanel currPanel);
        public FileInfo[] GetFiles(SelectedPanel currPanel);
        public void SetCurrentDirectory(SelectedPanel selectedPanel);
        public string GetCurrentDirectory();
        public void PathsToClipboard(List<string> paths);
        public void Paste(string sourcePath, OperationEffect _effect);
        public void copyDirectory(string sourceDir, string destinationDir);
        public void Delete(List<string> paths);
        public bool IsFileExists(SelectedPanel cPanel, string? label);
        void ChangeFileName(SelectedPanel cPanel, string label, string oldName);
        void ShowProperties(string name);
    }
}
