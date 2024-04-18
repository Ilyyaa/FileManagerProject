using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject
{
    internal class FileManagerFacade
    {
        public OperationEffect _effect = OperationEffect.copy;

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
                    File.Delete(filePath);
                }
                else if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true); // Recursive delete for directories
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
