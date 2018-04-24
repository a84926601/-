using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Email_Management
{
    public partial class Form2 : Form
    {
        public Form1 form1 = null;
        private int orinum;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<String> oriString = new List<String>();
            List<String> delString = new List<String>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null)
                oriString.Add(row.Cells[0].Value.ToString().ToLower());
            }
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells[0].Value != null)
                    delString.Add(row.Cells[0].Value.ToString().ToLower());
            }
            oriString = oriString.Except(delString).ToList();
            //Save file
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files(*.txt)| *.txt | All files(*.*) | *.* ";
            saveFileDialog1.Title = "Save an txt File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName))
                {
                    file.WriteLine("Email");
                    foreach (String s in oriString)
                    {
                        file.WriteLine(s);
                    }
                    file.Close();
                }
                int diff = orinum - oriString.Count;
                MessageBox.Show("已經移除" + (diff<0?0:diff) + "筆資料");
            }
            Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //設定dataGridView1
            dataGridView1.DataSource = form1.DataGridView.DataSource;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.HeaderCell.Value = row.Index.ToString();
            }
            orinum = dataGridView1.Rows.Count - 2;
            label3.Text = orinum + "筆";
            //設定dataGridView2
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    string[] columnnames = sr.ReadLine().Split(',');
                    DataTable dt = new DataTable();
                    foreach (string c in columnnames)
                    {
                        dt.Columns.Add(c);
                    }
                    string newline;
                    while ((newline = sr.ReadLine()) != null)
                    {
                        DataRow dr = dt.NewRow();
                        string[] values = newline.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            dr[i] = values[i];
                        }
                        dt.Rows.Add(dr);
                    }
                    sr.Close();
                    dataGridView2.DataSource = dt;
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView2.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.HeaderCell.Value = row.Index.ToString();
                    }
                    label4.Text = (dataGridView2.Rows.Count - 2)+"筆";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            else Close();
        }
    }
}
