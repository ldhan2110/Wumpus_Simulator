using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Wumpus.Objects
{
    class Map
    {
        private Hunter player;
        public static int MAXIMUM_ROOM = 10;
        private Button[,] room = new Button[MAXIMUM_ROOM, MAXIMUM_ROOM + 1];

        private int num_wumpus, num_gold, num_pit;
        private static int ROOM_WIDTH = 35;
        private static int ROOM_HEIGHT = 35;

        private void Generate_Pit(int num_pits,Random rand,int pos_x,int pos_y)
        {
            
            for (int i = 0; i < num_pits; i++)
            {
                Loop:
                int x = rand.Next(0, 9);
                int y = rand.Next(0, 9);
                if (x == 0 && y == 0 || x == 9 && y == 9 || x == pos_x && y == pos_y)
                    goto Loop;
                room[x, y].Tag += "pit";
                if (x - 1 >= 0)
                    room[x - 1, y].Tag += "breeze";
                if (x + 1 < MAXIMUM_ROOM)
                    room[x + 1, y].Tag += "breeze";
                if (y - 1 >= 0)
                    room[x, y - 1].Tag += "breeze";
                if (y + 1 < MAXIMUM_ROOM)
                    room[x, y + 1].Tag += "breeze";
            }
        }

        private void Generate_Wumpus(int num_wumpus,Random rand, int pos_x, int pos_y)
        {
            for (int i = 0; i < num_wumpus; i++)
            {
                Loop:
                int x = rand.Next(0, 9);
                int y = rand.Next(0, 9);
                if (x == 0 && y == 0 || x == 9 && y == 9 || room[x, y].Tag != null && room[x, y].Tag.ToString().Contains("pit") == true || x==pos_x && y == pos_y)
                {
                    goto Loop;
                }
                room[x, y].Tag += "wumpus";
                if (x - 1 >= 0)
                    room[x - 1, y].Tag += "stench";
                if (x + 1 < MAXIMUM_ROOM)
                    room[x + 1, y].Tag += "stench";
                if (y - 1 >= 0)
                    room[x, y - 1].Tag += "stench";
                if (y + 1 < MAXIMUM_ROOM)
                    room[x, y + 1].Tag += "stench";
            }
        }

        private void Generate_Gold(int num_gold, Random rand)
        {
            for (int i = 0; i < num_gold; i++)
            {
                Loop:
                int x = rand.Next(0, 9);
                int y = rand.Next(0, 9);
                if (room[x, y].Tag != null && room[x, y].Tag.ToString().Contains("pit") == true)
                {
                    goto Loop;
                }
                room[x, y].Tag += "glitter";
            }
        }

        public void DrawMap(Panel map)
        {
           
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0), Tag = "0,0" };

            for (int i = 0; i < MAXIMUM_ROOM; i++)
            {
                for (int j = 0; j < MAXIMUM_ROOM + 1; j++)
                {
                    Button btn = new Button()
                    {
                        Name = i.ToString() + ',' + j.ToString(),
                        Width = ROOM_WIDTH,
                        Height = ROOM_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch

                    };
                    room[i, j] = btn;
                    map.Controls.Add(btn);
                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + ROOM_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }

        public void UpdateMap(Button dst, TextBox status, Keys action, Label score,bool isDie,int Score)
        {
            if (isDie == false)
            {
                switch (action)
                {
                    case Keys.Q:

                        if (dst.Tag != null)
                        {
                            if (dst.Tag.ToString().Contains("glitter") == true)
                            {
                                status.Text = "";
                                num_gold--;
                                dst.Tag = dst.Tag.ToString().Replace("glitter", "");
                                status.Enabled = true;
                                status.Text = dst.Tag.ToString();
                                status.Enabled = false;
                                score.Text = "Score: " + Score;
                                if (dst.BackColor != Color.White || dst.Tag is null)
                                    dst.BackColor = Color.White;
                            }
                        }
                        break;
                    case Keys.A:
                    case Keys.S:
                    case Keys.W:
                    case Keys.D:
                        {
                            status.Text = "";
                           
                            status.Enabled = true;
                            if (dst.Tag != null)
                            {
                                string temp = dst.Tag.ToString();
                                if (temp.Contains("pit")) status.Text += "Pit ";
                                if (temp.Contains("wumpus")) status.Text += "Wumpus ";
                                if (temp.Contains("breeze")) status.Text += "Breeze ";
                                if (temp.Contains("stench")) status.Text += "Stench ";
                                if (temp.Contains("glitter")) status.Text += "Glitter ";
                            }
                            status.Enabled = false;
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("You Die");
             
               
            }

        }

        public void Generate_Obstacle(int pits,int gold,int wumpus,int x, int y)
        {
            Random rand = new Random();
            num_gold = gold;
            num_wumpus = wumpus;
            num_pit = pits;
            Generate_Pit(pits,rand,x,y);
            Generate_Wumpus(wumpus,rand,x,y);
            Generate_Gold(gold,rand);
        }

        public int GetLength()
        {
            return MAXIMUM_ROOM;
        }

        public Button[,] GetMap()
        {
            return room;
        }

   
    }
}
