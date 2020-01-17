using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SystAnalys_lr1
{
    public partial class Form1 : Form
    {
        List<Epicenter> Epics;
        List<grid_part> TheGrid;
        DrawGraph G;

        static List<BusPark> buses;

        static List<List<BusPark>> busesPark;

        Dictionary<int, List<Vertex>> routes = new Dictionary<int, List<Vertex>>() { { 7, route7}, { 23, route23 }, { 62, route62 }, { 404, route404 },
                                                                                           { 20, route20 }, { 43, route43 }, { 107, route107 }};

        static BusPark Bus7_1, Bus7_2, Bus7_3, Bus7_4, Bus7_5, Bus7_6, Bus7_7, Bus7_8, Bus7_9, Bus7_10, Bus7_11, Bus7_12, Bus7_13, Bus7_14, Bus7_15, Bus7_16, Bus23_1,
        Bus23_2, Bus62_1, Bus404_1, Bus404_2,
        Bus_107, Bus_43, Bus_20;

        static List<BusPark> park7;
        static List<BusPark> park23;
        static List<BusPark> park62;
        static List<BusPark> park404;
        static List<BusPark> park20;


        static List<BusPark> park43;
        static List<BusPark> park107;

        static List<Vertex> route7;
        static List<Vertex> route23;
        static List<Vertex> route62;
        static List<Vertex> route404;

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        static List<Vertex> route20;
        static List<Vertex> route43;
        static List<Vertex> route107;

        List<Vertex> stop;
        List<Edge> E;
        List<Point> AllRotationsPoints;

        List<Dictionary<int, int>> grids = new List<Dictionary<int, int>>();
        Dictionary<int, int> globgrid;

        int max = 0;

        private void timer2_Tick(object sender, EventArgs e)
        {
            globgrid = new Dictionary<int, int>();
            for (int i = 0; i < TheGrid.Count; i++)
            {
                globgrid.Add(i, 0);
            }
            label3.Text = Bus7_1.getGrids()[6].res.ToString();
            foreach (var bus in buses)
            {
                if (bus.grids != null)
                {
                    for (int i = 0; i < TheGrid.Count; i++)
                    {
                        if (bus.getLocate() == i)
                        {
                            globgrid[i] += 1;
                        }
                    }
                }
            }
          grids.Add(globgrid);
          GridMatrix();
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
             // timer1.Start();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Matrix();
        }


        private void pictureBox5_MouseClick(object sender, MouseEventArgs e)
        {
            label2.Text = e.X.ToString() + ";" + e.Y.ToString();
            for (int i = 0; i < TheGrid.Count; i++)
            {
                if (GetDistance((double)e.X, (double)e.Y,(double)TheGrid[i].x + TheGrid[i].width / 2, (double)TheGrid[i].y + TheGrid[i].height / 2) < TheGrid[i].width/2)
                {
                    label4.Text = TheGrid[i].res.ToString();
                }

            }
        }
        public double GetDistance(double x1, double y1, double x2, double y2)
        {
            return (int)Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        private void GridMatrix()
        {
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();
            dataGridView3.Refresh();
            dataGridView3.ColumnCount = grids.Count();
            dataGridView3.RowCount = TheGrid.Count();
            for (int i = 0; i < grids.Count(); i++)
            {
                dataGridView3.Columns[i].HeaderText = "Пак данных - "+i.ToString();
            
            }
            for (int i = 0; i < TheGrid.Count; ++i)
            {
                dataGridView3.Rows[i].HeaderCell.Value = "Квадрат - "+ i.ToString(); ;
                
            }
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.RowHeadersWidth = 100;
            for (int i = 0;i < grids.Count(); i++)
            {
             
                for (int j=0;j<grids[i].Count();j++)
                {
                    
                    dataGridView3.Rows[j].Cells[i].Value = grids[i][j].ToString();

                }
            }
        }

        private void Matrix()
        {
           
            int parkSize = 0;

            foreach (var x in busesPark)
            {
                parkSize = Math.Max(parkSize, x.Count);
            }

            int[,] myArr = new int[routes.Count, parkSize];

            dataGridView1.RowCount = routes.Count;
            dataGridView1.ColumnCount = parkSize + 1;

            for (int i = 1; i < parkSize; i++)
            {
                dataGridView1.Columns[i - 1].HeaderText = i.ToString();
                if (i + 1 == parkSize)
                {
                    dataGridView1.Columns[i].HeaderText = parkSize.ToString();
                }
            }

            dataGridView1.Columns[parkSize].HeaderText = "Total";

            for (int i = 0; i < routes.Count; ++i)
            {
                dataGridView1.Rows[i].HeaderCell.Value = routes.ElementAt(i).Key.ToString();
            }

            int total;
            for (int i = 0; i < busesPark.Count; ++i)
            {
                total = 0;
                for (int j = 0; j < parkSize + 1; ++j)
                {
                    if (j < busesPark[i].Count)
                    {
                        if (busesPark[i][j].getRoute() == routes.ElementAt(i).Key)
                        {
                            myArr[i, j] = 1;
                            total++;
                        }
                        else
                        {
                            myArr[i, j] = 0;
                        }
                        dataGridView1.Rows[i].Cells[j].Value = myArr[i, j];
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[j].Value = 0;
                    }
                    dataGridView1.Rows[i].Cells[parkSize].Value = total;
                }
            }



        }

        public Form1()
        {
            InitializeComponent();

           

            AllRotations rotations = new AllRotations();
            G = new DrawGraph(sheet.Width, sheet.Height);
            E = new List<Edge>();

            label1.Text = (pictureBox5.Size.Height * pictureBox5.Size.Width).ToString();

            AllRotationsPoints = AllRotations.GetAllRotationsPoints();
            route7 = AllRotations.GetRoute7();
            route23 = AllRotations.GetRoute23();
            stop = AllRotations.GetStop();
            route62 = AllRotations.GetRoute62();
            route404 = AllRotations.GetRoute404();
            route107 = AllRotations.GetRoute107();
            route43 = AllRotations.GetRoute43();
            route20 = AllRotations.GetRoute20();

            Routes(route107);
            Routes(route7);
            Routes(route62);
            Routes(route23);
            Routes(route43);


            AddEpicenters();

            CreateGrid();


            BusPark.setEpicenters(Epics);
            BusPark.setGrid(TheGrid);
            BusPark.setMap(pictureBox5);

            AddBuses();

       

            timer2.Start();
            //DisplayEpicenters Ep = new DisplayEpicenters(Epics);

            //Ep.Show();
            
            

        }
        private void CreateGrid()
        {
            TheGrid = new List<grid_part>();
            for (int i = 0; i < pictureBox5.Height; i += pictureBox5.Height / 8)
            {
                for (int j = 0; j < pictureBox5.Width; j += pictureBox5.Width / 16)
                {

                    TheGrid.Add(new grid_part(j, i, pictureBox5.Height / 8, pictureBox5.Width / 16));

                }
            }

            void pictureBox5_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
            {

                Graphics g = e.Graphics;

                for (int i = 0; i < TheGrid.Count; i++)
                {
                    TheGrid[i].DrawPart(g);
                }
            }
            pictureBox5.Paint += new System.Windows.Forms.PaintEventHandler(pictureBox5_Paint);

        }
       
        private void AddBuses()
        {
            Bus7_1 = new BusPark(route7, pictureBus7_1, 0, stop, 7);
            Bus7_1.Start();
            Bus7_2 = new BusPark(route7, pictureBox7_3, 10, stop, 7);
            Bus7_2.Start();
            Bus7_3 = new BusPark(route7, pictureBus7_2, 20, stop, 7);
            Bus7_3.Start();
            Bus7_4 = new BusPark(route7, pictureBox1, 1, stop, 7);
            Bus7_4.Start();
            Bus7_5 = new BusPark(route7, pictureBox2, 3, true, stop, 7);
            Bus7_5.Start();
            Bus7_6 = new BusPark(route7, pictureBox3, 5, stop, 7);
            Bus7_6.Start();
            Bus7_7 = new BusPark(route7, pictureBox4, 7, stop, 7);
            Bus7_7.Start();
            Bus7_8 = new BusPark(route7, pictureBox6, 9, true, stop, 7);
            Bus7_8.Start();
            Bus7_9 = new BusPark(route7, pictureBox8, 16, stop, 7);
            Bus7_9.Start();
            Bus7_10 = new BusPark(route7, pictureBox9, 13, stop, 7);
            Bus7_10.Start();
            Bus7_11 = new BusPark(route7, pictureBox10, 14, stop, 7);
            Bus7_11.Start();
            Bus7_12 = new BusPark(route7, pictureBox11, 16, true, stop, 7);
            Bus7_12.Start();
            Bus7_13 = new BusPark(route7, pictureBox12, 18, stop, 7);
            Bus7_13.Start();
            Bus7_14 = new BusPark(route7, pictureBox13, 20, stop, 7);
            Bus7_14.Start();
            Bus7_15 = new BusPark(route7, pictureBox14, 6, stop, 7);
            Bus7_15.Start();
            Bus7_16 = new BusPark(route7, pictureBox7, 10, true, stop, 7);
            Bus7_16.Start();

            Bus23_1 = new BusPark(route23, pictureBus23_1, 5, stop, 7);
            Bus23_1.Start();
            Bus23_2 = new BusPark(route23, pictureBus23_2, 14, stop, 23);
            Bus23_2.Start();

            Bus62_1 = new BusPark(route62, pictureBus62_1, 1, stop, 62);
            Bus62_1.Start();

            Bus404_1 = new BusPark(route404, pictureBus404_1, 1, stop, 404);
            Bus404_1.Start();
            Bus404_2 = new BusPark(route404, pictureBus404_2, route404.Count - 1, stop, 404);
            Bus404_2.Start();

            Bus_107 = new BusPark(route107, pictureBox15, 0, stop, 107);
            Bus_107.Start();
            Bus_43 = new BusPark(route43, pictureBox16, 0, stop, 43);
            Bus_43.Start();
            Bus_20 = new BusPark(route20, pictureBox17, 0, stop, 20);
            Bus_20.Start();


            park7 = new List<BusPark>() { Bus7_1, Bus7_2, Bus7_3, Bus7_4, Bus7_5, Bus7_6, Bus7_7, Bus7_8, Bus7_9, Bus7_10, Bus7_11, Bus7_12, Bus7_13, Bus7_14, Bus7_15, Bus7_16 };
            park23 = new List<BusPark>() { Bus23_1, Bus23_2 };
            park62 = new List<BusPark>() { Bus62_1 };
            park404 = new List<BusPark>() { Bus404_1, Bus404_2 };
            park20 = new List<BusPark>() { Bus_20 };
            park43 = new List<BusPark>() { Bus_43 };
            park107 = new List<BusPark>() { Bus_107 };

            buses = new List<BusPark>() { Bus7_1, Bus7_2, Bus7_3, Bus62_1, Bus23_2, Bus7_4, Bus7_5, Bus7_6, Bus7_7, Bus7_8, Bus7_9,
                Bus7_10, Bus7_11, Bus7_12, Bus7_13, Bus7_14, Bus7_15, Bus7_16, Bus23_1, Bus404_1, Bus404_2, Bus_20, Bus_43, Bus_107 };
            busesPark = new List<List<BusPark>>() { park7, park23, park62, park404, park20, park43, park107 };

        }

        private void Routes(List<Vertex> route)
        {
            for (int i = 0; i < route.Count; i++)
            {
                G.drawVertex(route[i].x, route[i].y, route.Count.ToString());
                sheet.Image = G.GetBitmap();
                if (i + 1 < route.Count)
                {
                    E.Add(new Edge(i, i + 1));
                    G.drawEdge(route[i], route[i + 1], E[E.Count - 1], E.Count - 1);
                    sheet.Image = G.GetBitmap();
                }
            }
        }


        private void AddEpicenters()
        {
            Epics = new List<Epicenter>();
            Epics.Add(new Epicenter(pictureBox5, 500, 500, 300));
            Epics.Add(new Epicenter(pictureBox5, 930, 400, 150));
            Epics.Add(new Epicenter(pictureBox5, 700, 730, 250));
            Epics.Add(new Epicenter(pictureBox5, 800, 100, 220));
            Epics.Add(new Epicenter(pictureBox5, 400, 370, 120));
            Epics.Add(new Epicenter(pictureBox5, 900, 790, 134));
            Epics.Add(new Epicenter(pictureBox5, 817, 395, 300));

            Epics.Add(new Epicenter(pictureBox5, 571, 214, 300));
            Epics.Add(new Epicenter(pictureBox5, 656, 301, 150));
            Epics.Add(new Epicenter(pictureBox5, 286, 583, 200));
            Epics.Add(new Epicenter(pictureBox5, 182, 468, 150));
            Epics.Add(new Epicenter(pictureBox5, 962, 611, 250));
            Epics.Add(new Epicenter(pictureBox5, 1115, 514, 150));
            Epics.Add(new Epicenter(pictureBox5, 999, 223, 200));
            Epics.Add(new Epicenter(pictureBox5, 1324, 512, 300));
            Epics.Add(new Epicenter(pictureBox5, 1701, 497, 500));
            Epics.Add(new Epicenter(pictureBox5, 1165, 660, 350));
            Epics.Add(new Epicenter(pictureBox5, 309, 216, 350));
            Epics.Add(new Epicenter(pictureBox5, 1022, 47, 250));
            Epics.Add(new Epicenter(pictureBox5, 510, 700, 250));
        }


        private void pictureBox5_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < Epics.Count; i++)
            //{
            //    Epics[i].DrawEpicenter(pictureBox5);
            //}


        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {           

            int[,] myArr = new int[routes.Count, buses.Count];

            dataGridView2.RowCount = routes.Count;
            dataGridView2.ColumnCount = buses.Count;

            for (int i = 1; i < buses.Count; i++)
            {
                dataGridView2.Columns[i - 1].HeaderText = i.ToString();
                if (i + 1 == buses.Count)
                {
                    dataGridView2.Columns[i].HeaderText = buses.Count.ToString();
                }
            }

            dataGridView2.Rows[0].HeaderCell.Value = "Данные";

            for (int j = 0; j < buses.Count; ++j)
            {
                myArr[0, j] = (int)buses[j].Date;
                dataGridView2.Rows[0].Cells[j].Value = myArr[0, j];
            }
            
        }


    }
}
