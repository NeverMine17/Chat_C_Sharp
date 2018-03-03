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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            FileStream file2 = new FileStream("peregovory.txt", FileMode.Open); //создаем файловый поток
            StreamReader reader = new StreamReader(file2); // создаем «потоковый читатель» и связываем его с файловым потоком
            this.Height = 651;
            this.Width =  547;


            int i = 0;
            while (reader.Peek() >= 0)
            {
                string stroka_iz_faila = reader.ReadLine().Trim();
                string[] podstroki = stroka_iz_faila.Split(new String[] { rasd }, StringSplitOptions.None);
                
                if (podstroki.Length > 2)
                {
                    messages[i].day = Convert.ToDateTime(podstroki[0]);
                    messages[i].login = podstroki[1];
                    messages[i].text = podstroki[2];
                    textBox2.Text = textBox2.Text + podstroki[0] + Environment.NewLine;
                    textBox2.Text = textBox2.Text + podstroki[1] + Environment.NewLine;
                    textBox2.Text = textBox2.Text + podstroki[2] + Environment.NewLine;
                    i++;
                }
            }

            reader.Close(); //закрываем поток
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                DateTime thisDay = DateTime.Now;
                textBox2.AppendText(thisDay.ToString("dd-MM-yyyy hh:mm:ss") + Environment.NewLine + login + ":   " + textBox1.Text + Environment.NewLine);
                System.IO.File.AppendAllText("peregovory.txt", Environment.NewLine + thisDay.ToString("dd-MM-yyyy hh:mm:ss") + rasd + login + rasd + textBox1.Text);
            } 
            
            textBox1.Text = null;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
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
                Font = fontDialog1.Font;
                ForeColor = fontDialog1.Color;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("get.exe", "\"peregovory.txt\" \"peregovory.txt\"");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("put.exe", "peregovory.txt peregovory.txt");
        }
    }

    
}
