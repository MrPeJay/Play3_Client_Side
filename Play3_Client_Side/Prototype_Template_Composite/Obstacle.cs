using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Play3_Client_Side.Properties;
using Play3_Client_Side.Prototype_Template_Composite;

namespace Play3_Client_Side.Prototype
{
    class Obstacle : Object
    {
        public Obstacle()
        {
            objectControl = new PictureBox
            {
                Tag = ObjectType.Obstacle,
                Image = Resources.Obstacle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
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
            this.size = size / 4;
            objectControl.Size = new Size(this.size, this.size);
        }

        public override object Clone()
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
