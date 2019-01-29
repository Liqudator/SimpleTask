using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox t = new TextBox();

        public MainWindow()
        {
            InitializeComponent();
        } 
       
        /// <summary>
        /// Кнопка очистки окна TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonForClear_Click(object sender, RoutedEventArgs e)
        {
            textBox.Clear();
        }

        /// <summary>
        /// Асинхронный метод поиска подстроки в поле текста TextBox
        /// </summary>
        /// <param name="str1">Текущая строка из TextBox</param>
        /// <param name="str2">Стравниваемая подстрока</param>
        private async void FoundStringAsync()
        {
            await Task.Run(() => FoundString());   
        }


        private async void FoundString()
        {
            /*// массив char для хранения содержимого TextBox
            //char[] bufferChars = new char[t.Text.Length];
            int lenghtTextIsTextBox = await t.Text.Length;
            char[] bufferChars = new char[lenghtTextIsTextBox];            
            // стрим для чтения потока char из TextBox
            using (StringReader reader = new StringReader(t.Text))
            {
                // асинхронное чтение
                await reader.ReadAsync(bufferChars, 0, t.Text.Length);
            }

            // контейнер для записи результата
            StringBuilder sb = new StringBuilder();
            // стрим для записи потока string
            using (StringWriter writer = new StringWriter(sb))
            {
                foreach (char c in bufferChars)
                {
                    // если символ - это пробел
                    if (c == ' ')
                    {
                        // записываем вместо пробела перенос строки 
                        await writer.WriteLineAsync();
                        // возвращаем каретку
                        //await writer.WriteAsync('\n');
                    }
                    else
                    {
                        // 
                        await writer.WriteAsync(c);
                    }
                }
                
                // запись результата в TextBox
                t.Text = sb.ToString();
            }*/
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            t = Clone<TextBox>(textBox);        
            char[] bufferChars = new char[t.Text.Length];
            // стрим для чтения потока char из TextBox
            using (StringReader reader = new StringReader(t.Text))
            {
                // асинхронное чтение
                await reader.ReadAsync(bufferChars, 0, t.Text.Length);
            }

            // контейнер для записи результата
            StringBuilder sb = new StringBuilder();
            // стрим для записи потока string
            using (StringWriter writer = new StringWriter(sb))
            {
                foreach (char c in bufferChars)
                {
                    // если символ - это пробел
                    if (c == ' ')
                    {
                        // записываем вместо пробела перенос строки 
                        //await writer.WriteLineAsync();
                        // возвращаем каретку
                        await writer.WriteAsync("\r\n");
                    }
                    else
                    {
                        // 
                        await writer.WriteAsync(c);
                    }
                }

                // запись результата в TextBox
                t.Text = sb.ToString();
            }
            await Task.Run(() => { textBox = Clone<TextBox>(t); });
            //Task.Run(textBox = Clone<TextBox>(t));
        }

        /// <summary>
        /// Обобщенный метод клонирования объектов Contol
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T Clone<T>(T t)
        {
            // создание экземпляра textbox на этапе выполнения (позднее связывание)
            T instance = Activator.CreateInstance<T>();
            Type control = t.GetType();
            PropertyInfo[] info = control.GetProperties();

            foreach(PropertyInfo pi in info)
            {
                if((pi.CanWrite) && !(pi.Name == "WindowTarget") && !(pi.Name == "Capture"))
                { 
                    pi.SetValue(instance, pi.GetValue(t, null));
                    //MessageBox.Show(String.Format($"Name = {pi.Name}\nValue = {pi.GetValue(t, null)}"));
                }
            }
            return instance;
        }
    }
}


// использовать копирование объекта, извлечь состояние объекта, изменить и заменить в основной поток
