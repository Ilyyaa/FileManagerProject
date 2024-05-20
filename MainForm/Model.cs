using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace FileManagerProject.MainForm
{
    public class Model : IMainModel
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

        public void Delete(List<string> paths)
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

        public bool IsFileExists(SelectedPanel cPanel, string? label)
        {
            if (cPanel == SelectedPanel.left)
            {
                if (File.Exists(leftDirectory.FullName + "\\" + label))
                    return true;
                else
                    return false;
            }
            else
                if (File.Exists(rightDirectory.FullName + "\\" + label))
                return true;
            else
                return false;

        }

        public void ChangeFileName(SelectedPanel cPanel, string label, string oldName)
        {
        
                if (cPanel == SelectedPanel.left)
                {
                    if(File.Exists(leftDirectory.FullName + "\\" + oldName))
                        System.IO.File.Move(leftDirectory.FullName + "\\" + oldName, leftDirectory.FullName + "\\" + label);
                    else if(Directory.Exists(leftDirectory.FullName + "\\" + oldName))
                        System.IO.Directory.Move(leftDirectory.FullName + "\\" + oldName, leftDirectory.FullName + "\\" + label);
                }
                else
                {
                if (File.Exists(rightDirectory.FullName + "\\" + oldName))
                    System.IO.File.Move(rightDirectory.FullName + "\\" + oldName, rightDirectory.FullName + "\\" + label);
                else if (Directory.Exists(rightDirectory.FullName + "\\" + oldName))
                    System.IO.File.Move(rightDirectory.FullName + "\\" + oldName, rightDirectory.FullName + "\\" + label);
            }
                    
            
            

        }
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;
        public static bool ShowFilePropertiesDialog(string Filename)
        {
            SHELLEXECUTEINFO properties = new SHELLEXECUTEINFO();
            properties.cbSize = Marshal.SizeOf(properties);
            properties.lpVerb = "properties";
            properties.lpFile = Filename;
            properties.nShow = SW_SHOW;
            properties.fMask = SEE_MASK_INVOKEIDLIST;
            return ShellExecuteEx(ref properties);
        }
        public void ShowProperties(string name)
        {
            ShowFilePropertiesDialog(Directory.GetCurrentDirectory() + "\\" + name);
        }
    }
}
