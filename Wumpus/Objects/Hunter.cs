using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wumpus.Objects
{
    class Hunter
    { 
        private Button current;
        private int score = 0;

        public void Start(Button e,Image a)
        {

            current = e;
            e.Enabled = true;
            e.BackgroundImage = a;
            score = 0;
        }

        public void Move(Button dst,Image var)
        {           
            dst.Enabled = true;
            current.BackgroundImage = null;
            dst.BackgroundImageLayout = ImageLayout.Stretch;
            dst.BackgroundImage = var;

            if (current.BackColor != Color.White && current.Tag is null)
                current.BackColor = Color.White;
            if (current.Tag != null && current.Tag.ToString().Contains("stench"))
            {
                current.BackColor = Color.Red;
            }

            if (current.Tag != null && current.Tag.ToString().Contains("breeze"))
            {
                current.BackColor = Color.Blue;
            }
            current.Enabled = false;
            current = dst;
        }

        public void Pick_gold(Button pos)
        {
            if (pos.Tag != null)
            {
                if (pos.Tag.ToString().Contains("glitter"))
                    score += 100;
            }
        }

        public bool isDie()
        {
            if (current.Tag != null)
            {       if (current.Tag.ToString().Contains("pit") || current.Tag.ToString().Contains("wumpus"))
                    return true;
            }
            return false;
        }
        
        public int Get_Score()
        {
            return this.score;
        }

        public Button Get_current() { return current; }
    }
}