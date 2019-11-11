using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Play3_Client_Side.API;
using Play3_Client_Side.Properties;

namespace Play3_Client_Side
{
    public partial class GameWindow : Form
    {
        private DTO.GameDTO gameData;

        private DTO.PlayerDTO currentPlayer;
        private Control playerObject;

        private List<string> playerList = new List<string>();
        private List<string> foodList = new List<string>();
        private List<string> obstacleList = new List<string>();

        private bool movingUp, movingDown, movingRight, movingLeft;

        private int maxY, maxX;

        public GameWindow()
        {
            InitializeComponent();
            maxY = ClientSize.Height;
            maxX = ClientSize.Width;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            ApiHelper.InitializeClient();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (playerObject == null) return;

            var moved = false;
            int speed = 5;

            if (movingUp)
            {
                int newCoord = playerObject.Top - speed;
                if (newCoord < 0) return;
                playerObject.Top = newCoord;
                moved = true;
            }

            if (movingDown)
            {
                int newCoord = playerObject.Top + speed;
                if (newCoord > maxY) return;
                playerObject.Top = newCoord;
                moved = true;
            }

            if (movingLeft)
            {
                int newCoord = playerObject.Left - speed;
                if (newCoord < 0) return;
                playerObject.Left = newCoord;
                moved = true;
            }

            if (movingRight)
            {
                int newCoord = playerObject.Left + speed;
                if (newCoord > maxX) return;
                playerObject.Left = newCoord;
                moved = true;
            }

            //Send player locations to server.
            if (moved)
            {
                var content = new Dictionary<string, string>
                {
                    {"playerUuid", currentPlayer.Uuid},
                    {"xCoord", playerObject.Location.X.ToString()},
                    {"yCoord", playerObject.Location.Y.ToString()}
                };

                Processor.PostData("api/player/move", content);
            }

            checkForCollision();
        }

        private void checkForCollision()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if (x.Tag.Equals(ObjectType.Food))
                    {
                        // Jei susiduria su maistu
                        if (playerObject.Bounds.IntersectsWith(x.Bounds))
                        {
                            Dictionary<string, string> content = new Dictionary<string, string>
                        {
                            {"playerUuid", currentPlayer.Uuid },
                            {"foodUuid", x.Name }
                        };
                            Processor.PostData("api/player/eat-food", content);
                        }
                    }

                    if (x.Tag.Equals(ObjectType.Player))
                    {
                        // Jei susiduria su žaidėju
                    }

                    if (x.Tag.Equals(ObjectType.Obstacle))
                    {
                        // Jei susiduria su kliūtimi
                    }
                }
            }
        }


        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                movingUp = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                movingDown = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                movingRight = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                movingLeft = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                movingUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                movingDown = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                movingRight = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                movingLeft = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void Play_Button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Name_Input.Text))
            {
                Error_Label.Text = "Please input your name".ToUpper();
                Error_Label.Visible = true;
                return;
            }

            await Processor.LoadData("api/player", (data) =>
            { 
                var players = JsonConvert.DeserializeObject<List<DTO.PlayerDTO>>(data);

                if (players.Where(player => !string.IsNullOrEmpty(player.Name)).Any(player => Name_Input.Text.ToLower().Equals(player.Name.ToLower())))
                {
                    Error_Label.Text = "Name already exists";
                    Error_Label.Visible = true;
                    return;
                }

                //Disable labels
                var content = new Dictionary<string, string> {{"name", Name_Input.Text}};

                Processor.PostData("api/player/new", content, (player) =>
                {
                    currentPlayer = JsonConvert.DeserializeObject<DTO.PlayerDTO>(player);
                    ToggleGame(false);
                });
            }, (error) => { });
        }

        private async void ToggleGame(bool toggle)
        {
            Error_Label.Visible = toggle;
            Name_Input.Visible = toggle;
            GameName_Label.Visible = toggle;
            Play_Button.Visible = toggle;
            EnterName_Label.Visible = toggle;
            //Enable loading screen.
            LoadingText.Visible = true;

            timer1.Enabled = true;
            timer2.Enabled = true;

            await Processor.LoadData("api/game", (data) =>
            {
                gameData = JsonConvert.DeserializeObject<DTO.GameDTO>(data);

                SpawnObjects();

                //Disable Loading Screen.
                LoadingText.Visible = false;

            }, (error) => { });
        }

        private void SpawnObjects()
        {
            foreach (var foodObject in gameData.Foods.Select(food => GetObject(ObjectType.Food, food.Uuid, food.XCoord, food.YCoord)))
            { 
                Controls.Add(foodObject);
                foodList.Add(foodObject.Name);
            }

            foreach (var obstacleObject in gameData.Obstacles.Select(obstacle => GetObject(ObjectType.Obstacle, obstacle.Uuid, obstacle.XCoord, obstacle.YCoord)))
            {
                Controls.Add(obstacleObject);
                obstacleList.Add(obstacleObject.Name);
            }

            foreach (var playerObject in gameData.Players.Select(player => GetObject(ObjectType.Player, player.Uuid, player.XCoord, player.YCoord)))
            {
                Controls.Add(playerObject);
                playerList.Add(playerObject.Name);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void playerName_Click(object sender, EventArgs e)
        {

        }

        private async void timer2_Tick(object sender, EventArgs e)
        {
            //Request server other player locations
            await Processor.LoadData("api/player", (otherPlayers) =>
            {
                var players = JsonConvert.DeserializeObject<List<DTO.PlayerDTO>>(otherPlayers);
                var playerIDs = players.Select(player => player.Uuid).ToList();

                //Delete player that no longer exist in server.
                foreach (var control in playerList.Where(player => !playerIDs.Contains(player))
                    .SelectMany(player => Controls.Find(player, false)))
                {
                    Controls.Remove(control);
                }

                foreach (var player in players.Where(player => !player.Uuid.Equals(currentPlayer.Uuid)))
                {
                    //If new player create a new object
                    if (!playerList.Contains(player.Uuid))
                    {
                        playerList.Add(player.Uuid);

                        var playerObject = GetObject(ObjectType.Player, player.Uuid, player.XCoord, player.YCoord);
                        Controls.Add(playerObject);
                    }
                    //If not new, update
                    else
                    {
                        foreach (var foundPlayer in Controls.Find(player.Uuid, false))
                        {
                            foundPlayer.Location = new Point(player.XCoord, player.YCoord);
                        }
                    }
                }
            });
        }

        private void GameName_Label_Click(object sender, EventArgs e)
        {

        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            Processor.DeleteData("api/player/delete", currentPlayer.Uuid);
        }

        private void OnControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control.Name.Equals(currentPlayer.Uuid))
            {
                playerObject = e.Control;
            }
        }

        private PictureBox GetObject(ObjectType type, string id, int xCoord, int yCoord)
        {
            switch (type)
            {
                case ObjectType.Food:
                    return new PictureBox
                    {
                        Name = id,
                        Tag = ObjectType.Food,
                        Size = new Size(2, 2),
                        Location = new Point(xCoord, yCoord),
                        Image = Resources.Player,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent
                    };
                case ObjectType.Obstacle:
                    return new PictureBox
                    {
                        Name = id,
                        Tag = ObjectType.Obstacle,
                        Size = new Size(4, 4),
                        Location = new Point(xCoord, yCoord),
                        Image = Resources.Player,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Red
                    };
                case ObjectType.Player:
                    return new PictureBox
                    {
                        Name = id,
                        Tag = ObjectType.Player,
                        Size = new Size(10, 10),
                        Location = new Point(xCoord, yCoord),
                        Image = Resources.Player,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private enum ObjectType
        {
            Player,
            Food,
            Obstacle
        }
    }
}
