using MauiCustomMapIcons.Models;

namespace MauiCustomMapIcons
{
    public partial class MainPage : ContentPage
    {
        private List<MapPin> _pins;
        public List<MapPin> Pins 
        { 
            get { return _pins; }
            set { _pins = value; OnPropertyChanged(); }
        }

        public MainPage()
        {
            InitializeComponent();

            BindingContext = this;

            Pins = new List<MapPin>()
            {
                new MapPin(MapPinClicked)
                {
                    Id = Guid.NewGuid().ToString(),
                    Position = new Location(51.731551, -0.156230),
                    IconSrc = "icon_type_one"
                },
                new MapPin(MapPinClicked)
                {
                    Id = Guid.NewGuid().ToString(),
                    Position = new Location(51.762951, -0.182317),
                    IconSrc = "icon_type_two"
                },
                new MapPin(MapPinClicked)
                {
                    Id = Guid.NewGuid().ToString(),
                    Position = new Location(51.754034, -0.074997),
                    IconSrc = "icon_type_three"
                },
                new MapPin(MapPinClicked)
                {
                    Id = Guid.NewGuid().ToString(),
                    Position = new Location(51.704029, -0.135474),
                    IconSrc = "icon_type_four"
                }
            };
        }

        private void MapPinClicked(MapPin pin)
        {
            // Handle pin click
        }
    }
}