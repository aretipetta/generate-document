using GenerateDocument.WebDataConnector;
using GenerateDocument.WebDataConnector.Domain;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GenerateDocument
{
    public partial class InsertRecordToDB : Form
    {
        Form1 form1;
        public InsertRecordToDB(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
        }

        private void InsertRecordToDB_Load(object sender, EventArgs e)
        {
            setControls();
        }

        private void setControls()
        {
            resetControls();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] {
                                CategoryProcess.categoryEnumToGreek(CategoryEnum.UPPER_BODY),
                                CategoryProcess.categoryEnumToGreek(CategoryEnum.LEGS),
                                CategoryProcess.categoryEnumToGreek(CategoryEnum.MIX) });
            List<String> muscleGroups = new List<string>();
            foreach(int mg in Enum.GetValues(typeof(TerminologyEnum)))
            {
                muscleGroups.Add(MuscleGroup.muscleGroupToGreek(mg));
            }
            comboBox2.Items.AddRange(muscleGroups.ToArray());
        }

        private void resetControls()
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox1.Clear();
            checkBox1.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // check that none field is empty and then save the record
            if(!allFieldsAreRequired())
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            CategoryEnum selectedCategory = (CategoryEnum)comboBox1.SelectedIndex + 1;
            String muscleGroup = comboBox2.SelectedItem.ToString();
            String exercise = textBox1.Text.ToString().Trim().ToUpper();
            ConfigFirebase config = new ConfigFirebase();
            if (!checkBox1.Checked)
            {
                // just one record
                CustomResponseFromFBDB resp = config.addNewExerciseToDB(selectedCategory.ToString(), muscleGroup, exercise);
                if (resp.OK)
                {
                    MessageBox.Show("New exercise has been added to database.");
                    resetControls();
                    return;
                }
                MessageBox.Show("Could not insert new record to database.");
                return;
            }
            // add it to all the categories
            CustomResponseFromFBDB resp1 = config.addNewExerciseToDB(CategoryEnum.UPPER_BODY.ToString(), muscleGroup, exercise);
            CustomResponseFromFBDB resp2 = config.addNewExerciseToDB(CategoryEnum.LEGS.ToString(), muscleGroup, exercise);
            CustomResponseFromFBDB resp3 = config.addNewExerciseToDB(CategoryEnum.MIX.ToString(), muscleGroup, exercise);
            if (resp1.OK && resp2.OK && resp3.OK)
            {
                MessageBox.Show("New exercise has been added to database.");
                resetControls();
                return;
            }
        }

        private bool allFieldsAreRequired()
        {
            return comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && !textBox1.Text.ToString().Trim().Equals("");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // go back
            form1.Show();
            this.Close()
;        }
    }
}
