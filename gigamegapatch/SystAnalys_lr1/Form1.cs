using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Serialization;

namespace SystAnalys_lr1
{
    public partial class Form1 : Form
    {
        List<Epicenter> Epics;
        List<grid_part> TheGrid;
        DrawGraph G;

        static List<BusPark> buses;

        static List<List<BusPark>> busesPark;
        int selected1; //выбранные вершины, для соединения линиями
        int selected2;

        Dictionary<int, List<Vertex>> routes;

        static BusPark Bus7_1, Bus7_2, Bus7_3, Bus7_4, Bus7_5, Bus7_6, Bus7_7, Bus7_8, Bus7_9, Bus7_10, Bus7_11, Bus7_12, Bus7_13, Bus7_14, Bus7_15, Bus7_16, Bus23_1,
        Bus23_2, Bus62_1,
         Bus_43, Bus_20;

        static List<BusPark> park7;
        static List<BusPark> park23;
        static List<BusPark> park62;
        static List<BusPark> park20;

        static List<BusPark> park43;
        //static List<BusPark> park107;

        static List<Vertex> route7;
        static List<Vertex> route23;
        static List<Vertex> route62;
        static List<Vertex> route20;
        static List<Vertex> route43;
        static List<Vertex> route107;


        private void pictureBox7_3_Click(object sender, EventArgs e)
        {

        }
        List<Vertex> stop;
        List<Edge> E;
        List<Point> AllRotationsPoints;

        List<Dictionary<int, int>> grids = new List<Dictionary<int, int>>();
        Dictionary<int, int> globgrid;

        int max = 0;


        private void deleteALLButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            const string message = "Вы действительно хотите полностью удалить граф?";
            const string caption = "Удаление";
            var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (MBSave == DialogResult.Yes)
            {
                routes[int.Parse(comboBox1.Text)].Clear();
                E.Clear();
                G.clearSheet();
                sheet.Image = G.GetBitmap();
            }

        }


        private void deleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            G.clearSheet();
            G.drawALLGraph(routes[int.Parse(comboBox1.Text)], E);
            sheet.Image = G.GetBitmap();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < routes.Count; i++)
            {
                if (routes.ElementAt(i).Key == int.Parse(comboBox1.Text))
                {
                    selectButton.Enabled = false;
                    drawVertexButton.Enabled = true;
                    drawEdgeButton.Enabled = true;
                    deleteButton.Enabled = true;
                    G.clearSheet();
                    G.drawALLGraph(routes.ElementAt(i).Value, E);
                    sheet.Image = G.GetBitmap();
                    selected1 = -1;
                };
            }
        }

        private void drawEdgeButton_Click(object sender, EventArgs e)
        {
            drawEdgeButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            deleteButton.Enabled = true;
            G.clearSheet();
            G.drawALLGraph(routes[int.Parse(comboBox1.Text)], E);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
            selected2 = -1;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            File.Delete("vertex.xml");
            File.Delete("Edge.xml");

            XmlSerializer Ver = new XmlSerializer(typeof(List<Vertex>));
            FileStream file = new FileStream("vertex.xml", FileMode.OpenOrCreate);
            Ver.Serialize(file, route23);
            Console.WriteLine("Объект сериализован");
            file.Close();

            XmlSerializer Edge = new XmlSerializer(typeof(List<Edge>));
            FileStream file_2 = new FileStream("Edge.xml", FileMode.OpenOrCreate);
            Edge.Serialize(file_2, E);
            Console.WriteLine("Объект сериализован");
            file_2.Close();

        }

        private void Load_Click(object sender, EventArgs e)
        {
            string path = "vertex.xml";
            string pathEdge = "Edge.xml";
            StreamReader reader = new StreamReader(path);
            XmlSerializer ver = new XmlSerializer(typeof(List<Vertex>));
            routes[int.Parse(comboBox1.Text)] = (List<Vertex>)ver.Deserialize(reader);
            G.clearSheet();
            G.drawALLGraph(routes[int.Parse(comboBox1.Text)], E);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
            reader = new StreamReader(pathEdge);
            ver = new XmlSerializer(typeof(List<Edge>));
            E = (List<Edge>)ver.Deserialize(reader);
            reader.Close();
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = false;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            G.clearSheet();
            G.drawALLGraph(routes[int.Parse(comboBox1.Text)], E);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
        }


        private void drawVertexButton_Click(object sender, EventArgs e)
        {
            drawVertexButton.Enabled = false;
            selectButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            G.clearSheet();
            G.drawALLGraph(routes[int.Parse(comboBox1.Text)], E);
            sheet.Image = G.GetBitmap();
        }


        private void timer2_Tick(object sender, EventArgs e)
        {


            //label3.Text = Bus7_1.getGrids()[6].res.ToString();
            foreach (var bus in buses)
            {
                if (bus.grids != null)
                {
                    for (int i = 0; i < TheGrid.Count; i++)
                    {
                        if (bus.getLocate() == i)
                        {
                            if (bus.lastLocate != bus.Locate)
                            {
                                globgrid[i] += 1;
                                bus.lastLocate = bus.Locate;
                            }
                        }
                    }
                }
            }


        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            grids.Add(globgrid);
            GridMatrix();
            globgrid = new Dictionary<int, int>();
            for (int i = 0; i < TheGrid.Count; i++)
            {
                globgrid.Add(i, 0);
            }


        }



        public Form1()
        {
            InitializeComponent();

            pictureBox5.Image = new Bitmap("../../Resources/map2.PNG");


            G = new DrawGraph(sheet.Width, sheet.Height);
            E = new List<Edge>();

            label1.Text = (pictureBox5.Size.Height * pictureBox5.Size.Width).ToString();

            AllRotationsPoints = AllRotations.GetAllRotationsPoints();
            route7 = AllRotations.GetRoute7();
            route23 = AllRotations.GetRoute23();
            stop = AllRotations.GetStop();
            route62 = AllRotations.GetRoute62();
            route107 = AllRotations.GetRoute107();
            route43 = AllRotations.GetRoute43();
            route20 = AllRotations.GetRoute20();

            //Routes(route107);
            Routes(route7);
            Routes(route62);
            Routes(route23);
            Routes(route43);

            routes = new Dictionary<int, List<Vertex>>() { { 7, route7}, { 23, route23 }, { 62, route62 },
                                                                                           { 20, route20 }, { 43, route43 }};

            AddEpicenters();

            CreateGrid();


            BusPark.setEpicenters(Epics);
            BusPark.setGrid(TheGrid);
            BusPark.setMap(pictureBox5);

            AddBuses();
            globgrid = new Dictionary<int, int>();
            for (int i = 0; i < TheGrid.Count; i++)
            {
                globgrid.Add(i, 0);
            }
            // sheet.Image = new Bitmap("../../Resources/map2.PNG");
            timer1.Interval = 25000;
            timer1.Start();
            timer2.Interval = 1;
            timer2.Start();
            DisplayEpicenters Ep = new DisplayEpicenters(Epics);

            Ep.Show();

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
                    //  TheGrid[i].DrawNum(g);
                }
            }
            pictureBox5.Paint += new System.Windows.Forms.PaintEventHandler(pictureBox5_Paint);

        }

        private void AddBuses()
        {
            Bus7_1 = new BusPark(route7, pictureBus7_1, 0, stop, 7);
            Bus7_1.Start();
            Bus7_2 = new BusPark(route7, pictureBox7_3, 1, stop, 7);
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

            Bus23_1 = new BusPark(route23, pictureBus23_1, 1, stop, 23);
            Bus23_1.Start();
            Bus23_2 = new BusPark(route23, pictureBus23_2, 14, stop, 23);
            Bus23_2.Start();

            Bus62_1 = new BusPark(route62, pictureBus62_1, 1, stop, 62);
            Bus62_1.Start();




            Bus_43 = new BusPark(route43, pictureBox16, 0, stop, 43);
            Bus_43.Start();
            Bus_20 = new BusPark(route20, pictureBox17, 0, stop, 20);
            Bus_20.Start();


            park7 = new List<BusPark>() { Bus7_1, Bus7_2, Bus7_3, Bus7_4, Bus7_5, Bus7_6, Bus7_7, Bus7_8, Bus7_9, Bus7_10, Bus7_11, Bus7_12, Bus7_13, Bus7_14, Bus7_15, Bus7_16 };
            park23 = new List<BusPark>() { Bus23_1, Bus23_2 };
            park62 = new List<BusPark>() { Bus62_1 };

            park20 = new List<BusPark>() { Bus_20 };
            park43 = new List<BusPark>() { Bus_43 };
            //park107 = new List<BusPark>() { Bus_107 };

            buses = new List<BusPark>() { Bus7_1, Bus7_2, Bus7_3, Bus62_1, Bus23_2, Bus7_4, Bus7_5, Bus7_6, Bus7_7, Bus7_8, Bus7_9,
                Bus7_10, Bus7_11, Bus7_12, Bus7_13, Bus7_14, Bus7_15, Bus7_16, Bus23_1,  Bus_20, Bus_43};
            busesPark = new List<List<BusPark>>() { park7, park23, park62, park20, park43 };

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

        private void sheet_MouseClick(object sender, MouseEventArgs e)
        {
            List<Vertex> V = routes[int.Parse(comboBox1.Text)];
            //нажата кнопка "выбрать вершину", ищем степень вершины
            if (selectButton.Enabled == false)
            {
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        if (selected1 != -1)
                        {
                            selected1 = -1;
                            G.clearSheet();
                            G.drawALLGraph(V, E);
                            sheet.Image = G.GetBitmap();
                        }
                        if (selected1 == -1)
                        {
                            G.drawSelectedVertex(V[i].x, V[i].y);
                            selected1 = i;
                            sheet.Image = G.GetBitmap();
                            //createAdjAndOut();
                            //listBoxMatrix.Items.Clear();
                            //int degree = 0;
                            //for (int j = 0; j < V.Count; j++)
                            //    degree += AMatrix[selected1, j];
                            //listBoxMatrix.Items.Add("Степень вершины №" + (selected1 + 1) + " равна " + degree);
                            break;
                        }
                    }
                }
            }
            //нажата кнопка "рисовать вершину"
            if (drawVertexButton.Enabled == false)
            {
                V.Add(new Vertex(e.X, e.Y));
                G.drawVertex(e.X, e.Y, V.Count.ToString());
                sheet.Image = G.GetBitmap();
            }
            //нажата кнопка "рисовать ребро"
            if (drawEdgeButton.Enabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    for (int i = 0; i < V.Count; i++)
                    {
                        if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                        {
                            if (selected1 == -1)
                            {
                                G.drawSelectedVertex(V[i].x, V[i].y);
                                selected1 = i;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                            if (selected2 == -1)
                            {
                                G.drawSelectedVertex(V[i].x, V[i].y);
                                selected2 = i;
                                E.Add(new Edge(selected1, selected2));
                                G.drawEdge(V[selected1], V[selected2], E[E.Count - 1], E.Count - 1);
                                selected1 = -1;
                                selected2 = -1;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                        }
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if ((selected1 != -1) &&
                        (Math.Pow((V[selected1].x - e.X), 2) + Math.Pow((V[selected1].y - e.Y), 2) <= G.R * G.R))
                    {
                        G.drawVertex(V[selected1].x, V[selected1].y, (selected1 + 1).ToString());
                        selected1 = -1;
                        sheet.Image = G.GetBitmap();
                    }
                }
            }
            //нажата кнопка "удалить элемент"
            if (deleteButton.Enabled == false)
            {
                bool flag = false; //удалили ли что-нибудь по ЭТОМУ клику
                //ищем, возможно была нажата вершина
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        for (int j = 0; j < E.Count; j++)
                        {
                            if ((E[j].v1 == i) || (E[j].v2 == i))
                            {
                                E.RemoveAt(j);
                                j--;
                            }
                            else
                            {
                                if (E[j].v1 > i) E[j].v1--;
                                if (E[j].v2 > i) E[j].v2--;
                            }
                        }
                        V.RemoveAt(i);
                        flag = true;
                        break;
                    }
                }
                //ищем, возможно было нажато ребро
                if (!flag)
                {
                    for (int i = 0; i < E.Count; i++)
                    {
                        if (E[i].v1 == E[i].v2) //если это петля
                        {
                            if ((Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) <= ((G.R + 2) * (G.R + 2))) &&
                                (Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) >= ((G.R - 2) * (G.R - 2))))
                            {
                                E.RemoveAt(i);
                                flag = true;
                                break;
                            }
                        }
                        else //не петля
                        {
                            if (((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) <= (e.Y + 4) &&
                                ((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) >= (e.Y - 4))
                            {
                                if ((V[E[i].v1].x <= V[E[i].v2].x && V[E[i].v1].x <= e.X && e.X <= V[E[i].v2].x) ||
                                    (V[E[i].v1].x >= V[E[i].v2].x && V[E[i].v1].x >= e.X && e.X >= V[E[i].v2].x))
                                {
                                    E.RemoveAt(i);
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                //если что-то было удалено, то обновляем граф на экране
                if (flag)
                {
                    G.clearSheet();
                    G.drawALLGraph(V, E);
                    sheet.Image = G.GetBitmap();
                }
            }
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
                if (GetDistance((double)e.X, (double)e.Y, (double)TheGrid[i].x + TheGrid[i].width / 2, (double)TheGrid[i].y + TheGrid[i].height / 2) < TheGrid[i].width / 2)
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
                dataGridView3.Columns[i].HeaderText = "Пак данных - " + i.ToString();

            }
            for (int i = 0; i < TheGrid.Count; ++i)
            {
                dataGridView3.Rows[i].HeaderCell.Value = "Квадрат - " + i.ToString(); ;

            }
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.RowHeadersWidth = 100;
            for (int i = 0; i < grids.Count(); i++)
            {

                for (int j = 0; j < grids[i].Count(); j++)
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




    }
}
