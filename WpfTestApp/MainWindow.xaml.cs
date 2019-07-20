using BruTile.Predefined;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnOSM_Click(object sender, RoutedEventArgs e)
        {
            //this.xMapControl.RenderMode = Mapsui.UI.Wpf.RenderMode.Wpf;
            this.xMapControl.Map = new Mapsui.Map();                     
            this.xMapControl.Map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.BingHybrid)));
            this.xMapControl.Refresh();
        }

        private void BtnOSMWithUS_Click(object sender, RoutedEventArgs e)
        {
            var customTrans = new CustomMinimalTransformation();
            customTrans.LoadSourceWKT(null);

            //this.xMapControl.RenderMode = Mapsui.UI.Wpf.RenderMode.Wpf;
            this.xMapControl.Map = new Mapsui.Map() { CRS = "EPSG:3857", Transformation = customTrans };
            this.xMapControl.Map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.BingHybrid)));
            IProvider wShapeFile = new Mapsui.Desktop.Shapefile.ShapeFile("lower48.shp", true) { CRS = "EPSG:4326" };
            //IProvider wShapeFile = new Mapsui.Desktop.Shapefile.ShapeFile("lower48.shp", true);

            var wLayerStyle = new Mapsui.Styles.VectorStyle();
            wLayerStyle.Fill = new Mapsui.Styles.Brush { FillStyle = FillStyle.Solid, Color = Mapsui.Styles.Color.Orange };
            wLayerStyle.Line = new Mapsui.Styles.Pen { Color = Mapsui.Styles.Color.Black, PenStyle = PenStyle.Solid };
            wLayerStyle.Outline = new Mapsui.Styles.Pen { Color = Mapsui.Styles.Color.Red, PenStyle = PenStyle.Solid };

            var wLayer = new Layer { Name = "Lower 48 States", DataSource = wShapeFile, Style = wLayerStyle, Enabled = true };
            this.xMapControl.Map.Layers.Add(wLayer);
            this.xMapControl.Refresh();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            xMapControl.Map.Layers.Clear();
            xMapControl.Clear();
            xMapControl.Refresh();

        }

        private void BtnOSMWithNY_Click(object sender, RoutedEventArgs e)
        {
            var customTrans = new CustomMinimalTransformation();
            customTrans.LoadSourceWKT("NYTOWNS_POLY.prj");

            //this.xMapControl.RenderMode = Mapsui.UI.Wpf.RenderMode.Wpf;
            this.xMapControl.Map = new Mapsui.Map() { CRS = "EPSG:3857", Transformation = customTrans };
            this.xMapControl.Map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.BingHybrid)));

            IProvider wShapeFile = new Mapsui.Desktop.Shapefile.ShapeFile("NYTOWNS_POLY.shp", true) { CRS = "EPSG:CUSTOM" };

            var wLayerStyle = new Mapsui.Styles.VectorStyle();
            wLayerStyle.Fill = new Mapsui.Styles.Brush { FillStyle = FillStyle.Solid, Color = Mapsui.Styles.Color.Gray };
            wLayerStyle.Line = new Mapsui.Styles.Pen { Color = Mapsui.Styles.Color.Black, PenStyle = PenStyle.Solid };
            wLayerStyle.Outline = new Mapsui.Styles.Pen { Color = Mapsui.Styles.Color.Black, PenStyle = PenStyle.Solid };

            var wLayer = new Layer { Name = "NY Townships", DataSource = wShapeFile, Style = wLayerStyle, Enabled = true };
            this.xMapControl.Map.Layers.Add(wLayer);
            this.xMapControl.Refresh();

        }

        private void BtnNY_Click(object sender, RoutedEventArgs e)
        {
                       
            this.xMapControl.Map = new Mapsui.Map();

            IProvider wShapeFile = new Mapsui.Desktop.Shapefile.ShapeFile("NYTOWNS_POLY.shp", true);

            var wLayerStyle = new Mapsui.Styles.VectorStyle();
            wLayerStyle.Fill = new Mapsui.Styles.Brush { FillStyle = FillStyle.Solid, Color = Mapsui.Styles.Color.Gray };
            wLayerStyle.Line = new Mapsui.Styles.Pen { Color = Mapsui.Styles.Color.Black, PenStyle = PenStyle.Solid };
            wLayerStyle.Outline = new Mapsui.Styles.Pen { Color = Mapsui.Styles.Color.Black, PenStyle = PenStyle.Solid };

            var wLayer = new Layer { Name = "NY Townships", DataSource = wShapeFile, Style = wLayerStyle, Enabled = true };
            this.xMapControl.Map.Layers.Add(wLayer);
            this.xMapControl.Refresh();


            this.xMapControl.Refresh();
        }

        private void XMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            var screenPosition = e.GetPosition(xMapControl);
            var worldPosition = xMapControl.Viewport.ScreenToWorld(screenPosition.X, screenPosition.Y);
            //var pt = SphericalMercator.ToLonLat(worldPosition.X, worldPosition.Y);
            //MouseCoordinates.Text = $"{pt.X:F2}, {pt.Y:F2}";

            MouseCoordinates.Text = $"{worldPosition.X:F2}, {worldPosition.Y:F2}";
        }
    }
}
