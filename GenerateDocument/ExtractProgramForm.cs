using Aspose.Words;
using Aspose.Words.Tables;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Section = Microsoft.Office.Interop.Word.Section;

namespace GenerateDocument
{
    public partial class ExtractProgramForm : Form
    {
        String docNameValidation = "^[a-zA-Z]+[a-zA-Z0-9]*$";
        String nameValidation = "^[a-zA-Z]+(\\s?[a-zA-Z]+){0,3}$";
        String goalValidation = "^[a-zA-Z]+(\\s?[a-zA-Z]+)*$";
        String dateValidation = "^([0-9]{1,2}/){2}[0-9]{4}$";
        String ageValidation = "^[0-9]{1,3}$";

        DailyProgram dailyProgram;
        int days;
        List<TableOfProgram> tablesOfProgram;

        public ExtractProgramForm(DailyProgram dailyProgram, int days, List<TableOfProgram> tablesOfProgram)
        {
            this.dailyProgram = dailyProgram;
            this.days = days;
            this.tablesOfProgram = tablesOfProgram;
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            dailyProgram.Enabled = true;
            base.OnFormClosing(e);
        }

        // dhmiourgia programmatos kai extract .docx file
        private void button1_Click(object sender, EventArgs e)
        {
            validateInputs();
        }

        protected void validateInputs()
        {
            if(!Regex.IsMatch(textBox2.Text.Trim(), nameValidation))
            {
                MessageBox.Show("Μη έγκυρο όνομα στο πεδίο 'Υπεύθυνος/η γυμναστής/τρια'.");
                return;
            }
            if (!Regex.IsMatch(textBox3.Text.Trim(), nameValidation))
            {
                MessageBox.Show("Μη έγκυρο όνομα στο πεδίο 'Ονοματεπώνυμο συνδρομητή/ριας'.");
                return;
            }
            if (!Regex.IsMatch(textBox4.Text.Trim(), goalValidation))
            {
                MessageBox.Show("Μη έγκυρη εισαγωγή στόχου προγράμματος.");
                return;
            }
            if (!Regex.IsMatch(textBox6.Text.Trim(), dateValidation))
            {
                MessageBox.Show("Μη έγκυρη ημερομηνία έναρξης προγράμματος.");
                return;
            }
            else
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(textBox6.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    int result = DateTime.Compare(DateTime.Today, dt);
                    if(result > 0)
                    {
                        MessageBox.Show("Μη έγκυρη ημερομηνία έναρξης προγράμματος.");
                        return;
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show("Μη έγκυρη ημερομηνία έναρξης προγράμματος.");
                    return;
                }
            }
            if (!Regex.IsMatch(textBox7.Text.Trim(), dateValidation))
            {
                MessageBox.Show("Μη έγκυρη ημερομηνία λήξης προγράμματος.");
                return;
            }
            else
            {
                try
                {
                    DateTime dtS = DateTime.ParseExact(textBox6.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime dtE = DateTime.ParseExact(textBox7.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    int result = DateTime.Compare(dtS, dtE);
                    if (result >= 0)
                    {
                        MessageBox.Show("Μη έγκυρη ημερομηνία λήξης προγράμματος.");
                        return;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Μη έγκυρη ημερομηνία λήξης προγράμματος.");
                    return;
                }
            }
            if (!Regex.IsMatch(textBox5.Text.Trim(), ageValidation))
            {
                MessageBox.Show("Μη έγκυρη ηλικία συνδρομητή/ριας.");
                return;
            }
            else
            {
                int age;
                if (!int.TryParse(textBox5.Text.Trim(), out age))
                {
                    MessageBox.Show("Μη έγκυρη ηλικία συνδρομητή.");
                    return;
                }
                else
                {
                    if(age < 5 && age > 120)
                    {
                        MessageBox.Show("Μη έγκυρη ηλικία συνδρομητή.");
                        return;
                    }
                }
            }
            if(!Regex.IsMatch(textBox1.Text.Trim(), docNameValidation))
            {
                MessageBox.Show("Μη έγκυρο όνομα αρχείου.");
                return;
            }
            this.Enabled = false;
            createDoc();
        }

        public void createDoc()
        {
            try
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.ShowAnimation = false;
                wordApp.Visible = false;
                object missing = Missing.Value;
                Microsoft.Office.Interop.Word.Document wordDocument = wordApp.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                wordDocument.PageSetup.TopMargin = (float)0.5;
                wordDocument.PageSetup.BottomMargin = (float)0.5;
                wordDocument.PageSetup.LeftMargin = (float)4.5;
                wordDocument.PageSetup.RightMargin = (float)4.5;

                foreach (Section section in wordDocument.Sections)
                {
                    //Get the header range and add the header details.
                    //Microsoft.Office.Interop.Word.Range headerRange = section.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    //headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage);
                    //headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    //headerRange.Font.ColorIndex = WdColorIndex.wdBlack;
                    //headerRange.Font.Size = 9;
                    //headerRange.Text = "ALTERLIFE - Εθ. Αντιστάσεως 173, Δραπετσώνα 186 48";

                    //Get the footer range and add the footer details.
                    Microsoft.Office.Interop.Word.Range footerRange = section.Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = WdColorIndex.wdGray50;
                    footerRange.Font.Size = 9;
                    footerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = ":')";
                }

                // add image
                Microsoft.Office.Interop.Word.Paragraph pp = wordDocument.Content.Paragraphs.Add(ref missing);
                Object styleH = WdBuiltinStyle.wdStyleHeading1;
                pp.Range.set_Style(ref styleH);
                pp.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustifyMed;
                string filePathToImage = Path.Combine(Environment.CurrentDirectory, "ALTERLIFE.PNG");
                pp.Range.InlineShapes.AddPicture(filePathToImage);

                // details
                // add table with extra details
                Microsoft.Office.Interop.Word.Paragraph p = wordDocument.Content.Paragraphs.Add(ref missing);
                Object styleHeading = WdBuiltinStyle.wdStyleHeading1;
                p.Range.set_Style(ref styleHeading);
                p.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustifyMed;
                p.Range.Text = Environment.NewLine + "Στοιχεία";
                p.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Table t = wordDocument.Tables.Add(p.Range, 3, 4, ref missing, ref missing); // 6 rows for details (names, age etc) and 2 columns (key-value)
                // actually the table will be like 2 3X2 tables side by side
                t.Borders.Enable = 1;
                String[] labels = new string[] { "Υπεύθυνος/η γυμναστής/ρια", "Συνδρομητής/ρια", "Στόχος προγράμματος", "Έναρξη προγράμματος", "Λήξη προγράμματος", "Ηλικία συνδρομητή/ριας" };
                String[] detailsFromForm = new string[] { textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim(), textBox6.Text.Trim(), textBox7.Text.Trim(), textBox5.Text.Trim() };
                int offset = 0;
                foreach (Microsoft.Office.Interop.Word.Row row in t.Rows)
                {
                    foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                    {
                        if (cell.ColumnIndex == 1 || cell.ColumnIndex == 3)
                        {
                            cell.Range.Text = labels[row.Index - 1 + offset];
                            cell.Range.Font.Bold = 1;
                            cell.Shading.BackgroundPatternColor = (cell.RowIndex % 2 == 0) ? WdColor.wdColorLightYellow
                                : WdColor.wdColorYellow;
                            cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        }
                        else if (cell.ColumnIndex == 2 || cell.ColumnIndex == 4)
                        {
                            cell.Range.Text = detailsFromForm[row.Index - 1 + offset];
                            cell.Shading.BackgroundPatternColor = (cell.RowIndex % 2 == 0) ? WdColor.wdColorGray05
                                : WdColor.wdColorGray10;
                        }
                        if(cell.ColumnIndex == 2) offset++;
                        cell.Range.Font.Size = 9;
                    }
                }

                // create tables for each program
                for (int i = 0; i < days; i++)
                {
                    // programmata
                    TableOfProgram tbl = tablesOfProgram[i]; // the whole table with exercises

                    Microsoft.Office.Interop.Word.Paragraph par = wordDocument.Content.Paragraphs.Add(ref missing);
                    object oPageBreak = WdBreakType.wdPageBreak;
                    par.Range.InsertBreak(ref oPageBreak);
                    Object styleHeading1 = WdBuiltinStyle.wdStyleHeading1;
                    par.Range.set_Style(ref styleHeading1);
                    par.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustifyMed;
                    par.Range.ParagraphFormat.PageBreakBefore = 0;
                    par.Range.Text = "Πρόγραμμα " + (i + 1) + ": " + CategoryProcess.categoryEnumToGreek(tablesOfProgram[i].Category);
                    par.Range.InsertParagraphAfter();

                    // create table
                    //Create a table and insert some dummy records (rows: +1 for headers and +3 for aerobic/stretching before and after)
                    Microsoft.Office.Interop.Word.Table table = wordDocument.Tables.Add(par.Range, tbl.Exercises.Count + 1 + 3, Enum.GetValues(typeof(ColumnEnum)).Length, ref missing, ref missing);
                    table.Borders.Enable = 1;

                    String[] exercisesToRowVector = tbl.listToRowVector();
                    int counter = 0;
                    int idxRow = 0;
                    foreach (Microsoft.Office.Interop.Word.Row row in table.Rows)
                    {
                        row.AllowBreakAcrossPages = 0;
                        // header row of table
                        if (row.Index == 1)
                        {
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                if ((cell.ColumnIndex > 3 && cell.ColumnIndex < 7) || cell.ColumnIndex == 1) cell.Column.AutoFit();
                                //else cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
                                cell.Range.Text = ColumnProcess.columnEnumToGreek(cell.ColumnIndex); //columnEnumToGreek(cell.ColumnIndex); //((ColumnEnum)cell.ColumnIndex).ToString();
                                cell.Range.Font.Bold = 1;
                                cell.Range.Font.Size = 9;
                                //other format properties goes here  
                                //cell.Range.Font.Name = "verdana";
                          
                                cell.Shading.BackgroundPatternColor = WdColor.wdColorYellow;
                                //Center alignment for the Header cells  
                                cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            }
                        }
                        else if (row.Index == 2) // prothermansi
                        {
                            List<String> items = new List<string>(ConfigurationManager.AppSettings["WARM-UP"].Split(';'));
                            int j = 0;
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                if (cell.ColumnIndex > 3 && cell.ColumnIndex < 7) cell.Column.AutoFit();
                                cell.Range.Font.Size = 9;
                                cell.Range.Text = items[j];
                                cell.Shading.BackgroundPatternColor = WdColor.wdColorGray10;
                                j++;
                            }
                        }
                        else if (row.Index == tbl.Exercises.Count + 3)
                        {
                            List<String> items = new List<string>(ConfigurationManager.AppSettings["AEROBIC"].Split(';'));
                            int j = 0;
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                if (cell.ColumnIndex > 3 && cell.ColumnIndex < 7) cell.Column.AutoFit();
                                cell.Shading.BackgroundPatternColor = (row.Index % 2 == 0) ? WdColor.wdColorGray10
                                    : WdColor.wdColorGray05;
                                cell.Range.Font.Size = 9;
                                cell.Range.Text = items[j];
                                j++;
                            }
                        }
                        else if (row.Index == tbl.Exercises.Count + 4)
                        {
                            List<String> items = new List<string>(ConfigurationManager.AppSettings["STRETCHING"].Split(';'));
                            int j = 0;
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                if (cell.ColumnIndex > 3 && cell.ColumnIndex < 7) cell.Column.AutoFit();
                                cell.Shading.BackgroundPatternColor = (row.Index % 2 == 0) ? WdColor.wdColorGray10
                                    : WdColor.wdColorGray05;
                                cell.Range.Font.Size = 9;
                                cell.Range.Text = items[j];
                                j++;
                            }
                        }
                        else // data rows
                        {
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                if (cell.ColumnIndex > 3 && cell.ColumnIndex < 7) cell.Column.AutoFit();
                                cell.Range.Font.Size = 9;
                                cell.Shading.BackgroundPatternColor = (row.Index % 2 == 0) ? WdColor.wdColorGray10
                                    : WdColor.wdColorGray05;
                                cell.Range.Text = exercisesToRowVector[counter];
                                counter++;
                            }
                        }
                        idxRow++;
                    }
                    table.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow);
                }


                //Save the document  
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), textBox1.Text.Trim() + ".docx");
                wordDocument.SaveAs(filePath);

                // closing word doc
                wordDocument.Close(ref missing, ref missing, ref missing);
                wordDocument = null;
                wordApp.Quit(ref missing, ref missing, ref missing);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                wordApp = null;

                MessageBox.Show("Document created successfully !");
                // return stin prohgoumenh
                dailyProgram.Enabled = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // cancel button
            dailyProgram.Enabled = true;
            this.Close();
        }
    }
}
