using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rummikubGame
{
    public partial class Form1 : Form
    {
        private Button[,] tile_slot;
        private Button[] cards;

        public Form1()
        {
            InitializeComponent();
        }

        private float getDistance(Button moving_card, Button empty_slot)
        {
            return (float)Math.Sqrt(Math.Pow(moving_card.Location.X - empty_slot.Location.X, 2) + Math.Pow(moving_card.Location.Y - empty_slot.Location.Y, 2));
        }

        private void tile_MouseUp(object sender, MouseEventArgs e)
        {
            Button current_card = (Button)sender;
            float min_distance = getDistance(current_card, tile_slot[0, 0]);
            int min_i = 0;
            int min_j = 0;
            label1.Text = current_card.Location.ToString();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (getDistance(current_card, tile_slot[i, j]) < min_distance)
                    {
                        min_distance = getDistance(current_card, tile_slot[i, j]);
                        min_i = i;
                        min_j = j;
                    }
                }
            }

            current_card.Location = tile_slot[min_i, min_j].Location;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int xx = 85, yy = 350;
            tile_slot = new Button[2,10];

            // Generating the cards
            cards = new Button[14];

            for (int i = 0; i < 14; i++)
            {
                cards[i] = new Button();
                cards[i].Size = new Size(75, 100);
                cards[i].Location = new Point(xx, yy);
                cards[i].BackgroundImage = Image.FromFile("tile.png");
                cards[i].BackgroundImageLayout = ImageLayout.Stretch;
                cards[i].BringToFront();
                cards[i].Draggable(true);
                cards[i].FlatStyle = FlatStyle.Flat;
                cards[i].FlatAppearance.BorderSize = 0;

                cards[i].Text = rnd.Next(1,14).ToString();

                int color_number = rnd.Next(4);
                if(color_number == 0)
                    cards[i].ForeColor = (Color.Blue);
                else if(color_number == 1)
                    cards[i].ForeColor = (Color.Black);
                else if(color_number == 2)
                    cards[i].ForeColor = (Color.Yellow);
                else
                    cards[i].ForeColor = (Color.Red);


                cards[i].Font = new Font("Microsoft Sans Serif", 20);
                cards[i].MouseUp += new MouseEventHandler(this.tile_MouseUp);
                Controls.Add(cards[i]);
                xx += 80;
                if (i == 9) { yy += 105; xx = 85; }
            }

            xx = 85;
            yy = 350;
            // Generating the slots
            for (int i=0; i<2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tile_slot[i,j] = new Button();
                    tile_slot[i, j].BackgroundImage = Image.FromFile("slot.png");
                    tile_slot[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                    tile_slot[i,j].FlatStyle = FlatStyle.Flat;
                    tile_slot[i,j].FlatAppearance.BorderSize = 0;
                    tile_slot[i,j].Size = new Size(75, 100);
                    tile_slot[i,j].Location = new Point(xx, yy);
                    tile_slot[i, j].SendToBack();
                    Controls.Add(tile_slot[i,j]);
                    xx += 80;
                }
                yy += 105;
                xx = 85;
            }
            board.SendToBack();
        }

    }
}
