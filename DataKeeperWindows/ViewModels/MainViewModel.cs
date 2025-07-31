using DataKeeperWindows.Classes;
using DataKeeperWindows.Commands;
using DataKeeperWindows.Windows;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace DataKeeperWindows.ViewModels
{
    internal class MainViewModel : NotifyPropertyChanged
    {
        #region constructors
        public MainViewModel()
        {
            TablesView = [];
            StorageName = string.Empty;
            AddStorageEvents();
        }
        public MainViewModel(string filePath)
        {
            TablesView = [];
            StorageName = string.Empty;

            AddStorageEvents();


            // Открытие файла
            bool openFileSuccess = OpenStorageConnection(filePath);
            if (!openFileSuccess)
            {
                return;
            }

            UpdateAllViewDataAsync();
        }
        public MainViewModel(SturtupActions sturtupAction)
        {
            TablesView = [];
            StorageName = string.Empty;

            AddStorageEvents();

            // Получения пути к файлу
            string filePath;
            switch (sturtupAction)
            {
                case SturtupActions.CreateFile:
                    filePath = CreateNewFileDialog();
                    if (filePath == string.Empty)
                    {
                        return;
                    }
                    break;
                case SturtupActions.OpenFile:
                    filePath = OpenFileDialog();
                    if (filePath == string.Empty)
                    {
                        return;
                    }
                    break;
                default:
                    return;
            }

            // Открытие файла
            bool openFileSuccess = OpenStorageConnection(filePath);
            if (!openFileSuccess)
            {
                return;
            }

            UpdateAllViewDataAsync();
        }
        #endregion



        #region public
        public ObservableCollection<TableView> TablesView
        {
            get => _tablesView;
            private set
            {
                _tablesView = value;
                OnPropertyChanged();
            }
        }
        public string StorageName
        {
            get => _storageName;
            private set
            {
                _storageName = value;
                OnPropertyChanged();
            }
        }

        public bool OpenFileCorrect
        {
            get => _openFileCorrect;
            private set
            {
                _openFileCorrect = value;
                OnPropertyChanged();
            }
        }
        #endregion



        #region private 
        private readonly Storage _storage = new();
        private ObservableCollection<TableView> _tablesView = [];
        private string _storageName = string.Empty;
        private bool _openFileCorrect = false;


        /// <summary>
        /// Обновить все данные
        /// </summary>
#pragma warning disable IDE0051 // Удалите неиспользуемые закрытые члены
        private void UpdateAllViewData()
#pragma warning restore IDE0051 // Удалите неиспользуемые закрытые члены
        {
            // Формирование названий таблиц
            bool requestAllNameTablesSuccess = RequestAllTablesName();
            if (!requestAllNameTablesSuccess)
            {
                return;
            }

            // Формирвование названий столбцов
            bool requestAllColumnsNameSuccess = RequestAllColumnsName();
            if (!requestAllColumnsNameSuccess)
            {
                return;
            }

            // Строки
            _ = RequestAllCellsContent();
        }

        private async void UpdateAllViewDataAsync()
        {
            // Формирование названий таблиц
            bool requestAllNameTablesSuccess = await RequestAllTablesNameAsync();
            if (!requestAllNameTablesSuccess)
            {
                return;
            }


            // Формирвование названий столбцов
            bool requestAllColumnsNameSucces = await RequestAllColumnsNameAsync();
            if (!requestAllColumnsNameSucces)
            {
                return;
            }


            // Строки
            _ = await RequestAllCellsContentAsync();
        }

        /// <summary>
        /// Обновить таблицу в соответствии с фильтрами
        /// </summary>
        private async void UpdateTableWithFilterAsync(int tableNumber, string searchText, bool considerSignRegister = false) => _ = await RequestAllTableCellsWithFilterAsync(tableNumber, searchText, considerSignRegister);

        /// <summary>
        /// Запросить названия всех таблиц (чтение из файла и запись в TableView)
        /// </summary>
        /// <returns>Результат запроса</returns>
        private bool RequestAllTablesName()
        {
            using PleaseWaitWindow pleaseWaitWindow = new("Чтение названий таблиц");
            pleaseWaitWindow.Show();

            try
            {
                List<string>? readTablesName = _storage.GetTablesName();
                if (readTablesName == null)
                {
                    return false;
                }
                TablesView = new ObservableCollection<TableView>((
                    from item in readTablesName
                    select new TableView(item)).ToList());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private async Task<bool> RequestAllTablesNameAsync()
        {
            using PleaseWaitWindow pleaseWaitWindow = new("Чтение названий таблиц");
            pleaseWaitWindow.Show();

            try
            {
                List<string>? readTablesName = await _storage.GetTablesNameAsync();
                if (readTablesName == null)
                {
                    return false;
                }
                TablesView = new ObservableCollection<TableView>((
                    from item in readTablesName
                    select new TableView(item)).ToList());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Запросить названия всех колонок в таблице (чтение из файла и запись в TableView)
        /// </summary>
        /// <returns>Результат запроса</returns>
        private bool RequestAllColumnsName()
        {
            using PleaseWaitWindow pleaseWaitWindow = new("Чтение названий колонок");
            pleaseWaitWindow.Show();

            if (TablesView.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < TablesView.Count; i++)
            {
                List<string>? readColumnsName = _storage.GetColumnsName(TablesView[i].NameTable);
                if (readColumnsName == null)
                {
                    return false;
                }
                foreach (string item in readColumnsName)
                {
                    _ = TablesView[i].Table.Columns.Add(item);
                }
            }
            return true;
        }
        private async Task<bool> RequestAllColumnsNameAsync()
        {
            using PleaseWaitWindow pleaseWaitWindow = new("Чтение названий колонок");
            pleaseWaitWindow.Show();

            if (TablesView.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < TablesView.Count; i++)
            {
                List<string>? readColumnsName = await _storage.GetColumnsNameAsync(TablesView[i].NameTable);
                if (readColumnsName == null)
                {
                    return false;
                }
                foreach (string item in readColumnsName)
                {
                    _ = TablesView[i].Table.Columns.Add(item);
                }
            }
            return true;
        }

        /// <summary>
        /// Запросить содержимое всех ячеек в таблице (чтение из файла и запись в TableView)
        /// </summary>
        /// <returns>Результат запроса</returns>
        private bool RequestAllCellsContent()
        {
            using PleaseWaitWindow pleaseWaitWindow = new("Чтение данных");
            pleaseWaitWindow.Show();

            if (TablesView.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < TablesView.Count; i++)
            {
                List<List<string>>? readcellsContent = _storage.GetCellsContent(TablesView[i].NameTable);
                if (readcellsContent == null)
                {
                    return false;
                }
                for (int j = 0; j < readcellsContent.Count; j++)
                {
                    DataRow r = TablesView[i].Table.NewRow();
                    for (int k = 0; k < readcellsContent[j].Count; k++)
                    {
                        r[k] = readcellsContent[j][k];
                    }
                    TablesView[i].Table.Rows.Add(r);
                }
            }
            return true;
        }
        private async Task<bool> RequestAllCellsContentAsync()
        {
            using PleaseWaitWindow pleaseWaitWindow = new("Чтение данных");
            pleaseWaitWindow.Show();

            if (TablesView.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < TablesView.Count; i++)
            {
                List<List<string>>? readcellsContent = await _storage.GetCellsContentAsync(TablesView[i].NameTable);
                if (readcellsContent == null)
                {
                    return false;
                }
                for (int j = 0; j < readcellsContent.Count; j++)
                {
                    DataRow r = TablesView[i].Table.NewRow();
                    for (int k = 0; k < readcellsContent[j].Count; k++)
                    {
                        r[k] = readcellsContent[j][k];
                    }
                    TablesView[i].Table.Rows.Add(r);
                }
            }
            return true;
        }

        /// <summary>
        /// Запросить содержимое всех ячеек в таблице с фильтрацией (чтение из файла и запись в TableView)
        /// </summary>
        /// <returns>Результат запроса</returns>
        private async Task<bool> RequestAllTableCellsWithFilterAsync(int tableNumber, string searchText, bool considerSignRegister = false)
        {
            using PleaseWaitWindow pleaseWaitWindow = new("Применение фильтров");
            pleaseWaitWindow.Show();

            if (TablesView.Count <= tableNumber)
            {
                return true;
            }

            List<List<string>>? readcellsContent = await _storage.GetCellsContentAsync(TablesView[tableNumber].NameTable);
            if (readcellsContent == null)
            {
                return false;
            }

            TablesView[tableNumber].Table.Rows.Clear();

            for (int j = 0; j < readcellsContent.Count; j++)
            {
                bool rowContainsSubstring = (from p in readcellsContent[j]
                                             where p.Contains(searchText, considerSignRegister ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)
                                             select p).Any();

                if (!rowContainsSubstring)
                {
                    continue;
                }

                DataRow r = TablesView[tableNumber].Table.NewRow();
                for (int k = 0; k < readcellsContent[j].Count; k++)
                {
                    r[k] = readcellsContent[j][k];
                }
                TablesView[tableNumber].Table.Rows.Add(r);
            }
            return true;
        }

        /// <summary>
        /// Подписаться на все события объекта
        /// </summary>
        private void AddStorageEvents()
        {
            _storage.ChangeFileNameEvent += Storage_ChangeFileNameEvent;
        }

        /// <summary>
        /// Получение названия файла, которое не дублируется
        /// </summary>
        /// <returns>Новое название файла, которое отсутствует в базе</returns>
        private string GetNotRepeatingTableName(string name, int iteration = 0)
        {
            string newName = iteration == 0 ? name : name + iteration.ToString();

            return (from p in TablesView
                    where p.NameTable == newName
                    select p
                    ).Any() ?
                    GetNotRepeatingTableName(name, ++iteration) :
                    newName;
        }

        /// <summary>
        /// ОТкрыть соединение с базой данных
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Результат подключения</returns>
        private bool OpenStorageConnection(string filePath)
        {
            bool openFileSuccess = _storage.OpenNewConnection(filePath);
            if (openFileSuccess)
            {
                OpenFileCorrect = true;
                return true;
            }
            else
            {
                OpenFileCorrect = false;
            }

            return OpenEncryptedStorageConnection(filePath);
        }

        /// <summary>
        /// ОТкрыть соединение с зашифрованной базой данных
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Результат подключения</returns>
        private bool OpenEncryptedStorageConnection(string filePath)
        {
            InputPasswordWindow inputPasswordWindow = new();
            if (inputPasswordWindow.ShowDialog() == true)
            {
                string inputPassword = inputPasswordWindow.password.Password.ToString();
                if (_storage.OpenNewConnection(filePath, inputPassword))
                {
                    OpenFileCorrect = true;
                    return true;
                }
                else
                {
                    return OpenEncryptedStorageConnection(filePath);
                }
            }
            else
            {
                _ = new MessageBoxWindow("Не удалось открыть файл", "Ошибка открытия файла", MessageBoxButton.OK, MessageBoxImage.Error).ShowDialog();
                OpenFileCorrect = false;
                return false;
            }
        }

        /// <summary>
        /// Создать новый файл на дисковом простанстве
        /// </summary>
        /// <returns>Путь к созданному файлу</returns>
        private static string CreateNewFileDialog()
        {
            SaveFileDialog createFileDialog = new()
            {
                // DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DefaultExt = ".db",
                AddExtension = true,
                Filter = "Файлы SQLite базы данных (*.db *.sqlite *.sqlite3 *.db3)|*.db;*.sqlite;*.sqlite3;*.db3|Все файлы (*.*)|*.*",
                Title = "Создание файла",
            };
            if (createFileDialog.ShowDialog() == true)
            {
                if (!createFileDialog.FileName.EndsWith(".db") &&
                    !createFileDialog.FileName.EndsWith(".sqlite") &&
                    !createFileDialog.FileName.EndsWith(".sqlite3") &&
                    !createFileDialog.FileName.EndsWith(".db3"))
                {
                    _ = new MessageBoxWindow("\"DataKeeper\" не может создать файл с данным расширением", "Ошибка создания файла", MessageBoxButton.OK, MessageBoxImage.Error).ShowDialog();
                    return string.Empty;
                }
                else
                {
                    // Если файл существует, удалить его?
                    if (File.Exists(createFileDialog.FileName))
                    {
                        File.Delete(createFileDialog.FileName);
                    }
                    return createFileDialog.FileName;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Открыть файл на дисковом простанстве
        /// </summary>
        /// <returns>Путь к файлу</returns>
        private static string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new()
            {
                DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DefaultExt = ".db",
                AddExtension = true,
                Filter = "Файлы базы данных (*.db *.sqlite *.sqlite3 *.db3)|*.db;*.sqlite;*.sqlite3;*.db3|Все файлы (*.*)|*.*",
                Title = "Открытие файла",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                if (!File.Exists(openFileDialog.FileName))
                {
                    _ = new MessageBoxWindow("Файл не найден", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error).ShowDialog();
                    return string.Empty;
                }

                if (openFileDialog.FileName.EndsWith(".db") ||
                    openFileDialog.FileName.EndsWith(".sqlite") ||
                    openFileDialog.FileName.EndsWith(".sqlite3") ||
                    openFileDialog.FileName.EndsWith(".db3"))
                {
                    return openFileDialog.FileName;
                }
                else
                {
                    _ = new MessageBoxWindow("\"DataKeeper\" не может открыть файл с данным расширением", "Ошибка открытия файла", MessageBoxButton.OK, MessageBoxImage.Error).ShowDialog();
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion



        #region Events
        private void Storage_ChangeFileNameEvent()
        {
            StorageName = _storage.FileName ?? string.Empty;
        }
        #endregion



        #region Commands
        public ICommand CreateNewFileCommand => new RelayCommand(parametr =>
        {
            // Получения пути к файлу
            string filePath = CreateNewFileDialog();
            if (filePath == string.Empty)
            {
                return;
            }

            TablesView = [];
            StorageName = string.Empty;


            // Открытие файла
            bool openFileSuccess = OpenStorageConnection(filePath);
            if (!openFileSuccess)
            {
                return;
            }


            UpdateAllViewDataAsync();
        });

        public ICommand OpenFileCommand => new RelayCommand(parametr =>
        {
            // Получения пути к файлу
            string filePath = OpenFileDialog();
            if (filePath == string.Empty)
            {
                return;
            }

            TablesView = [];
            StorageName = string.Empty;


            // Открытие файла
            bool openFileSuccess = OpenStorageConnection(filePath);
            if (!openFileSuccess)
            {
                return;
            }


            UpdateAllViewDataAsync();
        });

        public ICommand AddNewTableCommand => new RelayCommand(parametr =>
        {
            if (!OpenFileCorrect)
            {
                return;
            }


            AddNewTableWindow addDockumentWindow = new() { Owner = Application.Current.MainWindow };
            if (addDockumentWindow.ShowDialog() != true)
            {
                return;
            }

            addDockumentWindow.GetTableParams(out string name, out List<string> columns);
            name = GetNotRepeatingTableName(name);  // Исключение дублирования таблиц
            string sColumns = $"id INTEGER PRIMARY KEY AUTOINCREMENT";
            foreach (string? item in columns.Distinct() /*Убираем дублированные названия*/)
            {
                if (item is null or "")
                {
                    continue;
                }

                sColumns += $", \"{item}\" TEXT";
            }

            if (_storage.CreateNewTable(name, sColumns))
            {
                TablesView.Add(new TableView(name));

                List<string>? readColumnsName = _storage.GetColumnsName(name);
                if (readColumnsName == null)
                {
                    return;
                }
                foreach (string item in readColumnsName)
                {
                    _ = TablesView[^1].Table.Columns.Add(item);
                }
            }
        });

        public ICommand RemoveTableCommand => new RelayCommand(parametr =>
        {
            if (!OpenFileCorrect || parametr == null || (int)parametr < 0 || (int)parametr >= TablesView.Count)
            {
                return;
            }


            bool? res = new MessageBoxWindow($"Вы действительно хотите удалить таблицу \"{TablesView[(int)parametr].NameTable}\"?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning).ShowDialog();

            if (res != true)
            {
                return;
            }

            if (_storage.RemoveTable(TablesView[(int)parametr].NameTable))
            {
                TablesView.RemoveAt((int)parametr);
            }
        });

        public ICommand RenameTableCommand => new RelayCommand(parametr =>
        {
            if (!OpenFileCorrect || parametr == null || (int)parametr < 0 || (int)parametr >= TablesView.Count)
            {
                return;
            }


            RenameTableWindow renameTableWindow = new()
            {
                Names = (from p in TablesView select p.NameTable).ToList(),
                Index = (int)parametr
            };

            if (renameTableWindow.ShowDialog() != true)
            {
                return;
            }

            string oldName = TablesView[(int)parametr].NameTable;
            string newName = renameTableWindow.inputName.Text;

            bool? res = new MessageBoxWindow($"Подтвердите переименование: \"{oldName}\"=>\"{newName}\"", "Подтверждение действия", MessageBoxButton.YesNo, MessageBoxImage.Information).ShowDialog();
            if (res != true)
            {
                return;
            }

            if (_storage.RenameTable(oldName, newName))
            {
                TablesView[(int)parametr].NameTable = newName;
            }
        });

        public ICommand UpdateAllCommand => new RelayCommand(parametr =>
        {
            if (!OpenFileCorrect)
            {
                return;
            }

            UpdateAllViewDataAsync();
        });

        public ICommand SearchCommand => new RelayCommand(parametr =>
        {
            if (!OpenFileCorrect)
            {
                return;
            }


            if (parametr is int tableNumber)
            {
                if (0 <= tableNumber && tableNumber < TablesView.Count)
                {
                    UpdateTableWithFilterAsync(tableNumber, ""); // Убираем фильтр
                }
            }
            else if (parametr is (int, string))
            {
                (int, string) parametrs = ((int, string))parametr;
                if (0 <= parametrs.Item1 && parametrs.Item1 < TablesView.Count)
                {
                    UpdateTableWithFilterAsync(parametrs.Item1, parametrs.Item2); // Применяем фильтр без учета регистра
                }
            }
            else if (parametr is (int, string, bool))
            {
                (int, string, bool) parametrs = ((int, string, bool))parametr;
                if (0 <= parametrs.Item1 && parametrs.Item1 < TablesView.Count)
                {
                    UpdateTableWithFilterAsync(parametrs.Item1, parametrs.Item2, parametrs.Item3); // Применяем фильтр
                }
            }
        });

        public ICommand AddRowCommand => new RelayCommand(parametr =>
        {
            if (!OpenFileCorrect || parametr == null || (int)parametr < 0 || (int)parametr >= TablesView.Count)
            {
                return;
            }

            int idNewRow = _storage.AddEmptyRow(TablesView[(int)parametr].NameTable);
            if (idNewRow == -2)
            {
                DataRow row = TablesView[(int)parametr].Table.NewRow();
                TablesView[(int)parametr].Table.Rows.Add(row);
            }
            else if (0 < idNewRow)
            {
                DataRow row = TablesView[(int)parametr].Table.NewRow();
                row[0] = idNewRow;
                TablesView[(int)parametr].Table.Rows.Add(row);
            }
        });

        public ICommand RemoveRowCommand => new RelayCommand(parametr =>
        {
            if (!OpenFileCorrect)
            {
                return;
            }


            else if (parametr is (int, int))
            {
                (int, int) parametrs = ((int, int))parametr;
                int tableNumber = parametrs.Item1;
                int rowNumber = parametrs.Item2;

                if (0 <= tableNumber && tableNumber < TablesView.Count &&
                    0 <= rowNumber && rowNumber < TablesView[tableNumber].Table.Rows.Count)
                {
                    if (_storage.RemoveRow(TablesView[tableNumber].NameTable, [(string)TablesView[tableNumber].Table.Rows[rowNumber][0]]))
                    {
                        TablesView[tableNumber].Table.Rows.RemoveAt(rowNumber);
                    }
                }
            }
            else if (parametr is (int, List<int>))
            {
                (int, List<int>) parametrs = ((int, List<int>))parametr;
                int tableNumber = parametrs.Item1;
                List<int> rowsNumbers = parametrs.Item2;

                if (0 <= tableNumber && tableNumber < TablesView.Count)
                {
                    string[] rowsId = (from p in rowsNumbers
                                       select (TablesView[tableNumber].Table.Rows[p][0] as string)).ToArray();

                    if (_storage.RemoveRow(TablesView[tableNumber].NameTable, rowsId))
                    {
                        rowsNumbers = [.. rowsNumbers.OrderByDescending(p => p)];
                        for (int i = 0; i < rowsNumbers.Count; i++)
                        {
                            TablesView[tableNumber].Table.Rows.RemoveAt(rowsNumbers[i]);
                        }
                    }
                }
            }
        });

        public ICommand ChangeCellCommand => new RelayCommand(parametr =>
        {
            if (!OpenFileCorrect)
            {
                return;
            }


            if (parametr is (int, int, int, string))
            {
                try
                {
                    (int, int, int, string) parametrs = ((int, int, int, string))parametr;

                    int tableNumber = parametrs.Item1;
                    int rowNumber = parametrs.Item2;
                    int columnNUmber = parametrs.Item3;
                    string newText = parametrs.Item4;


                    if (0 <= tableNumber && tableNumber < TablesView.Count &&
                        0 <= rowNumber && rowNumber < TablesView[tableNumber].Table.Rows.Count &&
                        0 <= columnNUmber && columnNUmber < TablesView[tableNumber].Table.Columns.Count)
                    {
                        if (_storage.ChangeCell(TablesView[tableNumber].NameTable, columnNUmber, (string)TablesView[tableNumber].Table.Rows[rowNumber][0], newText))
                        {
                            TablesView[tableNumber].Table.Rows[rowNumber][columnNUmber] = newText;
                        }
                        else
                        {
                            TablesView[tableNumber].Table.Rows[rowNumber][columnNUmber] = TablesView[tableNumber].Table.Rows[rowNumber][columnNUmber];
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        });
        #endregion



        #region todo
        // todo при раскрывании окна, оно перекрывается строку пуск. Надо режить этот вопрос

        #region Управление колонками??
        // todo подумать над необходимостью редактировать колонки. Если надо, то реализовать
        /* <MenuItem Header="Таблица1" IsEnabled="False">
                  <MenuItem Header="Удалить строки" Command="{Binding AddRowCommand}" CommandParameter="{Binding SelectedIndex, ElementName=dds}"/>
                <MenuItem Header="Можно редактировать" Command="{Binding AddRowCommand}" CommandParameter="{Binding SelectedIndex, ElementName=dds}"/>
                <MenuItem Header="Редактировать ячейки" Command="{Binding AddRowCommand}" CommandParameter="{Binding SelectedIndex, ElementName=dds}"/>
                <MenuItem Header="Показывать фальтр" Command="{Binding AddRowCommand}" CommandParameter="{Binding SelectedIndex, ElementName=dds}"/>

                <MenuItem Header="Добавить столбец" Command="{Binding AddRowCommand}" CommandParameter="{Binding SelectedIndex, ElementName=dds}"/>
                <MenuItem Header="Удалить столбец">
                    <MenuItem Header="Первый" Command="{Binding RemoveFirstColumnCommand}" CommandParameter="{Binding SelectedIndex, ElementName=tabControl}"></MenuItem>
                    <MenuItem Header="Текущий" ></MenuItem>
                    <MenuItem Header="Последний" Command="{Binding RemoveLastColumnCommand}" CommandParameter="{Binding SelectedIndex, ElementName=tabControl}" ></MenuItem>

                </MenuItem>*/

        public ICommand RemoveLastColumnCommand => new RelayCommand(parametr =>
        {
            int tableNumber = (int?)parametr ?? 0;
            bool? res = new MessageBoxWindow($"Вы действительно хотите столбец \"{TablesView[tableNumber].Table.Columns[^1].ColumnName}\"?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Information).ShowDialog();

            if (res != true)
            {
                return;
            }


            if (_storage.RemoveColumn(TablesView[tableNumber].NameTable, TablesView[tableNumber].Table.Columns[^1].ColumnName))
            {
                TablesView[tableNumber].Table.Columns.RemoveAt(TablesView[tableNumber].Table.Columns.Count - 1); // Не обновляются данные при установке
                // Для решения этого вопроса надо видимо заново обновлять содержимо таблицы
            }
        });
        public ICommand RemoveFirstColumnCommand => new RelayCommand(parametr =>
        {
            int tableNumber = (int?)parametr ?? 0;
            bool? res = new MessageBoxWindow($"Вы действительно хотите столбец \"{TablesView[tableNumber].Table.Columns[^1].ColumnName}\"?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Information).ShowDialog();

            if (res != true)
            {
                return;
            }


            if (_storage.RemoveColumn(TablesView[tableNumber].NameTable, TablesView[tableNumber].Table.Columns[1].ColumnName))
            {
                TablesView[tableNumber].Table.Columns.RemoveAt(1); // Не обновляются данные при установке
            }


        });

        #endregion

        #endregion
    }
}
