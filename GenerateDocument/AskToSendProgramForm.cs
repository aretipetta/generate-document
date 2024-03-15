using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net.Mail;

namespace GenerateDocument
{
    public partial class AskToSendProgramForm : Form
    {
        private String nameOfAttachmentFile;

        public AskToSendProgramForm(String nameOfAttachmentFile)
        {
            InitializeComponent();
            this.nameOfAttachmentFile = nameOfAttachmentFile;

        }

        /**
         * Skip sending the program
         */
        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Η αποστολή προγράμματος θα παραλειφθεί, όμως το αρχείο έχει αποθηκευτεί στην επιφάνεια εργασίας.");
            this.Close();
        }

        /**
         * Send program via email
         */
        private void button1_Click(object sender, EventArgs e)
        {
            // validate input
            string email = textBox1.Text.ToString().Trim();
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Η ηλεκτρονική διεύθυνση ταχυδρομίου μοιάζει λανθασμένη.");
                return;
            }
            // otherwise proceed and send the program
            SendProgramToClient(email);
        }

        private Boolean IsValidEmail(String email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        private void SendProgramToClient(string emailTo)
        {
            // using SMTP (Simple Mail Transfer Protocol)
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                smtpServer.EnableSsl = true;
                smtpServer.UseDefaultCredentials = false;

                // sth is going wrong. there is a problem with the authentication of the user

                mail.From = new MailAddress("---------");
                mail.To.Add(emailTo);
                mail.Subject = "Test";
                mail.Body = "Hello. This is some test";
                Attachment attachment = new Attachment(nameOfAttachmentFile);
                mail.Attachments.Add(attachment);

                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("-------------", "-----------");
                
                smtpServer.Send(mail);
                MessageBox.Show("Η αποστολή του προγράμματος ήταν επιτυχημένη.");
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
