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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // otan epileksei muscle group tote ta alla katharizoun kai ginontai ksana enabled
            if(comboBox1.SelectedIndex != -1)
            {
                clearControls();
                comboBox2.Items.AddRange(new object[] { "ενα", "δύο", "τρία" });
                comboBox2.Enabled = true;
            }
            else
            {
                clearControls();
            }
        }

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
            if(((int)categoryEnum) == 1)
            {
                // 1 = upper body
                comboBox1.Items.AddRange(new object[] { "τσίτος", "πλάτη", "δικέφαλοι", "τρικέφαλοι", "κοιλιακοί", "ραχιαίοι", "full" });
            }
            else
            {
                // legs
                comboBox1.Items.AddRange(new object[] { "πόδια", "ώμοι", "κοιλιακοί", "ραχιαίοι", "full" });
            }
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            dailyProgram.Enabled = true;
            base.OnFormClosing(e);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // an index > 0 ==> enable comboBox3
            if(comboBox2.SelectedIndex != -1)
            {
                comboBox3.Items.Clear();
                comboBox3.Items.AddRange(new object[] { "one", "two", "three" });
                comboBox3.Enabled = true;
            }
            else
            {
                clearControls();
            }
        }
    }
}
