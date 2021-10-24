using System;
using System.Windows.Forms;
using System.Linq;

namespace Лабораторная_работа__2__задание_2_
{
    public partial class Form1 : Form
    {
        public static bool enterLast = false; // проверка, введена ли последовательность цифр и нажата клавиша Enter
        public Form1()
        {
            InitializeComponent(); // вызов метода который формирует поля на форме, добавляет свойства,
            // всё то, что находится в Form1.Designer.cs

            txtOrder.Text = Properties.Settings.Default.order.ToString(); // считываем значения из настроек

            this.KeyPreview = true; // обрабатываем клавиши на уровне формы

            // отпускается клавиша, выполняется код Form1_KeyUp
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);

            // метод KeyEventHandler обрабатывает событие KeyDown, которое срабатывает, когда нажата клавиша
            txtOrder.KeyDown += new KeyEventHandler(keydown);
        }
        private void keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // если нажата клавиша Enter
            {
                txtOrder.Focus(); // установка фокуса на TextBox последовательности цифр
                enterLast = true; // введена последовательность цифр
                e.SuppressKeyPress = true; // отключаем системный звук
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (enterLast) // если введена последовательность цифр
            {
                button1.PerformClick(); // вызываем подпрограмму button1_Click
                this.Close(); // закрываем форму
            }
        }
        private void button1_Click(object sender, EventArgs e) // реакция на клик
        {
            string continuity; // последовательность цифр
            continuity = this.txtOrder.Text; // сохраняем введенную последовательность в переменные           
            Properties.Settings.Default.order = continuity; //  передаем введеное значение в параметры
            Properties.Settings.Default.Save(); // сохраняем переданные значения,
                                                // чтобы они восстановились пре очередном запуске

            if (Logic.Compare(continuity) != String.Empty) // если возвращаемое значение не является пустым сообщение-ошибкой, то
            {
                MessageBox.Show(Logic.Compare(continuity)); // выводим ответ
            }
        }
    }
    public class Logic // класс, где хранится логика
    {
        // функция Compare нужна, чтобы сформировать результирующее сообщение
        public static string Compare(string continuity)
        {
            string answer = ""; // сообщение-вывод
            string messageError = ""; // сообщение-ошибка
            bool result = true; // проверка, упорядочена ли последовательность по возрастанию

            // заменяем в строке все пробелы на String.Empty, то есть на "ничего"
            continuity = System.Text.RegularExpressions.Regex.Replace(continuity, "[ ]", String.Empty);
            string[] numbers; // массив подстрок               
            numbers = continuity.Split(','); // введённая строка разбивается на массив подстрок,
                                             // символом-разделителем является запятая

            string temp = numbers[0]; // временная переменная со значением предудущей цифры,
                                      // с которой будет сравниваться текущая цифра

            if (!continuity.ToLower().Contains(',')) // если в строке отсутствует запятая 
            {
                messageError = "Некорректный ввод, цифры вводятся через запятую";
                return messageError; // возвращаем пустое сообщение-ошибку
            }
            try
            {
                for (int i = 1; i < numbers.Length; i++)
                {
                    // если текущий элемент массива состоит из одного символа, как и предыдущий
                    if ((numbers[i].Length == 1) && (numbers[0].Length == 1))
                    {
                        // если текущая цифра <= предыдущей цифре
                        if (Convert.ToInt32(numbers[i]) <= Convert.ToInt32(temp))
                        {
                            result = false; // последовательность неупорядочена                                
                        }
                        temp = numbers[i]; // во временную переменную присваиваем
                                           // значение текущей цифры
                    }
                    else
                    {
                        messageError = "Некорректный ввод";
                        return messageError; // возвращаем пустое сообщение-ошибку
                    }
                }
            }
            catch (Exception)
            {
                messageError = "Некорректный ввод";
                return messageError; // возвращаем пустое сообщение-ошибку
            }
            // если последовательность упорядочена по возрастанию 
            if (result)
                answer = "Последовательность упорядочена по возрастанию";
            // если последовательность не упорядочена по возрастанию 
            else
                answer = "Последовательность не упорядочена по возрастанию";
            return answer;
        }
    }
}