using System.Drawing;
using System.Windows.Forms;
using Play3_Client_Side.Interfaces;
using Play3_Client_Side.Properties;

namespace Play3_Client_Side.Classes
{
    class Player : IObject
    {
        public int xCoord { get; set; }
        public int yCoord { get; set; }
        public string Uuid { get; set; }

        public int size { get; set; }
        public Control objectControl { get; set; }

        public Player()
        {
            objectControl = new PictureBox
            {
                Tag = GameWindow.ObjectType.Player,
                Image = Resources.Player,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
        }

        public Player(string id, int xCoord, int yCoord, int size)
        {
            this.xCoord = xCoord;
            this.yCoord = yCoord;
            this.size = size;
            Uuid = id;

            objectControl =  new PictureBox
            {
                Name = id,
                Tag = GameWindow.ObjectType.Player,
                Size = new Size(size, size),
                Location = new Point(xCoord, yCoord),
                Image = Resources.Player,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
        }

        public Player SetName(string name)
        {
            Uuid = name;

            objectControl.Name = Uuid;
            return this;
        }

        public Player SetLocation(int x, int y)
        {
            xCoord = x;
            yCoord = y;

            objectControl.Location = new Point(xCoord, yCoord);
            return this;
        }

        public Player SetSize(int size)
        {
            this.size = size;
            objectControl.Size = new Size(this.size, this.size);
            return this;
        }

        public void MoveX(int x)
        {
            xCoord = x;
            objectControl.Left = xCoord;
        }

        public void MoveY(int y)
        {
            yCoord = y;
            objectControl.Top = yCoord;
        }

        //Deep Cloning
        public object Clone()
        {
            var other = (Player)this.MemberwiseClone();
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
