using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GenerateDocument
{
    public partial class Form2 : Form
    {

        DailyProgram dailyProgram;
        CategoryEnum categoryEnum;
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
            // apla gemizei ta combobox me ta swsta
            clearControls();
            loadItems();
        }

        // an allaksei h timh sto 1o comboBox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // otan epileksei muscle group tote ta alla katharizoun kai ginontai ksana enabled
            clearControls();
            if (comboBox1.SelectedIndex != -1)
            {
                // pairnei to item tou comboBox1 kai psaxnei ta antistoixa apo to App.config
                String muscleGroup = toEn(comboBox1.SelectedItem.ToString());
                List<String> items = new List<string>(ConfigurationManager.AppSettings[muscleGroup].Split(';'));
                //comboBox1.Items.AddRange(items.ToArray());
                comboBox2.Items.AddRange(items.ToArray());
                comboBox2.Enabled = true;
            }
        }

        // an allaksei h timh sto 2o comboBox
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // an index > 0 ==> enable comboBox3
            if (comboBox2.SelectedIndex != -1)
            {
                comboBox3.Items.Clear();
                List<String> items = new List<string>(ConfigurationManager.AppSettings[ColumnEnum.EQUIPEMENT.ToString()].Split(';'));
                comboBox3.Items.AddRange(items.ToArray());
                comboBox3.Enabled = true;
            }
            else
            {
                clearControls();
            }
        }


        // add excercise
        private void button4_Click(object sender, EventArgs e)
        {
            // prwta apothikeush tou exercise sth lista kai emfanish sto gridView tis pisw form
            dailyProgram.addExerciseToTable(comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString(),
                (int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value, richTextBox1.Text);
            // edw kanei clear ta controls tou panel3
            clearControls();
        }

        // load items to comboBoxes
        private void loadItems()
        {
            List<String> items = new List<string>(ConfigurationManager.AppSettings[categoryEnum.ToString()].Split(';'));
            comboBox1.Items.AddRange(items.ToArray());
        }


        // clears all controls to load other items
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

            //foreach (Control c in panel3.Controls)
            //{
            //    if (c is ComboBox) ((ComboBox)c).Items.Clear();
            //    else if (c is NumericUpDown) ((NumericUpDown)c).Value = 1;
            //    else if (c is RichTextBox) ((RichTextBox)c).Clear();
            //}

        }

        // get the english terminology for a muscle group
        public String toEn(String termInGreek)
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


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            dailyProgram.Enabled = true;
            base.OnFormClosing(e);
        }

        
    }
}
