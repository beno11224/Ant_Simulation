using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ant_Simulation
{
    public partial class GenerateNewBoard : Form
    {
        Form1 Parent;

        public GenerateNewBoard(Form1 parent)
        {
            InitializeComponent();
            Parent = parent;
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (false == int.TryParse(NumberofAntsTextBox.Text, out Parent._numberOfNormalAnts))
            {
                Parent._numberOfNormalAnts = 5;
            }

            if (false == int.TryParse(WidthTextBox.Text, out Parent._boardWidth))
            {
                Parent._boardWidth = 20;
            }

            if (false == int.TryParse(HeightTextBox.Text, out Parent._boardHeight))
            {
                Parent._boardHeight = 20;
            }

            if (false == int.TryParse(GoalsTextBox.Text, out Parent._goalTiles))
            {
                Parent._goalTiles = 20;
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NumberofAntsTextBox_TextChanged(object sender, EventArgs e)
        {
            checkEnableConfirmButton();
        }

        private void HeightTextBox_TextChanged(object sender, EventArgs e)
        {
            checkEnableConfirmButton();
        }

        private void WidthTextBox_TextChanged(object sender, EventArgs e)
        {
            checkEnableConfirmButton();
        }

        private void checkEnableConfirmButton()
        {
            ConfirmButton.Enabled = (int.TryParse(NumberofAntsTextBox.Text, out int temp) && int.TryParse(WidthTextBox.Text, out int temp2) && int.TryParse(HeightTextBox.Text, out int temp3));
        }

        private void GenerateNewBoard_Load(object sender, EventArgs e)
        {

        }
    }
}
