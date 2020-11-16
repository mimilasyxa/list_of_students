using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace list_of_students // Тут происходит чёрт пойми что, при переходе с MySqlConnnect на SqlConnect всё полетело к чертям, без charset не получается присоединиться к серверу и посылать запросы
{
    public partial class Form1 : Form
    {
        //public static string Connect = "Server=127.0.0.1;Database=testbase;Data Source=localhost;user=root;password=123123;charset=utf8";// все строки переехали сюда чтобы был доступ у всех функций
        public static string Connect = "server=localhost;port=3307;username=root;password=root;database=students";
        public MySqlConnection con = new MySqlConnection(Connect);
        Random rand = new Random();
        public static string[] lname =  {"Смит", "Вэй", "Мюллер", "Дламини", "Сильва", "Сингх"};
        public static string[] fname = { "Алекс", "Кортни", "Тейлор", "Медисон", "Пейдж", "Эрин"};
        public static string[] mname = { "Александровна", "Никитович", "Матвеевич", "Михайловна", "Денисович", "Романович"};
        public Form1()
        {
            InitializeComponent();
        }
        //  string sql = string.Format("Insert Into group" +
                   //"(Lname, Fname, Mname, avg_score ,original_docs, budget) Values('{0}','{1}','{2}',{3},{4},{5})", textBox1.Text, textBox2.Text, textBox3.Text, Convert.ToDouble(textBox4.Text), Convert.ToInt32(checkBox1.Checked), Convert.ToInt32(checkBox2.Checked));
        private void button1_Click(object sender, EventArgs e) // ввод студента в группу
        {
            string orig_docs = "Да";
            string budget = "Да";
            if (checkBox1.Checked == false) {
                orig_docs = "Нет";
            }
            if (checkBox2.Checked == false)
            {
                budget = "Нет";
            }
            float num = float.Parse(textBox4.Text);
            string avg_score = num.ToString().Replace(',', '.');
            string sql = string.Format("Insert Into studs" +
                "(lname, fname, mname, avg_score, original_docs, budget) Values('{0}','{1}','{2}', '{3}', '{4}', '{5}');", textBox1.Text, textBox2.Text, textBox3.Text, avg_score, orig_docs, budget);


            using (MySqlCommand cmd = new MySqlCommand(sql, con))
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Добавление прошло успешно", "Добавление прошло успешно", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
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
                textBox4.Text = (Math.Round(rand.NextDouble() * 5, 1)).ToString("G");
                return true;     
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
/*
        private void Form1_Load(object sender, EventArgs e) // при загрузке формы 1 происходит выборка всех групп
        {
            string sql = string.Format("select group_name from _Groups");
            con.Open();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            string result = (string)cmd.ExecuteScalar();
            textBox1.Text = result;
        }
*/

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // закрытие соединения с бд при закрытии формы 1
        {
            con.Close();
        }
    }
}
