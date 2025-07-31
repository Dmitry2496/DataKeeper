using System.Collections.ObjectModel;
using System.Data;

namespace DataKeeperWindows.Classes
{
    public class TableView(string nameTable) : NotifyPropertyChanged
    {
        #region public
        public string NameTable
        {
            get => _nameTable;
            set 
            { 
                _nameTable = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string>? NameColumns
        {
            get => _nameColumns;
            set
            {
                _nameColumns = value;
                OnPropertyChanged();
            }
        }
        public DataTable Table
        {
            get => _table;
            set 
            { 
                _table = value; 
                OnPropertyChanged(); 
            }
        }
        #endregion


        #region private
        private string _nameTable = nameTable;
        private DataTable _table = new();
        private ObservableCollection<string>? _nameColumns = [];
        #endregion
    }
}
