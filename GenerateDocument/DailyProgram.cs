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
    public partial class DailyProgram : Form
    {
        int counter, days;
        Form1 form1;
        List<CategoryEnum> categoryPerDay;

        public DailyProgram(Form1 form1, int days, List<CategoryEnum> categoryPerDay)
        {
            InitializeComponent();
            this.form1 = form1;
            this.days = days;
            this.categoryPerDay = new List<CategoryEnum>();
            this.categoryPerDay = categoryPerDay;
        }

        // prev day
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            counter--;
            label1.Text = "Μέρα " + (counter + 1) + ": " + categoryPerDay[counter]; ;
            if (counter == 0) pictureBox1.Visible = false;
            if (counter == days - 2) pictureBox2.Visible = true;
        }

        // next day
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            counter++;
            label1.Text = "Μέρα " + (counter + 1) + ": " + categoryPerDay[counter]; ;
            if (counter + 1 == days) pictureBox2.Visible = false;
            if (counter == 1) pictureBox1.Visible = true;
        }

        // add exercise
        private void button2_Click(object sender, EventArgs e)
        {
            // open form2
            this.Enabled = false;
            Form2 form2 = new Form2(this, categoryPerDay[counter]);
            form2.Show();
        }

        private void DailyProgram_Load(object sender, EventArgs e)
        {
            counter = 0;
            label1.Text = "Μέρα 1: " + categoryPerDay[counter];
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            if (days > 1) pictureBox2.Visible = true;
        }



    }
}
