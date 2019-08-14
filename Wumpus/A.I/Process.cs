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

        public List<string> Calculate_Move()
        {
            Random rand = new Random();
            string[] temp = player.Get_current().Name.Split(',');
            int x = int.Parse(temp[0]);
            int y = int.Parse(temp[1]);
            StringBuilder move = new StringBuilder(3);
            List<string> result = new List<string>();

            if (player.Get_current().Tag is null || player.Get_current().Tag.ToString() == "")  //Nếu là ô trống
            {
                List<string> Next_Move = this.kb.Possible_Move(player.Get_current().Name);      //Tạo ra các nước có thể đi

                foreach (string s in Next_Move.ToList())
                {
                    if (kb.Visited.Contains(s))                     //Nếu nước đó đã đi, loại bỏ
                    {
                        Next_Move.Remove(s);
                    }
                    else
                    {
                        if (!kb.Safe.Contains(s))                   //Nếu nước đó chưa đi, mà trong KB an toàn chưa có thì add vào và xóa nó ra khỏi KB Unknown.
                        {
                            kb.Safe.Add(s);
                            if (kb.Unknown.Contains(s))
                            {
                                kb.Unknown.Remove(s);
                            }
                            if (kb.Pit.Contains(s)) kb.Pit.Remove(s);
                        }
                    }
                }
                if (Next_Move.Count != 0)                           //Nếu còn nước mới
                {
                    string MOVE = Next_Move[rand.Next(0, Next_Move.Count)];            //Chọn random trong các nước mới
                    result.Add(MOVE);                                                   //trả về KQ
                    kb.Visited.Add(MOVE);                                               //Thêm vào các nước đã thăm
                    while (kb.Safe.Contains(MOVE))                                  // Xóa nước đó khỏi KB an toàn
                        kb.Safe.Remove(MOVE);
                    history_move = player.Get_current().Name;               //Cập nhật nước đi cũ
                }
                else
                {
                    if (kb.Safe.Count != 0)                     //Nếu không còn nước đi mới, đi lại nước cũ có khả năng ra nước mới cao nhất.
                    {
                        result = Move_Loop(player.Get_current().Name, kb.Safe[kb.Safe.Count - 1]);  //Trả về danh sách các nước đi từ current -> nước mới.
                        history_move = result[result.Count - 2];        //Cập nhật nước cũ
                    }
                    else
                    {

                    }
                }

                return result;
            }

            if (player.Get_current().Tag.ToString().Contains("glitter"))        //Nếu ô đó có vàng, nhặt rồi tính tiếp.
            {
                player.Pick_gold(player.Get_current());

                result.Add(player.Get_current().Name + ",pick");
                return result;
            }

            else
            {
                string percept = player.Get_current().Tag.ToString();               //Kiểm tra tình trạng nút hiện tại
                kb.Visited.Add(player.Get_current().Name);
                List<string> Next_Move = this.kb.Possible_Move(player.Get_current().Name);      //Tạo ra các nút có thể đi
                if (percept.Contains("stench"))                                     //Nếu tình trạng có chứa stench
                    kb.Add_stench(player.Get_current().Name);               //Add stench vào KB

                if (percept.Contains("breeze"))                                     //Nếu có breeze add vào KB.
                    kb.Add_breeze(player.Get_current().Name);

                foreach (string s in Next_Move)                 //Chọn nước đi an toàn tiếp theo có trong KB Safe
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
                foreach (string s in Next_Move)                 //Nếu không có nước đi an toàn, chọn nước đã thăm rồi mà không có breeze và stench
                {
                    if (kb.Visited.Contains(s) && !this.kb.stench.Contains(s) && !this.kb.breeze.Contains(s))
                    {
                        result.Add(s);
                        history_move = player.Get_current().Name;
                        return result;
                    }
                   
                }

                result.Add(history_move);                               //Trở về nước đi cũ trước đó
                history_move = player.Get_current().Name;
                return result;
            }

        }

        public List<string> Move_Loop(string cur, string dst)       // Tìm đường đi từ nút cur đến nút dst qua các nút đã thăm rồi
        {
            string[] temp = cur.Split(',');
            int x = int.Parse(temp[0]);                             //Lấy tọa độ nút cur
            int y = int.Parse(temp[1]);
            List<string> Move;
            List<string> Result = new List<string>();

            Random rand = new Random();
            temp = dst.Split(',');
            int dst_x = int.Parse(temp[0]);                     //Lấy tọa độ nút dst
            int dst_y = int.Parse(temp[1]);

            while (dst != cur)
            {
                Move = this.kb.Possible_Move(dst);                      //Generate ra các nút có thể đi từ dst
                
                int count = Result.Count();
                foreach (string s in Move.ToList())
                {
                    if (!kb.Visited.Contains(s) || Result.Contains(s))      //Nếu nó đó chưa thăm hoặc đã có trong Result bỏ nút đó đi
                        Move.Remove(s);
                }

                if (Move.Count > 0)                             //Nếu có nước đi
                {
                    foreach (string s in Move.ToList())         //Lọc nước đi
                    {
                        temp = s.Split(',');
                        int temp_x = int.Parse(temp[0]);        //Lấy tọa độ nút mới
                        int temp_y = int.Parse(temp[1]);

                        if (Math.Abs(temp_x - x) + Math.Abs(temp_y - y) <= Math.Abs(dst_x - x) + Math.Abs(dst_y - y))        //Nếu tồn tại nước đi ngắn hơn, chọn nước đó
                        {
                            dst_x = temp_x;
                            dst_y = temp_y;                  //Cập nhật lại nút dst
                            Result.Add(s);
                            dst = s;
                            break;
                        }
                    }
                    if (count == Result.Count())
                    {
                        Result.Add(Move[0]);                    //Nếu không có nước đi tối ưu, chọn nút đầu tiên.
                        temp = Move[0].Split(',');
                        dst = Move[0];
                        dst_x = int.Parse(temp[0]);            //Cập nhật lại nút dst
                        dst_y = int.Parse(temp[1]);
                    }
                }

                else                                        
                {
                    Move = this.kb.Possible_Move(dst);
                    string s = Move[rand.Next(0, Move.Count - 1)];
                    Result.Add(s);
                    dst = s;
                }
            }
            Result.Reverse();
            return Result;                      //Trả kết quả.

        }

        private string Find_Nearest_Button(string cur)  //Tìm ra room gần nhất mà liền kề với room chưa thăm
        {
            string[] temp = cur.Split(',');
            int x = int.Parse(temp[0]);
            int y = int.Parse(temp[1]);



            temp = kb.Safe[0].Split(',');
            int dst_x = int.Parse(temp[0]);
            int dst_y = int.Parse(temp[1]);
            int MAX = Math.Abs((dst_x - x) + Math.Abs(dst_y - y));
            string result = kb.Safe[0];

            for (int i = 1; i < kb.Safe.Count; i++)
            {
                temp = kb.Safe[i].Split(',');
                int temp_x = int.Parse(temp[0]);
                int temp_y = int.Parse(temp[1]);

                int value = Math.Abs(temp_x - x) + Math.Abs(temp_y - y);
                if (MAX >= value)
                {
                    result = kb.Safe[i];
                    MAX = value;
                    dst_x = temp_x;
                    dst_y = temp_y - 1;
                }
            }
            return result;
        }

    }
}
