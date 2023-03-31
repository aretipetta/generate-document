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
    public partial class Form2 : Form
    {

        Form1 form1;
        int days;
        int counter;

        public Form2(Form1 form1, int days)
        {
            InitializeComponent();
            this.form1 = form1;
            this.days = days;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            label1.Text = "Μέρα 1";
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            panel3.Visible = false;
            if (days > 1) pictureBox2.Visible = true;
            counter = 0;
        }

        // previous day
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            counter--;
            label1.Text = "Μέρα " + (counter + 1);
            if (counter == 0) pictureBox1.Visible = false;
            if (counter == days - 2) pictureBox2.Visible = true;
        }

        //next day
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            counter++;
            label1.Text = "Μέρα " + (counter + 1);
            if (counter + 1 == days) pictureBox2.Visible = false;
            if (counter == 1) pictureBox1.Visible = true; 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // otan epileksei muscle group tote ta alla katharizoun kai ginontai ksana enabled
            comboBox2.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // emfanizei ta controls kai auto ginetai invisible
            // tha emfanistei ksana otan prostethei h askhsh sto daily
            panel3.Visible = true;
            button2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // prosthetei to programma sth lista kai kanei clear ola ta fields tou panel2
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // edw kanei clear ta controls tou panel3 kai disable olo to panel3, enable to add new exercise
            panel3.Visible = false;
            button2.Visible = true;
            foreach(Control c in panel3.Controls)
            {
                if (c is ComboBox) ((ComboBox)c).Items.Clear();
                else if (c is NumericUpDown) ((NumericUpDown)c).Value = 1;
                else if (c is RichTextBox) ((RichTextBox)c).Clear();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // edw kanei clear ta controls tou panel4 kai disable olo to panel4, enable to add new exercise
        }
    }
}
