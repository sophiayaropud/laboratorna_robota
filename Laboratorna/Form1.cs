using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorna
{
    public partial class Form1 : Form
    {
        private DiscretCriteria dc;
    
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dc= new DiscretCriteria();
            dataGridView1.RowCount = 2;
            dataGridView2.RowCount = 7;
            dataGridView2.Rows[0].HeaderCell.Value = "group";
            dataGridView2.Rows[1].HeaderCell.Value = "mI";
            dataGridView2.Rows[2].HeaderCell.Value = "pINormal";
            dataGridView2.Rows[3].HeaderCell.Value = "pIPyason";
            dataGridView2.Rows[4].HeaderCell.Value = "n*pINormal";
            dataGridView2.Rows[5].HeaderCell.Value = "n*pIPyason";
        }

        private void readFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dc.ReadFromFile(openFileDialog1.FileName);
                dc.ToTable(dataGridView1);
            }
        }

        private void createGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dc.CreateGroup();
            dc.ToTableGroup(dataGridView2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        { 
            double r = dc.HiDvaNormal();
            textBox1.Text = r.ToString();
            dc.ToTablePosibility(dataGridView2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double lamda = float.Parse(textBox3.Text);
            double r = dc.HiDvaPyason(lamda);
            textBox2.Text = r.ToString();
            dc.ToTablePosibility1(dataGridView2);
        }
    }
}
