﻿using Aspose.Words;
using Aspose.Words.Tables;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
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
        String ageValidation = "^[1-9][0-9]$";

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
            if (!Regex.IsMatch(textBox7.Text.Trim(), dateValidation))
            {
                MessageBox.Show("Μη έγκυρη ημερομηνία λήξης προγράμματος.");
                return;
            }
            if (!Regex.IsMatch(textBox5.Text.Trim(), ageValidation))
            {
                MessageBox.Show("Μη έγκυρη ηλικία συνδρομητή/ριας.");
                return;
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
                wordDocument.PageSetup.LeftMargin = (float)3.5;
                wordDocument.PageSetup.RightMargin = (float)3.5;

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
                    footerRange.Font.ColorIndex = WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 9;
                    footerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = ":)";
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
                //int idx = 0;
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
                    //idx++;
                }

                //adding text to document  
                //wordDocument.Content.SetRange(0, 0);
                //wordDocument.Content.Text = Environment.NewLine;

                // create tables for each program
                for (int i = 0; i < days; i++)
                {
                    // programmata
                    TableOfProgram tbl = tablesOfProgram[i]; // the whole table with exercises

                    Microsoft.Office.Interop.Word.Paragraph par = wordDocument.Content.Paragraphs.Add(ref missing);
                    Object styleHeading1 = WdBuiltinStyle.wdStyleHeading1;
                    par.Range.set_Style(ref styleHeading1);
                    par.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustifyMed;
                    par.Range.Text = Environment.NewLine + "Πρόγραμμα " + (i + 1) + ": " + CategoryProcess.categoryEnumToGreek(tablesOfProgram[i].Category);
                    //categoryEnumToGreek(tablesOfProgram[i].Category); // tablesOfProgram[i].Category.ToString().Replace.... + Environment.NewLine;
                    par.Range.InsertParagraphAfter();

                    // create table
                    //Create a table and insert some dummy records (rows: +1 for headers and +3 for aerobic/stretching before and after)
                    Microsoft.Office.Interop.Word.Table table = wordDocument.Tables.Add(par.Range, tbl.Exercises.Count + 1 + 3, Enum.GetValues(typeof(ColumnEnum)).Length, ref missing, ref missing);
                    table.Borders.Enable = 1;
                    Microsoft.Office.Interop.Word.ParagraphFormat pf = table.Range.ParagraphFormat;
                    pf.KeepWithNext = -1;
                    pf.KeepTogether = -1;

                    //table.AutoFitBehavior(Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow); //.wdAutoFitWindow
                    //  table.PreferredWidthType = Microsoft.Office.Interop.Word.WdPreferredWidthType.wdPreferredWidthPercent; // wordDocument.Sections[0].PageSetup.PageWidth - 20;
                    //firstTable.PreferredWidth = 30;
                    String[] exercisesToRowVector = tbl.listToRowVector();
                    int counter = 0;
                    int idxRow = 0;
                    foreach (Microsoft.Office.Interop.Word.Row row in table.Rows)
                    {
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
                                //   cell.Column.AutoFit();

                                //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                              
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
                                //else cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
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
                                //else cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
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
                                //else cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
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
                                //else cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
                                cell.Range.Font.Size = 9;
                                //cell.Column.AutoFit();
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
                //string pathToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  TODO: this doesn't work
                //object filename = @"c:\temp1.docx";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), textBox1.Text.Trim() + ".docx");

               // object filename = @"C:\Users\areti\Desktop\testDoc.doc";
                //object filename = pathToDesktop + @"\testDoc.docx";
                //wordDocument.Save();
                wordDocument.SaveAs(filePath);

                // closing word doc
                wordDocument.Close(ref missing, ref missing, ref missing);
                wordDocument = null;
                //wordDocument.Close(false);
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
