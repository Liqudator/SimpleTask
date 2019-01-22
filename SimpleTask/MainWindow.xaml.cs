using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Обработчик события на изменение текста в TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // массив char для хранения содержимого TextBox
            char[] bufferChars = new char[textBox.Text.Length];
            // стрим для чтения потока char из TextBox
            using (StringReader reader = new StringReader(textBox.Text))
            {
                // асинхронное чтение
                await reader.ReadAsync(bufferChars, 0, textBox.Text.Length);
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
                textBox.Text = sb.ToString();
            }
        }
    }
}
