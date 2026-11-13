using Ncto.Luk.iViewDB;

namespace IViewStrain.Models.Settings
{
    public class IViewStrainEnvironment
    {
        private readonly string _appName;
        private readonly string _registryKey;

        public IViewStrainEnvironment(string appName, string registryKey, iViewDBEnvironment viewDBEnvironment)
        {
            _registryKey = registryKey;
            _appName = appName;

            Init();
        }

        private void Init()
        {

        }

        /// <summary>
        /// Сохранить пользовательские настройки в реестр
        /// </summary>
        public void SaveToRegistry()
        {

        }
    }
}
