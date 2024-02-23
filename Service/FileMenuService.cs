using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace CoordinateMap.Service
{
    /// <summary>
    /// Класс для работы с меню
    /// </summary>
    public class FileMenuService : IFileMenuService
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Метод для открытия файла
        /// </summary>
        /// <returns>Булев флаг</returns>
        public bool OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Метод для сохранения файла
        /// </summary>
        /// <returns>Булев флаг</returns>
        public bool SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Метод для диалогового окна
        /// </summary>
        /// <param name="message">Текст успешного или неуспешного выполнения</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
