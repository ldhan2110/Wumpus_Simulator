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
      


        public List<string> Possible_Move(string e)    //Tạo ra các nút có thể đi từ nút hiện tại
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
                if (this.Pit.IndexOf(s) < 0 || this.Wumpus.IndexOf(s) < 0)
                    Move.Add(s);
            }

            if (x + 1 <= 9)
            {
                move.Clear();
                s = move.Insert(0, (char)(x + 1 + 48) + "," + (char)(y + 48)).ToString();
                if (this.Pit.IndexOf(s) < 0 || this.Wumpus.IndexOf(s) < 0)
                    Move.Add(move.ToString());
            }

            if (y + 1 <= 9)
            {
                move.Clear();
                s = move.Insert(0, (char)(x + 48) + "," + (char)(y + 1 + 48)).ToString();
                if (this.Pit.IndexOf(s) < 0 || this.Wumpus.IndexOf(s) < 0)
                    Move.Add(move.ToString());
            }

            if (y - 1 >= 0)
            {
                move.Clear();
                s = move.Insert(0, (char)(x + 48) + "," + (char)(y - 1 + 48)).ToString();
                if (this.Pit.IndexOf(s) < 0 || this.Wumpus.IndexOf(s) < 0)
                    Move.Add(move.ToString());
            }
            return Move;
        }

        public void Add_stench(string percept)
        {
            stench.Add(percept);
            List<string> Move = Possible_Move(percept);         //Tạo ra các nước có thể đi
            foreach (string s in Move.ToList())
            {
                if (this.Safe.Contains(s) || this.Visited.Contains(s))      //Xóa bỏ các nước đã thăm hoặc an toàn
                {
                    Move.Remove(s);
                    continue;
                }
                else                                               //Nếu nước tiếp theo unknown thì kiểm tra
                {
                    this.infere_Wumpus(s);
                }
            }


        }

        public void Add_breeze(string percept)
        {
            breeze.Add(percept);                        //Add ô hiện tại vào KB breeze

            List<string> Move = Possible_Move(percept);         //Tạo ra các nước có thể đi
            foreach (string s in Move.ToList())                  
            {
                if (this.Safe.Contains(s) || this.Visited.Contains(s))      //Xóa bỏ các nước đã thăm hoặc an toàn
                {
                    Move.Remove(s);
                    continue;
                }
                else                                               //Nếu nước tiếp theo unknown thì kiểm tra
                {
                    this.infere_Pit(s);
                }
            }
     
        }

        public void Add_Safe(string percept)
        {
            Safe.Add(percept);
        }

        private void infere_Pit(string e)                    //Kiểm tra có thể infere nút truyền vào có phải pit hay không ?
        {
            if (this.Pit.Contains(e)) return;
            int count = 0;
            List<string> breeze = Possible_Move(e);     //Tạo ra các nút lân cận nút hiện tại
            foreach (string s in breeze.ToList())
            {
                if (this.Visited.Contains(s) && !this.breeze.Contains(s))  //Nếu tồn tại nút lân cận mà đã thăm và không có breeze thì chắc chắn an toàn
                {
                    return;
                }
                else
                {
                    
                    if (this.breeze.Contains(s) && !this.Safe.Contains(s))        // Cỏn nếu các nút lân cận có breeze thì add vào Unknown.
                    {
                        count++;
                    }
                }
            }
            if (count >= 2 && !this.Pit.Contains(e))            //Nếu số breeze lân cận >= 2 => nút đó là pit
            {
                this.Pit.Add(e);                                //bỏ vào KB pit
                if (this.Unknown.Contains(e)) this.Unknown.Remove(e);
                foreach (string s in breeze)                    //Bỏ các nút lân cận chưa khám phá khỏi KB Unknown
                {
                    if (this.Unknown.Contains(s)) this.Unknown.Remove(s);       //Bỏ nút này khỏi KB Unknown
                }
            }
            else this.Unknown.Add(e);           //Còn nếu chưa đủ dữ kiện thì bỏ vào Unknown.
        }

        private void infere_Wumpus(string e)
        {
            if (this.Wumpus.Contains(e)) return;
            int count = 0;
            List<string> stench = Possible_Move(e);     //Tạo ra các nút lân cận nút hiện tại
            foreach (string s in stench.ToList())
            {
                if (this.Visited.Contains(s) && !this.stench.Contains(s))  //Nếu tồn tại nút lân cận mà đã thăm và không có breeze thì chắc chắn an toàn
                {
                    return;
                }
                else
                {

                    if (this.stench.Contains(s) && !this.Safe.Contains(s))        // Cỏn nếu các nút lân cận có breeze thì add vào Unknown.
                    {
                        count++;
                    }
                }
            }
            if (count >= 2 && !this.Wumpus.Contains(e))            //Nếu số Stench lân cận >= 2 => nút đó là Wumpus
            {
                this.Pit.Add(e);                                //bỏ vào KB pit
                if (this.Unknown.Contains(e)) this.Unknown.Remove(e);
                foreach (string s in stench)                    //Bỏ các nút lân cận chưa khám phá khỏi KB Unknown
                {
                    if (this.Unknown.Contains(s)) this.Unknown.Remove(s);       //Bỏ nút này khỏi KB Unknown
                }
            }
            else this.Unknown.Add(e);           //Còn nếu chưa đủ dữ kiện thì bỏ vào Unknown.
        }

      
    }
}
