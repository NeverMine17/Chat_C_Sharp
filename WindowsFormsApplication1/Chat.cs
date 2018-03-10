﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{    
    public struct Soobshenie
    {
        public string login;
        public string text;
        public DateTime day;
    };
   

    public partial class Chat : Form
    {
        public static Soobshenie[] messages = new Soobshenie[15016];
       
        public string login;
        public const string rasd = "$~#~@*&";

        public Chat(string _login)
        {
            InitializeComponent();
            login = _login;           

            saveFileDialog1.Filter = "Text files(*SaveFileDialog.txt)|*.txt|All files(*.*)|*.*";
            openFileDialog1.Filter = "Text files(*OpenFileDialog.txt)|*.txt|All files(*.*)|*.*";
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void Form2_Load(object sender, EventArgs e)
        {
            bool sos = false;
            FileStream file2 = null;
            try {
                file2 = new FileStream("peregovory.txt", FileMode.Open); //создаем файловый поток
            } catch (System.IO.FileNotFoundException) {
                sos = true;
       
            }
            if (!sos)
            {
                file2.Close();
                file2 = new FileStream("peregovory.txt", FileMode.Open); //создаем файловый поток
                StreamReader reader = new StreamReader(file2); // создаем «потоковый читатель» и связываем его с файловым потоком
                this.Height = 651;
                this.Width = 630;

                int i = 0;
                while (reader.Peek() >= 0)
                {
                    string stroka_iz_faila = reader.ReadLine().Trim();
                    string[] podstroki = stroka_iz_faila.Split(new String[] { rasd }, StringSplitOptions.None);

                    if (podstroki.Length > 2)
                    {
                        messages[i].day = Convert.ToDateTime(podstroki[0]);
                        messages[i].login = podstroki[1];
                        messages[i].text = podstroki[2].Replace("%%%%", Environment.NewLine);

                        i++;
                    }
                }

                int kolichestvo_soobsch = i;

                reader.Close(); //закрываем поток

                for (i = 0; i < kolichestvo_soobsch - 1; i++)
                {
                    for (int j = i + 1; j < kolichestvo_soobsch; j++)
                    {
                        if (messages[i].day > messages[j].day)
                        {
                            Soobshenie soob = messages[j];

                            messages[j].day = messages[i].day;
                            messages[i].day = soob.day;
                            messages[j].login = messages[i].login;
                            messages[i].login = soob.login;
                            messages[j].text = messages[i].text;
                            messages[i].text = soob.text;
                        }
                    }
                }

                for (i = 0; i < kolichestvo_soobsch; i++)
                {
                    textBox2.Text = textBox2.Text + messages[i].day + Environment.NewLine;
                    textBox2.Text = textBox2.Text + "     " + messages[i].login + "  cказал(а):  ";
                    textBox2.Text = textBox2.Text + messages[i].text + Environment.NewLine;
                }
            }
            else
            {
                textBox2.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                DateTime thisDay = DateTime.Now;
                String dateStr = thisDay.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                String dateStrfors = thisDay.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");

                textBox2.AppendText(dateStr + Environment.NewLine + login + ":   " +
                    textBox1.Text + Environment.NewLine);
                File.AppendAllText("peregovory.txt", Environment.NewLine + dateStr + rasd + login + rasd + textBox1.Text.Replace(Environment.NewLine, "%%%%"));
                File.AppendAllText("NewMessages.txt", dateStr + rasd + login + rasd + textBox1.Text.Replace(Environment.NewLine, "%%%%") + Environment.NewLine);
            } 
            
            textBox1.Text = null;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Enter)
            {
               // e.KeyChar = ' ';
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1_Click(sender, e);
            }
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowColor = true;            
            fontDialog1.Font = textBox1.Font;
            fontDialog1.Color = textBox1.ForeColor;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
                textBox1.Font = fontDialog1.Font;
                textBox2.Font = fontDialog1.Font;
               

                textBox1.ForeColor = fontDialog1.Color;
                textBox2.ForeColor = fontDialog1.Color;
                

                button1.ForeColor = fontDialog1.Color;
                button2.ForeColor = fontDialog1.Color;
                button3.ForeColor = fontDialog1.Color;

                button1.Font = fontDialog1.Font;
                button2.Font = fontDialog1.Font;
                button3.Font = fontDialog1.Font; 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            File.WriteAllText("AllMessages.txt", string.Empty);
            Process myProcess = Process.Start("get.exe", "peregovory.txt peregovory.txt");
            do
            {
                if (!myProcess.HasExited)
                {
                    // Refresh the current process property values.
                    myProcess.Refresh();
                }
            }
            while (!myProcess.WaitForExit(10000));
            


            FileStream file2 = new FileStream("NewMessages.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file2); // создаем «потоковый читатель» и связываем его с файловым потоком
           
            while (reader.Peek() >= 0)
            {
                string stroka_iz_faila = reader.ReadLine().Trim();
                File.AppendAllText("peregovory.txt", Environment.NewLine + stroka_iz_faila);                
            }

            reader.Close(); //закрываем поток
            
            textBox2.Clear();
            Form2_Load(sender, e);

            Process.Start("put.exe", "peregovory.txt peregovory.txt");
        }

        private void button4_Click(object sender, EventArgs e)
        {
           // Process.Start("put.exe", "peregovory.txt peregovory.txt");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}