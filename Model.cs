using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    public class Model : IModel
    {
        public DriveInfo[] DrivesArray { get; set; }
        public Model()
        {
            DrivesArray = DriveInfo.GetDrives();
        }

        public DriveInfo[] GetDrives()
        {
            return DrivesArray;
        }

        DirectoryInfo leftDirectory, rightDirectory;
        public void SetDirectory(SelectedPanel panel, DirectoryInfo rootDirectory)
        {
            if (panel == SelectedPanel.left)
            {
                leftDirectory = rootDirectory;
            }
            else
                rightDirectory = rootDirectory;
        }

        public bool IsRootDirectory(SelectedPanel panel)
        {
            if (panel == SelectedPanel.left)
                return leftDirectory.Parent == null;
            else
                return rightDirectory.Parent == null;
        }

        public DirectoryInfo[] GetDirectories(SelectedPanel currPanel)
        {
            if (currPanel == SelectedPanel.left)
            {
                return leftDirectory.GetDirectories();
            }
            else
                return rightDirectory.GetDirectories();
        }

        public FileInfo[] GetFiles(SelectedPanel currPanel)
        {
            if (currPanel == SelectedPanel.left)
            {
                return leftDirectory.GetFiles();
            }
            else
                return rightDirectory.GetFiles();
        }

        public void SetCurrentDirectory(SelectedPanel selectedPanel)
        {
            if (selectedPanel == SelectedPanel.left)
            {
                Directory.SetCurrentDirectory(leftDirectory.FullName);
            }
            else
                Directory.SetCurrentDirectory(rightDirectory.FullName);
        }

        public void PathsToClipboard(List<string> paths)
        {
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Serializable, paths);
        }
        
        public string GetCurrentDirectory()
        {
           return Directory.GetCurrentDirectory();
        }

        public void copyDirectory(string sourceDir, string destinationDir)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                copyDirectory(subDir.FullName, newDestinationDir);
            }
        }

        public void Paste(string sourcePath, OperationEffect _effect)
        {
            if (Clipboard.ContainsData(DataFormats.Serializable))
            {
                List<string> sourceFilePaths = (List<string>)Clipboard.GetData(DataFormats.Serializable);

                foreach (string sourceFilePath in sourceFilePaths)
                {
                    try
                    {
                        string fileName = Path.GetFileName(sourceFilePath);
                        string destinationFilePath = Path.Combine(sourcePath, fileName);

                        if (File.Exists(sourceFilePath))
                        {
                            if (_effect == OperationEffect.cut)
                            {
                                File.Move(sourceFilePath, destinationFilePath);
                            }
                            else
                            {
                                File.Copy(sourceFilePath, destinationFilePath, false);
                            }
                        }
                        else if (Directory.Exists(sourceFilePath))
                        {
                            string sourceFolderName = new DirectoryInfo(sourceFilePath).Name;
                            string destinationFolder = Path.Combine(sourcePath, sourceFolderName);

                            if (_effect == OperationEffect.cut)
                            {
                                Directory.Move(sourceFilePath, destinationFolder);
                            }
                            else
                            {
                                copyDirectory(sourceFilePath, destinationFolder);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }
    }
}
