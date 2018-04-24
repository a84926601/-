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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public DataGridView DataGridView
        {
            get
            {
                return dataGridView1;
            }
        }
        private void 開啟舊檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
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
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.HeaderCell.Value = row.Index.ToString();
                    }
                    label1.Text = (dataGridView1.Rows.Count - 2) + "筆";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.form1 = this;
            form2.Show();
        }
    }
}
