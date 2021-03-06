using System.Windows.Forms;
using System;
using System.Threading;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.IO;
using System.Linq;
using System.Net.Sockets;


namespace localSiteNodeReporterForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        Start:

            //streamreader for storedIP
            string storedIP = "No External IP Found";
            string localStoredIP = "No Local IP Found";

            try
            {
                using (StreamReader reader = new StreamReader("c:\\localSiteInfo.txt"))
                {
                    storedIP = reader.ReadLine() ?? "";
                    localStoredIP = File.ReadLines("c:\\localSiteInfo.txt").Skip(1).Take(1).First();

                }

            }
            catch (Exception t)
            {
                MessageBox.Show("The localSiteInfo file could not be read: Please Contact Support");
                //MessageBox.Show(t.Message);
            }


            //Get the external IP
            string externalip = new WebClient().DownloadString("http://icanhazip.com");
            string storedIpFix = storedIP + "\n";

            //Get the internal IP
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 53);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }



            string Timez = DateTime.Now.ToString();


            string lastLine = "---Last Line of localSiteInfo text file Unreadable---";


            //throw the last line of the localSiteInfo text into a variable
            try
            {
                StreamReader r = new StreamReader("c:\\localSiteInfo.txt");
                while (r.EndOfStream == false)
                {
                    lastLine = r.ReadLine();
                }
            }
            catch (Exception f)
            {
                //MessageBox.Show("The localSiteInfo file could not be read:");
                //MessageBox.Show(f.Message);
                //Too many error messages, only need the first one for file not found
            }


            string msTextFile = "No localSiteInfo Text File Data";
            try
            {
                using (StreamReader sr = new StreamReader("c:\\localSiteInfo.txt"))
                {

                    msTextFile = sr.ReadToEnd();

                }
            }
            catch (Exception j)
            {
                //MessageBox.Show("The localSiteInfo file could not be read:");
                //MessageBox.Show(j.Message);
                //there were too many messages for the typical HTUser
            }



            if (externalip != storedIpFix)
            {


                try
                {
                    // SMTP Stuff Begin
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.Host = "SMTP URL";
                    client.EnableSsl = true;

                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("SMTP UN", "SMTP PW");

                    MailMessage mm = new MailMessage("FROM-EMAIL@EMAIL.com", "TO-EMAIL@email.com", "EXTERNAL IP CHANGE AT " + lastLine, "WAN IP CHANGE FROM " + storedIP + " TO " + externalip + " - " + lastLine);
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.Send(mm);
                }
                catch (Exception EXCEP)
                {
                    MessageBox.Show(Timez + " - EXTERNAL IP CHANGED TO - " + externalip + " " + lastLine + " Please Contact Support with ALL of this Information!");


                    DateTime now = DateTime.Now;


                    try
                    {
                        string path = @"c:\localSiteInfoLog.txt";
                        // This text is added only once to the file.
                        if (!File.Exists(path))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine("localSiteInfoLog.txt Log File Begin");
                            }
                        }

                        // This text is always added, making the file longer over time
                        // if it is not deleted.
                        using (StreamWriter sw = File.AppendText(path))
                        {

                            sw.WriteLine(now + " - " + EXCEP);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Email Notifier & Log File Failure, Please Contact Support ASAP");
                    }

                }
            }









            if (localStoredIP != localIP)
            {


                try
                {
                    // SMTP Stuff Begin
                    SmtpClient client = new SmtpClient();
                    client.Port = 587;
                    client.Host = "SMTP URL";
                    client.EnableSsl = true;

                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("SMTP UN", "SMTP PW");

                    MailMessage mm = new MailMessage("FROM-EMAIL@EMAIL.com", "TO-EMAIL@email.com", "INTERNAL IP CHANGE AT " + lastLine, "LAN IP CHANGE FROM " + localStoredIP + " TO " + localIP + " - " + lastLine);
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.Send(mm);
                }
                catch (Exception EXCEP)
                {
                    MessageBox.Show(Timez + " - LOCAL IP CHANGED TO - " + localIP + " " + lastLine + " Please Contact Support with ALL of this Information!");


                    DateTime now = DateTime.Now;


                    try
                    {
                        string path = @"c:\localSiteInfoLog.txt";
                        // This text is added only once to the file.
                        if (!File.Exists(path))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine("localSiteInfoLog.txt Log File Begin");
                            }
                        }

                        // This text is always added, making the file longer over time
                        // if it is not deleted.
                        using (StreamWriter sw = File.AppendText(path))
                        {

                            sw.WriteLine(now + " - " + EXCEP);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Email Notifier & Log File Failure, Please Contact Support ASAP");
                    }

                    Thread.Sleep(21600000); //6 hours or 21.6 million recommended - 21600000

                    goto Start;

                }
            }
        }

    }

}



