using CoreLocation;
using MapKit;
using MauiCustomMapIcons.Controls;
using MauiCustomMapIcons.Models;
using Microsoft.Maui.Maps.Handlers;
using Microsoft.Maui.Maps.Platform;
using UIKit;

namespace MauiCustomMapIcons.Platforms.iOS
{
    public class CustomMapHandler : MapHandler
    {
        private readonly Dictionary<string, UIImage> _iconMap = [];

        public static new IPropertyMapper<MapEx, CustomMapHandler> Mapper = new PropertyMapper<MapEx, CustomMapHandler>(MapHandler.Mapper)
        {
            [nameof(MapEx.CustomPins)] = MapPins
        };

        public Dictionary<IMKAnnotation, MapPin> MarkerMap { get; } = [];

        public CustomMapHandler()
            : base(Mapper)
        {
        }

        protected override void ConnectHandler(MauiMKMapView platformView)
        {
            base.ConnectHandler(platformView);

            platformView.DidSelectAnnotationView += CustomMapHandler_DidSelectAnnotationView;
            platformView.GetViewForAnnotation += GetViewForAnnotation;
        }

        protected override void DisconnectHandler(MauiMKMapView platformView)
        {
            base.DisconnectHandler(platformView);

            platformView.DidSelectAnnotationView -= CustomMapHandler_DidSelectAnnotationView;
            platformView.GetViewForAnnotation -= GetViewForAnnotation;
        }

        private void CustomMapHandler_DidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (MarkerMap.TryGetValue(e.View.Annotation, out MapPin value))
            {
                value.ClickedCommand?.Execute(null);
                PlatformView.DeselectAnnotation(e.View.Annotation, false);
            }
        }

        private static new void MapPins(IMapHandler handler, Microsoft.Maui.Maps.IMap map)
        {
            if (handler is CustomMapHandler mapHandler && handler.VirtualView is MapEx mapEx)
            {
                handler.PlatformView.RemoveAnnotations(mapHandler.MarkerMap.Select(x => x.Key).ToArray());

                mapHandler.MarkerMap.Clear();

                mapHandler.AddPins();
            }
        }

        private void AddPins()
        {
            if (VirtualView is MapEx mapEx)
            {
                foreach (var pin in mapEx.CustomPins)
                {
                    var marker = new MKPointAnnotation(new CLLocationCoordinate2D(pin.Position.Latitude, pin.Position.Longitude));

                    PlatformView.AddAnnotation(marker);

                    MarkerMap.Add(marker, pin);
                }
            }
        }

        private MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            if (annotation == null || annotation is MKUserLocation)
            {
                return null;
            }

            var customPin = GetCustomPin(annotation);

            if (customPin == null)
            {
                return null;
            }

            return mapView.DequeueReusableAnnotation(customPin.Id) ?? new MKAnnotationView
            {
                Image = GetIcon(customPin.IconSrc),
                CanShowCallout = false
            };
        }

        private MapPin GetCustomPin(IMKAnnotation mkPointAnnotation)
        {
            if (MarkerMap.TryGetValue(mkPointAnnotation, out MapPin value))
            {
                return value;
            }

            return null;
        }

        private UIImage GetIcon(string icon)
        {
            if (_iconMap.TryGetValue(icon, out UIImage? value))
            {
                return value;
            }

            var image = UIImage.FromBundle(icon);

            _iconMap[icon] = image;

            return image;
        }
    }
}