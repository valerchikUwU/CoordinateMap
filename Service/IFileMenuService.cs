using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateMap.Service
{
    /// <summary>
    /// Интерфейс для работы с файловым меню
    /// </summary>
    public interface IFileMenuService
    {
        void ShowMessage(string message);   // показ сообщения
        string FilePath { get; set; }   // путь к выбранному файлу
        bool OpenFile();  // открытие файла
        bool SaveFile();  // сохранение файла
    }
}
