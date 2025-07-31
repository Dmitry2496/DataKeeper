using Microsoft.Data.Sqlite;

namespace DataKeeperWindows.Classes
{
    /// <summary>
    /// Хранилище всех данных текущего файла
    /// </summary>
    public class Storage : IDisposable
    {
        #region public
        public string FileName
        {
            get => _fileName;
            private set
            {
                _fileName = value;
                ChangeFileNameEvent?.Invoke();
            }
        }


        /// <summary>
        /// Метод для открытия подключения к базе данных
        /// </summary>
        /// <param name="_dataSource"> Строка подключения</param>
        /// <returns>результат открытия базы данных</returns>
        public bool OpenNewConnection(string filePath = "", string? password = null)
        {
            if (CheckStorageConnect())
            {
                _storageConnection.Close();
            }

            string dataSource = password == null ? FilePathToDataSource(filePath) : FilePathToDataSource(filePath, password);
      
            _storageConnection = new SqliteConnection(dataSource);
            try
            {
                _storageConnection.Open();
                SetFileName(filePath);


                // Проверка открытия (если пароль неверный, запрос выдас ошибку и метод вернет false)
                const string COMMAND = "SELECT name FROM sqlite_master WHERE type='table';";
                SqliteCommand command = new(COMMAND, _storageConnection);
                using SqliteDataReader reader = command.ExecuteReader();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Получить имена всех таблиц в файле
        /// </summary>
        /// <returns>Список имен. Если null-метод завершен с ошибкой</returns>
        public List<string>? GetTablesName()
        {
            if (!ReconnectIfNecessary())
            {
                return null;
            }


            const string COMMAND = "SELECT name FROM sqlite_master WHERE type='table';";
            const string SPECIAL_TABLE_NAME = "sqlite_sequence";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {
                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<string> names = [];

                    while (reader.Read())
                    {
                        if (Convert.ToString(reader["name"]) != SPECIAL_TABLE_NAME)
                        {
                            names.Add(Convert.ToString(reader["name"]) ?? "");
                        }
                    }
                    return names;
                }
                return [];
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<string>?> GetTablesNameAsync()
        {
            if (!ReconnectIfNecessary())
            {
                return null;
            }

            const string COMMAND = "SELECT name FROM sqlite_master WHERE type='table';";
            const string SPECIAL_TABLE_NAME = "sqlite_sequence";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {
                return await Task.Run(() =>
                {
                    using SqliteDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<string> names = [];

                        while (reader.Read())
                        {
                            if (Convert.ToString(reader["name"]) != SPECIAL_TABLE_NAME)
                            {
                                names.Add(Convert.ToString(reader["name"]) ?? "");
                            }
                        }
                        return names;
                    }
                    return [];
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Создать новую таблицу
        /// </summary>
        /// <param name="nameTable">Имя новой таблицы</param>
        /// <param name="columnsArray">Строка с названиями колонок таблицы</param>
        /// <returns>true - таблица создана успешно, false - ошибка создания таблицы</returns>
        /// Следует с осторожностью посылать строку с названиями колонок (должна быть сформирована по определенному правилу. 
        /// Для формирования строки использовать метод этого класса)
        public bool CreateNewTable(string nameTable, string columns)
        {
            if (!ReconnectIfNecessary())
            {
                return false;
            }


            string COMMAND = $"CREATE TABLE \"{nameTable}\" ({columns})";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {
                _ = command.ExecuteNonQuery();

                //Проверка на результат
                COMMAND = "SELECT name FROM sqlite_master WHERE type='table';";
                command = new(COMMAND, _storageConnection);

                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<string> names = [];

                    while (reader.Read())
                    {
                        if (Convert.ToString(reader["name"]) == nameTable)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Удалить таблицу
        /// </summary>
        /// <param name="name">Имя таблицы</param>
        /// <returns>Результат операции</returns>
        public bool RemoveTable(string nameTable)
        {
            if (!ReconnectIfNecessary())
            {
                return false;
            }


            string COMMAND = $"DROP TABLE IF EXISTS \"{nameTable}\";";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {
                _ = command.ExecuteNonQuery();

                COMMAND = "SELECT name FROM sqlite_master WHERE type='table';";

                command = new(COMMAND, _storageConnection);


                using SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    List<string> names = [];

                    while (reader.Read())
                    {
                        names.Add(Convert.ToString(reader["name"]) ?? "");


                    }
                    return !names.Contains(nameTable);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Переименовать таблицу
        /// </summary>
        /// <param name="oldName">Старое название</param>
        /// <param name="newName">Новое название</param>
        /// <returns>Результат операции</returns>
        public bool RenameTable(string oldName, string newName)
        {
            if (!ReconnectIfNecessary())
            {
                return false;
            }


            string COMMAND = $"ALTER TABLE \"{oldName}\" RENAME TO \"{newName}\"";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {
                _ = command.ExecuteNonQuery();
                COMMAND = "SELECT name FROM sqlite_master WHERE type='table';";
                command = new(COMMAND, _storageConnection);

                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if ((string)reader["name"] == newName)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Полуить список имен столбцов таблицы
        /// </summary>
        /// <param name="nameTable">Имя таблицы, у которой нужно получить имена столбцов</param>
        /// <returns>Список имен столбцов. Если null-метод завершен с ошибкой</returns>
        public List<string>? GetColumnsName(string nameTable)
        {
            if (!ReconnectIfNecessary())
            {
                return null;
            }

            string COMMAND = $"SELECT * FROM \"{nameTable}\"";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {
                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.FieldCount != 0)
                {
                    List<string> names = [];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        names.Add(reader.GetName(i));
                    }
                    return names;
                }
                else
                {
                    return [];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<string>?> GetColumnsNameAsync(string nameTable)
        {
            if (!ReconnectIfNecessary())
            {
                return null;
            }

            string COMMAND = $"SELECT * FROM \"{nameTable}\"";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {
                return await Task.Run(() =>
                {
                    using SqliteDataReader reader = command.ExecuteReader();
                    if (reader.FieldCount != 0)
                    {
                        List<string> names = [];

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            names.Add(reader.GetName(i));
                        }
                        return names;
                    }
                    else
                    {
                        return [];
                    }
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Удалить колонку
        /// </summary>
        /// <param name="nameTable">Имя таблицы</param>
        /// <param name="nameColumn">Имя колонки</param>
        /// <returns>Результат выполнения операции</returns>
        public bool RemoveColumn(string nameTable, string nameColumn)
        {
            if (!ReconnectIfNecessary())
            {
                return false;
            }


            string COMMAND = $"ALTER TABLE \"{nameTable}\" DROP COLUMN \"{nameColumn}\";";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {
                _ = command.ExecuteNonQuery();

                COMMAND = $"SELECT * FROM \"{nameTable}\"";
                command = new(COMMAND, _storageConnection);

                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.FieldCount != 0)
                {
                    List<string> names = [];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        names.Add(reader.GetName(i));
                    }
                    return names.Contains(nameColumn);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Полуить данные всех ячеек
        /// </summary>
        /// <param name="nameTable">Имя таблицы, у которой нужно получить имена столбцов</param>
        /// <returns>Список ячеек. Если null-метод завершен с ошибкой</returns>
        public List<List<string>>? GetCellsContent(string nameTable)
        {
            if (!ReconnectIfNecessary())
            {
                return null;
            }

            string COMMAND = $"SELECT * FROM \"{nameTable}\"";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {
                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    List<List<string>> cells = [];
                    while (reader.Read())   // построчно считываем данные
                    {
                        List<string> cellsInRow = [];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            cellsInRow.Add(reader.IsDBNull(i) ? "" : reader.GetString(i));
                        }
                        cells.Add(cellsInRow);
                    }
                    return cells;
                }
                else
                {
                    return [];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<List<string>>?> GetCellsContentAsync(string nameTable)
        {
            if (!ReconnectIfNecessary())
            {
                return null;
            }

            string COMMAND = $"SELECT * FROM \"{nameTable}\"";
            SqliteCommand command = new(COMMAND, _storageConnection);

            try
            {

                return await Task.Run(() =>
                {
                    using SqliteDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        List<List<string>> cells = [];
                        while (reader.Read())   // построчно считываем данные
                        {
                            List<string> cellsInRow = [];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                cellsInRow.Add(reader.IsDBNull(i) ? "" : reader.GetString(i));
                            }
                            cells.Add(cellsInRow);
                        }
                        return cells;
                    }
                    else
                    {
                        return [];
                    }
                });
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Добавить пустую строку
        /// </summary>
        /// <param name="nameTable">Имя таблицы</param>
        /// <returns>id добавленной строки. -1 - ошибка, -2 - не удалось пределить id у добавленной строки</returns>
        public int AddEmptyRow(string nameTable)
        {
            if (!ReconnectIfNecessary())
            {
                return -1;
            }

            try
            {
                // Формируем командную строку
                List<string>? columnsName = GetColumnsName(nameTable);
                if (columnsName == null || columnsName.Count == 0)
                {
                    return -1;
                }

                string sCommand;
                if (columnsName.Count == 1)
                {
                    sCommand = $"INSERT INTO \"{nameTable}\" DEFAULT VALUES";
                }
                else
                {
                    string sColumnsName = "";
                    string sColumnsValue = "";
                    for (int i = 1; i < columnsName.Count; i++)
                    {
                        sColumnsName += i < columnsName.Count - 1
                            ? $"\"{columnsName[i]}\", "
                            : $"\"{columnsName[i]}\"";
                        sColumnsValue += i < columnsName.Count - 1
                            ? $"'', "
                            : $"''";
                    }
                    sCommand = $"INSERT INTO \"{nameTable}\"({sColumnsName}) VALUES ({sColumnsValue})";
                }
                SqliteCommand command = new(sCommand, _storageConnection);


                // Посылаем команду
                int numberAddedRows = command.ExecuteNonQuery();


                // Проверка
                if (numberAddedRows != 1)
                {
                    return -1;
                }


                // Определение id добавленной строки
                sCommand = $"SELECT MAX({columnsName[0]}) FROM \"{nameTable}\"";
                command = new(sCommand, _storageConnection);

                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    reader.Read();
                    return reader.IsDBNull(0) ? -2 : reader.GetInt32(0);
                }
                else
                {
                    return -2;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Удалить строку
        /// </summary>
        /// <param name="nameTable">Имя таблицы</param>
        /// <param name="rowId">id строки которую надо удалить</param>
        /// <returns>Результат выполнения операции</returns>
        public bool RemoveRow(string nameTable, string[] rowsId)
        {
            if (!ReconnectIfNecessary())
            {
                return false;
            }


            try
            {
                // Формируем командную строку
                List<string>? columnsName = GetColumnsName(nameTable);
                if (columnsName == null || columnsName.Count == 0)
                {
                    return false;
                }

                if (rowsId.Length == 0)
                {
                    return true;
                }

                string sRowsId = "";
                if (1 < rowsId.Length)
                {
                    for (int i = 0; i < rowsId.Length; i++)
                    {
                        sRowsId += i < rowsId.Length - 1
                            ? $"\"{rowsId[i]}\", "
                            : $"\"{rowsId[i]}\"";
                    }
                }


                // Удаление строк
                string sCommand = $"DELETE FROM '{nameTable}' WHERE {columnsName[0]} IN ({sRowsId})";
                SqliteCommand command = new(sCommand, _storageConnection);
                int numberDeleteRows = command.ExecuteNonQuery();


                // Возврат результата
                return rowsId.Length == numberDeleteRows;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Изменить ячейку
        /// </summary>
        /// <param name="nameTable">Имя таблицы</param>
        /// <param name="columnNumber">Номер нужной колонки</param>
        /// <param name="rowId">id строки с нужной ячейкой</param>
        /// <param name="newText">Новый текст ячейки</param>
        /// <returns>Результат выполнения операции</returns>
        public bool ChangeCell(string nameTable, int columnNumber, string rowId, string newText)
        {
            if (!ReconnectIfNecessary())
            {
                return false;
            }


            try
            {
                // Формируем командную строку
                List<string>? columnsName = GetColumnsName(nameTable);
                if (columnsName == null || columnsName.Count == 0)
                {
                    return false;
                }


                // Изменение ячейки
                string sCommand = $"UPDATE '{nameTable}' SET '{columnsName[columnNumber]}'='{newText}'  WHERE \"{columnsName[0]}\"=\"{rowId}\"";
                SqliteCommand command = new(sCommand, _storageConnection);
                int numberDeleteRows = command.ExecuteNonQuery();


                // Возврат результата
                return numberDeleteRows == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion


        #region private       
        private SqliteConnection _storageConnection = new();
        private string _fileName = "";


        /// <summary>
        /// Установить имя файла
        /// </summary>
        /// <param name="filePath"></param>
        private void SetFileName(string filePath = "")
        {
            if (filePath.Contains('\\'))
            {
                string[] filePathParts = filePath.Split('\\');
                FileName = filePathParts[^1];
            }
            else
            {
                FileName = filePath;
            }
        }

        /// <summary>
        /// Проверка наличия открытого соединения с базой данных
        /// </summary>
        /// <returns>Результат наличия подключения к БД</returns>
        private bool CheckStorageConnect() => _storageConnection.State == System.Data.ConnectionState.Open;

        /// <summary>
        /// Метод переподключения к базе данных
        /// </summary>
        /// <returns>результат переподключения к базе данных</returns>
        private bool Reconnect()
        {
            try
            {
                _storageConnection.Open();
                return CheckStorageConnect();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Метод переподключения к базе данных, если это необходимо
        /// </summary>
        /// <returns>результат переподключения к базе данных</returns>
        private bool ReconnectIfNecessary() => CheckStorageConnect() || Reconnect();

        private static string FilePathToDataSource(string filePath) => $"Data Source = {filePath}";
        private static string FilePathToDataSource(string filePath, string password) => new SqliteConnectionStringBuilder()
        {
            DataSource = filePath,
            Password = password,
        }.ToString();
        #endregion


        #region Events
        public delegate void ChangeData();
        public event ChangeData? ChangeFileNameEvent;
        #endregion


#pragma warning disable CA1816 // Методы Dispose должны вызывать SuppressFinalize
        public void Dispose() => _storageConnection.Dispose();
#pragma warning restore CA1816 // Методы Dispose должны вызывать SuppressFinalize
    }
}