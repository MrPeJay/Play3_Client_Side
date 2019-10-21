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

namespace Play3_Client_Side
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }


        private void KeyIsDown(object sender, KeyEventArgs e)
        {
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {

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

            await Processor.LoadData("api/player", onSuccess: (data) =>
            { 
                var players = JsonConvert.DeserializeObject<List<DTO.PlayerDTO>>(data);

                foreach (var player in players)
                {
                    if (!string.IsNullOrEmpty(player.Name))
                    {
                        if (Name_Input.Text.ToLower().Equals(player.Name.ToLower()))
                        {
                            Error_Label.Text = "Name already exists";
                            Error_Label.Visible = true;
                            return;
                        }
                    }
                }

                //Disable labels
                var content = new Dictionary<string, string> {{"name", Name_Input.Text}};

                Processor.PostData("api/player/new", content, onSuccess: () =>
                {
                    ToggleGame(false);
                });
            }, onFailure: (error) => { });
        }

        private void ToggleGame(bool toggle)
        {
            Error_Label.Visible = toggle;
            Name_Input.Visible = toggle;
            GameName_Label.Visible = toggle;
            Play_Button.Visible = toggle;
            EnterName_Label.Visible = toggle;
        }
    }
}
