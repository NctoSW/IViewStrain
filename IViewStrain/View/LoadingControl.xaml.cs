using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using IViewStrain.Models.Parameters;
using IViewStrain.Repository.Managers;
using Ncto.Luk.Controls.WPF;
using Ncto.Luk.iViewDB;
using Ncto.Luk.PigData.Imu.Idat;
using Ncto.Luk.PigData.Imu.Ndat;
using Ncto.Luk.Tools.IO;
using Ncto.Luk.UI.WPF;
using static Ncto.Luk.Controls.WPF.Luk_MessageBox;

namespace IViewStrain.View
{
    /// <summary>
    /// Логика взаимодействия для LoadingControl.xaml
    /// </summary>
    public partial class LoadingControl : UserControl
    {
        private const string IVDPROJECTEXTENSION = ".ivdproj";
        private readonly GlobalManager _globalManager;
        private InitParameters _initParameters;
        private bool SetEnabledBtn
        {
            set
            {
                UploadDF_Button.IsEnabled = value;
                UploadDB_Button.IsEnabled = value;
                UploadPF_Button.IsEnabled = value;
            }
        }
        public ObservableCollection<object> Items { get; set; }

        public LoadingControl(GlobalManager globalManager)
        {
            InitializeComponent();

            _globalManager = globalManager;
            UpdateCulture();

            RecentFiles.Init(globalManager.IViewDBEnvironment.RecentFiles);

            Items = new ObservableCollection<object>();
        }

        private HashSet<string> GetSelectedInspections(string dbName, string caption)
        {
            if (_initParameters is null) return new HashSet<string>();
            HashSet<string> selectedInspections = _initParameters.PreviousRuns.Where(w => string.Equals(w.DbName, dbName) && !string.Equals(w.InspectionCaption, caption))
                                                                              .Select(s => s.InspectionCaption)
                                                                              .ToHashSet();
            if (string.Equals(_initParameters.DbName, dbName) && !string.Equals(_initParameters.InspectionCaption, caption))
            {
                selectedInspections.Add(_initParameters.InspectionCaption);
            }

            return selectedInspections;
        }
        private void UpdateCulture()
        {
            switch (_globalManager.IViewDBEnvironment.LangID)
            {
                case LangId.LANG_Ru:
                    Language_Image.SetResourceReference(Image.SourceProperty, "Flag_Ru_16x16");
                    break;
                case LangId.LANG_En:
                    Language_Image.SetResourceReference(Image.SourceProperty, "Flag_Uk_16x16");
                    break;
            }
        }
        private void Close()
        {
            
        }
        private void Reset()
        {
            _initParameters = null;
            SetEnabledBtn = true;

            RecentFiles.Visibility = Visibility.Visible;
            UploadDF_StackPanel.Visibility = Visibility.Collapsed;
            UploadDB_StackPanel.Visibility = Visibility.Collapsed;
        }

        // EVENTS
        private void AddItem_BtnClick(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;

            if (fe.DataContext is InitParameters ip)
            {
                ip.PreviousRuns.Add(new InitParameters());
            }
        }
        private void DeleteItem_BtnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (_initParameters?.PreviousRuns is ObservableCollection<InitParameters> source)
                {
                    if (btn.DataContext is InitParameters parameters)
                    {
                        source.Remove(parameters);
                    }
                }
            }
        }
        private void BackUpload_BtnClick(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        // Загрузить файлы данных
        private void UploadDF_BtnClick(object sender, RoutedEventArgs e)
        {
            _initParameters = new();
            SetEnabledBtn = false;
            UploadDF_StackPanel.Visibility = Visibility.Visible;
            UploadDF_StackPanel.DataContext = _initParameters;

            RecentFiles.Visibility = Visibility.Collapsed;
        }
        private void UploadFile_BtnClick(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (string.IsNullOrEmpty(fe.Tag.ToString())) return;

            if (fe.DataContext is InitParameters ip)
            {
                bool isIMU = string.Equals(fe.Tag, IdatFile.DefaultExt);

                try
                {
                    if (isIMU)
                    {
                        string path = FileDialogs.OpenFileDialog(App.Current.MainWindow,
                                                                 App.Current.TryFindResource("VSstr_OpenFile") as string,
                                                                 new FileDialogs.Filter(App.Current.TryFindResource("VSstr_IMUFile") as string, IdatFile.FileExtension),
                                                                 IdatFile.DefaultExt);
                        if (!PathTools.FileExist(path)) return;

                        ip.ImuDataPath = path;
                    }
                    else
                    {
                        string path = FileDialogs.OpenFileDialog(App.Current.MainWindow,
                                                                 App.Current.TryFindResource("VSstr_OpenFile") as string,
                                                                 new FileDialogs.Filter(App.Current.TryFindResource("VSstr_ProfileFile") as string, NdatFile.FileExtension),
                                                                 NdatFile.DefaultExt);
                        if (!PathTools.FileExist(path)) return;

                        ip.ProfileDataPath = path;
                    }
                }
                catch (Exception ex)
                {
                    Luk_MessageBox.Show(App.Current.MainWindow,
                                        App.Current.TryFindResource("VSstr_Error") as string,
                                        ex.Message,
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error,
                                        _globalManager.IViewDBEnvironment.LangID);
                }
            }
        }
        private void RemoveFile_BtnClick(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (string.IsNullOrEmpty(fe.Tag.ToString())) return;

            if (fe.DataContext is InitParameters ip)
            {
                bool isIMU = string.Equals(fe.Tag, IdatFile.DefaultExt);

                if (isIMU) ip.ImuDataPath = null;
                else ip.ProfileDataPath = null;
            }
        }
        private void ContinueDF_BtnClick(object sender, RoutedEventArgs e)
        {

        }
        // Загрузить инспекции
        private void UploadDB_BtnClick(object sender, RoutedEventArgs e)
        {
            _initParameters = new();
            SetEnabledBtn = false;
            UploadDB_StackPanel.Visibility = Visibility.Visible;
            UploadDB_StackPanel.DataContext = _initParameters;

            RecentFiles.Visibility = Visibility.Collapsed;
        }
        private void LoadDB_BtnClick(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;

            if (fe.DataContext is InitParameters ip)
            {
                try
                {
                    string path = FileDialogs.OpenFileDialog(App.Current.MainWindow,
                                                             App.Current.TryFindResource("VSstr_OpenFile") as string,
                                                             new FileDialogs.Filter(App.Current.TryFindResource("VSstr_Database") as string, ".db"),
                                                             ".db");
                    if (!PathTools.FileExist(path)) return;

                    ip.DbName = path;
                    ip.Inspections = null;
                    ip.InspectionCaption = null;
                    ip.IVDProjecFile = null;
                    ip.ImuDataPath = null;
                    ip.ProfileDataPath = null;
                }
                catch (Exception ex)
                {
                    Luk_MessageBox.Show(App.Current.MainWindow,
                                        App.Current.TryFindResource("VSstr_Error") as string,
                                        ex.Message,
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error,
                                        _globalManager.IViewDBEnvironment.LangID);
                }
            }
        }
        private void DropDownOpened_CBDropOpened(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;

            if (cb.DataContext is InitParameters ip)
            {
                if (ip.DbName is null) return;

                var selectedInspections = GetSelectedInspections(ip.DbName, ip.InspectionCaption);
                ProjectInfo projectInfo = new(ip.DbName, true);
                // Список с актуальными элементами
                var targetInspections = projectInfo.InspInfo.Where(w => w.TableId is Ncto.Luk.iViewDBIDs.TableId.Features
                                                                     && !selectedInspections.Contains(w.Caption))
                                                            .ToList();
                if (ip.Inspections is null)
                {
                    ip.Inspections = new(targetInspections);
                }
                else
                {
                    // Удаляем не актуальные элементы 
                    for (int i = ip.Inspections.Count - 1; i >= 0; i--)
                    {
                        var item = ip.Inspections[i];
                        if (!targetInspections.Any(t => string.Equals(t.Caption, item.Caption)))
                        {
                            ip.Inspections.RemoveAt(i);
                        }
                    }
                    // Добавляем актуальные элементы 
                    foreach (var item in targetInspections)
                    {
                        if (!ip.Inspections.Any(c => string.Equals(c.Caption, item.Caption)))
                        {
                            ip.Inspections.Add(item);
                        }
                    }
                }
            }
        }
        private void SelectionTable_CBSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;

            if (cb.SelectedValue is InspectionInfo insp && cb.DataContext is InitParameters ip)
            {
                ip.InspectionCaption = insp.Caption;
                ip.ImuDataPath = insp.ImuDataFile;
                ip.ProfileDataPath= insp.ProfileDataFile;
                ip.IVDProjecFile = null;
            }
        }
        private void LoadIvdPF_BtnClick(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;

            if (fe.DataContext is InitParameters ip)
            {
                string path = FileDialogs.OpenFileDialog(App.Current.MainWindow,
                                                         App.Current.TryFindResource("VSstr_OpenFile") as string,
                                                         new FileDialogs.Filter(App.Current.TryFindResource("VSstr_IViewDataProjectFile") as string, IVDPROJECTEXTENSION),
                                                         IVDPROJECTEXTENSION);
                if (!PathTools.FileExist(path)) return;

                ip.IVDProjecFile = path;
            }
        }
        private void ContinueDB_BtnClick(object sender, RoutedEventArgs e)
        {

        }
        // Загрузить файл-проект
        private void UploadPF_BtnClick(object sender, RoutedEventArgs e)
        {
            _initParameters = new();

            try
            {
                string path = FileDialogs.OpenFileDialog(App.Current.MainWindow,
                                                         App.Current.TryFindResource("VSstr_OpenFile") as string,
                                                         new FileDialogs.Filter(App.Current.TryFindResource("VSstr_ProjectFile") as string, GlobalManager.PROJECTEXTENSION),
                                                         GlobalManager.PROJECTEXTENSION);
                if (!PathTools.FileExist(path)) return;

            }
            catch (Exception ex)
            {
                Luk_MessageBox.Show(App.Current.MainWindow,
                                    App.Current.TryFindResource("VSstr_Error") as string,
                                    ex.Message,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error,
                                    _globalManager.IViewDBEnvironment.LangID);
            }
        }
        // Кнопки на главном экране
        private void SwitchCulture_BtnClick(object sender, RoutedEventArgs e)
        {
            _globalManager.SwitchCulture();
            UpdateCulture();
        }
        private void OpenDBSettingsWindow_BtnClick(object sender, RoutedEventArgs e)
        {
            _globalManager.OpenDBSettingsWindow();
            Close();
        }
        private void OpenUserSettingsWindow_BtnClick(object sender, RoutedEventArgs e)
        {

        }
        private void CloseProgram_BtnClick(object sender, RoutedEventArgs e)
        {
            _globalManager.CloseProgram();
        }
        private void RecentlyUsed_RFSelectionChanged(object sender, Ncto.Luk.Controls.Luk_ValueChangedEventArgs e)
        {

        }
    }
}
