using System.Windows;
using Ncto.Luk.UI.WPF.Localization;

namespace IViewStrain
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void SelectCulture(string culture)
        {
            string requestedSource = string.Format("/IViewStrain;component/Models/Resources/Resources.{0}.xaml", culture);
            string defSource = "/IViewStrain;component/Models/Resources/Resources.en-US.xaml";
            ResourceCulture.Select(Current.Resources, requestedSource, defSource, culture, "en-US", true);
        }
    }
}
