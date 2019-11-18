using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Play3_Client_Side.API;
using Play3_Client_Side.Classes;
using Play3_Client_Side.Properties;

namespace Play3_Client_Side
{
    public partial class GameWindow : Form
    {
        private DTO.GameDTO gameData;

        private DTO.PlayerDTO currentPlayer;
        private Player playerObject;

        private List<string> playerList = new List<string>();
        private List<string> foodList = new List<string>();
        private List<string> obstacleList = new List<string>();

        private int growMultiplier = 5;
        //Player properties
        public int minPlayerSpeed = 1, maxPlayerSpeed = 5, maxPlayerSize = 200, minPlayerSize = 10;

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

        //On form load set custom fonts.
        private void Form1_Load(object sender, EventArgs e)
        {
            var fontCollection = new PrivateFontCollection();

            // Select your font from the resources.
            var fontLength = Properties.Resources.Amethyst.Length;

            // create a buffer to read in to
            var fontdata = Properties.Resources.Amethyst;

            // create an unsafe memory block for the font data
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, fontLength);

            // pass the font to the font collection
            fontCollection.AddMemoryFont(data, fontLength);

            GameName_Label.Font = new Font(fontCollection.Families[0], GameName_Label.Font.Size);
            LoadingText.Font = new Font(fontCollection.Families[0], LoadingText.Font.Size);
        }

        private void SetScore(int score)
        {
            ScoreLabel.Text = "Score: " + score;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (playerObject == null)
            {
                return;
            }

            var moved = false;
            var calc = minPlayerSpeed * (maxPlayerSize / playerObject.size);

            var speed = calc > maxPlayerSpeed ? maxPlayerSpeed : calc;
            if (movingUp)
            {
                var newCoord = playerObject.yCoord - speed;
                if (newCoord < 0) return;
                playerObject.MoveY(newCoord);
                moved = true;
            }

            if (movingDown)
            {
                var newCoord = playerObject.yCoord + speed;
                if (newCoord > maxY) return;
                playerObject.MoveY(newCoord);
                moved = true;
            }

            if (movingLeft)
            {
                var newCoord = playerObject.xCoord - speed;
                if (newCoord < 0) return;
                playerObject.MoveX(newCoord);
                moved = true;
            }

            if (movingRight)
            {
                var newCoord = playerObject.xCoord + speed;
                if (newCoord > maxX) return;
                playerObject.MoveX(newCoord);
                moved = true;
            }

            //Send player locations to server.
            if (moved)
            {
                var content = new Dictionary<string, string>
                {
                    {"playerUuid", currentPlayer.Uuid},
                    {"xCoord", playerObject.objectControl.Location.X.ToString()},
                    {"yCoord", playerObject.objectControl.Location.Y.ToString()}
                };

                Processor.PostData("api/player/move", content);
            }

            CheckForCollision();
        }

        private DTO.FoodDTO GetFoodByID(string ID)
        {
            return gameData.Foods.FirstOrDefault(food => food.Uuid.Equals(ID));
        }
        private DTO.ObstacleDTO GetObstacleByID(string ID)
        {
            return gameData.Obstacles.FirstOrDefault(obstacle => obstacle.Uuid.Equals(ID));
        }

        private void CheckForCollision()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if (x.Tag.Equals(ObjectType.Food))
                    {
                        // Jei susiduria su maistu
                        if (playerObject.objectControl.Bounds.IntersectsWith(x.Bounds))
                        {
                            var content = new Dictionary<string, string>
                            {
                                {"playerUuid", playerObject.Uuid},
                                {"foodUuid", x.Name}
                            };

                            //foodList.Remove(x.Name);
                            Controls.Remove(x);

                            //Find food object and increase player size.
                            /*
                           var foodObject = GetFoodByID(x.Name);

                           if (foodObject == null)
                           {
                               return;
                           }

                           var newSize = (int) (playerObject.size + foodObject.HealthPoints) > maxPlayerSize
                               ? maxPlayerSize
                               : (int) (playerObject.size +
                                        foodObject.HealthPoints / playerObject.size * growMultiplier) + 1;

                           playerObject.SetSize(newSize);
                           */

                            Processor.PostData("api/player/eat-food", content);
                        }
                    }

                    if (playerObject.objectControl.Bounds.IntersectsWith(x.Bounds) && x.Tag.Equals(ObjectType.Player) && playerObject.Uuid != x.Name)
                    {
                        DTO.PlayerDTO currentPlayer = gameData.Players.Find(player => player.Uuid == playerObject.Uuid);
                        DTO.PlayerDTO targetPlayer = gameData.Players.Find(player => player.Uuid == x.Name);

                        if (currentPlayer.Health >= targetPlayer.Health)
                        {
                            playerList.Remove(x.Name);
                            Controls.Remove(x);

                            /*
                            var newSize = (int)(playerObject.size + targetPlayer.Health) > maxPlayerSize
                                ? maxPlayerSize
                                : (int)(playerObject.size +
                                         targetPlayer.Health / playerObject.size * growMultiplier) + 1;


                            currentPlayer.Health += targetPlayer.Health;
                            playerObject.SetSize(newSize);
                            */
                            var data = new Dictionary<string, string>
                            {
                                {"playerUuid", currentPlayer.Uuid},
                                {"secondPlayerUuid", targetPlayer.Uuid}
                            };
                            Processor.PostData("api/player/eat-player", data);
                        }
                        else
                        {
                            playerList.Remove(playerObject.Uuid);
                            Controls.Remove(playerObject.objectControl);
                            //int newSize = x.Size.Width + (int)currentPlayer.Health/5;
                            targetPlayer.Health += currentPlayer.Health;
                            //playerObject.SetSize(newSize);
                            var data = new Dictionary<string, string>
                            {
                                {"playerUuid", targetPlayer.Uuid},
                                {"secondPlayerUuid", currentPlayer.Uuid}
                            };
                            Processor.PostData("api/player/eat-player", data);
                        }
                    }

                    if (x.Tag.Equals(ObjectType.Obstacle))
                    {
                        if (x.Tag.Equals(ObjectType.Obstacle))
                        {
                            // If collides with obstacle
                            if (playerObject.objectControl.Bounds.IntersectsWith(x.Bounds))
                            {
                                var content = new Dictionary<string, string>
                                {
                                    {"playerUuid", playerObject.Uuid},
                                    {"obstacleUuid", x.Name}
                                };

                                obstacleList.Remove(x.Name);
                                Controls.Remove(x);

                                //Find food object and increase player size.
                                //var obstacleObject = GetObstacleByID(x.Name);

                                /*
                                var tempSize = (int) (playerObject.size - obstacleObject.DamagePoints);

                                var newSize =
                                    tempSize < minPlayerSize
                                        ? minPlayerSize
                                        : tempSize;

                                playerObject.SetSize(newSize);
                                */
                                Processor.PostData("api/player/touch-obstacle", content);
                            }
                        }
                    }
                }
            }

            SetScore(playerObject.size * 10);
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
                ScoreLabel.Visible = true;

            }, (error) => { });
        }

        private void SpawnObjects()
        {
            var playerCloneableObject = new Player();
            var foodCloneableObject = new Food();
            var obstacleCloneableObject = new Obstacle();

            foreach (var player in gameData.Players)
            {
                var clonedPlayer = (Player)playerCloneableObject.Clone();

                var tempSize = (int)player.Health / 10;
                var calc = tempSize > maxPlayerSize ? maxPlayerSize :
                    player.Health < minPlayerSize ? minPlayerSize : tempSize;

                clonedPlayer.SetSize(calc).SetLocation(player.XCoord, player.YCoord).SetName(player.Uuid);

                Controls.Add(clonedPlayer.objectControl);
                playerList.Add(clonedPlayer.Uuid);
            }

            foreach (var food in gameData.Foods)
            {
                var clonedFood = (Food)foodCloneableObject.Clone();

                clonedFood.SetSize((int)food.HealthPoints).SetLocation(food.XCoord, food.YCoord).SetName(food.Uuid);

                Controls.Add(clonedFood.objectControl);
                foodList.Add(clonedFood.Uuid);
            }


            foreach (var obstacle in gameData.Obstacles)
            {
                var clonedObstacle = (Obstacle)obstacleCloneableObject.Clone();

                clonedObstacle.SetSize((int)obstacle.DamagePoints).SetLocation(obstacle.XCoord, obstacle.YCoord).SetName(obstacle.Uuid);

                Controls.Add(clonedObstacle.objectControl);
                obstacleList.Add(clonedObstacle.Uuid);
            }
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

                foreach (var player in players)
                {
                    //Update current player
                    if (player.Uuid.Equals(currentPlayer.Uuid))
                    {
                        var tempSize = (int)player.Health / 10;
                        var calc = tempSize > maxPlayerSize ? maxPlayerSize :
                                player.Health < minPlayerSize ? minPlayerSize : tempSize;

                        playerObject.SetSize(calc);
                        continue;
                    }

                    //If new player create a new object
                    if (!playerList.Contains(player.Uuid))
                    {
                        playerList.Add(player.Uuid);

                        var newPlayerObject = new Player(player.Uuid, player.XCoord, player.YCoord,
                            (int)player.Health);

                        Controls.Add(newPlayerObject.objectControl);
                    }
                    //If not new, update
                    else
                    {
                        foreach (var foundPlayer in Controls.Find(player.Uuid, false))
                        {
                            foundPlayer.Location = new Point(player.XCoord, player.YCoord);

                            var tempSize = (int)player.Health / 10;
                            var calc = tempSize > maxPlayerSize ? maxPlayerSize :
                                player.Health < minPlayerSize ? minPlayerSize : tempSize;

                            foundPlayer.Size = new Size(calc, calc);
                        }
                    }
                }
            });

            await Processor.LoadData("api/food", (response) =>
            {
                var foods = JsonConvert.DeserializeObject<List<DTO.FoodDTO>>(response);

                var foodIds = foods.Select(food => food.Uuid).ToList();
                //Delete player that no longer exist in server.
                foreach (var control in foodList.Where(food => !foodIds.Contains(food))
                    .SelectMany(food => Controls.Find(food, false)))
                {
                    Controls.Remove(control);
                }
                foreach (var food in foods)
                {
                    //If new food create a new object
                    if (!foodList.Contains(food.Uuid))
                    {
                        foodList.Add(food.Uuid);
                        var newFoodObject = new Food(food.Uuid, food.XCoord, food.YCoord,
                            (int)food.HealthPoints);

                        Controls.Add(newFoodObject.objectControl);
                    }
                    //If not new, update
                    else
                    {
                        foreach (var foodToUpdate in Controls.Find(food.Uuid, false))
                        {
                            foodToUpdate.Location = new Point(food.XCoord, food.YCoord);
                        }
                    }
                }
            });
        }


        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            if (currentPlayer != null)
            {
                Processor.DeleteData("api/player/delete", currentPlayer.Uuid);
            }
        }

        private void OnControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control.Name.Equals(currentPlayer.Uuid))
            {
                playerObject = new Player(currentPlayer.Uuid, e.Control.Location.X, e.Control.Location.Y,
                    e.Control.Size.Width) {objectControl = e.Control};
            }
        }

        private PictureBox GetObject(ObjectType type, string id, int xCoord, int yCoord, int size)
        {
            switch (type)
            {
                case ObjectType.Food:
                    return new PictureBox
                    {
                        Name = id,
                        Tag = ObjectType.Food,
                        Size = new Size(size / 2, size / 2),
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
                        Size = new Size(size / 4, size / 4),
                        Location = new Point(xCoord, yCoord),
                        Image = Resources.Obstacle,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public enum ObjectType
        {
            Player,
            Food,
            Obstacle
        }
    }
}
