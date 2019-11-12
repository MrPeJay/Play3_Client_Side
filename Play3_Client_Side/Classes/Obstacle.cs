using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Play3_Client_Side.Properties;

namespace Play3_Client_Side.Classes
{
    class Obstacle : IObject
    {
        public int xCoord { get; set; }
        public int yCoord { get; set; }
        public string Uuid { get; set; }
        public int size { get; set; }
        public Control objectControl { get; set; }

        public Obstacle()
        {
            objectControl = new PictureBox
            {
                Tag = GameWindow.ObjectType.Obstacle,
                Image = Resources.Obstacle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
        }

        public Obstacle(string id, int xCoord, int yCoord, int damagePoints)
        {
            this.xCoord = xCoord;
            this.yCoord = yCoord;
            this.size = damagePoints / 4;
            Uuid = id;

            objectControl = new PictureBox
            {
                Name = id,
                Tag = GameWindow.ObjectType.Obstacle,
                Size = new Size(size, size),
                Location = new Point(xCoord, yCoord),
                Image = Resources.Obstacle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
        }

        public Obstacle SetName(string name)
        {
            Uuid = name;

            objectControl.Name = Uuid;
            return this;
        }

        public Obstacle SetLocation(int x, int y)
        {
            xCoord = x;
            yCoord = y;

            objectControl.Location = new Point(xCoord, yCoord);
            return this;
        }

        public Obstacle SetSize(int size)
        {
            this.size = size / 4;
            objectControl.Size = new Size(this.size, this.size);
            return this;
        }

        public object Clone()
        {
            var other = (Obstacle)this.MemberwiseClone();
            other.objectControl = new PictureBox
            {
                Name = objectControl.Name,
                Tag = objectControl.Tag,
                Size = objectControl.Size,
                Location = objectControl.Location,
                Image = Resources.Obstacle,
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
