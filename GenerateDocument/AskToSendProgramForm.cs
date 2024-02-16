using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateDocument
{
    public partial class AskToSendProgramForm : Form
    {
        public AskToSendProgramForm()
        {
            InitializeComponent();
        }

        /**
         * Skip sending the program
         */
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Η αποστολή προγράμματος θα παραλειφθεί, όμως το αρχείο έχει αποθηκευτεί στην επιφάνεια εργασίας.");
            this.Close();
        }
    }
}
