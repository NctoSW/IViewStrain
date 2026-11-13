using System.IO;
using System.Reflection;
using System.Windows;
using IViewStrain.Models.Parameters;
using IViewStrain.Models.Settings;
using IViewStrain.View;
using Ncto.Luk.iViewDB;
using Ncto.Luk.iViewDB.Controls;
using static Ncto.Luk.Controls.WPF.Luk_MessageBox;

namespace IViewStrain.Repository.Managers
{
    public class GlobalManager
    {
        public const string APPLICATIONNAME = "IViewStrain";
        public const string PROJECTEXTENSION = ".ivsproj";
        private static readonly string _globalSettingPath = @$"Software\\NCTO\\{APPLICATIONNAME}";

        private readonly MainWindow _mainWindow;
        private readonly string _title;

        public readonly iViewDBEnvironment IViewDBEnvironment;
        public readonly IViewStrainEnvironment IViewStrainEnvironment;

        public GlobalManager(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;

            // Init global setting
            IViewDBEnvironment = new(APPLICATIONNAME, _globalSettingPath);
            IViewStrainEnvironment = new(APPLICATIONNAME, _globalSettingPath, IViewDBEnvironment);
            InitCulture();

            // Title
            _title = CreateTitle();
            SetApplicationTitle();

            OpenLoadingControl();
            _mainWindow.Closing += MainWindow_Closing;
        }

        private void OpenLoadingControl()
        {
            LoadingControl loadingControl = new(this);
            _mainWindow.Content_UserControl.Content = loadingControl;
        }
        private static string CreateTitle()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string appName = Path.Combine(AppContext.BaseDirectory, System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe");
            string time = File.GetLastWriteTime(appName).ToString();

            return $"{APPLICATIONNAME} ({version}, {time})";
        }

        /// <summary>
        /// Переключить культуру
        /// <para>Изменяет языковые ресурсы</para>
        /// </summary>
        public void SwitchCulture()
        {
            switch (IViewDBEnvironment.LangID)
            {
                case LangId.LANG_Ru:
                    IViewDBEnvironment.LangID = LangId.LANG_En;
                    break;
                case LangId.LANG_En:
                    IViewDBEnvironment.LangID = LangId.LANG_Ru;
                    break;
            }

            InitCulture();
        }
        /// <summary>
        /// Инициализации культуры
        /// </summary>
        public void InitCulture()
        {
            switch (IViewDBEnvironment.LangID)
            {
                case LangId.LANG_Ru:
                    App.SelectCulture("ru-RU");
                    break;
                case LangId.LANG_En:
                    App.SelectCulture("en-US");
                    break;
            }

            IViewDBEnvironment.InitDictionaries();
        }
        /// <summary>
        /// Задать заголовок окна (MainWindow)
        /// </summary>
        /// <param name="name"></param>
        public void SetApplicationTitle(string name = null)
        {
            string title = _title;
            if (!string.IsNullOrEmpty(name))
            {
                title += $" - {name}";
            }
            _mainWindow.Title = title;
        }
        /// <summary>
        /// Открыть окно настроек базы данных
        /// </summary>
        public void OpenDBSettingsWindow()
        {
            ProjectEditorControl projectEditorControl = new(IViewDBEnvironment);
            projectEditorControl.CloseControl += ProjectEditorControl_CloseControl;
            _mainWindow.Content_UserControl.Content = projectEditorControl;
        }
        /// <summary>
        /// Закрыть программу
        /// </summary>
        public void CloseProgram()
        {
            _mainWindow.Close();
        }
        public bool InitChartManager(InitParameters parameters, string fileProject = null)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            bool result = true;

            try
            {

            }
            catch (Exception ex)
            {
                result = false;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            
            return result;
        }

        // EVENTS
        private void ProjectEditorControl_CloseControl(System.Windows.Controls.UserControl sender)
        {
            (sender as IClosableUserControl).CloseControl -= ProjectEditorControl_CloseControl;
            OpenLoadingControl();
        }
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Закрыть все второстепенные окна
            foreach (Window item in Application.Current.Windows)
            {
                if (item == _mainWindow) continue;
                item.Close();
            }

            // Сохранить файл- проект

            // Сохранить глобальные настройки
            IViewDBEnvironment.SaveToRegistry();
            IViewStrainEnvironment.SaveToRegistry();
        }
    }
}
