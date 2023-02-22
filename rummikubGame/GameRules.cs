using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace rummikubGame
{
    public partial class GameRules : Form
    {
        // rtf file path
        public static string RTF_INSTRUCTIONS_PATH = "InstructionsAssets/instructions_rtf.rtf";

        public GameRules()
        {
            InitializeComponent();
        }

        private void GameRules_Load(object sender, EventArgs e)
        {
            // Set the background color of the form to the color of the header
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");

            // sets design of the close button
            close_instructions_btn.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            close_instructions_btn.BackColor = System.Drawing.ColorTranslator.FromHtml("#383B9A");
            close_instructions_btn.FlatStyle = FlatStyle.Flat;
            close_instructions_btn.FlatAppearance.BorderSize = 0;

            // changing groupbox color
            instructions_groupbox.BackColor = System.Drawing.ColorTranslator.FromHtml("#454691");

            // Define the RTF format for the instructions.
            instructions_rtf.BackColor = System.Drawing.ColorTranslator.FromHtml("#454691");
            instructions_rtf.ReadOnly = true;
            instructions_rtf.BorderStyle = BorderStyle.None;
            instructions_rtf.GotFocus += new EventHandler(instructions_rtf_GotFocus);

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

        private void instructions_rtf_GotFocus(object sender, EventArgs e)
        {
            close_instructions_btn.Focus();
        }

        private void close_instructions_btn_Click(object sender, EventArgs e)
        {
            // close the current form
            this.Close();
        }
    }
}
