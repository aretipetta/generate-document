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
    public partial class Form1 : Form
    {
        List<Panel> panels;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = (int)numericUpDown1.Value; // days
            for(int i = 0; i < n; i++)
            {
                foreach (Control c in panels[i].Controls)
                {
                    if (c is ComboBox comboBox)
                    {
                        if (((ComboBox)c).SelectedIndex == -1)
                        {
                            MessageBox.Show("All fields are required.");
                            return;
                        }
                    }
                }
            }

            List<CategoryEnum> categoryPerDay = new List<CategoryEnum>();  // list of program per day
            for (int i = 0; i < n; i++)
            {
                // pairnei tis times apo ta comboBoxes gia th lista pou tha steilei sthn next form
                foreach (Control c in panels[i].Controls)
                {
                    if (c is ComboBox comboBox) categoryPerDay.Add((CategoryEnum)((ComboBox)c).SelectedItem);
                }
            }
            // go to daily program
            DailyProgram dailyProgram = new DailyProgram(this, n, categoryPerDay);
            dailyProgram.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setControls();
        }

        public void setControls()
        {
            // prwta ola einai invisible
            panels = new List<Panel>();
            numericUpDown1.Value = 1;
            List<Panel> temp = new List<Panel>();
            foreach (Control c in this.Controls)
            {
                if (c is Panel panel)
                {
                    foreach (Control c2 in panel.Controls)
                    {
                        if (c2 is ComboBox comboBox)
                        {
                            comboBox.Items.Clear();
                            comboBox.Items.AddRange(new object[] { CategoryEnum.UPPER_BODY, CategoryEnum.LEGS, CategoryEnum.MIX });
                        }
                    }
                    temp.Add(panel);
                }
            }

            temp.ForEach(p => p.Visible = false);
            panels = temp.OrderBy(p => p.Name).ToList();
            button1.Visible = false;
        }

        // ok button
        private void button2_Click(object sender, EventArgs e)
        {
            // kanei visible ta n prwta panels
            int n = (int)numericUpDown1.Value;
            for (int i = 0; i < n; i++) {
                panels[i].Visible = true;
            }
            button1.Visible = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // opote allazei kanei invisible ola ta alla
            button1.Visible = false;
            panels.ForEach(p => p.Visible = false);
        }
    }
}
