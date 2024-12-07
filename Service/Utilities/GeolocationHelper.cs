namespace Service.Utilities
{
    public static class GeolocationHelper
    {
        public static double CalculateDistance(double? lat1, double? lon1, double lat2, double lon2)
        {
            if (!lat1.HasValue || !lon1.HasValue) return double.MaxValue;

            var R = 6371e3; // mètres
            var φ1 = lat1.Value * Math.PI / 180; // φ, λ en radians
            var φ2 = lat2 * Math.PI / 180;
            var Δφ = (lat2 - lat1.Value) * Math.PI / 180;
            var Δλ = (lon2 - lon1.Value) * Math.PI / 180;

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = R * c; // en mètres
            return distance / 1000; // en kilomètres
        }
    }
}
