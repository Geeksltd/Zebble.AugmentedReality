namespace Zebble.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Zebble.Services;
    using Zebble.Device;

    public class AugmentedReality : Canvas
    {
        const float FULL_CIRCLE = 360;
        public const float CameraViewFieldDegrees = 40;
        List<PointOfInterest> PointsOfInterest = new List<PointOfInterest>();
        CameraView CameraView;
        IGeoLocation ObserverLocation;
        float CurrentValue;
        SmoothCompass Compass;

        public AugmentedReality()
        {
            Height.Set(100.Percent());
            Shown.Handle(OnShown);
        }

        public float PixelsPerDegree { get; private set; }

        public AugmentedReality(IEnumerable<PointOfInterest> pois) : this() { PointsOfInterest = pois.ToList(); }

        public async Task Add(PointOfInterest poi)
        {
            PointsOfInterest.Add(poi);
            if (IsRendered()) await Show(poi);
        }

        public async override Task OnInitializing()
        {
            await base.OnInitializing();
            await Add(CameraView = new CameraView());

            Compass = await SmoothCompass.Create();
            Compass.Changed.Handle(h => Compass_Changed(h));
        }

        void Compass_Changed(SmoothCompass.Heading newHeading)
        {
            if (!IsShown) return;
            if (Math.Abs(CurrentValue - newHeading.SmoothValue) < 1) return;

            CurrentValue = newHeading.SmoothValue;
            Thread.UI.RunAction(PositionPointsOfInterest);
        }

        async Task Show(PointOfInterest poi)
        {
            if (ObserverLocation == null) return;
            poi.View.Style.Absolute = true;
            poi.Angle = (float)ObserverLocation.GetCompassAngle(poi.Location);
            await Add(poi.View);
        }

        async Task OnShown()
        {
            PixelsPerDegree = Root.ActualWidth / CameraViewFieldDegrees;

            var position = await Location.GetCurrentPosition();
            if (position != null)
            {
                ObserverLocation = new GeoLocation(position.Latitude, position.Longitude);
                foreach (var poi in PointsOfInterest) await Show(poi);
            }

            PositionPointsOfInterest();
        }

        void PositionPointsOfInterest()
        {
            var heading = CurrentValue; // Guaranteed to be 0-360

            var minViewField = heading - CameraViewFieldDegrees / 2;

            foreach (var point in PointsOfInterest)
            {
                if (point.Angle == null) { point.View.Hide(); continue; }

                var pointLeft = point.Angle.Value - minViewField;

                if (pointLeft < -FULL_CIRCLE / 2) pointLeft += FULL_CIRCLE;
                else if (pointLeft > FULL_CIRCLE / 2) pointLeft -= FULL_CIRCLE;

                var newX = pointLeft * PixelsPerDegree;

                point.View.X(newX);

                var newY = (ActualHeight - point.View.ActualHeight) / 2;
                if (point.VerticalAngle.HasValue) newY += PixelsPerDegree * point.VerticalAngle.Value;

                point.View.Y(newY);
            }
        }

        public override void Dispose() { Compass?.Dispose(); base.Dispose(); }

        public class PointOfInterest
        {
            public View View { get; set; }
            public IGeoLocation Location { get; set; }
            public float? Angle { get; internal set; }
            public float? VerticalAngle { get; set; }

            public object Tag;
        }
    }
}