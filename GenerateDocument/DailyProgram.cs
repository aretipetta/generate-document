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
                for (int j = 0; j < 8; j++)
                {
                    DataGridViewColumn dataGridViewColumn = new DataGridViewColumn();
                    dataGridViewColumn.CellTemplate = new DataGridViewTextBoxCell();
                    dataGridViewColumn.ReadOnly = true;
                }
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

                dataGridView.DataSource = tablesOfProgram[i].Exercises;  // panta auto tha exei gia source
                dataGridViews.Add(dataGridView);
                this.Controls.Add(dataGridView);
            }
        }

        // prev day
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            dataGridViews[counter].Visible = false;
            counter--;
            label1.Text = "Μέρα " + (counter + 1) + ": " + CategoryProcess.categoryEnumToGreek(categoryPerDay[counter]); //categoryPerDay[counter];
            dataGridViews[counter].Visible = true;
            if (counter == 0) pictureBox1.Visible = false;
            if (counter == days - 2) pictureBox2.Visible = true;
            // elegxos gia to button2.enabled ==> add new exercise
            this.button2.Enabled = !(tablesOfProgram[counter].Exercises.Count == 15);
        }

        // next day
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            dataGridViews[counter].Visible = false;
            counter++;
            label1.Text = "Μέρα " + (counter + 1) + ": " + CategoryProcess.categoryEnumToGreek(categoryPerDay[counter]); //categoryPerDay[counter];
            dataGridViews[counter].Visible = true;
            if (counter + 1 == days) pictureBox2.Visible = false;
            if (counter == 1) pictureBox1.Visible = true;
            // elegxos gia to button2.enabled ==> add new exercise
            this.button2.Enabled = !(tablesOfProgram[counter].Exercises.Count == 15);
        }

        // add exercise
        private void button2_Click(object sender, EventArgs e)
        {
            // open form2
            this.Enabled = false;
            Form2 form2 = new Form2(this, categoryPerDay[counter], counter);
            form2.Show();
        }

        private void DailyProgram_Load(object sender, EventArgs e)
        {
            counter = 0;
            label1.Text = "Μέρα 1: " + CategoryProcess.categoryEnumToGreek(categoryPerDay[counter]); //categoryPerDay[counter];
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            if (days > 1) pictureBox2.Visible = true;
            dataGridViews[counter].Visible = true;
        }

        // eksagwgh programmatos
        private void button3_Click(object sender, EventArgs e)
        {
            // elegxos oti oloi oi pinakes exoune megethos megalutero apo 1 kai mikrotero apo 16
            foreach(TableOfProgram tp in tablesOfProgram)
            {
                if (tp.Exercises.Count < 1 || tp.Exercises.Count > 15)
                {
                    MessageBox.Show("Οι πίνακες πρέπει να έχουν τουλάχιστον 1 εγγραφή και το πολύ 17.");
                    return;
                }
            }

            // metavash se allh forma me olous tous pinakes gia epivevaiwsh (?)
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
            // prosthetei ena row sto datagridView ths day meras
            // arxika prepei na valei sth lista to record kai meta isws na ta fortwnei ola mazi apo thn arxh
            //TableOfProgram tableOfProgram = tablesOfProgram[counter];
            tablesOfProgram[counter].addExercise(muscleGroup, description, equipment, set, reps, rest, notes);
            dataGridViews[counter].Height += 20;

            // telos elegxos oti den exei valei hdh 15 askiseis (15 einai to max) 
            if (tablesOfProgram[counter].getTablesSize() == 17) // tablesOfProgram[counter].Excercises.Count == 15
            {
                this.button2.Enabled = false;
                MessageBox.Show("Φτάσατε το μέγιστο όριο ασκήσεων που μπο΄ρείτε να προσθέσετε σε ένα πρόγραμμα.");
            }
        }

    }
}
