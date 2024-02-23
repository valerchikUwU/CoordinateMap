using CoordinateMap.Infrustructure;
using CoordinateMap.Model;
using CoordinateMap.Service;
using CoordinateMap.Utilities;
using CoordinateMap.View;
using Microsoft.EntityFrameworkCore;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CoordinateMap.ModelView
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        IFileService fileService;
        IFileMenuService fileMenuService;

        AppDbContext db = new AppDbContext();


        /// <summary>
        /// Выбранные координаты
        /// </summary>
        private Coordinates selectedCoordinates;

        /// <summary>
        /// Модель координатной плоскости
        /// </summary>
        private PlotModel plotModel;

        /// <summary>
        /// Список координат
        /// </summary>
        public ObservableCollection<Coordinates> Coordinates { get; set; }

        /// <summary>
        /// Команда для добавления координат
        /// </summary>
        private RelayCommand addCommand;

        /// <summary>
        /// Команда для удаления координат
        /// </summary>
        private RelayCommand removeCommand;

        /// <summary>
        /// Команда для редактирования координат
        /// </summary>
        private RelayCommand editCommand;

        /// <summary>
        /// Команда для сохранения координат в файл
        /// </summary>
        private RelayCommand saveCommand;

        /// <summary>
        /// Команда для открытия файла с координатами
        /// </summary>
        private RelayCommand openCommand;


        /// <summary>
        /// <see cref="addCommand"/>
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand((o) =>
                    {
                        CoordinateWindow coordinateWindow = new CoordinateWindow(new Coordinates());
                        if (coordinateWindow.ShowDialog() == true)
                        {
                            Coordinates coordinates = coordinateWindow.Coordinates;
                            db.Coordinates.Add(coordinates);
                            db.SaveChanges();
                        }
                    }));
            }
        }

        /// <summary>
        /// <see cref="removeCommand"/>
        /// </summary>
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand((selectedCoordinates) =>
                  {
                      // получаем выделенный объект
                      Coordinates coordinates = selectedCoordinates as Coordinates;
                      if (coordinates == null) return;
                      db.Coordinates.Remove(coordinates);
                      db.SaveChanges();
                  }));
            }
        }

        /// <summary>
        /// <see cref="editCommand"/>
        /// </summary>
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedCoordinates) =>
                  {
                      // получаем выделенный объект
                      Coordinates coordinates = selectedCoordinates as Coordinates;
                      if (coordinates == null) return;

                      Coordinates dto = new Coordinates
                      {
                          Id = coordinates.Id,
                          Coordinate_x = coordinates.Coordinate_x,
                          Coordinate_y = coordinates.Coordinate_y
                      };
                      CoordinateWindow coordinateWindow = new CoordinateWindow(dto);


                      if (coordinateWindow.ShowDialog() == true)
                      {
                          coordinates.Coordinate_x = coordinateWindow.Coordinates.Coordinate_x;
                          coordinates.Coordinate_y = coordinateWindow.Coordinates.Coordinate_y;
                          db.Entry(coordinates).State = EntityState.Modified;
                          db.SaveChanges();
                      }
                  }));
            }
        }

        /// <summary>
        /// <see cref="saveCommand"/>
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (fileMenuService.SaveFile() == true)
                          {
                              fileService.Save(fileMenuService.FilePath, Coordinates.ToList());
                              fileMenuService.ShowMessage("Файл сохранен");
                          }
                      }
                      catch (Exception ex)
                      {
                          fileMenuService.ShowMessage(ex.Message);
                      }
                  }));
            }
        }


        /// <summary>
        /// <see cref="openCommand"/>
        /// </summary>
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          if (fileMenuService.OpenFile() == true)
                          {
                              var phones = fileService.Open(fileMenuService.FilePath);
                              Coordinates.Clear();
                              foreach (var p in phones)
                                  Coordinates.Add(p);
                              fileMenuService.ShowMessage("Файл открыт");
                          }
                      }
                      catch (Exception ex)
                      {
                          fileMenuService.ShowMessage(ex.Message);
                      }
                  }));
            }
        }

        /// <summary>
        /// Сеттеры и геттеры для выбранных координат
        /// </summary>
        public Coordinates SelectedCoordinates
        {
            get { return selectedCoordinates; }
            set
            {
                selectedCoordinates = value;
                OnPropertyChanged("SelectedCoordinates");
                UpdatePlot();
            }
        }

        /// <summary>
        /// Конструктор для координатной плоскости
        /// </summary>
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set
            {
                plotModel = value;
                OnPropertyChanged("PlotModel");
            }
        }


        /// <summary>
        /// Конструктор модели представления
        /// </summary>
        /// <param name="fileMenuService">Сервис для работы с меню</param>
        /// <param name="fileService">Сервис для работы с файлом</param>
        public ViewModelBase(IFileMenuService fileMenuService, IFileService fileService)
        {
            this.fileMenuService = fileMenuService;
            this.fileService = fileService;

            db.Database.EnsureCreated();
            db.Coordinates.Load();
            Coordinates = db.Coordinates.Local.ToObservableCollection();

            
            PlotModel = new PlotModel { Title = "Dynamic Plot" };
            /// Создаем две оси с ограничениями 
            var xAxis = new LinearAxis { Position = AxisPosition.Bottom, Minimum = -10, Maximum = 10 };
            var yAxis = new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 };
            PlotModel.Axes.Add(xAxis);
            PlotModel.Axes.Add(yAxis);
            var lineSeries = new LineSeries { Title = "Dynamic Line Series" };
            PlotModel.Series.Add(lineSeries);
        }




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <summary>
        /// Метод для обновления координатной плоскости при выборе координат из списка
        /// </summary>
        public void UpdatePlot()
        {
            var lineSeries = PlotModel.Series[0] as LineSeries;
            if (lineSeries != null && selectedCoordinates != null)
            {
                lineSeries.Points.Clear();
                lineSeries.Points.Add(new DataPoint(selectedCoordinates.Coordinate_x, selectedCoordinates.Coordinate_y));
                lineSeries.MarkerFill = OxyColor.FromRgb(127, 0, 0);
                lineSeries.MarkerType = MarkerType.Circle;
                lineSeries.MarkerSize = 5;

                PlotModel.InvalidatePlot(true);
            }
        }
    }
}
