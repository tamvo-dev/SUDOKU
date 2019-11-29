using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sodoku
{
    public partial class Form1 : Form
    {
        private const int firstX = 50;
        private const int firstY = 50;
        private const int margin = 10;
        private const int buttonSize = 40;
        private Button[,] buttons;
        private Button buttonNhapSo;
        private Button buttonEnd;
        private Button currentButton;
        private TextBox textBox;
        private const String NAME_BUTTON = "button_name";

        private const String easyPuzzle = "534678912672195348198342567859761423426853791713924856961537284287419635345286179";
      

        public Form1()
        {
            InitializeComponent();

            this.buttons = new System.Windows.Forms.Button[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    buttons[i, j] = new System.Windows.Forms.Button();
                    buttons[i, j].Location = new System.Drawing.Point(i * buttonSize + firstX, j * buttonSize + firstY);
                    buttons[i, j].Size = new System.Drawing.Size(buttonSize, buttonSize);
                    buttons[i, j].UseVisualStyleBackColor = true;

                }
            }

            textBox = new TextBox();
            textBox.Size = new Size(100, 40);
            textBox.Location = new Point(10 * buttonSize + firstX, 6 * buttonSize + firstY);

            buttonNhapSo = new Button();
            buttonNhapSo.Size = new Size(100, 40);
            buttonNhapSo.Text = "Nhap so";
            buttonNhapSo.Location = new Point(10 * buttonSize + firstX, 7 * buttonSize + firstY);

            buttonEnd = new Button();
            buttonEnd.Size = new Size(100, 40);
            buttonEnd.Text = "End game";
            buttonEnd.Location = new Point(10 * buttonSize + firstX, 8 * buttonSize + firstY);

            // goi ham initGame
            initGame();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.Controls.Add(buttons[i, j]);
                }
            }
            this.Controls.Add(buttonNhapSo);
            this.Controls.Add(buttonEnd);
            this.Controls.Add(textBox);

            buttonEnd.Click += new EventHandler(this.endGame_Click);
            buttonNhapSo.Click += new EventHandler(this.nhapSo_Click);

        }

        private void button_Click(object sender, EventArgs e)
        {
            currentButton = ((Button)sender);
        }

        private void endGame_Click(object sender, EventArgs e)
        {
            if (isWin())
            {
                winGame();
            }
            else
            {
                lostGame();
            }
        }

        private void nhapSo_Click(object sender, EventArgs e)
        {
            if(currentButton == null)
            {
                Message message = new Message();
                message.sendMessage("Vui lòng chọn ô để nhập!");
                message.Show();
                return;
            }

            int number = 0;
            try
            {
                number = int.Parse(textBox.Text);
            }catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            if(number < 10 && number > 0 && currentButton != null)
            {
                currentButton.Text = number + "";
                currentButton = null;
                textBox.Text = null;
            }
            else
            {
                Message message = new Message();
                message.sendMessage("Phải nhập vào số từ 1 đến 9!");
                message.Show();
            }
        }

        private void initGame()
        {
            // Random cac  so
            Random random = new Random();
            char[] arr = easyPuzzle.ToCharArray();

            for(int i=0; i<10; i++)
            {
                int index = random.Next(0, 81);
                arr[index] = '0'; 
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    char num = arr[i * 9 + j];
                    buttons[i, j].Text = num + "";
                    if (num == '0')
                    {
                        buttons[i, j].BackColor = Color.Blue;
                        buttons[i, j].Name = NAME_BUTTON;
                        buttons[i, j].Click += new System.EventHandler(this.button_Click);
                    }

                }
            }
        }

        private bool isWin()
        {
            // Tao mot ngan xep
            STACK<Po> stack = new STACK<Po>();

            for(int i=0; i<9; i++)
            {
                for(int j=0; j<9; j++)
                {
                    if(buttons[i, j].Name == NAME_BUTTON)
                    {
                        stack.push(new Po(i, j));
                    }
                }
            }

            while(!stack.isEmpty())
            {
                Po p = stack.pop();
                if(checkPoint(p) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private bool checkPoint(Po p)
        {
            char num = buttons[p.x, p.y].Text[0];
            if (num == '0')
                return false;

            // Lay doc
            STACK<String> vetical = new STACK<string>();
            for(int i=0; i<9; i++)
            {
                if(i != p.y)
                {
                    vetical.push(buttons[p.x, i].Text);
                } 
            }
            while ( !vetical.isEmpty())
            {
                String item = vetical.pop();
                if(item == buttons[p.x, p.y].Text)
                {
                    return false;
                }
            }

            // Lay ngang
            STACK<String> horizontal = new STACK<string>();
            for (int i = 0; i < 9; i++)
            {
                if (i != p.x)
                {
                    horizontal.push(buttons[i, p.y].Text);
                }
            }
            while ( !horizontal.isEmpty())
            {
                String item = horizontal.pop();
                if (item == buttons[p.x, p.y].Text)
                {
                    return false;
                }
            }

            // Lay block 3*3
            STACK<String> block = new STACK<string>();
            int startx = (p.x / 3) * 3;
            int starty = (p.y / 3) * 3;
            for (int i = startx; i < startx + 3; i++)
            {
                for (int j = starty; j < starty + 3; j++)
                {
                    if (i == p.x && j == p.y)
                        continue;
                    block.push(buttons[i, j].Text);
                }

            }
            while ( !block.isEmpty())
            {
                String item = block.pop();
                if (item == buttons[p.x, p.y].Text)
                {
                    return false;
                }
            }


            return true;
        }

        private void winGame()
        {
            Message message = new Message();
            message.sendMessage("Bạn đã chiến thắng!");
            message.Show();
        }

        private void lostGame()
        {
            Message message = new Message();
            message.sendMessage("Mời bạn thử lại");
            message.Show();
        }

    }
}
