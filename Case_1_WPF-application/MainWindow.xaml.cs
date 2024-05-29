using System;
using System.Collections.Generic;
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


namespace Case_1_WPF_application
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		File_Reading f_reading = new File_Reading("Лист%20в%203#.xlsx"); // В качестве параметра указано название файла
		bool flag = false;
		public MainWindow()
		{
			InitializeComponent();
		}

		// Обработка события при введении текста пользователем
		private void year_TextChanged(object sender, TextChangedEventArgs e)
		{
			flag = false;
			try
			{
				// Ограничение диапазона вводимых символов
				if (year.Text.Length == 4)
				{
					var input = year.Text;
					int curr_year;
					if (!int.TryParse(input, out curr_year) || curr_year < 2020 || curr_year > 2050)
					{
						flag = true;				
						MessageBox.Show("Введите год в диапазоне от 2020 до 2050", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				if (year.Text.Length >= 5) 
				{
					flag = true;
					MessageBox.Show("Неверно введен год", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			catch (Exception error)
			{
				Console.WriteLine($"Ошибка: {error.Message}");
			}
		}

		// Обработка события при введении текста пользователем
		private void discount_rate_TextChanged(object sender, TextChangedEventArgs e)
		{
			flag = false;
			try
			{
				double selectRate = double.Parse(discount_rate.Text);
				if (selectRate <= 0.0 || selectRate > 1.0)
				{
					flag = true;	
					MessageBox.Show("Введите ставку дисконтирования в пределах от 0 до 1", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			catch (Exception error)
			{
				Console.WriteLine($"Ошибка: {error.Message}");
			}
		}

		private void count_Click(object sender, RoutedEventArgs e)
		{
			int selectYear = int.Parse(year.Text);
			double selectRate = double.Parse(discount_rate.Text);
			if(flag)
				MessageBox.Show("Проверьте введенные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			else
			{
				var result = f_reading.Count_NPV(selectYear, selectRate);
				if (result <= 0)
					MessageBox.Show("Ошибка в подсчете", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				else
				{
					resultLabel.Content = "";
					resultLabel.Content = result;
				}
			}
		}
	}
}
