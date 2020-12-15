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
        //public static string Connect = "Server=localhost;Database=students;user=root;password=123123;charset=utf8";// все строки переехали сюда чтобы был доступ у всех функций
        public static string Connect = "server=localhost;port=3306;username=root;password=root;database=students";
        public MySqlConnection con = new MySqlConnection(Connect);
        Random rand = new Random();
        public static string[] lname =  {"Смит", "Вэй", "Мюллер", "Дламини", "Сильва", "Сингх", "Морто", "Кринж", "Вортекс", "Трапой", "Стивенс", "Волкер", "Перри", "Элиот", "Сандерс", "Андерсон", "Хавкинс", "Майерс", "Лонг", "Джордан"};
        public static string[] fname = { "Алекс", "Кортни", "Тейлор", "Медисон", "Пейдж", "Эрин", "Пендс", "Флос", "Колд", "Джейсон", "Майк", "Корей", "Джозеп", "Сильвия", "Памела", "Руби", "Джон", "Александр", "Наоми", "Джанет" };
        public static string[] mname = { "Александровна", "Никитович", "Матвеевич", "Михайловна", "Денисович", "Романович", "Олегович", "Фортнайтович", "Артёмович", "Петрович", "Андреевич", "Мироновна", "Львовна", "Сергеевна", "Данилович", "Георгиевич", "Владимировна", "Павловна", "Лукич", "Саввич"};
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
                return lname[rand.Next(20)];
            }
            if (type == "fname")
            {
                return fname[rand.Next(20)];
            }
            if (type == "mname")
            {
                return mname[rand.Next(20)];
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

        private void button3_Click(object sender, EventArgs e) // Вывод отфильтрованных студентов в word
        {
            int group_counter = 0;
            string[] letter = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k" };
            int group_name = rand.Next(200, 800);
            int plebs = 0;
            int freePlebs = 0;
            int table_id = 2;
            int counter = 1;
            string sql1 = string.Format("select max_countPlebs, max_countFreePlebs from name_specialty where id_name_specialty = '{0}'", (comboBox1.SelectedIndex + 1));
            string sql_plebs = string.Format("select * from student where fk_id_original_documents = 1  and fk_id_name_specialty = {0} and budget = 'Да' order by average_score desc;", (comboBox1.SelectedIndex + 1));
            string sql_kings = string.Format("select * from student where fk_id_original_documents = 1  and fk_id_name_specialty = {0} and budget = 'Нет' order by average_score desc;", (comboBox1.SelectedIndex + 1));

            MySqlCommand cmd = new MySqlCommand(sql1, con);
            MySqlDataReader dataReader;
            if (comboBox1.Text == "<Выбор направления>")
            {
                MessageBox.Show("Выбери направление", "Ошибка");
                Application.Restart();
            }
                dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                plebs = Convert.ToInt32(dataReader["max_countPlebs"]);
                freePlebs = Convert.ToInt32(dataReader["max_countFreePlebs"]);
            }
            dataReader.Close();
            // Получить объект приложения Word.

            List<string> all_students = new List<string>();
            //List<string> all_students;
            MySqlCommand cmd1 = new MySqlCommand(sql_plebs, con);
            MySqlDataReader dataReader1;
            dataReader1 = cmd1.ExecuteReader();
            for (int i = 0; i < freePlebs; i++)
            {
                dataReader1.Read();
                all_students.Add(dataReader1["lname"].ToString() + " " + dataReader1["fname"].ToString() + " " + dataReader1["mname"].ToString());
            }
            all_students.Sort();
            dataReader1.Close();
            MySqlCommand cmd2 = new MySqlCommand(sql_kings, con);
            MySqlDataReader dataReader2;
            dataReader2 = cmd2.ExecuteReader();
            for (int i = 0; i < (plebs - freePlebs); i++)
            {
                dataReader2.Read();
                all_students.Add(dataReader2["lname"].ToString() + " " + dataReader2["fname"].ToString() + " " + dataReader2["mname"].ToString());
            }
            all_students.Sort();
            dataReader2.Close();
            // Работа с вордом, создание и всякое
            Word._Application word_app = new Word.Application();
            word_app.Visible = true;
            object missing = Type.Missing;
            Word._Document word_doc = word_app.Documents.Add(
                ref missing, ref missing, ref missing, ref missing);
            // Переменная ренжи и создание первой таблице с отдельной ренжой 0-25 т.к. она первая. Задание стиля для таблицы иначе его вообще не будет
            Word.Range initialRange;
            Word.Range myRange = word_doc.Range(0, 0);
            word_doc.Tables.Add(myRange, 1, 1, ref missing, ref missing);
            Word.Table table = word_doc.Tables[1];
            word_doc.Tables[1].Cell(1, 1).Range.Text = (group_name.ToString() + letter[group_counter]);
            initialRange = word_doc.Tables[1].Range;
            initialRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            initialRange.InsertParagraphAfter(); 
            initialRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
            word_doc.Tables.Add(initialRange, 25, 1, ref missing, ref missing);
            word_doc.Tables[table_id].set_Style("Сетка таблицы 1");
            // Заполнение таблиц элементами из листа all_students
            for (int i = 0; i < all_students.Count(); i++)
            {
                word_doc.Tables[table_id].Cell(counter, 1).Range.Text =  ((i % 25) + 1).ToString() + ". " + all_students[i];
                counter++;
                if (counter > 25 & (table_id) < (plebs/25) * 2)
                {
                    initialRange = word_doc.Tables[table_id].Range;
                    initialRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                    initialRange.InsertParagraphAfter();
                    initialRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                    table_id++;
                    counter = 1;
                    word_doc.Tables.Add(initialRange, 1, 1, ref missing, ref missing);
                    group_counter++;
                    word_doc.Tables[table_id].Cell(1, 1).Range.Text = (group_name.ToString() + letter[group_counter]);


                    initialRange = word_doc.Tables[table_id].Range;
                    initialRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                    initialRange.InsertParagraphAfter();
                    initialRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                    table_id++;
                    counter = 1;
                    word_doc.Tables.Add(initialRange, 25, 1, ref missing, ref missing);
                    word_doc.Tables[table_id].set_Style("Сетка таблицы 1");
                }
            }
  
        }
    }
}
