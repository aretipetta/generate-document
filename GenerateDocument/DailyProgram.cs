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
            label1.Text = "Μέρα " + (counter + 1) + ": " + categoryPerDay[counter];
            dataGridViews[counter].Visible = true;
            if (counter == 0) pictureBox1.Visible = false;
            if (counter == days - 2) pictureBox2.Visible = true;
        }

        // next day
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            dataGridViews[counter].Visible = false;
            counter++;
            label1.Text = "Μέρα " + (counter + 1) + ": " + categoryPerDay[counter];
            dataGridViews[counter].Visible = true;
            if (counter + 1 == days) pictureBox2.Visible = false;
            if (counter == 1) pictureBox1.Visible = true;
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
            label1.Text = "Μέρα 1: " + categoryPerDay[counter];
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            if (days > 1) pictureBox2.Visible = true;
            dataGridViews[counter].Visible = true;
        }

        public void addExerciseToTable(String muscleGroup, String description, String equipment, int set, int reps, int rest, String notes)
        {
            // prosthetei ena row sto datagridView ths day meras
            // arxika prepei na valei sth lista to record kai meta isws na ta fortwnei ola mazi apo thn arxh
            //TableOfProgram tableOfProgram = tablesOfProgram[counter];
            tablesOfProgram[counter].addExercise(muscleGroup, description, equipment, set, reps, rest, notes);
            dataGridViews[counter].Height += 20;
        }

    }
}
