using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using MauiCustomMapIcons.Controls;
using MauiCustomMapIcons.Models;
using Microsoft.Maui.Maps.Handlers;

namespace MauiCustomMapIcons.Platforms.Android
{
    public class CustomMapHandler : MapHandler
    {
        private const int _iconSize = 60;

        private readonly Dictionary<string, BitmapDescriptor> _iconMap = [];

        public static new IPropertyMapper<MapEx, CustomMapHandler> Mapper = new PropertyMapper<MapEx, CustomMapHandler>(MapHandler.Mapper)
        {
            [nameof(MapEx.CustomPins)] = MapPins
        };

        public Dictionary<string, (Marker Marker, MapPin Pin)> MarkerMap { get; } = [];

        public CustomMapHandler()
            : base(Mapper)
        {
        }

        protected override void ConnectHandler(MapView platformView)
        {
            base.ConnectHandler(platformView);
            var mapReady = new MapCallbackHandler(this);

            PlatformView.GetMapAsync(mapReady);
        }

        private static new void MapPins(IMapHandler handler, Microsoft.Maui.Maps.IMap map)
        {
            if (handler.Map is null || handler.MauiContext is null)
            {
                return;
            }

            if (handler is CustomMapHandler mapHandler)
            {
                foreach (var marker in mapHandler.MarkerMap)
                {
                    marker.Value.Marker.Remove();
                }

                mapHandler.MarkerMap.Clear();

                mapHandler.AddPins();
            }
        }

        private BitmapDescriptor GetIcon(string icon)
        {
            if (_iconMap.TryGetValue(icon, out BitmapDescriptor? value))
            {
                return value;
            }

            var drawable = Context.Resources.GetIdentifier(icon, "drawable", Context.PackageName);
            var bitmap = BitmapFactory.DecodeResource(Context.Resources, drawable);
            var scaled = Bitmap.CreateScaledBitmap(bitmap, _iconSize, _iconSize, false);
            bitmap.Recycle();
            var descriptor = BitmapDescriptorFactory.FromBitmap(scaled);

            _iconMap[icon] = descriptor;

            return descriptor;
        }

        private void AddPins()
        {
            if (VirtualView is MapEx mapEx && mapEx.CustomPins != null)
            {
                foreach (var pin in mapEx.CustomPins)
                {
                    var markerOption = new MarkerOptions();
                    markerOption.SetTitle(string.Empty);
                    markerOption.SetIcon(GetIcon(pin.IconSrc));
                    markerOption.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
                    var marker = Map.AddMarker(markerOption);

                    MarkerMap.Add(marker.Id, (marker, pin));
                }
            }
        }

        public void MarkerClick(object sender, GoogleMap.MarkerClickEventArgs args)
        {
            if (MarkerMap.TryGetValue(args.Marker.Id, out (Marker Marker, MapPin Pin) value))
            {
                value.Pin.ClickedCommand?.Execute(null);
            }
        }
    }

    public class MapCallbackHandler : Java.Lang.Object, IOnMapReadyCallback
    {
        private readonly CustomMapHandler mapHandler;

        public MapCallbackHandler(CustomMapHandler mapHandler)
        {
            this.mapHandler = mapHandler;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            mapHandler.UpdateValue(nameof(MapEx.CustomPins));
            googleMap.MarkerClick += mapHandler.MarkerClick;
        }
    }
}