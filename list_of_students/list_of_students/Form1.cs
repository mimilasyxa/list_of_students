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
    }
}
