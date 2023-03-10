using rummikubGame.Utilities;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace rummikubGame
{
    public partial class GameRules : Form
    {
        public static string INSTRUCTIONS_ASSETS_PATH = ConfigurationManager.AppSettings["InstructionsAssetsPath"];
        public static string RTF_INSTRUCTIONS_PATH = Path.Combine(INSTRUCTIONS_ASSETS_PATH, "instructions_rtf.rtf");

        public GameRules()
        {
            InitializeComponent();
        }

        private void GameRules_Load(object sender, EventArgs e)
        {
            this.BackColor = Constants.BACKGROUND_COLOR;

            // Sets design of the close button
            close_instructions_btn.ForeColor = Color.Black;
            close_instructions_btn.BackColor = Constants.BACKGROUND_COLOR;
            close_instructions_btn.FlatStyle = FlatStyle.Flat;
            close_instructions_btn.FlatAppearance.BorderSize = 0;

            // Changing groupbox color
            instructions_groupbox.BackColor = Constants.COMPUTER_BOARD_COLOR;

            // Define the RTF format for the instructions.
            instructions_rtf.BackColor = Constants.COMPUTER_BOARD_COLOR;
            instructions_rtf.ReadOnly = true;
            instructions_rtf.BorderStyle = BorderStyle.None;
            instructions_rtf.GotFocus += new EventHandler(Instructions_GotFocus);

            // Load the instructions from the RTF file.
            try
            {
                string rtfContent = File.ReadAllText(RTF_INSTRUCTIONS_PATH);
                instructions_rtf.Rtf = rtfContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                this.Close();
            }
        }

        private void Instructions_GotFocus(object sender, EventArgs e)
        {
            /* This code is required in order to prevent the user from
               selecting the text in the instructions_rtf file.
               the solution is to convert the focus to another element,
               in this case the close button. */
            
            close_instructions_btn.Focus();
        }

        private void CloseInstructions_ButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
