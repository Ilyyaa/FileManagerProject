using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileManagerProject.MainForm;

namespace FileManagerProject
{
    public interface IView
    {
        /// <summary>
        /// Добавляет в комбинированный список переданные диски
        /// </summary>
        /// <param name="drives"></param>
        public void SetUpComboboxes(DriveInfo[] drives);

        /// <summary>
        /// Устанавливает представителя для объекта представления
        /// </summary>
        /// <param name="presenter"></param>
        public void SetPresenter(Presenter presenter);

        /// <summary>
        /// Возвращает выбранный диск в комбинированном списке N1
        /// </summary>
        public DriveInfo selectedDrive1 { get; }

        /// <summary>
        /// Возвращает выбранный диск в комбинированном списке N2
        /// </summary>
        public DriveInfo selectedDrive2 { get; }

        public void PopulateListView(SelectedPanel selectedPanel);

        public void AddItemToListView(SelectedPanel selectedPanel, ListViewItem item);
        void AddImageToList(SelectedPanel selectedPanel, string str, Bitmap image);
        void ImageListClear();
        void ListViewItemsClear(SelectedPanel selectedPanel);
    }
}
