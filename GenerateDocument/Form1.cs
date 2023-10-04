using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        /**
         * Confirm the number of the selected days and the respective categories per day
         */
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
                 // get the values of all the selected comboBoxes in order to send them to the next form
                 foreach (Control c in panels[i].Controls)
                 {
                     if (c is ComboBox comboBox) categoryPerDay.Add((CategoryEnum)((ComboBox)c).SelectedIndex + 1);
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
            // everything is invisible
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
                            comboBox.Items.AddRange(new object[] {
                                CategoryProcess.categoryEnumToGreek(CategoryEnum.UPPER_BODY), 
                                CategoryProcess.categoryEnumToGreek(CategoryEnum.LEGS), 
                                CategoryProcess.categoryEnumToGreek(CategoryEnum.MIX) });
                        }
                    }
                    temp.Add(panel);
                }
            }

            temp.ForEach(p => p.Visible = false);
            panels = temp.OrderBy(p => p.Name).ToList();
            button1.Visible = false;
        }

        /**
         * Button for days selection
         */
        private void button2_Click(object sender, EventArgs e)
        {
            // set visible first 'n' panels
            int n = (int)numericUpDown1.Value;
            for (int i = 0; i < n; i++) {
                panels[i].Visible = true;
            }
            button1.Visible = true;
        }

        /**
         * Whenever the numericUpDown changes, all the other controls are being invisible 
         */
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            button1.Visible = false;
            panels.ForEach(p => p.Visible = false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // add new exercise to db
            InsertRecordToDB insertRecordToDB = new InsertRecordToDB(this);
            insertRecordToDB.Show();
            this.Hide();
        }
    }
}
