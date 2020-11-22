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

namespace list_of_students // После неудачной попытки перехода на Sql мы вернулись к MySql
{
    public partial class Form1 : Form
    {
        public static string Connect = "Server=localhost;Database=students;user=root;password=123123;charset=utf8";// все строки переехали сюда чтобы был доступ у всех функций
        //public static string Connect = "server=localhost;port=3307;username=root;password=root;database=students";
        public MySqlConnection con = new MySqlConnection(Connect);
        Random rand = new Random();
        public static string[] lname =  {"Смит", "Вэй", "Мюллер", "Дламини", "Сильва", "Сингх"};
        public static string[] fname = { "Алекс", "Кортни", "Тейлор", "Медисон", "Пейдж", "Эрин"};
        public static string[] mname = { "Александровна", "Никитович", "Матвеевич", "Михайловна", "Денисович", "Романович"};
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
                string sql = string.Format("Insert Into students" +
                    "(lname, fname, mname, average_score, fk_id_original_documents, budget, fk_id_groups) Values('{0}','{1}','{2}', '{3}', '{4}', '{5}', '{6}');", textBox1.Text, textBox2.Text, textBox3.Text, avg_score, orig_docs, budget, (comboBox1.SelectedIndex + 1));
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // Функция для работы горячих клавиш, в нашем случае ctrl + R генерирует случайного студента
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
        private void Form1_Load(object sender, EventArgs e) // при загрузке формы 1 происходит выборка всех групп
        {
            try
            {
                string sql = string.Format("select * from students.groups");
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dataReader;
                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        comboBox1.Items.Add(dataReader["group"].ToString());
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
            try
            {
                int row = 2; // Начинаем с 2 т.к первая строчка хранит в себе оглавление таблицы
                int counter = 1;
                string sql = string.Format("select id_students , lname , fname , mname, average_score, original_documents, budget, " +
                    " students.groups.group from students, students.groups, original_documents where groups.group = '{0}' AND students.fk_id_groups = students.groups.id_groups " + "" +
                    " and students.fk_id_original_documents = original_documents.id_original_documents and id_students < 26 order by average_score desc, fk_id_original_documents asc;", comboBox1.SelectedItem);
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader dataReader;
                if (comboBox1.Text == "<Выбор группы>")
                {
                    MessageBox.Show("Выбери группу", "Ошибка");
                    Application.Restart();
                }
                dataReader = cmd.ExecuteReader();
                Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();
                ex.Visible = true;
                ex.SheetsInNewWorkbook = 2;
                Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
                ex.DisplayAlerts = false;
                Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
                sheet.Name = comboBox1.SelectedItem.ToString();
                // Заполнение названий столбцов (номер студента, фамилия, имя и так далее)
                sheet.StandardWidth = 25;
                sheet.Cells[1, 1] = "№";
                sheet.Columns[1].ColumnWidth = 5;
                sheet.Cells[1, 2] = "Фамилия";
                sheet.Cells[1, 3] = "Имя";
                sheet.Cells[1, 4] = "Отчество";
                sheet.Cells[1, 5] = "Средний балл";
                sheet.Cells[1, 6] = "Оригиналы документов";
                sheet.Cells[1, 7] = "Бюджетник";
                while (dataReader.Read())
                {
                    sheet.Cells[row, 1] = counter;
                    sheet.Cells[row, 2] = dataReader["lname"].ToString();
                    sheet.Cells[row, 3] = dataReader["fname"].ToString();
                    sheet.Cells[row, 4] = dataReader["mname"].ToString();
                    sheet.Cells[row, 5] = Convert.ToDecimal(dataReader["average_score"]);
                    sheet.Cells[row, 6] = dataReader["original_documents"].ToString();
                    sheet.Cells[row, 7] = dataReader["budget"].ToString();
                    counter++;
                    row++;
                }
                dataReader.Close();
            }
            catch(Exception)
            {

            }
        }
    }
}
