using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wumpus.Objects;

namespace Wumpus.A.I
{
    class KB
    {
        public List<string> Unknown = new List<string>();
        public List<string> Pit = new List<string>();
        public List<string> Wumpus = new List<string>();
        public List<string> stench = new List<string>();
        public List<string> breeze = new List<string>();
        public List<string> Safe = new List<string>();
        public List<string> Visited = new List<string>();

        public void Add_stench(string percept)
        {
            stench.Add(percept);
            this.Infere_wumpus();
        }

        public void Add_breeze(string percept)
        {
            breeze.Add(percept);
            this.Infere_pit();
        }

        public void Add_Safe(string percept)
        {
            Safe.Add(percept);
        }

        private void Infere_pit()
        {
            for (int i = 0; i < breeze.Count; i++)
            {
                string[] temp = breeze[i].Split(',');
                int x = int.Parse(temp[0]);
                int y = int.Parse(temp[1]);

                StringBuilder diagnose = new StringBuilder(3);
                string temp1;

                if (x + 1 <= 9 && y + 1 <= 9)
                {
                    temp1 = diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y + 1)).ToString();
                    if (!breeze.Contains(temp1))
                    {
                        if (Visited.Contains(temp1) || Safe.Contains(temp1))
                        {
                            if (Safe.Contains(diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y - 1)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y - 1)).ToString());
                            if (Safe.Contains(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y + 1)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y + 1)).ToString());
                        }
                    }
                }  //DOWN RIGHT DIAG

                if (x - 1 >= 9 && y + 1 <= 9)
                {
                    temp1 = diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y + 1)).ToString();
                    if (!breeze.Contains(temp1))
                    {
                        if (Visited.Contains(temp1) || Safe.Contains(temp1))
                        {
                            if (!Safe.Contains(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString());
                            if (!Safe.Contains(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString());
                        }
                    }
                }    //UP RIGHT DIAG

                if (x - 1 >= 0 && y - 1 >= 0)
                {
                    temp1 = diagnose.Insert(0, (char)x - 1 + ',' + (char)(y - 1)).ToString();
                    if (!breeze.Contains(temp1))
                    {
                        if (Visited.Contains(temp1) || Safe.Contains(temp1))
                        {
                            if (!Safe.Contains(diagnose.Insert(0, (char)(x) + ',' + (char)(y - 1)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x) + ',' + (char)(y - 1)).ToString());
                            if (Safe.Contains(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString());
                        }
                    }
                }  //up left diag

                if (x + 1 <= 9 && y - 1 >= 0)
                {
                    temp1 = diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y - 1)).ToString();
                    if (!breeze.Contains(temp1))
                    {
                        if (Visited.Contains(temp1) || Safe.Contains(temp1))
                        {
                            if (!Safe.Contains(diagnose.Insert(0, (char)(x) + ',' + (char)(y - 1)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x) + ',' + (char)(y - 1)).ToString());
                            if (Safe.Contains(diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y)).ToString());
                        }
                    }

                }//DOWN LEFT DIAG
            }
        }

        private void Infere_wumpus()
        {
            for (int i = 0; i < stench.Count; i++)
            {
                string[] temp = stench[i].Split(',');
                int x = int.Parse(temp[0]);
                int y = int.Parse(temp[1]);

                StringBuilder diagnose = new StringBuilder(3);
                string temp1;

                if (x + 1 <= 9 && y + 1 <= 9)
                {
                    temp1 = diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y + 1)).ToString();
                    if (!stench.Contains(temp1))
                    {
                        if (Visited.Contains(temp1) || Safe.Contains(temp1))
                        {
                            if (Safe.Contains(diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y - 1)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y - 1)).ToString());
                            if (Safe.Contains(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y + 1)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y + 1)).ToString());
                        }
                    }
                }  //DOWN RIGHT DIAG

                if (x - 1 >= 9 && y + 1 <= 9)
                {
                    temp1 = diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y + 1)).ToString();
                    if (!stench.Contains(temp1))
                    {
                        if (Visited.Contains(temp1) || Safe.Contains(temp1))
                        {
                            if (!Safe.Contains(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString());
                            if (!Safe.Contains(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString());
                        }
                    }
                }    //UP RIGHT DIAG

                if (x - 1 >= 0 && y - 1 >= 0)
                {
                    temp1 = diagnose.Insert(0, (char)x - 1 + ',' + (char)(y - 1)).ToString();
                    if (!stench.Contains(temp1))
                    {
                        if (Visited.Contains(temp1) || Safe.Contains(temp1))
                        {
                            if (!Safe.Contains(diagnose.Insert(0, (char)(x) + ',' + (char)(y - 1)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x) + ',' + (char)(y - 1)).ToString());
                            if (Safe.Contains(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x - 1) + ',' + (char)(y)).ToString());
                        }
                    }
                }  //up left diag

                if (x + 1 <= 9 && y - 1 >= 0)
                {
                    temp1 = diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y - 1)).ToString();
                    if (!stench.Contains(temp1))
                    {
                        if (Visited.Contains(temp1) || Safe.Contains(temp1))
                        {
                            if (!Safe.Contains(diagnose.Insert(0, (char)(x) + ',' + (char)(y - 1)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x) + ',' + (char)(y - 1)).ToString());
                            if (Safe.Contains(diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y)).ToString()))
                                Safe.Add(diagnose.Insert(0, (char)(x + 1) + ',' + (char)(y)).ToString());
                        }
                    }

                }//DOWN LEFT DIAG


            }
        }

      
    }
}
