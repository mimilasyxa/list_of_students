using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace list_of_students
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        public static string[] lname =  {"Смит", "Вэй", "Мюллер", "Дламини", "Сильва", "Сингх"};
        public static string[] fname = { "Алекс", "Кортни", "Тейлор", "Медисон", "Пейдж", "Эрин" };
        public static string[] mname = { "Александровна", "Никитович", "Матвеевич", "Михайловна", "Денисович", "Романович" };
        public Form1()
        {
            InitializeComponent();
        }
        //  string sql = string.Format("Insert Into group" +
                   //"(Lname, Fname, Mname, avg_score ,original_docs, budget) Values('{0}','{1}','{2}',{3},{4},{5})", textBox1.Text, textBox2.Text, textBox3.Text, Convert.ToDouble(textBox4.Text), Convert.ToInt32(checkBox1.Checked), Convert.ToInt32(checkBox2.Checked));
        private void button1_Click(object sender, EventArgs e)
        {
            string Connect = "server=localhost;port=3307;username=root;password=root;database=students";
            MySqlConnection con = new MySqlConnection(Connect);
            con.Open();
            string sql = string.Format("Insert Into students" +
                "(lname, fname, mname, average_score) Values('{0}','{1}','{2}', '{3}');", textBox1.Text, textBox2.Text, textBox3.Text, Convert.ToInt32(textBox4.Text));


            using (MySqlCommand cmd = new MySqlCommand(sql, con))
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Добавление прошло успешно", "Добавление прошло успешно", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            con.Close();
        }
        private string GetString(string type) {
            if (type == "lname")
            {
                return lname[rand.Next(6)];
            }
            if (type == "fname")
            {   
                return fname[rand.Next(6)];
            }
            if (type == "mname")
            {
                return mname[rand.Next(6)];
            }
            else
            {
                return "error";
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.R))
            {
                textBox1.Text = GetString("lname");
                textBox2.Text = GetString("fname");
                textBox3.Text = GetString("mname");
                textBox4.Text = (Math.Round(rand.NextDouble() * 5, 1)).ToString();
                return true;     
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
