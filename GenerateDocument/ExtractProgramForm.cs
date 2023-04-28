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
            MessageBox.Show("hello???");
            //letsSee();
            createDoc();
        }


        protected void letsSee()
        {
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);
            // Specify font formatting
            Aspose.Words.Font font = builder.Font;
            font.Size = 32;
            font.Bold = true;
            font.Color = System.Drawing.Color.Black;
            font.Name = "Arial";
            font.Underline = Underline.Single;

            // Insert text
            builder.Writeln("This is the first page.");
            builder.Writeln();

            // Change formatting for next elements.
            font.Underline = Underline.None;
            font.Size = 10;
            font.Color = System.Drawing.Color.Blue;

            builder.Writeln("This following is a table");
            // Insert a table
            Table table = builder.StartTable();
            // Insert a cell
            builder.InsertCell();
            // Use fixed column widths.
            table.AutoFit(AutoFitBehavior.AutoFitToContents);
            builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
            builder.Write("This is row 1 cell 1");
            // Insert a cell
            builder.InsertCell();
            builder.Write("This is row 1 cell 2");
            builder.EndRow();
            builder.InsertCell();
            builder.Write("This is row 2 cell 1");
            builder.InsertCell();
            builder.Write("This is row 2 cell 2");
            builder.EndRow();
            builder.EndTable();
            builder.Writeln();
            // Save the document
            doc.Save("Document.docx");
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
                    TableOfProgram tbl = tablesOfProgram[i]; // the whole table with exercises
                    Object styleHeading1 = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading1;
                    para1.Range.set_Style(ref styleHeading1);
                    para1.Range.Text = "Πρόγραμμα " + (i + 1) + ": " + tbl.Category;
                    para1.Range.InsertParagraphAfter();

                    // create table
                    //Create a table and insert some dummy record  
                    Microsoft.Office.Interop.Word.Table firstTable = wordDocument.Tables.Add(para1.Range, tbl.Exercises.Count + 1, Enum.GetValues(typeof(ColumnEnum)).Length, ref missing, ref missing);
                    firstTable.Borders.Enable = 1;
                    String[] exercisesToRowVector = tbl.listToRowVector();
                    MessageBox.Show("lenght = " + exercisesToRowVector.Length);
                    int counter = 0; 
                    int idxRow = 0;
                    foreach (Microsoft.Office.Interop.Word.Row row in firstTable.Rows)
                    {
                        // header row of table
                        if (row.Index == 1)
                        {
                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
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
                        }
                        else // data rows
                        {
                            int idxCol = 0;
                            //String[] rowValues = new string[Enum.GetValues(typeof(ColumnEnum)).Length];
                            //rowValues[0] = tbl.Exercises[idxRow].MuscleGroup;
                            //rowValues[1] = tbl.Exercises[idxRow].Description;
                            //rowValues[2] = tbl.Exercises[idxRow].Equipment;
                            //rowValues[3] = tbl.Exercises[idxRow].Set.ToString();
                            //rowValues[4] = tbl.Exercises[idxRow].Reps.ToString();
                            //rowValues[5] = tbl.Exercises[idxRow].Rest.ToString();
                            //rowValues[6] = tbl.Exercises[idxRow].Notes;
                            //MessageBox.Show("values: " + rowValues[0]);

                            //rowValues.Append(tbl.Exercises[idxRow].MuscleGroup);
                            //rowValues.Append(tbl.Exercises[idxRow].Description);
                            //rowValues.Append(tbl.Exercises[idxRow].Equipment);
                            //rowValues.Append(tbl.Exercises[idxRow].Set.ToString());
                            //rowValues.Append(tbl.Exercises[idxRow].Reps.ToString());
                            //rowValues.Append(tbl.Exercises[idxRow].Rest.ToString());
                            //rowValues.Append(tbl.Exercises[idxRow].Notes.ToString());

                            foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                            {
                                Microsoft.Office.Interop.Word.Range range = wordDocument.Paragraphs.Add().Range;
                                // get the exercise under the specific table
                                //Exercise exercise = tbl.Exercises[cell.RowIndex]; // the exact record that has to be added on doc's table
                                // todo: add all exercises to a vector
                                //  cell.Range.Text = "t" + (cell.ColumnIndex).ToString(); //cell.RowIndex - 2 + 
                                //cell.Range.Text = tbl.Exercises[cell.RowIndex].MuscleGroup + tbl.Exercises[cell.RowIndex].Description + tbl.Exercises[cell.RowIndex].Equipment
                                //    + tbl.Exercises[cell.RowIndex].Set + tbl.Exercises[cell.RowIndex].Reps + tbl.Exercises[cell.RowIndex].Rest + tbl.Exercises[cell.RowIndex].Notes;

                                //cell.Range.Text = rowValues[idxCol];
                                MessageBox.Show("counter = " + counter);
                                cell.Range.Text = exercisesToRowVector[counter];
                                MessageBox.Show("counter meta = " + counter);
                                counter++;
                                idxCol++;
                            }
                            wordDocument.Content.Text = Environment.NewLine + "telos programmatos";
                        }
                        idxRow++;


                        //foreach (Microsoft.Office.Interop.Word.Cell cell in row.Cells)
                        //{
                        //    //Header row  
                        //    if (cell.RowIndex == 1)
                        //    {
                        //        //ColumnEnum col = (ColumnEnum)cell.ColumnIndex;
                        //        cell.Range.Text = ((ColumnEnum)cell.ColumnIndex).ToString();
                        //        cell.Range.Font.Bold = 1;
                        //        //other format properties goes here  
                        //        cell.Range.Font.Name = "verdana";
                        //        cell.Range.Font.Size = 10;
                        //        //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                              
                        //        cell.Shading.BackgroundPatternColor = Microsoft.Office.Interop.Word.WdColor.wdColorGray25;
                        //        //Center alignment for the Header cells  
                        //        cell.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        //        cell.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                        //    }
                        //    else //Data row  
                        //    {
                        //        Microsoft.Office.Interop.Word.Range range = wordDocument.Paragraphs.Add().Range;
                        //        // get the exercise under the specific table
                        //        //Exercise exercise = tbl.Exercises[cell.RowIndex]; // the exact record that has to be added on doc's table
                        //        // todo: add all exercises to a vector
                        //        //  cell.Range.Text = "t" + (cell.ColumnIndex).ToString(); //cell.RowIndex - 2 + 
                        //        //cell.Range.Text = tbl.Exercises[cell.RowIndex].MuscleGroup + tbl.Exercises[cell.RowIndex].Description + tbl.Exercises[cell.RowIndex].Equipment
                        //        //    + tbl.Exercises[cell.RowIndex].Set + tbl.Exercises[cell.RowIndex].Reps + tbl.Exercises[cell.RowIndex].Rest + tbl.Exercises[cell.RowIndex].Notes;

                        //        MessageBox.Show("prin");
                        //        cell.Range.Text = rowValues[idxCol];
                        //        MessageBox.Show("meta");
                        //    }
                        //    idxCol++;
                        //}

                    }
                    // telos programmatos
                    // TODOOOOOOOOOoo
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
