using CoordinateMap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateMap.Service
{
    /// <summary>
    /// Интерфейс для работы со списком координат из файла
    /// </summary>
    public interface IFileService
    {

        List<Coordinates> Open(string filename);
        void Save(string filename, List<Coordinates> coordinatesList);
    }
}
