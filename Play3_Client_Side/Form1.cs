using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json;
using Play3_Client_Side.Adapter;
using Play3_Client_Side.API;
using Play3_Client_Side.Command;
using Play3_Client_Side.Prototype;
using Play3_Client_Side.Prototype_Template_Composite;

namespace Play3_Client_Side
{
    public partial class GameWindow : Form
    {
        #region Variables

        //Current game data.
        private DTO.GameDTO gameData;

        //Prototype holder
        private PrototypeHolder prototypes;

        //Current player data and object control.
        private DTO.PlayerDTO currentPlayer;
        private Player playerObject;

        private IUpdater updater = new ServerAdapter();

        private ObjectComponent playerObjects;
        private ObjectComponent eatenPlayerObjects;

        private ObjectComponent foodObjects;
        private ObjectComponent eatenFoodObjects;

        private ObjectComponent obstacleObjects;
        private ObjectComponent eatenObstacleObjects;

        private ObjectComponent objects;

        //Player properties
        [Serializable]
        public static class PlayerSettings
        {
            public static int minPlayerSpeed = 1,
                maxPlayerSpeed = 5,
                maxPlayerSize = 200,
                minPlayerSize = 10;

            public static bool movingUp,
                movingDown,
                movingRight,
                movingLeft;
        }

        //Max x and y positions of the player.
        private int maxY, maxX;

        #endregion

        public GameWindow()
        {
            InitializeComponent();
            maxY = ClientSize.Height;
            maxX = ClientSize.Width;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            ApiHelper.InitializeClient();

            //Create 
            objects = new ObjectGroup("All objects", "Every object in the scene:");

            playerObjects = new ObjectGroup("Player Objects", "Every Player object in the scene:");
            eatenPlayerObjects =
                new ObjectGroup("Eaten Player objects", "All player objects that were eaten by you:");

            foodObjects = new ObjectGroup("Food Objects", "Every food object in the scene:");
            eatenFoodObjects =
                new ObjectGroup("Eaten Food objects", "All food objects that were eaten by you:");

            obstacleObjects =
                new ObjectGroup("Obstacle Objects", "Every obstacle object in the scene:");
            eatenObstacleObjects =
                new ObjectGroup("Eaten Obstacle objects", "All obstacle objects that were eaten by you:");


            playerObjects.AddObject(eatenPlayerObjects);
            foodObjects.AddObject(eatenFoodObjects);
            obstacleObjects.AddObject(eatenObstacleObjects);

            objects.AddObject(playerObjects);
            objects.AddObject(foodObjects);
            objects.AddObject(obstacleObjects);

            prototypes = new PrototypeHolder();
        }

        /// <summary>
        /// Updates player score text.
        /// </summary>
        /// <param name="score"></param>
        private void SetScore(int score)
        {
            ScoreLabel.Text = "Score: " + score;
        }

        /// <summary>
        /// Checks for player collisions with other objects.
        /// </summary>
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

                            updater.PostData("api/player/eat-food", Processor.PostDataType.Post, content);
                        }
                    }

                    if (playerObject.objectControl.Bounds.IntersectsWith(x.Bounds) && x.Tag.Equals(ObjectType.Player) && playerObject.Uuid != x.Name)
                    {
                        var currentPlayer = gameData.Players.Find(player => player.Uuid == playerObject.Uuid);
                        var targetPlayer = gameData.Players.Find(player => player.Uuid == x.Name);

                        if (currentPlayer.Health >= targetPlayer.Health)
                        {
                            //playerList.Remove(x.Name);
                            Controls.Remove(x);

                            var data = new Dictionary<string, string>
                            {
                                {"playerUuid", currentPlayer.Uuid},
                                {"secondPlayerUuid", targetPlayer.Uuid}
                            };
                            updater.PostData("api/player/eat-player", Processor.PostDataType.Post, data);
                        }
                        else
                        {
                            //playerList.Remove(playerObject.Uuid);
                            Controls.Remove(playerObject.objectControl);
                            targetPlayer.Health += currentPlayer.Health;

                            var data = new Dictionary<string, string>
                            {
                                {"playerUuid", targetPlayer.Uuid},
                                {"secondPlayerUuid", currentPlayer.Uuid}
                            };
                            updater.PostData("api/player/eat-player", Processor.PostDataType.Post, data);
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

                                //obstacleList.Remove(x.Name);
                                Controls.Remove(x);

                                updater.PostData("api/player/touch-obstacle", Processor.PostDataType.Post, content);
                            }
                        }
                    }
                }
            }

            SetScore(playerObject.size * 10);
        }

        #region Key Events
        /// <summary>
        /// Event triggered when a key is pressed down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                PlayerSettings.movingUp = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                PlayerSettings.movingDown = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                PlayerSettings.movingRight = true;
            }
            if (e.KeyCode == Keys.Left)
            {
                PlayerSettings.movingLeft = true;
            }
        }

        /// <summary>
        /// Event triggered when a key is pressed up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                PlayerSettings.movingUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                PlayerSettings.movingDown = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                PlayerSettings.movingRight = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                PlayerSettings.movingLeft = false;
            }
        }
        #endregion

        /// <summary>
        /// Loads game data.
        /// </summary>
        /// <param name="toggle"></param>
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

            await updater.LoadData("api/game", (data) =>
            {
                gameData = JsonConvert.DeserializeObject<DTO.GameDTO>(data);

                SpawnObjects();

                //Disable Loading Screen.
                LoadingText.Visible = false;
                ScoreLabel.Visible = true;

            }, (error) => { });
        }

        /// <summary>
        /// Spawns all types of objects.
        /// </summary>
        private void SpawnObjects()
        {
            //Spawn Player objects
            foreach (var player in gameData.Players)
            {
                var clonedPlayer = prototypes.GetPlayerClone();

                var tempSize = (int)player.Health / 10;
                var calc = tempSize > PlayerSettings.maxPlayerSize ? PlayerSettings.maxPlayerSize :
                    player.Health < PlayerSettings.minPlayerSize ? PlayerSettings.minPlayerSize : tempSize;

                clonedPlayer.Update(player.Uuid, player.XCoord, player.YCoord, calc);

                Controls.Add(clonedPlayer.objectControl);
                //playerList.Add(clonedPlayer.Uuid);
                playerObjects.AddObject(clonedPlayer);
            }
            
            //Spawn Food objects
            foreach (var food in gameData.Foods)
            {
                var clonedFood = prototypes.GetFoodClone();

                clonedFood.Update(food.Uuid, food.XCoord, food.YCoord, (int)food.HealthPoints);

                Controls.Add(clonedFood.objectControl);
                //foodList.Add(clonedFood.Uuid);
                foodObjects.AddObject(clonedFood);
            }

            //Spawn Obstacle objects
            foreach (var obstacle in gameData.Obstacles)
            {
                var clonedObstacle = prototypes.GetObstacleClone();

                clonedObstacle.Update(obstacle.Uuid, obstacle.XCoord, obstacle.YCoord, (int)obstacle.DamagePoints);

                Controls.Add(clonedObstacle.objectControl);
                //obstacleList.Add(clonedObstacle.Uuid);
                obstacleObjects.AddObject(clonedObstacle);
            }
        }

        #region Timer Events

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (playerObject == null)
            {
                return;
            }

            var moved = false;
            var calc = PlayerSettings.minPlayerSpeed * (PlayerSettings.maxPlayerSize / playerObject.size);

            var speed = calc > PlayerSettings.maxPlayerSpeed ? PlayerSettings.maxPlayerSpeed : calc;
            var mover = new PlayerMover();
            if (PlayerSettings.movingUp)
            {
                var newCoord = playerObject.yCoord - speed;
                if (newCoord < 0) return;

                ICommand moveUp = new MoveUp(playerObject, newCoord);
                mover.performMove(moveUp);
                moved = true;
            }

            if (PlayerSettings.movingDown)
            {
                var newCoord = playerObject.yCoord + speed;
                if (newCoord > maxY) return;

                ICommand moveDown = new MoveDown(playerObject, newCoord);
                mover.performMove(moveDown);
                moved = true;
            }

            if (PlayerSettings.movingLeft)
            {
                var newCoord = playerObject.xCoord - speed;
                if (newCoord < 0) return;

                ICommand moveLeft = new MoveLeft(playerObject, newCoord);
                mover.performMove(moveLeft);
                moved = true;
            }

            if (PlayerSettings.movingRight)
            {
                var newCoord = playerObject.xCoord + speed;
                if (newCoord > maxX) return;

                ICommand moveRight = new MoveRight(playerObject, newCoord);
                mover.performMove(moveRight);
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

                updater.PostData("api/player/move", Processor.PostDataType.Post, content);
            }

            CheckForCollision();
        }

        private async void timer2_Tick(object sender, EventArgs e)
        {
            //Request server other player locations
            await updater.LoadData("api/player", (otherPlayers) =>
            {
                var players = JsonConvert.DeserializeObject<List<DTO.PlayerDTO>>(otherPlayers);
                var playerIDs = players.Select(player => player.Uuid).ToList();

                //Delete player that no longer exist in server.
                /*
                foreach (var control in playerObjects.Where(player => !playerIDs.Contains(player))
                    .SelectMany(player => Controls.Find(player, false)))
                {
                    Controls.Remove(control);
                }
                */

                foreach (var player in players)
                {
                    //Update current player
                    if (player.Uuid.Equals(currentPlayer.Uuid))
                    {
                        var tempSize = (int)player.Health / 10;
                        var calc = tempSize > PlayerSettings.maxPlayerSize ? PlayerSettings.maxPlayerSize :
                                player.Health < PlayerSettings.minPlayerSize ? PlayerSettings.minPlayerSize : tempSize;

                        playerObject.SetSize(calc);
                        continue;
                    }

                    var playerControl = Controls.Find(player.Uuid, true);

                    //If new player create a new object
                    if (playerControl.Length <= 0)
                    {
                        //playerList.Add(player.Uuid);

                        var newPlayerObject = prototypes.GetPlayerClone();

                        newPlayerObject.Update(player.Uuid, player.XCoord, player.YCoord,
                            (int)player.Health);

                        Controls.Add(newPlayerObject.objectControl);

                        playerObjects.AddObject(newPlayerObject);
                    }
                    
                    //If not new, update
                    else
                    {
                        foreach (var foundPlayer in Controls.Find(player.Uuid, false))
                        {
                            foundPlayer.Location = new Point(player.XCoord, player.YCoord);

                            var tempSize = (int)player.Health / 10;
                            var calc = tempSize > PlayerSettings.maxPlayerSize ? PlayerSettings.maxPlayerSize :
                                player.Health < PlayerSettings.minPlayerSize ? PlayerSettings.minPlayerSize : tempSize;

                            foundPlayer.Size = new Size(calc, calc);
                        }
                    }
                }
            });

            await updater.LoadData("api/food", (response) =>
            {
                var foods = JsonConvert.DeserializeObject<List<DTO.FoodDTO>>(response);

                var foodIds = foods.Select(food => food.Uuid).ToList();
                //Delete player that no longer exist in server.
                /*
                foreach (var control in foodList.Where(food => !foodIds.Contains(food))
                    .SelectMany(food => Controls.Find(food, false)))
                {
                    Controls.Remove(control);
                }
                */

                foreach (var food in foods)
                {
                    var foodControl = Controls.Find(food.Uuid, true);

                    //If new food create a new object
                    if (foodControl.Length <= 0)
                    {
                        //foodList.Add(food.Uuid);

                        var newFoodObject = prototypes.GetFoodClone();

                        newFoodObject.Update(food.Uuid, food.XCoord, food.YCoord,
                            (int)food.HealthPoints);

                        Controls.Add(newFoodObject.objectControl);

                        foodObjects.AddObject(newFoodObject);
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

        #endregion

        #region Form actions

        /// <summary>
        /// On form loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event triggered when form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            //If was connected, delete current player from database.
            if (currentPlayer == null) return;

            updater.PostData("api/player/delete", Processor.PostDataType.Delete, uuid: currentPlayer.Uuid);
        }

        /// <summary>
        /// Event triggered when control is added to the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control.Name.Equals(currentPlayer.Uuid))
            {
                playerObject = prototypes.GetPlayerClone();
                playerObject.objectControl = e.Control;

                playerObject.Update(currentPlayer.Uuid, e.Control.Location.X, e.Control.Location.Y,
                    e.Control.Size.Width);
            }
        }

        /// <summary>
        /// Method triggered on Play button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Play_Button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Name_Input.Text))
            {
                Error_Label.Text = "Please input your name".ToUpper();
                Error_Label.Visible = true;
                return;
            }

            await updater.LoadData("api/player", (data) =>
            {
                var players = JsonConvert.DeserializeObject<List<DTO.PlayerDTO>>(data);

                if (players.Where(player => !string.IsNullOrEmpty(player.Name)).Any(player => Name_Input.Text.ToLower().Equals(player.Name.ToLower())))
                {
                    Error_Label.Text = "Name already exists";
                    Error_Label.Visible = true;
                    return;
                }

                //Disable labels
                var content = new Dictionary<string, string> { { "name", Name_Input.Text } };

                updater.PostData("api/player/new", Processor.PostDataType.Post, content, (player) =>
                {
                    currentPlayer = JsonConvert.DeserializeObject<DTO.PlayerDTO>(player);
                    ToggleGame(false);
                });
            }, (error) => { });
        }

        #endregion
    }
}
