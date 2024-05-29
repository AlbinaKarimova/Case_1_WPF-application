using Spire.Xls;
using Spire.Xls.AI;
using Spire.Xls.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OfficeOpenXml;
using System.IO;
using Aspose.Cells;
using System.Windows.Markup;
using System.Windows;

namespace Case_1_WPF_application
{

	// Класс для работы с файловыми данными
	public class File_Reading
	{
		public string f_name;

		public File_Reading(string name)
		{
			this.f_name = name;
		}

		// Метод для считывания данных из файла с расширением xlsx
		// Возвращаемым значением является массив с элементами типа UsingData
		private UsingData[] Read()
		{
			try
			{
				FileInfo file = new FileInfo(f_name);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using (var package = new ExcelPackage(file))
				{
					using (ExcelWorksheet workSheet = package.Workbook.Worksheets[1])
					{
						UsingData[] data = new UsingData[workSheet.Dimension.End.Row];
						int countData = 0;
						// Выгрузка всех значений
						var list1 = workSheet.Cells[workSheet.Dimension.Start.Row + 1, 1, workSheet.Dimension.End.Row, 3].ToList();
						//var list2 = workSheet.Cells[workSheet.Dimension.Start.Row + 1, 2, workSheet.Dimension.End.Row, 1].ToList();
						//var list3 = workSheet.Cells[workSheet.Dimension.Start.Row + 1, 3, workSheet.Dimension.End.Row, 1].ToList();
						for (int i = 0; i < list1.Count; i += 3)
						{
							string str1 = list1[i].Text;
							//string str2 = list2[i].Text;
							//string str3 = list3[i].Text;
							data[countData] = new UsingData();
							data[countData].year = int.Parse(list1[i].Text);
							data[countData].income = double.Parse(list1[i+1].Text);
							data[countData].expenses = double.Parse(list1[i+2].Text);
							countData++;
						}

						return data;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error read xslx!!! " + ex.Message.ToString());
			}
			return null;
		}

		// Расчёт NPV во данным, которые ввел пользователь
		public double Count_NPV(int select_year, double select_discountRate)
		{
			UsingData[] new_data = Read(); // Считываем данные с файла
			double current_NPV = 0.0;
			for (int i = 0; i < new_data.Length; i++)
			{
				new_data[i].difference = new_data[i].income - new_data[i].expenses; // Считаем чистый денежный поток
				if (i == 0)
				{
					new_data[i].NPV = new_data[i].difference / Math.Pow(1 + select_discountRate, i + 1);
				}
				else
				{
					new_data[i].NPV = new_data[i].difference / Math.Pow(1 + select_discountRate, i + 1) + new_data[i-1].NPV;
				}

				if(select_year == new_data[i].year)
				{
					current_NPV = new_data[i].NPV;
					break;
					//return Math.Round(current_NPV, 3);
				}
			}

			if (current_NPV != 0.0)
				return Math.Round(current_NPV, 3); // Округляем значение для удобства просмотра результата в окне
			else
				throw new Exception("Для выбранного года невозможно посчитать NPV. Возможно недостаточно известных данных");
		}
	}
}

		
