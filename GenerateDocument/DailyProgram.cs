using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GenerateDocument
{
    public partial class DailyProgram : Form
    {
        int counter, days;
        Form1 form1;
        List<CategoryEnum> categoryPerDay;
        List<TableOfProgram> tablesOfProgram;
        List<DataGridView> dataGridViews;

        public DailyProgram(Form1 form1, int days, List<CategoryEnum> categoryPerDay)
        {
            InitializeComponent();
            this.form1 = form1;
            this.days = days;
            this.categoryPerDay = new List<CategoryEnum>();
            this.categoryPerDay = categoryPerDay;
            // init TablesOfprogram and dataGridViews. dataGridViews are invisible at first
            initDataGridViews();
        }

        private void initDataGridViews()
        {
            tablesOfProgram = new List<TableOfProgram>();
            dataGridViews = new List<DataGridView>();
            for (int i = 0; i < days; i++)
            {
                tablesOfProgram.Add(new TableOfProgram(categoryPerDay[i]));
                DataGridView dataGridView = new DataGridView();
                /* for (int j = 0; j < 8; j++)
                 {
                     DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();
                     dataGridViewColumn.CellTemplate = new DataGridViewTextBoxCell();
                     dataGridViewColumn.ReadOnly = true;
                 }*/

                // position and size of dataGridView
                dataGridView.Size = new Size(this.Width - 50, 50);
                dataGridView.Location = new Point(10, 200);
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.Visible = false;
                dataGridView.ReadOnly = true;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToResizeRows = false;
                dataGridView.AllowUserToResizeColumns = false;
                dataGridView.AllowUserToOrderColumns = false;
                dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                dataGridView.DataSource = tablesOfProgram[i].Exercises;  // this will be the dataSource

                // add delete button column
                DataGridViewButtonColumn deleteBtn = new DataGridViewButtonColumn();
                deleteBtn.Name = "dataGridViewDeleteButton" + i.ToString();
                deleteBtn.HeaderText = "Remove";
                deleteBtn.Text = "Αφαίρεση";
                deleteBtn.UseColumnTextForButtonValue = true;
                dataGridView.Columns.Add(deleteBtn);
                dataGridView.CellContentClick += removeRowFromGrid;

                dataGridViews.Add(dataGridView);
                this.Controls.Add(dataGridView);
            }
        }

        /**
         * Remove a row from the table
         */
        public void removeRowFromGrid(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGrid = (DataGridView)sender;
            if(dataGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                // remove the element from the list at the current position
                tablesOfProgram[counter].Exercises.RemoveAt(e.ColumnIndex);
                // and resize the height of the dataGridView
                dataGridViews[counter].Height -= 20;
            }
        }
        

        /**
         * Go to the exact previous day program, if it exists or else disable controls and enable others
         */
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            dataGridViews[counter].Visible = false;
            counter--;
            label1.Text = "Μέρα " + (counter + 1) + ": " + CategoryProcess.categoryEnumToGreek(categoryPerDay[counter]);
            dataGridViews[counter].Visible = true;
            if (counter == 0) pictureBox1.Visible = false;
            if (counter == days - 2) pictureBox2.Visible = true;
            this.button2.Enabled = !(tablesOfProgram[counter].Exercises.Count == 15);
        }

        /**
        * Go to the exact next day program, if it exists or else disable controls and enable others
        */
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            dataGridViews[counter].Visible = false;
            counter++;
            label1.Text = "Μέρα " + (counter + 1) + ": " + CategoryProcess.categoryEnumToGreek(categoryPerDay[counter]);
            dataGridViews[counter].Visible = true;
            if (counter + 1 == days) pictureBox2.Visible = false;
            if (counter == 1) pictureBox1.Visible = true;
            this.button2.Enabled = !(tablesOfProgram[counter].Exercises.Count == 15);
        }

        /**
         * Add new exercise to daily program
         */
        private void button2_Click(object sender, EventArgs e)
        {
            // Go to form2
            this.Enabled = false;
            Form2 form2 = new Form2(this, categoryPerDay[counter], counter);
            form2.Show();
        }

        private void DailyProgram_Load(object sender, EventArgs e)
        {
            counter = 0;
            label1.Text = "Μέρα 1: " + CategoryProcess.categoryEnumToGreek(categoryPerDay[counter]);
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            if (days > 1) pictureBox2.Visible = true;
            dataGridViews[counter].Visible = true;
        }

        /**
         * Extract program
         */
        private void button3_Click(object sender, EventArgs e)
        {
            // elegxos oti oloi oi pinakes exoune megethos megalutero apo 1 kai mikrotero apo 16
            // verify that all the tables-lists have length > 1 and < 16 before we extract any program
            foreach(TableOfProgram tp in tablesOfProgram)
            {
                if (tp.Exercises.Count < 1 || tp.Exercises.Count > 15)
                {
                    MessageBox.Show("Οι πίνακες πρέπει να έχουν τουλάχιστον 1 εγγραφή και το πολύ 17.");
                    return;
                }
            }

            ExtractProgramForm extractProgramForm = new ExtractProgramForm(this, days, tablesOfProgram);
            this.Enabled = false;
            extractProgramForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // exit button
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // go back
            form1.setControls();
            form1.Show();
            this.Close();
        }

        public void addExerciseToTable(String muscleGroup, String description, String equipment, int set, int reps, int rest, String notes)
        {
            // Add a new row to dataGridView for a specific day
            // Append record to list so it can be displayed on gridView
            tablesOfProgram[counter].addExercise(muscleGroup, description, equipment, set, reps, rest, notes);
            dataGridViews[counter].Height += 20;

            // max rexords per daily program == 16
            if (tablesOfProgram[counter].getTablesSize() == 17)
            {
                this.button2.Enabled = false;
                MessageBox.Show("Φτάσατε το μέγιστο όριο ασκήσεων που μπο΄ρείτε να προσθέσετε σε ένα πρόγραμμα.");
            }
        }

    }
}
