using System.Collections.ObjectModel;
using System.ComponentModel;
using Ncto.Luk.iViewDB;

namespace IViewStrain.Models.Parameters
{
    public class InitParameters : INotifyPropertyChanged
    {
        private string _dbName;
        private ObservableCollection<InspectionInfo> _inspections;
        private string _inspectionCaption;
        private string _imuDataPath;
        private string _profileDataPath;
        private string _ivdProjecFile;
        private ObservableCollection<InitParameters> _previousRuns = new();

        /// <summary>
        /// Полное имя БД
        /// </summary>
        public string DbName
        {
            get => _dbName;
            set
            {
                _dbName = value;
                OnPropertyChanged(nameof(DbName));
            }
        }
        /// <summary>
        /// Коллекция доступных инспекций
        /// </summary>
        public ObservableCollection<InspectionInfo> Inspections
        {
            get => _inspections;
            set
            {
                _inspections = value;
                OnPropertyChanged(nameof(Inspections));
            }
        }
        /// <summary>
        /// Имя выбранной инспекции
        /// </summary>
        public string InspectionCaption
        {
            get => _inspectionCaption;
            set
            {
                _inspectionCaption = value;
                OnPropertyChanged(nameof(InspectionCaption));
            }
        }
        /// <summary>
        /// Полное имя IMU файла
        /// </summary>
        public string ImuDataPath
        {
            get => _imuDataPath;
            set
            {
                _imuDataPath = value;
                OnPropertyChanged(nameof(ImuDataPath));
            }
        }
        /// <summary>
        /// Полное имя профиля файла
        /// </summary>
        public string ProfileDataPath
        {
            get => _profileDataPath;
            set
            {
                _profileDataPath = value;
                OnPropertyChanged(nameof(ProfileDataPath));
            }
        }
        /// <summary>
        /// Полное имя файла-проекта IViewData (.ivdproj)
        /// </summary>
        public string IVDProjecFile
        {
            get => _ivdProjecFile;
            set
            {
                _ivdProjecFile = value;
                OnPropertyChanged(nameof(IVDProjecFile));
            }
        }
        /// <summary>
        /// Коллекция предыдущих прогонов
        /// </summary>
        public ObservableCollection<InitParameters> PreviousRuns
        {
            get => _previousRuns;
            set
            {
                _previousRuns = value;
                OnPropertyChanged(nameof(PreviousRuns));
            }
        }

        #region Реализация интерфейса INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
