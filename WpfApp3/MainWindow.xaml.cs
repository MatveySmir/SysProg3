using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
            public MainWindow()
            {
                InitializeComponent();
            }

            private void SetEnabled(bool state)
            {
                StartWithoutThreadButton.IsEnabled = state;  //блокируем кнопки
                StartWithThreadButton.IsEnabled = state;      //блокируем кнопки
                System.Windows.Forms.Application.DoEvents();  //
            }

            private void StartWithoutThreadButton_CLick(object sender, RoutedEventArgs e)
            {
            if (long.TryParse(FirstNumTextBox.Text, out long number) && long.TryParse(SecondNumTextBox.Text, out long number1))
            {
                long num1 = long.Parse(FirstNumTextBox.Text);
                long num2 = long.Parse(SecondNumTextBox.Text);
                if (num1 < 0)
                    num1 = num1 * (-1);
                if (num2 < 0)
                    num2 = num2 * (-1);
                ResultListBox.Items.Clear();
                SetEnabled(false);

                try
                {
                    if (IsNumPrime(num1))
                    {
                        ResultListBox.Items.Add("Первое число является простым");
                    }
                    else
                    {
                        ResultListBox.Items.Add("Первое число не является простым");
                    }
                }
                catch (FormatException)
                {
                    ResultListBox.Items.Add("Ошибка ввода первого числа");
                }

                try
                {
                    if (IsNumPrime(num2))
                    {
                        ResultListBox.Items.Add("Второе число является простым");
                    }
                    else
                    {
                        ResultListBox.Items.Add("Второе число не является простым");
                    }

                }
                catch (FormatException)
                {
                    ResultListBox.Items.Add("Ошибка ввода второго числа");
                }

                System.Windows.Forms.Application.DoEvents();
                SetEnabled(true);
            }
        }
            
            private void StartWithThreadButton_CLick(object sender, RoutedEventArgs e)
            {
            if (long.TryParse(FirstNumTextBox.Text, out long number) && long.TryParse(SecondNumTextBox.Text, out long number1))
            {
                long num1 = long.Parse(FirstNumTextBox.Text);
                long num2 = long.Parse(SecondNumTextBox.Text);
                if (num1 < 0)
                    num1 = num1 * (-1);
                if (num2 < 0)
                    num2 = num2 * (-1);


                ResultListBox.Items.Clear();
                SetEnabled(false);
                try
                {
                    string name1 = "Первое число";
                    threadParams p1 = new threadParams();
                    p1.num = num1;
                    p1.name = name1;
                    Thread t1 = new Thread(new ParameterizedThreadStart(ExecuteThread));

                    string name2 = "Второе число";
                    threadParams p2 = new threadParams();
                    p2.num = num2;
                    p2.name = name2;
                    Thread t2 = new Thread(new ParameterizedThreadStart(ExecuteThread));


                    t1.Start(p1);
                    t2.Start(p2);
                    //t1.Join();
                    //t2.Join();
                    System.Windows.Forms.Application.DoEvents();
                }
                catch
                {
                    ResultListBox.Items.Add("Critical ERROR!!!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Введите число!!!", "Ошибка", MessageBoxButton.OK);
            }
        }

        private void ExecuteThread(object threadParams)
        {
            long num = (threadParams as threadParams).num;
            string name = (threadParams as threadParams).name;

            if (IsNumPrime(num))
            {
                Dispatcher.BeginInvoke(new Action(
                    () =>
                    {
                        ResultListBox.Items.Add($"{name} является простым");
                        Enables();
                    }));
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(
                    () =>
                    {
                        ResultListBox.Items.Add($"{name} не является простым");
                        Enables();
                    }));
            }
        }

        protected byte Enable = 0;
        private void Enables()
        {
            Enable++;
            if (Enable == 2)
            {
                SetEnabled(true);
                Enable = 0;
            }
        }

        private bool IsNumPrime(long num)
            {
                for (long i = 2; i < num; i++)
            {
                if (num % i == 0)
                    return false;
            }
            return true;
            }


    }     
}
