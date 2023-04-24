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
            MessageBox.Show("hello???");
            createDoc();
        }


        public void createDoc()
        {
            MessageBox.Show("wtf????");
            try
            {

                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.ShowAnimation = false;
                wordApp.Visible = false;
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Word.Document wordDocument = wordApp.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                MessageBox.Show("Before sections");
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
                wordDocument.Content.Text = "Your weekly program" + Environment.NewLine;

                // create tables for each program

                for (int i = 0; i < days; i++)
                {
                    MessageBox.Show("table " + i);
                    // add header
                    //Add paragraph with Heading 1 style  
                    Microsoft.Office.Interop.Word.Paragraph para1 = wordDocument.Content.Paragraphs.Add(ref missing);
                    //object styleHeading1 = "Επικεφαλίδα 1";
                    Object styleHeading1 = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading1;
                    para1.Range.set_Style(ref styleHeading1);
                    para1.Range.Text = "Πρόγραμμα " + (i + 1);
                    para1.Range.InsertParagraphAfter();

                    // create table
                    //Create a table and insert some dummy record  
                    Microsoft.Office.Interop.Word.Table firstTable = wordDocument.Tables.Add(para1.Range, 5, 7, ref missing, ref missing);
                    firstTable.Borders.Enable = 1;
                    TableOfProgram tbl = tablesOfProgram[i]; // the whole table with exercises
                    foreach (Microsoft.Office.Interop.Word.Row row in firstTable.Rows)
                    {
                        foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                        {
                            //Header row  
                            if (cell.RowIndex == 1)
                            {
                                //ColumnEnum col = (ColumnEnum)cell.ColumnIndex;
                                cell.Range.Text = ((ColumnEnum)cell.ColumnIndex).ToString();
                                cell.Range.Font.Bold = 1;
                                //other format properties goes here  
                                cell.Range.Font.Name = "verdana";
                                cell.Range.Font.Size = 10;
                                //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                              
                                cell.Shading.BackgroundPatternColor = Microsoft.Office.Interop.Word.WdColor.wdColorGray25;
                                //Center alignment for the Header cells  
                                cell.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                cell.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                            }
                            else //Data row  
                            {
                                Microsoft.Office.Interop.Word.Range range = wordDocument.Paragraphs.Add().Range;
                                // get the exercise under the specific table
                                Exercise exercise = tbl.Exercises[cell.RowIndex - 1]; // the exact record that has to be added on doc's table
                                
                                cell.Range.Text = "t" + (cell.RowIndex - 2 + cell.ColumnIndex).ToString();
                            }
                        }
                    }
                }

                ////Add paragraph with Heading 2 style  
                //Microsoft.Office.Interop.Word.Paragraph para2 = wordDocument.Content.Paragraphs.Add(ref missing);
                ////object styleHeading2 = "Επικεφαλίδα 2";
                //Object styleHeading2 = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading2;
                //para2.Range.set_Style(ref styleHeading2);
                //para2.Range.Text = "Para 2 text";
                //para2.Range.InsertParagraphAfter();


                //Save the document  
                //string pathToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  TODO: this doesn't work
                //object filename = @"c:\temp1.docx";
                object filename = @"C:\Users\areti\Desktop\testDoc.docx";
                //object filename = pathToDesktop + @"\testDoc.docx";
                //wordDocument.Save();
                wordDocument.SaveAs(ref filename);
                wordDocument.Close(ref missing, ref missing, ref missing);
                wordDocument = null;
                wordApp.Quit(ref missing, ref missing, ref missing);
                wordApp = null;
                MessageBox.Show("Document created successfully !");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
