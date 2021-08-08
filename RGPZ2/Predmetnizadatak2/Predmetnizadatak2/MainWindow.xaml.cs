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
using System.Xml;

namespace Predmetnizadatak2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public double tempX, tempY;

        public List<Substation> subs = new List<Substation>();
        public List<Node> nodes = new List<Node>();
        public List<Switch> switches = new List<Switch>();
        public List<Line> lines = new List<Line>();
        public List<PowerEntity> nodesPointi = new List<PowerEntity>();
        public List<PowerEntity> nodeConv = new List<PowerEntity>();
        public List<PowerEntity> swiPointi = new List<PowerEntity>();
        public List<PowerEntity> swiConv = new List<PowerEntity>();
        public List<PowerEntity> subPointi = new List<PowerEntity>();
        public List<PowerEntity> subConv = new List<PowerEntity>();
        public List<Tuple<int, int>> tuplesN = new List<Tuple<int, int>>();
        public List<Tuple<int, int>> tuplesSw = new List<Tuple<int, int>>();
        public List<Tuple<int, int>> tuplesSub = new List<Tuple<int, int>>();
        public Dictionary<long, PowerEntity> sveTacke = new Dictionary<long, PowerEntity>();
        public static Dictionary<string, string> iscrtaneTacke = new Dictionary<string, string>();
        public List<Line> linesProverene = new List<Line>();



        public MainWindow()
        {
            InitializeComponent();
            Ucitaj_Click();
            LoadNodes();
            LoadSwitch();
            LoadSub();
            ProveriLinije(sveTacke);
            LoadLines(linesProverene);
        }

        public void Ucitaj_Click()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml");

            XmlNodeList nodeList;

            

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            foreach (XmlNode node in nodeList)
            {
                Substation sub = new Substation();
                sub.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sub.Name = node.SelectSingleNode("Name").InnerText;
                sub.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sub.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                subs.Add(sub);
            }

           

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            foreach (XmlNode node in nodeList)
            {
                Node nodeobj = new Node();
                nodeobj.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                nodeobj.Name = node.SelectSingleNode("Name").InnerText;
                nodeobj.X = double.Parse(node.SelectSingleNode("X").InnerText);
                nodeobj.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                nodes.Add(nodeobj);
            }
          

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            foreach (XmlNode node in nodeList)
            {
                Switch switchobj = new Switch();
                switchobj.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                switchobj.Name = node.SelectSingleNode("Name").InnerText;
                switchobj.X = double.Parse(node.SelectSingleNode("X").InnerText);
                switchobj.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                switchobj.Status = node.SelectSingleNode("Status").InnerText;
 
                switches.Add(switchobj);
            }

            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            foreach (XmlNode node in nodeList)
            {
                Line l = new Line();
                l.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                l.Name = node.SelectSingleNode("Name").InnerText;
                if (node.SelectSingleNode("IsUnderground").InnerText.Equals("true"))
                {
                    l.IsUnderground = true;
                }
                else
                {
                    l.IsUnderground = false;
                }
                l.R = float.Parse(node.SelectSingleNode("R").InnerText);
                l.ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText;
                l.LineType = node.SelectSingleNode("LineType").InnerText;
                l.ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText);
                l.FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                l.SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);

                foreach (XmlNode pointNode in node.ChildNodes[9].ChildNodes) 
                {
                    Point p = new Point();

                    p.X = double.Parse(pointNode.SelectSingleNode("X").InnerText);
                    p.Y = double.Parse(pointNode.SelectSingleNode("Y").InnerText);

                  
                }

                
                lines.Add(l);
            }

        }

        public void ProveriLinije(Dictionary<long,PowerEntity> sveTacke)
        {
            
            foreach (var item in lines)
            {
                if (sveTacke.ContainsKey(item.FirstEnd) && sveTacke.ContainsKey(item.SecondEnd))
                {
                    linesProverene.Add(item); 
                }

            }
        }

        public void LoadLines(List<Line> linesProverene)
        {
            

            foreach (var ln in linesProverene)
            {
                System.Windows.Shapes.Line newLine = new System.Windows.Shapes.Line();

                newLine.Y1 = sveTacke[ln.FirstEnd].X + 1;
                newLine.X1 = sveTacke[ln.FirstEnd].Y + 1;
                newLine.X2 = sveTacke[ln.FirstEnd].Y + 1;
                newLine.Y2 = sveTacke[ln.SecondEnd].X + 1;

                newLine.Stroke = Brushes.LightGreen;
                newLine.StrokeThickness = 0.5;
                newLine.ToolTip = "Line\nID: " + ln.Id.ToString() + "\nName: " + ln.Name + "\nFirstEnd: " + ln.FirstEnd + "\nSecondEnd: " + ln.SecondEnd;
                newLine.MouseRightButtonDown += Boji;

                canv.Children.Add(newLine);
             

                System.Windows.Shapes.Line newLine2 = new System.Windows.Shapes.Line();
                newLine2.X1 = sveTacke[ln.FirstEnd].Y + 1;
                newLine2.Y1 = sveTacke[ln.SecondEnd].X + 1;
                newLine2.Y2 = sveTacke[ln.SecondEnd].X + 1;
                newLine2.X2 = sveTacke[ln.SecondEnd].Y + 1;

               
                newLine2.Stroke = Brushes.LightGreen;
                newLine2.StrokeThickness = 0.5;
                newLine2.ToolTip = "Line\nID: " + ln.Id.ToString() + "\nName: " + ln.Name + "\nFirstEnd: "+ ln.FirstEnd + "\nSecondEnd: " + ln.SecondEnd;
              

                canv.Children.Add(newLine2);


            }
        }


        public void Boji(object sender, MouseButtonEventArgs e)
        {
            Shape clickedShape = (Shape)e.OriginalSource;

            
            string[] tool = clickedShape.ToolTip.ToString().Split(':');

            double FirstNodeId = double.Parse((tool[3].Split('\n'))[0]);
            double SecoundNodeId = double.Parse(tool[4]);
            
            foreach (var node in canv.Children)
            {
                Shape ellipse = (Shape)node;
                if (ellipse.GetType().Name.ToString() == "Ellipse")
                {
                    double nodeID = double.Parse(ellipse.ToolTip.ToString().Split('\n')[1].Split(':')[1]);

                    if (nodeID == FirstNodeId)
                    {
                        ellipse.Stroke = Brushes.Yellow;
                    }
                    else if (nodeID == SecoundNodeId)
                    {
                        ellipse.Stroke = Brushes.Yellow;
                    }
                }
            }

        }

        public void LoadNodes()
        {
            foreach (Node nd in nodes)
            {
                Node sss = new Node();
                double latitude;
                double longitude;

                ToLatLon(nd.X, nd.Y, 34, out latitude, out longitude);
                sss.Id = nd.Id;
                sss.Name = nd.Name;
                sss.X = latitude;
                sss.Y = longitude;

                nodesPointi.Add(sss);
            }
            string fl = "Node";

            ConvertCoordinates(nodesPointi, out nodeConv,fl);
           


            foreach (var pt in nodeConv)
            {
                Ellipse grid1 = new Ellipse();

                grid1.Stroke = Brushes.Black;
                grid1.Height = 2;
                grid1.Width = 2;
                grid1.ToolTip = "Node\nID: " + pt.Id.ToString() + "\nName: " + pt.Name;
               

                
                iscrtaneTacke[pt.X + "," + pt.Y] = "iscrtan";
                sveTacke.Add(pt.Id, pt);
               
                Canvas.SetTop(grid1, pt.X);
                Canvas.SetLeft(grid1, pt.Y);

                canv.Children.Add(grid1);

            }
        }



        public void LoadSwitch()
        {
            foreach (Switch nd in switches)
            {
                Switch sss = new Switch();
                double latitude;
                double longitude;

                ToLatLon(nd.X, nd.Y, 34, out latitude, out longitude);
                sss.Id = nd.Id;
                sss.Name = nd.Name;
                sss.Status = nd.Status;
                sss.X = latitude;
                sss.Y = longitude;

                swiPointi.Add(sss);
            }

            string fl = "Switch";

            ConvertCoordinates(swiPointi, out swiConv, fl);
            foreach (var item in swiConv)
            {
                DrawSw(item,item.X,item.Y);
            }
            
            
        }


        public void DrawSw(PowerEntity pt, double x, double y)
        {

                tempX = pt.X;
                tempY = pt.Y;

              if (!iscrtaneTacke.ContainsKey(tempX + "," + tempY))
                {

                    Ellipse grid1 = new Ellipse();

                    grid1.Stroke = Brushes.Red;
                    grid1.Height = 2;
                    grid1.Width = 2;
                    grid1.ToolTip = "Switch\nID: " + pt.Id.ToString() + "\nName: " + pt.Name;

                    iscrtaneTacke[tempX + "," + tempY] = "iscrtan";
                    sveTacke.Add(pt.Id, pt);
                    Canvas.SetTop(grid1, tempX);
                    Canvas.SetLeft(grid1, tempY);       
                    canv.Children.Add(grid1);
                }
                else
                {
                    Proveri(pt, out double newX, out double newY);
                    pt.X = newX;
                    pt.Y = newY;
                    DrawSw(pt, newX, newY);
                }

            
        }

        public void LoadSub()
        {
            foreach (Substation nd in subs)
            {
                Substation sss = new Substation();
                double latitude;
                double longitude;

                ToLatLon(nd.X, nd.Y, 34, out latitude, out longitude);
                sss.Id = nd.Id;
                sss.Name = nd.Name;
                sss.X = latitude;
                sss.Y = longitude;

                subPointi.Add(sss);
            }

            string fl = "Sub";
            ConvertCoordinates(subPointi, out subConv, fl);
            foreach (var item in subConv)
            {
               DrawS(item, item.X, item.Y);
            }

        }

        public void DrawS(PowerEntity pt, double x, double y)
        {

            tempX = pt.X;
            tempY = pt.Y;

           if (!iscrtaneTacke.ContainsKey(tempX + "," + tempY))
             {

                Ellipse grid1 = new Ellipse();

                grid1.Stroke = Brushes.Blue;
                grid1.Height = 2;
                grid1.Width = 2;
                grid1.ToolTip = "Substation\nID: " + pt.Id.ToString() + "\nName: " + pt.Name;

                iscrtaneTacke[tempX + "," + tempY] = "iscrtan";
                sveTacke.Add(pt.Id, pt);
                Canvas.SetTop(grid1, tempX);
                Canvas.SetLeft(grid1, tempY);
                canv.Children.Add(grid1);
            }
            else
            {
                Proveri(pt, out double newX, out double newY);
                pt.X = newX;
                pt.Y = newY;
                DrawS(pt, newX, newY);
            }


        }

      



        public void Proveri(PowerEntity proveri, out double x, out double y)
        {

            if (!iscrtaneTacke.ContainsKey(proveri.X + "," + (proveri.Y - 5)))
            {
                x = proveri.X;
                y = proveri.Y - 5;
            }
            else if (!iscrtaneTacke.ContainsKey(proveri.X + "," + (proveri.Y + 5)))
            {
                x = proveri.X;
                y = proveri.Y + 5;
            }
            else if (!iscrtaneTacke.ContainsKey((proveri.X - 5) + "," + proveri.Y))
            {
                x = proveri.X - 5;
                y = proveri.Y;
            }
            else if (!iscrtaneTacke.ContainsKey((proveri.X + 5) + "," + proveri.Y))
            {
                x = proveri.X + 5;
                y = proveri.Y;
            }
            else if (!iscrtaneTacke.ContainsKey((proveri.X + 5) + "," + (proveri.Y + 5)))
            {
                x = proveri.X + 5;
                y = proveri.Y + 5;
            }
            else
            {
                x = proveri.X - 5;
                y = proveri.Y - 5;
            }

        }



        public void ConvertCoordinates(List<PowerEntity> pointi, out List<PowerEntity> pointsList,string flag)
        {
            List<PowerEntity> points = new List<PowerEntity>();
            

            System.Windows.Point minPoint = new System.Windows.Point();
            System.Windows.Point maxPoint = new System.Windows.Point();

            MinMaxCordinates(pointi, out minPoint, out maxPoint);

            int xMax, yMax = 0;
            xMax = (int)Math.Round(maxPoint.X * 100000);
            yMax = (int)Math.Round(maxPoint.Y * 100000);

            int x, y = 0;
           

            foreach (var point in pointi)
            {
                x = (int)Math.Round(point.X * 100000);
                y = (int)Math.Round(point.Y * 100000);

                int novaX, novaY;
                
                novaX = ((xMax - x) / 10) / 2;
                novaY = (2000 - ((yMax - y) / 10)) / 2;
              

                
                int remainder = novaX % 10;
                if (remainder <= 5)
                {
                    
                    novaX -= remainder;
                }
                else
                {
                   
                    int saberi = 10 - remainder;
                    novaX += saberi;
                }

              
                int remainder2 = novaY % 10;
                if (remainder2 <= 5)
                {
                    
                    novaY -= remainder2;
                }
                else
                {
                    
                    int saberi = 10 - remainder2;
                    novaY += saberi;
                }
                

                points.Add(new PowerEntity(point.Id,point.Name, novaX, novaY));
            }
           

            pointsList = points;
        }



        public void MinMaxCordinates(List<PowerEntity> pointi, out System.Windows.Point min, out System.Windows.Point max)
        {
            System.Windows.Point minPoint = new System.Windows.Point();
            System.Windows.Point maxPoint = new System.Windows.Point();

            double xMin = 0;
            double yMin = 0;
            double xMax = 0;
            double yMax = 0;

            bool first = true;
            foreach (var point in pointi)
            {
                if (first)
                {
                    xMin = point.X;
                    yMin = point.Y;
                    xMax = point.X;
                    yMax = point.Y;
                    first = false;
                }
               else
                {
                    if (point.X < xMin)
                    {
                        xMin = point.X;
                        minPoint.X = xMin;
                    }
                    if (point.Y < yMin)
                    {
                        yMin = point.Y;
                        minPoint.Y = yMin;
                    }

                    if (point.X > xMax)
                    {
                        xMax = point.X;
                        maxPoint.X = xMax;
                    }

                    if (point.Y > yMax)
                    {
                        yMax = point.Y;
                        maxPoint.Y = yMax;
                    }
               }
            }
            
            min = minPoint;
            max = maxPoint;
        }

        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
    }
}
