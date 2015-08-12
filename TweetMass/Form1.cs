using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TweetSharp;
using Hammock;
using System.Diagnostics;
using System.IO;
using System.Threading;
using MySql.Data.MySqlClient;

namespace TweetMass
{
    public partial class Form1 : Form
    {
        TwitterService servicio = new TwitterService("0bdLdGVAhzyTY39wS29duDv5N", "txdksHEgQ2BjixKsnuiMSm0dp0Xequ5YAmRiZNW9t2feMOf9jq");
        OAuthRequestToken requestToken;
        List<string> tweets = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        public void bdConnect()
        {
            MySqlConnection cnx = new MySqlConnection();
            cnx.ConnectionString = "Server=201.175.12.209;Port=3306;Database=limocarc_tm;Uid=limocarc_tm@201.175.12.209;Pwd=06051990";
            try
            {
                cnx.Open();
            }
            catch (MySqlException Ex)
            {
                MessageBox.Show(Ex.Message);
                //throw;
            }
            if (cnx.State == ConnectionState.Open)
                MessageBox.Show("conectado");
            else
                MessageBox.Show("no conectado");
        }

        public void conectar()
        {
            try
            {
                requestToken = servicio.GetRequestToken();
                string authurl = servicio.GetAuthenticationUrl(requestToken).ToString();
                Process.Start(authurl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No estás conectado a internet", "Error de Conexión", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conectar();
            label2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pin = textBox1.Text;
            OAuthAccessToken accessToken = servicio.GetAccessToken(requestToken, pin);
            servicio.AuthenticateWith(accessToken.Token, accessToken.TokenSecret); //Si todo va bien, hasta aquí se logró conectar con el servicio
            label2.Text = accessToken.ScreenName;
            textBox1.Enabled = false;
            button1.Enabled = false;
            textBox5.Text = label2.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                string linea = "";
                List<string> seguidoreslimpio = new List<string>();
                while (linea != null)
                {
                    linea = sr.ReadLine();
                    if (linea != null && linea != "")
                    {
                        seguidoreslimpio.Add(linea.TrimStart('@'));
                        listBox1.Items.Add(linea.TrimStart('@'));
                    }
                }
                sr.Close();
            }
            label8.Text = listBox1.Items.Count.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<object> amigosMencionados = new List<object>();
            try
            {
                if (listBox1.Items.Count == 0)
                {
                    MessageBox.Show("La lista está vacía", "Lista vacía", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    progressBar1.Maximum = listBox1.Items.Count;
                    progressBar1.Value = 1;
                    progressBar1.Step = 1;
                    foreach (Object amigo in listBox1.Items)
                    {
                        if(textBox3.Text.Equals(""))
                        {
                            MessageBox.Show("Escribe un mensaje para enviar","No se ha escrito un mensaje",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                        else
                        {
                            string mensaje = "@" + amigo + " " + textBox3.Text + " ";
                            tweets.Add(mensaje);
                            TwitterStatus tweet = servicio.SendTweet(new SendTweetOptions { Status = mensaje });
                            if (servicio.Response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                amigosMencionados.Add(amigo);
                                progressBar1.PerformStep();
                            }
                            else
                                throw new Exception(servicio.Response.StatusCode.ToString());
                            //listBox1.Items.Remove(amigo);
                        }
                    }
                    foreach (object mencion in amigosMencionados)
                    {
                        listBox1.Items.Remove(mencion);
                        label8.Text = listBox1.Items.Count.ToString();
                    }
                    label8.Text = listBox1.Items.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Parece que no estás conectado\n\rAutoriza de nuevo con un usuario válido", "Hubo un problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label8.Text = listBox1.Items.Count.ToString();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label6.Text = textBox3.TextLength.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Sergio Ricardo Álvarez Ponce de León\n\rTwitter: @SergioRAlvarezP\r\nwww.sergioalvarez.com.mx", "Creado por: ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Selecciona un usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                label8.Text = listBox1.Items.Count.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox4.Text!="")
                listBox1.Items.Add(textBox4.Text.TrimStart('@'));
            textBox4.Text = "";
            textBox4.Focus();
            label8.Text = listBox1.Items.Count.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            label8.Text = listBox1.Items.Count.ToString();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.twitter.com/SergioRAlvarezP");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivo de Texto|*.txt";
            sfd.Title = "Guardar lista: "; sfd.ShowDialog();
            if(sfd.FileName != "")
            {
                StreamWriter sw = new StreamWriter(@sfd.FileName);
                foreach (object lista in listBox1.Items)
                {
                    sw.WriteLine(lista.ToString());
                }
                sw.Close();
            }
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (textBox4.Text != "")
                    listBox1.Items.Add(textBox4.Text.TrimStart('@'));
                textBox4.Text = "";
                textBox4.Focus();
                label8.Text = listBox1.Items.Count.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                try
                {
                    listBox1.Items.Clear();
                    System.IO.StreamReader sr = new System.IO.StreamReader(textBox2.Text);

                    string linea = "";
                    List<string> seguidoreslimpio = new List<string>();
                    while (linea != null)
                    {
                        linea = sr.ReadLine();
                        if (linea != null && linea != "")
                        {
                            seguidoreslimpio.Add(linea.TrimStart('@'));
                            listBox1.Items.Add(linea.TrimStart('@'));
                        }
                    }
                    sr.Close();
                    label8.Text = listBox1.Items.Count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Escribe correctamente el nombre del archivo","Error al cargar el archivo",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox2.SelectedItem.ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            bdConnect();
        }
    }
}
