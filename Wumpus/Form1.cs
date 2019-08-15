using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wumpus.A.I;
using Wumpus.Objects;

namespace Wumpus
{
    public partial class Form1 : Form
    {
        Map map = new Map();
        Hunter player = new Hunter();
        Button[,] state;
        Random rand = new Random();
        Process pr;
        List<string> move = new List<string>();
        int MAX_ROOMS;
        int VISITED_ROOMS;

        public Form1()
        {
            InitializeComponent();

            int x = rand.Next(0, 9); int y = rand.Next(0, 9);
            map.DrawMap(map1);
            state = map.GetMap();
            player.Start(state[x, y], hunter.Images[2]);
            map.Generate_Obstacle(4, 4, 4, x, y);

            foreach (Button e in state)
            {
                e.BackColor = Color.Black;
                // e.KeyDown += Button1_KeyDown;
                e.Enabled = false;
            }

            map.UpdateMap(state[x, y], tbstatus, Keys.Q, score, false, player.Get_Score());
            score.Text = "Score: " + player.Get_Score();
            pr = new Process(player);
        }


        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = rand.Next(0, 9); int y = rand.Next(0, 9);
            map1.Controls.Clear();
            map.DrawMap(map1);
            map.Generate_Obstacle(0, 0, 0, x, y);
            state = map.GetMap();
            foreach (Button f in state)
            {
                f.BackColor = Color.Black;
                //f.KeyDown += Button1_KeyDown;
                f.Enabled = false;
            }
            player.Start(state[x, y], hunter.Images[0]);
            map.UpdateMap(state[x, y], tbstatus, Keys.Q, score, false, player.Get_Score());
            score.Text = "Score: " + player.Get_Score();
            pr = new Process(player);
        }

        private void Auto_Click(object sender, EventArgs e)
        {
            if (VISITED_ROOMS != MAX_ROOMS)
            {
                if (move.Count == 0)
                {
                    move = pr.Calculate_Move();
                }

                if (move.Count == 0 || move[0] is null && move.Count != 0) { MessageBox.Show("No move to go"); return; }
                else
                {
                    string s = move[0];
                    string[] move_next = s.Split(',');
                    int x = int.Parse(move_next[0]);
                    int y = int.Parse(move_next[1]);
                    string[] p_move = player.Get_current().Name.Split(',');
                    int p_x = int.Parse(p_move[0]);
                    int p_y = int.Parse(p_move[1]);



                    if (s.Contains("pick"))
                    {
                        map.UpdateMap(state[x, y], tbstatus, Keys.Q, score, player.isDie(), player.Get_Score());
                    }
                    else
                    {
                        Image cur;
                        if (p_x == x && p_y != y)
                        {
                            if (p_y > y)
                                cur = hunter.Images[3];            //Left
                            else cur = hunter.Images[2];        // Right
                        }
                        else
                        {
                            if (p_x > x && p_y == y)
                                cur = hunter.Images[1];            // Down
                            else cur = hunter.Images[0];            //Up
                        }
                        player.Move(state[x, y], cur);

                        map.UpdateMap(state[x, y], tbstatus, Keys.A, score, player.isDie(), player.Get_Score());

                    }
                    move.Remove(s);
                }
            }
        }

        
    }

}



