using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wumpus.Objects;

namespace Wumpus.A.I
{
    class Process
    {
        private string history_move;
        private Hunter player;
        private KB kb = new KB();

        public Process(Hunter p)
        {
            player = p;
            kb.Visited.Add(player.Get_current().Name);
        }

        private List<string> Possible_Move(string e)
        {
            string[] temp = e.Split(',');
            int x = int.Parse(temp[0]);
            int y = int.Parse(temp[1]);
            List<string> Move = new List<string>();
            StringBuilder move = new StringBuilder();
            string s;
            if (x - 1 >= 0)
            {
                move.Clear();
                s = move.Insert(0, (char)(x - 1 + 48) + "," + (char)(y + 48)).ToString();
                if (kb.Pit.IndexOf(s) < 0 || kb.Wumpus.IndexOf(s) < 0)
                    Move.Add(s);
            }

            if (x + 1 <= 9)
            {
                move.Clear();
                s = move.Insert(0, (char)(x + 1 + 48) + "," + (char)(y + 48)).ToString();
                if (kb.Pit.IndexOf(s) < 0 || kb.Wumpus.IndexOf(s) < 0)
                    Move.Add(move.ToString());
            }

            if (y + 1 <= 9)
            {
                move.Clear();
                s = move.Insert(0, (char)(x + 48) + "," + (char)(y + 1 + 48)).ToString();
                if (kb.Pit.IndexOf(s) < 0 || kb.Wumpus.IndexOf(s) < 0)
                    Move.Add(move.ToString());
            }

            if (y - 1 >= 0)
            {
                move.Clear();
                s = move.Insert(0, (char)(x + 48) + "," + (char)(y - 1 + 48)).ToString();
                if (kb.Pit.IndexOf(s) < 0 || kb.Wumpus.IndexOf(s) < 0)
                    Move.Add(move.ToString());
            }
            return Move;
        }

        public List<string> Calculate_Move()
        {
            Random rand = new Random();
            string[] temp = player.Get_current().Name.Split(',');
            int x = int.Parse(temp[0]);
            int y = int.Parse(temp[1]);
            StringBuilder move = new StringBuilder(3);
            List<string> result = new List<string>();

            if (player.Get_current().Tag is null || player.Get_current().Tag.ToString() == "")
            {
                List<string> Next_Move = Possible_Move(player.Get_current().Name);

                foreach (string s in Next_Move.ToList())
                {
                    if (kb.Visited.Contains(s))
                    {
                        Next_Move.Remove(s);
                    }
                    else
                    {
                        if (!kb.Safe.Contains(s))
                            kb.Safe.Add(s);
                    }
                }
                if (Next_Move.Count != 0)
                {
                    string MOVE = Next_Move[rand.Next(0, Next_Move.Count)];
                    result.Add(MOVE);
                    kb.Visited.Add(MOVE);
                    while (kb.Safe.Contains(MOVE))
                        kb.Safe.Remove(MOVE);
                    history_move = player.Get_current().Name;
                }
                else
                {
                    if (kb.Safe.Count != 0)
                    {
                        result = Move_Loop(player.Get_current().Name, Find_Nearest_Button(player.Get_current().Name));
                        history_move = result[result.Count - 2];
                    }
                }
                return result;
            }

            if (player.Get_current().Tag.ToString().Contains("glitter"))
            {
                player.Pick_gold(player.Get_current());

                result.Add(player.Get_current().Name + ",pick");
                return result;
            }

            else
            {
                string percept = player.Get_current().Tag.ToString();
                List<string> Next_Move = Possible_Move(player.Get_current().Name);
                if (percept.Contains("stench"))
                {
                    kb.Add_stench(player.Get_current().Name);
                    kb.Visited.Add(player.Get_current().Name);
                    foreach (string s in Next_Move.ToList())
                    {
                        if (kb.Visited.IndexOf(s) >= 0) Next_Move.Remove(s);
                    }
                    foreach (string s in Next_Move)
                    {
                        if (kb.Safe.Contains(s))
                        {
                            kb.Visited.Add(s);
                            while (kb.Safe.Contains(s))
                                kb.Safe.Remove(s);
                            history_move = player.Get_current().Name;
                            result.Add(s);
                            return result;
                        }
                    }

                   
                    result.Add(history_move);
                    history_move = player.Get_current().Name;
                    return result;

                }
                else
                {
                    kb.Add_breeze(player.Get_current().Name);
                    kb.Visited.Add(player.Get_current().Name);
                    foreach (string s in Next_Move.ToList())
                    {
                        if (kb.Visited.IndexOf(s) >= 0) Next_Move.Remove(s);

                    }
                    foreach (string s in Next_Move)
                    {
                        if (kb.Safe.Contains(s))
                        {
                            kb.Visited.Add(s);
                            while (kb.Safe.Contains(s))
                                kb.Safe.Remove(s);
                            history_move = player.Get_current().Name;
                            result.Add(s);
                            return result;
                        }
                    }


                    
                    result.Add(history_move);
                    history_move = player.Get_current().Name;
                    return result;
                }
            }

        }

        public List<string> Move_Loop(string cur, string dst)
        {
            string[] temp = cur.Split(',');
            int x = int.Parse(temp[0]);
            int y = int.Parse(temp[1]);
            List<string> Move;
            List<string> Result = new List<string>();


            temp = dst.Split(',');
            int dst_x = int.Parse(temp[0]);
            int dst_y = int.Parse(temp[1]);

            while (dst != cur)
            {
                Move = Possible_Move(dst);
                Move.Sort();

                foreach (string s in Move.ToList())
                {
                    if (!kb.Visited.Contains(s) || Result.Contains(s))
                        Move.Remove(s);
                }

                if (Move.Count > 1)
                {
                    foreach (string s in Move.ToList())
                    {
                        temp = s.Split(',');
                        int temp_x = int.Parse(temp[0]);
                        int temp_y = int.Parse(temp[1]);

                        if (Math.Abs(temp_x - x) + Math.Abs(temp_y - y) < Math.Abs(dst_x - x) + Math.Abs(dst_y - y))
                        {
                            dst_x = temp_x;
                            dst_y = temp_y;
                            Result.Add(s);
                            dst = s;
                            break;
                        }

                    }
                }

                else
                {
                    temp = Move[0].Split(',');
                    int temp_x = int.Parse(temp[0]);
                    int temp_y = int.Parse(temp[1]);
                    dst = Move[0];
                    dst_x = temp_x;
                    dst_y = temp_y;
                    Result.Add(Move[0]);
                }
            }
            Result.Reverse();
            return Result;

        }

        private string Find_Nearest_Button(string cur)
        {
            string[] temp = cur.Split(',');
            int x = int.Parse(temp[0]);
            int y = int.Parse(temp[1]);


            temp = kb.Safe[0].Split(',');
            int dst_x = int.Parse(temp[0]);
            int dst_y = int.Parse(temp[1]);
            int MAX = Math.Abs(dst_x - x) + Math.Abs(dst_y - y);
            string result = kb.Safe[0];

            for (int i = 0; i < kb.Safe.Count; i++)
            {
                temp = kb.Safe[i].Split(',');
                int temp_x = int.Parse(temp[0]);
                int temp_y = int.Parse(temp[1]);

                int value = Math.Abs(temp_x - x) + Math.Abs(temp_y - y);
                if (MAX > value)
                {
                    result = kb.Safe[i];
                    MAX = value;
                    dst_x = temp_x;
                    dst_y = temp_y;
                }
            }
            return result;
        }

    }
}
