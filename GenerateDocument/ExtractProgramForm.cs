using Aspose.Words;
using Aspose.Words.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                wordDocument.PageSetup.LeftMargin = (float)4.5;
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
                    footerRange.Text = "for more send here: 'aretpett@gmail.com'    please, no dp :)";
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
                    par.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    par.Range.Text = Environment.NewLine + "Πρόγραμμα " + (i + 1) + ": " + tablesOfProgram[i].Category + Environment.NewLine;
                    par.Range.InsertParagraphAfter();

                    // create table
                    //Create a table and insert some dummy records
                    Microsoft.Office.Interop.Word.Table firstTable = wordDocument.Tables.Add(par.Range, tbl.Exercises.Count + 1, Enum.GetValues(typeof(ColumnEnum)).Length, ref missing, ref missing);
                    firstTable.Borders.Enable = 1;

                    String[] exercisesToRowVector = tbl.listToRowVector();
                    int counter = 0;
                    int idxRow = 0;
                    foreach (Microsoft.Office.Interop.Word.Row row in firstTable.Rows)
                    {
                        // header row of table
                        if (row.Index == 1)
                        {
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                cell.Range.Text = ((ColumnEnum)cell.ColumnIndex).ToString();
                                cell.Range.Font.Bold = 1;
                                //other format properties goes here  
                                //cell.Range.Font.Name = "verdana";
                                //cell.Range.Font.Size = 8;
                                cell.Column.AutoFit();
                                cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
                                //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                              
                                cell.Shading.BackgroundPatternColor = Microsoft.Office.Interop.Word.WdColor.wdColorGray25;
                                //Center alignment for the Header cells  
                                cell.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                cell.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                            }
                        }
                        else // data rows
                        {
                            MessageBox.Show("Data");
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                cell.Range.Font.Size = 10;
                                cell.Column.AutoFit();
                                cell.SetWidth(cell.Column.Width, Microsoft.Office.Interop.Word.WdRulerStyle.wdAdjustProportional);
                                cell.Range.Text = exercisesToRowVector[counter];
                                counter++;
                            }
                        }
                        idxRow++;
                    }
                }

                //Save the document  
                //string pathToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  TODO: this doesn't work
                //object filename = @"c:\temp1.docx";
                object filename = @"C:\Users\areti\Desktop\testDoc.doc";
                //object filename = pathToDesktop + @"\testDoc.docx";
                //wordDocument.Save();
                wordDocument.SaveAs(ref filename);

                // closing word doc
                wordDocument.Close(ref missing, ref missing, ref missing);
                wordDocument = null;
                //wordDocument.Close(false);
                wordApp.Quit(ref missing, ref missing, ref missing);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                wordApp = null;

                MessageBox.Show("Document created successfully !");
                this.Enabled = true;
                // return stin prohgoumenh 
                dailyProgram.Show();
                this.Close();
                dailyProgram.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
