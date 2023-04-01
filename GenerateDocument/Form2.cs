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

        public Form2(DailyProgram dailyProgram, CategoryEnum categoryEnum)
        {
            InitializeComponent();
            this.dailyProgram = dailyProgram;
            this.categoryEnum = categoryEnum;
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
            clearControls();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // emfanizei ta controls kai auto ginetai invisible
            // tha emfanistei ksana otan prostethei h askhsh sto daily
            panel3.Visible = true;
            button2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // prosthetei to programma sth lista kai kanei clear ola ta fields tou panel2
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // edw kanei clear ta controls tou panel3 kai disable olo to panel3, enable to add new exercise
            panel3.Visible = false;
            button2.Visible = true;
            foreach(Control c in panel3.Controls)
            {
                if (c is ComboBox) ((ComboBox)c).Items.Clear();
                else if (c is NumericUpDown) ((NumericUpDown)c).Value = 1;
                else if (c is RichTextBox) ((RichTextBox)c).Clear();
            }
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
            comboBox3.Items.Clear();
            numericUpDown1.Value = 1;
            numericUpDown2.Value = 1;
            numericUpDown3.Value = 1;
            richTextBox1.Clear();
        }

    }
}
