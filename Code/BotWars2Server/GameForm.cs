using BotWars2Server.Code.Logic;
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
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
        }


        public void StartGame(Arena arena, params Player[] players)
        {
            var gameManager = new GameManager(arena, players);

            gameManager.Play(Update);
        }

        public void Update(Arena arena)
        {

        }
    }
}
