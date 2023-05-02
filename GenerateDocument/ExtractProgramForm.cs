using Aspose.Words;
using Aspose.Words.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerateDocument
{
    public partial class ExtractProgramForm : Form
    {
        Regex rgx = new Regex(@"^[a-zA-Z0-9]{1, 50}$");
        //String rgx = "^[a-zA-Z0-9]$";

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

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if(!System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "^[a-zA-Z0-9]"))
            {
                errorProvider1.SetError(textBox1, "Μη έγκυρο όνομα.");
                MessageBox.Show("invalid");
            }
            else
            {
                errorProvider1.SetError(textBox1, null);
                MessageBox.Show("einai ok");
            }
        }
        // todo: fix validation


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            dailyProgram.Enabled = true;
            base.OnFormClosing(e);
        }

        // dhmiourgia programmatos kai extract .docx file
        private void button1_Click(object sender, EventArgs e)
        {
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
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Document wordDocument = wordApp.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                wordDocument.PageSetup.TopMargin = (float)0.5;
                wordDocument.PageSetup.BottomMargin = (float)0.5;
                wordDocument.PageSetup.LeftMargin = (float)3.5;
                wordDocument.PageSetup.RightMargin = (float)3.5;

                foreach (Microsoft.Office.Interop.Word.Section section in wordDocument.Sections)
                {
                    //Get the header range and add the header details.
                    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                    headerRange.Font.Size = 20;
                    headerRange.Text = "ΠΡΟΓΡΑΜΜΑ";
                    //Get the footer range and add the footer details.
                    Microsoft.Office.Interop.Word.Range footerRange = section.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 9;
                    footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = "Program";
                }

                //adding text to document  
                wordDocument.Content.SetRange(0, 0);
                wordDocument.Content.Text = Environment.NewLine;

                // create tables for each program
                for (int i = 0; i < days; i++)
                {
                    TableOfProgram tbl = tablesOfProgram[i]; // the whole table with exercises

                    Microsoft.Office.Interop.Word.Paragraph par = wordDocument.Content.Paragraphs.Add(ref missing);
                    Object styleHeading1 = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading1;
                    par.Range.set_Style(ref styleHeading1);
                    par.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustifyMed;
                    par.Range.Text = Environment.NewLine + "Πρόγραμμα " + (i + 1) + ": " + tablesOfProgram[i].Category + Environment.NewLine;
                    par.Range.InsertParagraphAfter();

                    // create table
                    //Create a table and insert some dummy records (rows: +1 for headers and +3 for aerobic/stretching before and after)
                    Microsoft.Office.Interop.Word.Table table = wordDocument.Tables.Add(par.Range, tbl.Exercises.Count + 1 + 3, Enum.GetValues(typeof(ColumnEnum)).Length, ref missing, ref missing);
                    table.Borders.Enable = 1;
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
                                cell.Range.Text = ((ColumnEnum)cell.ColumnIndex).ToString();
                                cell.Range.Font.Bold = 1;
                              //  cell.Range.Font.Size = 8;
                                //other format properties goes here  
                                //cell.Range.Font.Name = "verdana";
                                //   cell.Column.AutoFit();

                                //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                              
                                cell.Shading.BackgroundPatternColor = Microsoft.Office.Interop.Word.WdColor.wdColorYellow;
                                //Center alignment for the Header cells  
                                cell.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                cell.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                            }
                        }
                        else if(row.Index == 2) // prothermansi
                        {
                            List<String> items = new List<string>(ConfigurationManager.AppSettings["WARM-UP"].Split(';'));
                            int j = 0;
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                if (cell.ColumnIndex > 3 && cell.ColumnIndex < 7) cell.Column.AutoFit();
                                //else cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
                                cell.Range.Font.Size = 9;
                                cell.Range.Text = items[j];
                                j++;
                            }
                        }
                        else if(row.Index == tbl.Exercises.Count + 3)
                        {
                            List<String> items = new List<string>(ConfigurationManager.AppSettings["AEROBIC"].Split(';'));
                            int j = 0;
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                if (cell.ColumnIndex > 3 && cell.ColumnIndex < 7) cell.Column.AutoFit();
                                //else cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
                                cell.Range.Font.Size = 9;
                                cell.Range.Text = items[j];
                                j++;
                            }
                        }
                        else if(row.Index == tbl.Exercises.Count + 4)
                        {
                            List<String> items = new List<string>(ConfigurationManager.AppSettings["STRETCHING"].Split(';'));
                            int j = 0;
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                if (cell.ColumnIndex > 3 && cell.ColumnIndex < 7) cell.Column.AutoFit();
                                //else cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
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
                                
                                cell.Range.Text = exercisesToRowVector[counter];
                                counter++;
                            }
                        }
                        idxRow++;
                    }
                    table.AutoFitBehavior(Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitWindow);
                }

                

                // add to the top new row for aerobic
                //foreach(Table table in wordDocument.Tables)
                //{

                //}





                //Save the document  
                //string pathToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  TODO: this doesn't work
                //object filename = @"c:\temp1.docx";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MyDoc.docx");

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
                button2.PerformClick();
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
