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
            this.xMapControl.RenderMode = Mapsui.UI.Wpf.RenderMode.Wpf;
            this.xMapControl.Map = new Mapsui.Map();                     
            this.xMapControl.Map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.OpenStreetMap)));
            this.xMapControl.Refresh();
        }

        private void BtnOSMWithUS_Click(object sender, RoutedEventArgs e)
        {
            this.xMapControl.RenderMode = Mapsui.UI.Wpf.RenderMode.Wpf;
            this.xMapControl.Map = new Mapsui.Map() { CRS = "EPSG:3857", Transformation = new MinimalTransformation() };
            this.xMapControl.Map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.OpenStreetMap)));
            IProvider wShapeFile = new Mapsui.Desktop.Shapefile.ShapeFile("lower48.shp", true) { CRS = "EPSG:4326" };

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
    }
}
