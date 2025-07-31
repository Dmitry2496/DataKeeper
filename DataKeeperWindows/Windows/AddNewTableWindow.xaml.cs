using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataKeeperWindows.Classes;

namespace DataKeeperWindows.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddNewTableWindow.xaml
    /// </summary>
    public partial class AddNewTableWindow : Window
    {
        public ObservableCollection<SimpleString> Lines { get; set; } = [new("")];

        public AddNewTableWindow()
        {
            InitializeComponent();
        }

        public void GetTableParams(out string nameTable, out List<string> nameColumns)
        {
            nameTable = textBoxName.Text;
            nameColumns = [];
            foreach (SimpleString line in Lines)
            {
                nameColumns.Add(line.Value);
            }
        }



        private void TopPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNumberDataCorrect())
            {
                DialogResult = true;
            }
            else
            {
                textBoxNumber.Focus();
            }
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }



        private void TextBoxNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxNumberDataCorrect() || textBoxNumber.Text is "" or null)
            {
                return;
            }


            textBoxNumber.Text = new(
                (from p in textBoxNumber.Text.ToCharArray()
                 where char.IsDigit(p)
                 select p
                 ).ToArray());
        }
        private void TextBoxNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBoxNumberDataCorrect())
                {
                    int newNumberLines = int.Parse(textBoxNumber.Text);
                    if (Lines.Count < newNumberLines)
                    {
                        for (; Lines.Count < newNumberLines;)
                        {
                            Lines.Add(new SimpleString(""));
                        }
                    }
                    else if (newNumberLines < Lines.Count)
                    {
                        for (; newNumberLines < Lines.Count;)
                        {
                            Lines.RemoveAt(Lines.Count - 1);
                        }
                    }
                }
            }
            catch (Exception) { }
        }
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
            }

            if (e.Key == Key.Enter)
            {
                if (textBoxName.IsFocused)
                {
                    textBoxNumber.Focus();
                }
                else if (textBoxNumber.IsFocused)
                {
                    if (TextBoxNumberDataCorrect())
                    {
                        dataGrid.SelectedIndex = 0;
                        dataGrid.Focus();
                    }
                }
                else if (dataGrid.IsFocused) { }
                else
                {
                    if (TextBoxNumberDataCorrect())
                    {
                        DialogResult = true;
                    }
                    else
                    {
                        textBoxNumber.Focus();
                    }
                }
            }
        }



        private bool TextBoxNumberDataCorrect()
        {
            try
            {
                string text = textBoxNumber!.Text;
                Regex regex = RegexNumber();
                return regex.IsMatch(text) && int.Parse(text) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //[GeneratedRegex("[0-9]+")]  // В строке помимо прочего обязательно должны быть цифры
        [GeneratedRegex("^\\d+$")]    // В строке только цифры
        private static partial Regex RegexNumber();
    }
}
