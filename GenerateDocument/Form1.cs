﻿using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // go to form2
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
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
                foreach (Microsoft.Office.Interop.Word.Section section in wordDocument.Sections)
                {
                    //Get the header range and add the header details.  
                    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                    headerRange.Font.Size = 20;
                    headerRange.Text = "YOUR PROGRAM";
                    //Get the footer range and add the footer details.  
                    Microsoft.Office.Interop.Word.Range footerRange = section.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 9;
                    footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = "made by me :)";
                }

                //adding text to document  
                wordDocument.Content.SetRange(0, 0);
                wordDocument.Content.Text = "Your weekly program" + Environment.NewLine;

                int numOfProgramms = (int)numericUpDown1.Value;

                for (int i = 0; i < numOfProgramms; i++)
                {
                    // add header
                    //Add paragraph with Heading 1 style  
                    Microsoft.Office.Interop.Word.Paragraph para1 = wordDocument.Content.Paragraphs.Add(ref missing);
                    //object styleHeading1 = "Επικεφαλίδα 1";
                    Object styleHeading1 = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading1;
                    para1.Range.set_Style(ref styleHeading1);
                    para1.Range.Text = "Program " + i;
                    para1.Range.InsertParagraphAfter();

                    // create table
                    //Create a table and insert some dummy record  
                    Microsoft.Office.Interop.Word.Table firstTable = wordDocument.Tables.Add(para1.Range, 5, 7, ref missing, ref missing);
                    firstTable.Borders.Enable = 1;
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
                                cell.Range.Text = (cell.RowIndex - 2 + cell.ColumnIndex).ToString();
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
                //string pathToDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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
