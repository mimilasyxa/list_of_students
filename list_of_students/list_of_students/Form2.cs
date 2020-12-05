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
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace list_of_students
{
    public partial class Form2 : Form
    {
        public static string Connect = "Server=localhost;Database=students;user=root;password=123123;charset=utf8";// все строки переехали сюда чтобы был доступ у всех функций
        //public static string Connect = "server=localhost;port=3307;username=root;password=root;database=students";
        public MySqlConnection con = new MySqlConnection(Connect);
        private ComboBox group_list;
        public Form2(ComboBox groups)
        {
            group_list = groups;
            Form1 main = this.Owner as Form1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Да нельзя!", "Хватит");
            }
            else
            {
                string sql = string.Format("Insert Into name_specialty(name_specialty.name_specialty, specialty_code, max_countPlebs, max_countFreePlebs) Values ('{0}', '{1}','{2}','{3}');", textBox1.Text, textBox2.Text, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text));
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Создание прошло успешно", "Создание прошло успешно", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    group_list.Items.Add(textBox1.Text);
                    this.Close();
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            con.Open();
        }
    }
}
