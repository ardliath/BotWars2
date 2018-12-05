using BotWars2Server.Code.Communication;
using BotWars2Server.Code.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotWars2Server
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var gf = new GameForm();
            gf.Show();
            gf.StartGame(new Arena
            {
                Height = 200,
                Width = 200,
            },
            new RemoteBot("Player 1", "", new Position(5, 5)),
            new RemoteBot("Player 2", "", new Position(50, 50)));
        }
    }
}
