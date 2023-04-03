using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateDocument
{
    public partial class ExtractProgramForm : Form
    {
        Regex rgx = new Regex(@"^[a-zA-Z0-9]{1, 50}$");
        //String rgx = "^[a-zA-Z0-9]$";

        public ExtractProgramForm()
        {
            InitializeComponent();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if(!System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "^[a-zA-Z0-9]"))
            {
                errorProvider1.SetError(textBox1, "Μη έγκυρο όνομα.");
                MessageBox.Show("invalid");
            }
            else
            {
                errorProvider1.SetError(textBox1, null);
                MessageBox.Show("tf?");
            }
        }
        // todo: fix validation

    }
}
