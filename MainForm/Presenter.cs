using FileManagerProject.MainForm;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FileManagerProject.MainForm
{
    public class Presenter
    {
        public IView view;
        public IMainModel model;
        private OperationEffect _effect = OperationEffect.copy;

        public Presenter(IView _view, IMainModel _model)
        {
            view = _view;
            model = _model;
            view.SetPresenter(this); 
            //model.SetPresenter(this);
        }
        /// <summary>
        /// Получает из модели информацию о доступных дисках и передает ее представлению
        /// </summary>
        public void SetUpDriveLists()
        {
            DriveInfo[] drives = model.GetDrives();
            view.SetUpComboboxes(drives);
        }
        public DriveInfo SelectedDrive { get; set; }
        /// <summary>
        /// Дает команду модели установить 
        /// </summary>
        /// <param name="selectedPanel"></param>
        /// 
        public void ChangeDrive(SelectedPanel selectedPanel)
        {
            if (selectedPanel == SelectedPanel.left)
                SelectedDrive = view.selectedDrive1;
            else
                SelectedDrive = view.selectedDrive2;
            model.SetDirectory(selectedPanel, SelectedDrive.RootDirectory);
            SetListView(selectedPanel);
        }

        public void SetListView(SelectedPanel currPanel)
        {
            //System.Windows.Forms.ListView listView;
            view.ListViewItemsClear(currPanel);
            if (!model.IsRootDirectory(currPanel))
            {
                ListViewItem item = new ListViewItem("..");
                item.Tag = "ArrowUp";
                item.ImageKey = "arrowup";
                view.AddItemToListView(currPanel, item);
                view.AddImageToList(currPanel, "arrowup", Properties.Resources.ArrowUp);
            }
            view.AddImageToList(currPanel, "folder", Properties.Resources.folder);
            foreach (var dir in model.GetDirectories(currPanel))
            {
                ListViewItem item = new ListViewItem(dir.Name);
                item.Tag = "Dir";
                item.SubItems.Add("");
                item.SubItems.Add("<DIR>");
                item.SubItems.Add(dir.CreationTime.ToShortDateString());
                item.ImageKey = "folder";
                view.AddItemToListView(currPanel, item);

            }

            foreach (var file in model.GetFiles(currPanel))
            {

                ListViewItem item = new ListViewItem(file.Name);
                item.Tag = "File";
                item.ImageKey = file.FullName;
                // Добавить элемент в ListView
                view.AddItemToListView(currPanel, item);
                item.SubItems.Add(file.Extension);
                item.SubItems.Add(file.Length.ToString());
                item.SubItems.Add(file.CreationTime.ToShortDateString());
                view.AddImageToList(currPanel, file.FullName, Icon.ExtractAssociatedIcon(file.FullName).ToBitmap());
            }

        }

        public void SetCurrentDirectory(SelectedPanel selectedPanel)
        {
            model.SetCurrentDirectory(selectedPanel);
        }

        public void SetDirectory(SelectedPanel cPanel, DirectoryInfo newDir)
        {
            model.SetDirectory(cPanel, newDir);
        }

        public void ChangeDir(string path, SelectedPanel cPanel)
        {
            var newDir = new DirectoryInfo(path);
            SetDirectory(cPanel, newDir);
            SetCurrentDirectory(cPanel);
            SetListView(cPanel);
        }

        public void copy(List<string> items)
        {
                string CurrentDir = model.GetCurrentDirectory();
                List<string> paths = new List<string>();
                foreach (var item in items)
                {
                    paths.Add(CurrentDir + "\\" + item);
                }
                if (paths.Any())
                {
                    _effect = OperationEffect.copy; // set the flag
                    model.PathsToClipboard(paths);
                }
        }

        public void paste()
        {
            string sourcePath;
            sourcePath = model.GetCurrentDirectory();
            model.Paste(sourcePath, _effect);
            

        }

        public void cut(List<string> items)
        {
                string CurrentDir = model.GetCurrentDirectory();
                List<string> paths = new List<string>();
                foreach (var item in items)
                {
                    paths.Add(CurrentDir + "\\" + item);
                }
                if (items.Any())
                {
                    _effect = OperationEffect.cut; // set the flag
                    model.PathsToClipboard(paths);
                }
        }

        public async void delete(List<string> items)
        {
            try
            {
                string CurrentDir = model.GetCurrentDirectory();
                List<string> paths = new List<string>();
                foreach (var item in items)
                {
                    paths.Add(CurrentDir + "\\" + item);
                }
                if (items.Any())
                {
                    await Task.Factory.StartNew(() => {
                        model.Delete(items);
                    });
                    SetListView(SelectedPanel.left);
                    SetListView(SelectedPanel.right);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool AllowChangeLabel(SelectedPanel cPanel, string? label)
        {
            if (model.IsFileExists(cPanel, label))
                return false;
            else
                return true;
        }

        public void ChangeFileName(string oldName, SelectedPanel cPanel, string label)
        {
            model.ChangeFileName(cPanel, label, oldName);
        }

        public void ShowProperties(string name)
        {
            model.ShowProperties(name);
        }



        public void CreateShellLink(string text)
        {
            model.CreateShellLink(text);
            SetListView(SelectedPanel.left);
            SetListView(SelectedPanel.right);
        }
    }
}
