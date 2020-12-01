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
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;

namespace list_of_students // После неудачной попытки перехода на Sql мы вернулись к MySql
{
    public partial class Form1 : Form
    {
        public static string Connect = "Server=localhost;Database=students;user=root;password=123123;charset=utf8";// все строки переехали сюда чтобы был доступ у всех функций
        //public static string Connect = "server=localhost;port=3307;username=root;password=root;database=students";
        public MySqlConnection con = new MySqlConnection(Connect);
        Random rand = new Random();
        public static string[] lname =  {"Смит", "Вэй", "Мюллер", "Дламини", "Сильва", "Сингх", "Морто", "Кринж"};
        public static string[] fname = { "Алекс", "Кортни", "Тейлор", "Медисон", "Пейдж", "Эрин", "Пендс", "Флос" };
        public static string[] mname = { "Александровна", "Никитович", "Матвеевич", "Михайловна", "Денисович", "Романович", "Олегович", "Фортнайтович"};
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) // ввод студента в группу
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "") // Проверка на то что в формах есть хоть что-то
                {
                    MessageBox.Show("Поля (Фамилия, Имя, Отчество, Средний балл) обязательны к заполнению", "Ошибка");
                }

                int orig_docs = 1;
                string budget = "Да";
                if (checkBox1.Checked == false)
                {
                    orig_docs = 2;
                }
                if (checkBox2.Checked == false)
                {
                    budget = "Нет";
                }
                float num = float.Parse(textBox4.Text);
                string avg_score = num.ToString().Replace(',', '.'); // Облегчённая работа с float, пользователь может использовать как точку так и запятую при вводе среднего бала
                string sql = string.Format("Insert Into student" +
                    "(lname, fname, mname, average_score, fk_id_original_documents, budget, fk_id_name_specialty) Values('{0}','{1}','{2}', '{3}', '{4}', '{5}', '{6}');", textBox1.Text, textBox2.Text, textBox3.Text, avg_score, orig_docs, budget, (comboBox1.SelectedIndex + 1));
                // Ввод студента, берутся все данные из форм и переключателей

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Добавление прошло успешно", "Добавление прошло успешно", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (MySql.Data.MySqlClient.MySqlException)
                    {
                        MessageBox.Show("Выберите группу для абитуриента", "Ошибка");

                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private string GetString(string type) { // rand.Next(количество айтемов в массиве), массивы в начале кода
            if (type == "lname")
            {
                return lname[rand.Next(7)];
            }
            if (type == "fname")
            {
                return fname[rand.Next(7)];
            }
            if (type == "mname")
            {
                return mname[rand.Next(7)];
            }
            else
            {
                return "error";
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // Функция для работы горячих клавиш, в нашем случае ctrl + R генерирует случайного студента
        {
            if (keyData == (Keys.Control | Keys.R))
            {
                textBox1.Text = GetString("lname");
                textBox2.Text = GetString("fname");
                textBox3.Text = GetString("mname");
                textBox4.Text = (Math.Round(rand.NextDouble() * 5, 1)).ToString("G");
                if (Math.Round(rand.NextDouble()) == 1)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                if (Math.Round(rand.NextDouble()) == 1)
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }
                int orig_docs = 1;
                string budget = "Да";
                if (checkBox1.Checked == false)
                {
                    orig_docs = 2;
                }
                if (checkBox2.Checked == false)
                {
                    budget = "Нет";
                }
                float num = float.Parse(textBox4.Text);
                string avg_score = num.ToString().Replace(',', '.'); // Облегчённая работа с float, пользователь может использовать как точку так и запятую при вводе среднего бала
                string sql = string.Format("Insert Into student" +
                    "(lname, fname, mname, average_score, fk_id_original_documents, budget, fk_id_name_specialty) Values('{0}','{1}','{2}', '{3}', '{4}', '{5}', '{6}');", textBox1.Text, textBox2.Text, textBox3.Text, avg_score, orig_docs, budget, (comboBox1.SelectedIndex + 1));
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Form1_Load(object sender, EventArgs e) // при загрузке формы 1 происходит выборка всех групп
        {
            try
            {
                string sql = string.Format("select * from name_specialty");
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dataReader;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        comboBox1.Items.Add(dataReader["name_specialty"].ToString());
                    }
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Недействительное подключение к базе данных", "Ошибка подключения");
                Application.Exit();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // закрытие соединения с бд при закрытии формы 1
        {
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e) // Создание второй формы и передача в неё ComboBox1 который хранит в себе группы для последующего добавления новой группы
        {
            Form2 secondform = new Form2(comboBox1);
            secondform.Show();
        }

        private void button3_Click(object sender, EventArgs e) // Вывод отфильтрованных студентов в книгу MS Excel
        { 
                int row = 2; // Начинаем с 2 т.к первая строчка хранит в себе оглавление таблицы
                int counter = 1;
                string sql = string.Format("select id_student, lname, fname, mname, " +
                    " average_score, original_documents, budget, name_specialty, " + 
                    " specialty_code from student, name_specialty, original_documents where student.fk_id_name_specialty = '{0}' and  student.fk_id_name_specialty = name_specialty.id_name_specialty  " + " " +
                    "and student.fk_id_original_documents = original_documents.id_original_documents order by average_score desc, fk_id_original_documents asc;", (comboBox1.SelectedIndex + 1));
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dataReader;
                if (comboBox1.Text == "<Выбор направления>")
                {
                    MessageBox.Show("Выбери группу", "Ошибка");
                    Application.Restart();
                }
                dataReader = cmd.ExecuteReader(); 
            // Получить объект приложения Word.
            Word._Application word_app = new Word.Application();

            // Сделать Word видимым (необязательно).
            word_app.Visible = true;
            // Создаем документ Word.
            object missing = Type.Missing;
             Word._Document word_doc = word_app.Documents.Add(
                ref missing, ref missing, ref missing, ref missing);
            Word.Range tableLocation = word_doc.Range(0, 0);
            word_doc.Tables.Add(tableLocation, 25, 1);
            Word.Table table = word_doc.Tables[1];
            table.set_Style("Сетка таблицы 1");
            while (dataReader.Read())
            {
                table.Cell(counter, 1).Range.Text = dataReader["lname"].ToString() + " " + dataReader["fname"].ToString() + " " + dataReader["mname"].ToString();
                counter++;
                if (counter > 25) break;
            }
            dataReader.Close();
        }
    }
}
