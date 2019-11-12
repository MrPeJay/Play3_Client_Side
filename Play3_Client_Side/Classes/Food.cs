using System;
using System.Drawing;
using System.Windows.Forms;
using Play3_Client_Side.Interfaces;
using Play3_Client_Side.Properties;

namespace Play3_Client_Side.Classes
{
    class Food : IObject
    {
        public int xCoord { get; set; }
        public int yCoord { get; set; }
        public string Uuid { get; set; }
        public int size { get; set; }
        public Control objectControl { get; set; }

        public Food()
        {
            objectControl = new PictureBox
            {
                Tag = GameWindow.ObjectType.Food,
                Image = Resources.Player,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
        }

        public Food(string id, int xCoord, int yCoord, int healthPoints)
        {
            this.xCoord = xCoord;
            this.yCoord = yCoord;
            this.size = healthPoints / 2;
            Uuid = id;

            objectControl = new PictureBox
            {
                Name = id,
                Tag = GameWindow.ObjectType.Food,
                Size = new Size(size, size),
                Location = new Point(xCoord, yCoord),
                Image = Resources.Player,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
        }

        public Food SetName(string name)
        {
            Uuid = name;

            objectControl.Name = Uuid;
            return this;
        }

        public Food SetLocation(int x, int y)
        {
            xCoord = x;
            yCoord = y;

            objectControl.Location = new Point(xCoord, yCoord);
            return this;
        }

        public Food SetSize(int size)
        {
            this.size = size / 2;
            objectControl.Size = new Size(this.size, this.size);
            return this;
        }

        public object Clone()
        {
            var other = (Food)this.MemberwiseClone();
            other.objectControl = new PictureBox
            {
                Name = objectControl.Name,
                Tag = objectControl.Tag,
                Size = objectControl.Size,
                Location = objectControl.Location,
                Image = Resources.Player,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = objectControl.BackColor
            };
            other.Uuid = Uuid;
            other.xCoord = this.xCoord;
            other.yCoord = this.yCoord;

            return other;
        }
    }
}
