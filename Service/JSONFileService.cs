using CoordinateMap.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace CoordinateMap.Service
{
    /// <summary>
    /// Класс для сериализации данных
    /// </summary>
    public class JSONFileService : IFileService
    {
        /// <summary>
        /// Метод для десериализации данных
        /// </summary>
        /// <param name="filename">Название файла</param>
        /// <returns>Список координат</returns>
        public List<Coordinates> Open(string filename)
        {
            List<Coordinates> phones = new List<Coordinates>();
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(List<Coordinates>));
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                phones = jsonFormatter.ReadObject(fs) as List<Coordinates>;
            }

            return phones;
        }

        /// <summary>
        /// Метод для сериализации данных
        /// </summary>
        /// <param name="filename">Название файла</param>
        /// <param name="coordinatesList">Список координат</param>
        public void Save(string filename, List<Coordinates> coordinatesList)
        {
            DataContractJsonSerializer jsonFormatter =
                new DataContractJsonSerializer(typeof(List<Coordinates>));
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, coordinatesList);
            }
        }
    }
}
