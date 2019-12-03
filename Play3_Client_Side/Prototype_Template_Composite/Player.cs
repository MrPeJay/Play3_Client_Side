using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Play3_Client_Side.Properties;
using Play3_Client_Side.Prototype_Template_Composite;

namespace Play3_Client_Side.Prototype
{
    internal class Player : Object
    {
        public Player()
        {
            objectControl = new PictureBox
            {
                Tag = ObjectType.Player,
                Image = Resources.Player,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
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

        #region Parent overriden methods

        public override void SetName(string name)
        {
            Uuid = name;

            objectControl.Name = Uuid;
        }

        public override void SetLocation(int x, int y)
        {
            xCoord = x;
            yCoord = y;

            objectControl.Location = new Point(xCoord, yCoord);
        }

        public override void SetSize(int size)
        {
            this.size = size;
            objectControl.Size = new Size(this.size, this.size);
        }

        public override object Clone()
        {
            var other = (Player) this.MemberwiseClone();
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

        #endregion

        public override void AddObject(ObjectComponent newObjectComponent)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveObject(ObjectComponent objectComponentToRemove)
        {
            throw new System.NotImplementedException();
        }

        public override string GetComponentInfo()
        {
            return objectControl.Tag + " " + Uuid;
        }

        public override bool isLeaf()
        {
            return true;
        }

        public override List<ObjectComponent> GetLeafObjects()
        {
            return null;
        }
    }
}
