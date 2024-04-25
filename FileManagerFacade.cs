using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace FileManagerProject
{
    internal class FileManagerFacade
    {
        public OperationEffect _effect = OperationEffect.copy;
        
        DirectoryInfo leftDirectory, rightDirectory;
        public void SetDirectory(SelectedPanel currPanel, DirectoryInfo rootDirectory)
        {
            if (currPanel == SelectedPanel.left)
            {
                leftDirectory = rootDirectory;
            }
            else
                rightDirectory = rootDirectory;
        }
        public bool IsRootDirectory(SelectedPanel currPanel)
        {
            if(currPanel == SelectedPanel.left)
            {
                if (leftDirectory.Parent != null)
                    return false;
                else
                    return true;
            }
            else
            {
                if (rightDirectory.Parent != null)
                    return false;
                else
                    return true;
            }
        }

        public DirectoryInfo[] GetDirectories(SelectedPanel currPanel)
        {
            if(currPanel == SelectedPanel.left)
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

        public void SetCurrentDirectory(SelectedPanel currPanel)
        {
            if(currPanel == SelectedPanel.left)
            {
                Directory.SetCurrentDirectory(leftDirectory.FullName);
            }
            else
                Directory.SetCurrentDirectory(rightDirectory.FullName);
        }

        public void copy(List<string> paths)
        {
            if (paths.Any())
            {
                Clipboard.Clear();
                Clipboard.SetData(DataFormats.Serializable, paths);
                _effect = OperationEffect.copy; // Reset the flag
            }
        }

        public void paste(string destinationDir)
        {
            if (Clipboard.ContainsData(DataFormats.Serializable))
            {
                List<string> sourceFilePaths = (List<string>)Clipboard.GetData(DataFormats.Serializable);

                foreach (string sourceFilePath in sourceFilePaths)
                {
                    try
                    {
                        string fileName = Path.GetFileName(sourceFilePath);
                        string destinationFilePath = Path.Combine(destinationDir, fileName);

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
                            string destinationFolder = Path.Combine(destinationDir, sourceFolderName);

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

        private void copyDirectory(string sourceDir, string destinationDir)
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

        public void delete(List<string> paths)
        {
            foreach (string filePath in paths)
            {
                if (File.Exists(filePath))
                {
                    FileSystem.DeleteFile(filePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else if (Directory.Exists(filePath))
                {
                    FileSystem.DeleteDirectory(filePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin); // Recursive delete for directories
                }
            }
            paths.Clear(); // Clear the list after deletion
        }

        public void cut(List<string> paths)
        {
            if (paths.Any())
            {
                Clipboard.Clear();
                Clipboard.SetData(DataFormats.Serializable, paths);
                _effect = OperationEffect.cut; 
            }
        }

    }
}
