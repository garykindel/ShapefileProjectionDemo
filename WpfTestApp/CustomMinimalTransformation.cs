﻿using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using Mapsui.Geometries;
using Mapsui.Projection;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp
{
    public class CustomMinimalTransformation : ITransformation
    {
        private readonly IDictionary<string, Func<double, double, Point>> _toLonLat = new Dictionary<string, Func<double, double, Point>>();
        private readonly IDictionary<string, Func<double, double, Point>> _fromLonLat = new Dictionary<string, Func<double, double, Point>>();
        
        

        public CustomMinimalTransformation()
        {
            _toLonLat["EPSG:4326"] = (x, y) => new Point(x, y);
            _fromLonLat["EPSG:4326"] = (x, y) => new Point(x, y);
             _toLonLat["EPSG:3857M"] = SphericalMercator.ToLonLat;
             _fromLonLat["EPSG:3857M"] = SphericalMercator.FromLonLat;
             _toLonLat["EPSG:3857"] = CustomProjectionToLonLat;
             _fromLonLat["EPSG:3857"] = CustomProjectionFromLonLat;
            _toLonLat["EPSG:CUSTOM"] = CustomProjectionToLonLat;
            _fromLonLat["EPSG:CUSTOM"] = CustomProjectionFromLonLat;
        }
        
        public IGeometry Transform(string fromCRS, string toCRS, IGeometry geometry)
        {
            Transform(geometry.AllVertices(), _toLonLat[fromCRS]);
            Transform(geometry.AllVertices(), _fromLonLat[toCRS]);
            return geometry; // this method should not have a return value
        }

        private static void Transform(IEnumerable<Point> points, Func<double, double, Point> transformFunc)
        {
            foreach (var point in points)
            {
                var transformed = transformFunc(point.X, point.Y);
                point.X = transformed.X;
                point.Y = transformed.Y;
            }
        }

        public BoundingBox Transform(string fromCRS, string toCRS, BoundingBox boundingBox)
        {
            Transform(boundingBox.AllVertices(), _toLonLat[fromCRS]);
            Transform(boundingBox.AllVertices(), _fromLonLat[toCRS]);
            return boundingBox; // this method not have a return value
        }

        public bool? IsProjectionSupported(string fromCRS, string toCRS)
        {
            if (!_toLonLat.ContainsKey(fromCRS)) return false;
            if (!_fromLonLat.ContainsKey(toCRS)) return false;
            return true;
        }
        
        
        CoordinateTransformationFactory _ctFac;
        ICoordinateTransformation _ctTo;   //Custom to WGS84
        ICoordinateTransformation _ctFrom; //WGS84 to Custom

        public void LoadSourceWKT(string filepath)
        {
            ICoordinateSystemFactory csFac = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
            ICoordinateSystem csSource = null;
            ICoordinateSystem csTarget = null;
            if (!String.IsNullOrWhiteSpace(filepath) && System.IO.File.Exists(filepath))
            {
                string wkt = System.IO.File.ReadAllText(filepath);
                csSource = csFac.CreateFromWkt(wkt);
                csTarget = ProjectedCoordinateSystem.WebMercator;
            }
            else
            {
                csSource = GeographicCoordinateSystem.WGS84;
                csTarget = ProjectedCoordinateSystem.WebMercator;
            }
            _ctFac = new CoordinateTransformationFactory();

            _ctTo = _ctFac.CreateFromCoordinateSystems(csSource, csTarget);
            _ctFrom = _ctFac.CreateFromCoordinateSystems(csTarget, csSource);
        }

        public Point CustomProjectionFromLonLat(double lon, double lat)
        {
            Point pt = new Point(lon, lat);                       
            double[] result= _ctTo.MathTransform.Transform(pt.ToDoubleArray());
            return new Point(result[0], result[1]);
        }

        public Point CustomProjectionToLonLat(double x, double y)
        {
            Point pt = new Point(x, y);
            double[] result = _ctFrom.MathTransform.Transform(pt.ToDoubleArray());
            return new Point(result[0], result[1]);
        }

    }
}
