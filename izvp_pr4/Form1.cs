using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace izvp_pr4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }
        Dictionary<string, double[]> dict = new Dictionary<string, double[]>();
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
           
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            bool check = true;
            label1.BackColor = Color.White;
            label2.BackColor = Color.White;
            label3.BackColor = Color.White;
            label4.BackColor = Color.White;
            if (monthCalendar1.SelectionEnd.DayOfWeek==DayOfWeek.Saturday || monthCalendar1.SelectionEnd.DayOfWeek == DayOfWeek.Sunday)
            {
                check = false;
            }
            label1.Text = "День місяця: " + monthCalendar1.SelectionEnd.Day.ToString();
            label2.Text = "Місяць: " + monthCalendar1.SelectionEnd.Month.ToString();
            label3.Text = "Рік: " + monthCalendar1.SelectionEnd.Year.ToString();
            label4.Text = "День тижня: " + monthCalendar1.SelectionEnd.DayOfWeek.ToString();
            if (!check)
            {
                label1.BackColor = Color.Red;
                label2.BackColor = Color.Red;
                label3.BackColor = Color.Red;
                label4.BackColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt1 = dateTimePicker1.Value;
            DateTime dt2 = dateTimePicker2.Value;
            TimeSpan ts = dt2 - dt1;
            DateTime zeroTime = new DateTime(1, 1, 1);
            textBox5.Text  = Math.Round((dt2 - dt1).TotalDays,0).ToString();
            textBox4.Text = Math.Round((dt2 - dt1).TotalHours,0).ToString();
            textBox3.Text = Math.Round((dt2 - dt1).TotalMinutes,0).ToString();
            textBox1.Text = ((zeroTime+ts).Year-1).ToString();
            textBox6.Text = (((dt2.Year- dt1.Year)*12)+dt2.Month-dt1.Month).ToString();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime dt = dateTimePicker3.Value;
            if (radioButton1.Checked)
            {
               dt =  dt.AddYears(Convert.ToInt32(textBox2.Text));
            }
            else if (radioButton7.Checked)
            {
                dt = dt.AddMonths(Convert.ToInt32(textBox2.Text));
            }
            else if (radioButton6.Checked)
            {
                dt = dt.AddDays(Convert.ToInt32(textBox2.Text)*7);
            }
            else if (radioButton5.Checked)
            {
                dt = dt.AddDays(Convert.ToInt32(textBox2.Text));
            }
            else if (radioButton4.Checked)
            {
                dt = dt.AddHours(Convert.ToInt32(textBox2.Text));
            }
            else if (radioButton3.Checked)
            {
                dt = dt.AddMinutes(Convert.ToInt32(textBox2.Text));

            }
            else if (radioButton2.Checked)
            {
                dt = dt.AddSeconds(Convert.ToInt32(textBox2.Text));

            }
            dateTimePicker4.Value = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dict.Add(dateTimePicker5.Value.Day.ToString() + "." + dateTimePicker5.Value.Month.ToString() + "." + dateTimePicker5.Value.Year.ToString(), new double[] {Convert.ToDouble( textBox7.Text), Convert.ToDouble(textBox8.Text) });
            File.AppendAllText("data.txt",dateTimePicker5.Value.Day.ToString()+"."+dateTimePicker5.Value.Month.ToString()+"."+ dateTimePicker5.Value.Year.ToString() + "|"+textBox7.Text+"|"+textBox8.Text+"|\n");
            textBox7.Text = "";
            textBox8.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach(string line in File.ReadLines("data.txt"))
            {
                string[] data = line.Split('|');
                try
                {
                   
                    dict.Add(data[0], new double[] { Convert.ToDouble(data[1]), Convert.ToDouble(data[2]) });
                }
                catch (Exception exc) { Console.WriteLine("Cannot read from file"); }
            }
            foreach(string date in dict.Keys)
            {
                Console.WriteLine(date.ToString());
            }
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {

            DateTime dt = monthCalendar2.SelectionStart;
            string date = dt.Day.ToString() + "." + dt.Month.ToString() + "." + dt.Year.ToString();
            if (dict.ContainsKey(date))
            {
                label14.Text = "Тиск: "+dict[date][1];
                label15.Text = "Температура: " + dict[date][0];
            }
            else
            {
                label14.Text = "Тиск: немає даних";
                label15.Text = "Температура: немає даних";
            }
        }

        List<Task> tasks = new List<Task>();
        private void button4_Click(object sender, EventArgs e)
        {
            tasks.Add(new Task(richTextBox1.Text, dateTimePicker6.Value, false));
            dataGridView1.Rows.Add(richTextBox1.Text,dateTimePicker6.Value,"не виконано");
            dataGridView1.Rows[dataGridView1.Rows.Count-2].DefaultCellStyle.BackColor = Color.Red;
            
        }
        private void dataGridView1_CellContentClick(object sender,
    DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.RowIndex<dataGridView1.Rows.Count)
            {
                tasks.RemoveAt(e.RowIndex);
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
            else dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

        }

        /// <summary>
        /// Works with the above.
        /// </summary>
        private void dataGridView1_CellValueChanged(object sender,
            DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != dataGridView1.Rows.Count - 1)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[3].Value) == true)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[2].Value = "Виконано";
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                    }
                    else { 
                        dataGridView1.Rows[e.RowIndex].Cells[2].Value = "Не виконано";
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
            catch (Exception exc) { Console.WriteLine(exc); }
         
        }

        private void monthCalendar3_DateSelected(object sender, DateRangeEventArgs e)
        {
            dataGridView1.Rows.Clear();
            foreach(Task task in tasks)
            { 
                if(task.Date>monthCalendar3.SelectionStart && task.Date < monthCalendar3.SelectionEnd)
                {
                    dataGridView1.Rows.Add(task.Description, task.Date, task.Status);
                }
            }
        }
    }
}
