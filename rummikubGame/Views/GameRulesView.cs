using RummikubGame.Utilities;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Rummikub.Views
{
    public partial class GameRulesView : Form
    {
        private static readonly string instructionsAssetsPath = ConfigurationManager.AppSettings["InstructionsAssetsPath"];
        private static readonly string rtfInstructionsPath = Path.Combine(instructionsAssetsPath, "instructions_rtf.rtf");

        public GameRulesView()
        {
            InitializeComponent();
        }

        private void GameRulesView_Load(object sender, EventArgs e)
        {
            this.BackColor = Constants.BackgroundColor;

            // Sets design of the close button
            closeInstructionsButton.ForeColor = Color.Black;
            closeInstructionsButton.BackColor = Constants.MainButtonsColor;
            closeInstructionsButton.FlatStyle = FlatStyle.Flat;
            closeInstructionsButton.FlatAppearance.BorderSize = 0;

            // Changing groupbox color
            instructionsGroupbox.BackColor = Constants.ComputerBoardColor;

            // Define the RTF format for the instructions.
            instructionsRtf.BackColor = Constants.ComputerBoardColor;
            instructionsRtf.ReadOnly = true;
            instructionsRtf.BorderStyle = BorderStyle.None;
            instructionsRtf.GotFocus += Instructions_GotFocus;

            // Load the instructions from the RTF file.
            try
            {
                string rtfContent;
                using (StreamReader sr = new StreamReader(rtfInstructionsPath))
                {
                    rtfContent = sr.ReadToEnd();
                }
                instructionsRtf.Rtf = rtfContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                this.Close();
            }
        }

        private void Instructions_GotFocus(object sender, EventArgs e)
        {
            /* This code is required in order to prevent the user from
               selecting the text in the instructionsRtf file.
               the solution is to convert the focus to another element,
               in this case the close button. */
            closeInstructionsButton.Focus();
        }

        private void CloseInstructionsButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
