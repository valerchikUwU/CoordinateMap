using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateMap.Model
{
    
    public class Coordinates : INotifyPropertyChanged
    {
        /// <summary>
        /// Координата по оси абсцисс
        /// </summary>
        private int coordinate_x;

        /// <summary>
        /// Координата по оси ординат
        /// </summary>
        private int coordinate_y;

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Сеттеры и геттеры для свойства coordinate_x
        /// </summary>
        public int Coordinate_x
        {
            get { return coordinate_x; }
            set
            {
                coordinate_x = value;
                if (value >= -10 && value <= 10)
                {
                    OnPropertyChanged("Coordinate_x");
                }
                else
                {
                    throw new Exception("Number must be greater than -10 and less than 10");
                }
            }
        }

        /// <summary>
        /// Сеттеры и геттеры для свойства coordinate_y
        /// </summary>
        public int Coordinate_y
        {
            get { return coordinate_y; }
            set
            {
                coordinate_y = value;
                if (value >= -10 && value <= 10)
                {
                    OnPropertyChanged("Coordinate_y");
                }
                else 
                {
                    throw new Exception("Number must be greater than -10 and less than 10");
                }
            }
        }


        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
