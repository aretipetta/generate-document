using GenerateDocument.WebDataConnector;
using GenerateDocument.WebDataConnector.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
namespace GenerateDocument
{
    public partial class Form2 : Form
    {

        DailyProgram dailyProgram;
        CategoryEnum categoryEnum;
        List<ExerciseRecordFBDB> exercisesByBodyCategory;
        int day;

        public Form2(DailyProgram dailyProgram, CategoryEnum categoryEnum, int day)
        {
            InitializeComponent();
            this.dailyProgram = dailyProgram;
            this.categoryEnum = categoryEnum;
            this.day = day;
        }


        // cancel ==> return to dailyProgram
        private void button1_Click(object sender, EventArgs e)
        {
            dailyProgram.Enabled = true;
            this.Close();
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            // set values to comboBoxes
            clearControls();
            // Add muscle groups to combobox1 based on the selected bodycategory
            ConfigFirebase config = new ConfigFirebase();
            CustomResponseFromFBDB resp = config.selectMuscleGroupsByBodyCategory(categoryEnum.ToString());

            if(!resp.OK)
            {
                MessageBox.Show("Οι ασκήσεις δεν βρέθηκαν. Σφάλμα: " + resp.ResponseBody.ToString());
                return;
            }
            initListOfExercisesByCategory((List<ExerciseRecordFBDB>)resp.ResponseBody);
            List<String> muscleGroups = getMuscleGroupsFromExercises();
            comboBox1.Items.AddRange(muscleGroups.ToArray());
        }

        /**
         * Init list only once so we will have no missmatches
         */
        private void initListOfExercisesByCategory(List<ExerciseRecordFBDB> resultList)
        {
            exercisesByBodyCategory = resultList;
        }

        /**
         * Get distinct muscle groups from exercisesByBodyCategory list
         */
        private List<String> getMuscleGroupsFromExercises()
        {
            return exercisesByBodyCategory.ConvertAll<String>(ex => ex.MuscleGroup).Distinct().ToList();
        }


        // reset comboBoxes -set proper values- when comboBox1 changes value
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if user selects muscle group then clear comboBoxes and set them as enabled again
            clearControls();
            if (comboBox1.SelectedIndex != -1)
            {
                // comboBox2 values (descriptions) are related to the selected muscle group
                // so set the proper values based on the selected muscle group
                comboBox2.Items.AddRange(getDescriptionsByMuscleGroup(comboBox1.SelectedItem.ToString()).ToArray());
                comboBox2.Enabled = true;
            }
        }

        /**
         * Filter 'exercisesByBodyCategory' list to get the descriptions related to muscle group
         */
        private List<String> getDescriptionsByMuscleGroup(String muscleGroup)
        {
            // find those records that match to muscleGroup and get only the description (ExerciseName)
            return exercisesByBodyCategory
                .FindAll(ex => ex.MuscleGroup.Equals(muscleGroup))
                .ConvertAll<String>(e => e.ExerciseName).ToList();
        }

        // reset controls -set proper values- when user selects description from comboBox2
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if index > 0 ==> enable comboBox3
            if (comboBox2.SelectedIndex != -1)
            {
                comboBox3.Items.Clear();

                // Get equipement from db
                ConfigFirebase config = new ConfigFirebase();
                CustomResponseFromFBDB resp = config.getEquipementsFromDB();
                if (!resp.OK)
                {
                    MessageBox.Show("Σφάλμα: " + resp.ResponseBody);
                    return;
                }
                comboBox3.Items.AddRange(((List<String>)resp.ResponseBody).ToArray());
                comboBox3.Enabled = true;
            }
            else
            {
                clearControls();
            }
        }

        // Αdd excercise
        /**
         * Add new exercise to the table (for the document)
         */
        private void button4_Click(object sender, EventArgs e)
        {
            // validation: all fields are required (except the first one)
            if(!allFieldsAreRequired())
            {
                MessageBox.Show("Τα πεδία 'Μυική Μάζα', 'Περιγραφή' και 'Εξοπλισμός' είναι υποχρεωτικά.");
                return;
            } 

            // save exercise to the list and display it on gridView (on the form)
            dailyProgram.addExerciseToTable(comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString(),
                (int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value, richTextBox1.Text.Trim());
            // clear all the controls on panel3
            comboBox1.SelectedIndex = -1;
            clearControls();
        }


        // clear all controls to load other items
        private void clearControls()
        {
            comboBox2.Items.Clear();
            comboBox2.Enabled = false;
            comboBox3.Items.Clear();
            comboBox3.Enabled = false;
            numericUpDown1.Value = 1;
            numericUpDown2.Value = 1;
            numericUpDown3.Value = 1;
            richTextBox1.Clear();
        }

        // get the english terminology of a muscle group
        public String termToEn(String termInGreek)
        {
            String termInEnglish = null;
            foreach (TerminologyEnum term in Enum.GetValues(typeof(TerminologyEnum)))
            {
                FieldInfo fi = TerminologyEnum.ABS.GetType().GetField(term.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    if (attributes[0].Description == termInGreek) return term.ToString();
                }
            }
            return termInEnglish;
        }


        public bool allFieldsAreRequired()
        {
            return (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            dailyProgram.Enabled = true;
            base.OnFormClosing(e);
        }

        
    }
}
