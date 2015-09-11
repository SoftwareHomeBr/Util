using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace waLargeTextFiles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string m_filename = "";
        System.IO.StreamReader texto = null;
        int contador = 0;

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = m_filename;
            if(!string.IsNullOrEmpty(  m_filename))
                openFileDialog1.InitialDirectory  = System.IO.Path.GetDirectoryName(m_filename);
            if (string.IsNullOrEmpty(openFileDialog1.InitialDirectory))
            {
                openFileDialog1.InitialDirectory = System.Environment.GetEnvironmentVariables()["SystemDrive"] as string ;
            }
            openFileDialog1.Filter = "All|*.*";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_filename = openFileDialog1.FileName;
                this.Text = openFileDialog1.FileName;
                if(texto != null)
                    texto.Close();
            }
        }

        private void btVai_Click(object sender, EventArgs e)
        {
            int seek = -1;
            lbLido.Text = contador.ToString();
            lbLido.Refresh();
            List<string> linhas = new List<string>();
            if (!System.IO.File.Exists(m_filename))
                openToolStripMenuItem_Click(sender, e);
            if (System.IO.File.Exists(m_filename))
            {
                bool endTexto = true;
                try {
                    endTexto = texto.EndOfStream;
                }
                catch
                {
                    endTexto = true;
                }
                if (texto == null || endTexto )
                {
                    texto = System.IO.File.OpenText(m_filename);
                    contador = 0;
                }
                do
                {
                    linhas.Clear();
                    for (int ct = 0; ct < 10; ct++)
                    {
                        if (texto.EndOfStream)
                            break;
                        string ll = texto.ReadLine();
                        linhas.Add(ll);
                        contador++;
                    }
                    lbLido.Text = "linha:" + contador.ToString("N0");
                    lbLido.Refresh();
                } while (!texto.EndOfStream && ( seek = procuraFrase(linhas, textBox1.Text)) < 0);
                mostraLinhas(linhas);
                if (seek >= 0) {
                    textBox2.SelectionStart = textBox2.Text.ToLower().IndexOf(textBox1.Text.ToLower());
                    textBox2.SelectionLength = textBox1.Text.Length;
                    string pagode = textBox2.SelectedText;
                    textBox2.Select();
                }
            }
            else
                MessageBox.Show("Selecione um arquivo válido");
        }
        /// <summary>
        /// procura o text nas linhas
        /// </summary>
        /// <param name="linhas"></param>
        /// <param name="frase"></param>
        /// <returns>numero da linha que contem o texto</returns>
        int procuraFrase(List<string> linhas,  string frase)
        {
            int cc = 0;
            foreach (var linha in linhas)
            { 
                if (linha.ToLower().Contains(frase.ToLower()))
                {
                    return cc;
                }
                cc++;
            }
            return -1;
        }
        void mostraLinhas(List<string> linhas)
        {
            textBox2.Clear();
            foreach (var l in linhas)
            {
                textBox2.AppendText(l + "\n");
            }
            

        }

        private void chkWrap_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.WordWrap = (sender as CheckBox).Checked;
            textBox2.Select();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(texto != null)
                texto.Close();
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutBox1()).ShowDialog();
        }
    }
}
