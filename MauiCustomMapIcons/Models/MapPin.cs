using System.Windows.Input;

namespace MauiCustomMapIcons.Models
{
    public class MapPin
    {
        public string Id { get; set; }
        public Location Position { get; set; }
        public string IconSrc { get; set; }
        public ICommand ClickedCommand { get; set; }

        public MapPin(Action<MapPin> clicked)
        {
            ClickedCommand = new Command(() => clicked(this));
        }
    }
}