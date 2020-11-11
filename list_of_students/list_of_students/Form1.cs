using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace list_of_students
{
    public partial class Form1 : Form
    {
        public static string[] lname =  {"Смит", "Вэй", "Мюллер", "Дламини", "Сильва", "Сингх"};
        public static string[] fname = { "Алекс", "Кортни", "Тейлор", "Медисон", "Пейдж", "Эрин" };
        public static string[] mname = { "Александровна", "Никитович", "Матвеевич", "Михайловна", "Денисович", "Романович" };
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*string Connect = "Server=127.0.0.1;Database=shop;Data Source=localhost;user=root;";
            MySqlConnection con = new MySqlConnection(Connect);
            con.Open();

            string filename = filePath.Substring(filePath.LastIndexOf(@"\") + 1);
            string sql = string.Format("Insert Into goods" +
                   "(Name, Author, Description, Price ,Quantity, Photo) Values('{0}','{1}','{2}','{3}','{4}','{5}')", textBox1.Text, textBox5.Text, textBox2.Text, textBox3.Text, Convert.ToInt32(textBox4.Text), filename);
            using (MySqlCommand cmd = new MySqlCommand(sql, con))
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Добавление прошло успешно", "Добавление прошло успешно", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            con.Close();*/
        }
        private string GetString(string type) {
            if (type == "Lname")
            {

            }

            return "а";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.R))
            {
                textBox1.Text = lname[0];   // Implement the Ctrl+F short-cut keystroke
                return true;     // This keystroke was handled, don't pass to the control with the focus
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
