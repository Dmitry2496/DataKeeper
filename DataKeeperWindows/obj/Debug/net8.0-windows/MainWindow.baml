   M S B A M L   `   `   `  ����L  HDataKeeperWindows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:DataKeeperWindowsDataKeeperWindows  5   	   fEclr-namespace:DataKeeperWindows.ViewModels;assembly=DataKeeperWindowsDataKeeperWindows.ViewModels  5   	   "    DataKeeperWindows.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:DataKeeperWindows     5   	   [	viewModelEclr-namespace:DataKeeperWindows.ViewModels;assembly=DataKeeperWindows     5   	   -   5        $��window��5   	      Window.Style#���   5
        � WindowStartupLocation$  CenterScreen=�5   	   $	��550��5   	   $	��900��5   	   $	��900��5   	   $	��550��5   	    � 
ResizeMode$ CanResizeWithGrip=�5   	    � Title 5   	   �� *StorageName+'  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 0Q�5   	      ResizeBorderThickness$ 3q�5   	    J� InputBindings 5      �� 5   
   
 �� Key$ N��6    �� 	Modifiers$ Controlf�5      	 �� Command	 5      �� *CreateNewFileCommand+�� 5   
   $ O��6   $ Controlf�5      	 5       �� *OpenFileCommand+�� 5!   
   $ F6��6   	 5"      �� *AddNewTableCommand+�� 5#   
   $ Delete��6   $ Controlf�5$      	 5%      �� *RemoveTableCommand+
 �� CommandParameter
 5&      �� *SelectedIndex+ �� ElementName$ 
tabControl���� 5'   
   $ F2��6   	 5(      �� *RenameTableCommand+
 5)      �� *SelectedIndex+$ 
tabControl���� 5*   
   $ F5��6   	 5+      �� *UpdateAllCommand+�� 5,   
   $ F7��6   	 5-      �� *AddRowCommand+
 5.      �� *SelectedIndex+$ 
tabControl��.��50      �  � RowDefinitions 52   
   �� 53      $��25 �6   �� 54      $��35 �6   �� 55      $��* �6   �� 56      $
��Auto �6   �� 57      $
��Auto �6   �� 58      $
��Auto �6    � ColumnDefinitions 5:   
   �� 5;      $��* �6   .+�5A   
   �� -   5        $��	topCanvas��5C      $��0��5A      $��2��5B      .+�5D      �� -   5        $��topPanel��5D        TopPanel.Style#���  5E      $��25��5G      �� 5H        TopPanel.Icon.Style#���  6   �� 5I       �� Left$ 30��5J      
 �� Top$ 3��5K        TopPanel.Static.Foreground#���  5L      ��5I      �� *StorageName+5L      �� 5M      -   5         ��Name$ butMinimize��5M        TopButton.Minimize.Style#���  5N      5O      �� 5P      -   5        $ butMaximize��5P        TopButton.Maximize.Style#���  5Q      5R      �� 5S      -   5        $ butClose��5S        TopButton.Close.Style#���  5T      5U      5V      o� 5[   
   $��1��6   .@�5\      m� $��Файл��6   .@�5]      m� $��Создать��6    m� InputGestureText$ Ctrl+N��5^       m� Command 5_      �� *CreateNewFileCommand+m� 5`      $��Открыть��6   $ Ctrl+O��5a       5b      �� *OpenFileCommand+�� 5c        MenuSeparatorStyle#���  6   m� 5d      -   5        $��
Выход��5d      $ Alt+F4��5e      5f      5g      m� 5i      $��Параметры��6   .@�5j      m� $��Тема��6   .@�5k      m� -   5        $��theme1��5l      $��Светлая��5k       m� IsCheckable$
 True��5m      5n      m� 5p      -	   5        $��theme2��5q      $��Тёмная��5p      $
 True��5r      5s      5t      m� 5u      -
   5        $��menuShowSearch��5v      $!��Область поиска��5u      $
 True��5w       m� 	IsChecked$ False��5x      $ Ctrl+F��5y      5z      5{      m� 5}      $��Документ��6   {�5~      �� *OpenFileCorrect+.@�5      m� $%��Добавить таблицу��6   $ F6��5�       5�      �� *AddNewTableCommand+m� 5�      $#��Удалить таблицу��6   $ Ctrl+Del��5�       5�      �� *RemoveTableCommand+ m� CommandParameter 5�      �� *SelectedIndex+$ 
tabControl��{�5�      �� *SelectedIndex+$ 
tabControl��   SelectedIndexToBoolConverter �� 	Converter# [ m� 5�      $/��)Переименовать таблицу��6   $ F2��5�       5�      �� *RenameTableCommand+ 5�      �� *SelectedIndex+$ 
tabControl��{�5�      �� *SelectedIndex+$ 
tabControl��# [ �� 5�      #���  6   m� 5�      $#��Обновить данные��6   $ F5��5�       5�      �� *UpdateAllCommand+5�      m� 5�      $��Таблица��6   {�5�      �� *SelectedIndex+$ 
tabControl��# [ .@�5�      m� $#��Добавить строку��6   $ F7��5�       5�      �� *AddRowCommand+ 5�      �� *SelectedIndex+$ 
tabControl��5�      5�      �� 5�   
   -   5         ��Name$ 
tabControl��5�       	 LeftTabControl#��� 	 5�      $��2��5�      $
��Auto��5�      $��10,10,10,10q�5�      ��5�      �� *
TablesView+��5�      �� 5�      .R�5�      �� �� 5�      $��CharacterEllipsis=�5�   $   ��5�   $   �� *	NameTable+ � ToolTip 5�   $   �� *	NameTable+5�      5�       �� ContentTemplate 5�      �� 5�      .R�5�      �  5�      �� 5�      $��* �6,    5�      �� 5�      $��* �60   .+�5�      &   System.Windows.Controls.DataGrid    Name$
 data��6#   -   5        $
��Left=�5�   #   $	��Top=�5�   #   $
��Auto��5�   #   $
��Auto��5�   #    �� SelectedIndex$ -1��5�   #   $��0q�5�   #    
 DataGrid#��� 
 5�   #      CanUserReorderColumns$ False��5�   #      CanUserSortColumns$ False��5�   #      AutoGenerateColumns$
 True��5�   #      HeadersVisibility$ Column=�5�   #       CanUserAddRows$  False��5�   #   !   FrozenColumnCount$! 1��5�   #   "   CanUserResizeColumns$
" True��5�   #   $#   HorizontalScrollBarVisibility$
# Auto=�5�   #   "$   VerticalScrollBarVisibility$
$ Auto=�5�   #   %   CanUserDeleteRows$% False��5�   #   ��5�   #   �� *Table+&   	CellStyle& 5�      �� 5�   "   ' �� 
TargetType*  $System.Windows.Controls.DataGridCell"'  6(   .�5�   &   �� ( �� Property( �@�ToolTip) �� Value) 6@   �� * �� RelativeSource* �� *Self++ �� Path$+ Content.Text�5�   #   5�      5�      5�      5�      �� 5�   
   -   5        $��bottomCanvas��5�      $��3��5�      $y�	Collapsed=�5�      $��35��5�      .+�5�      �� -   5        $��bottomPanel��5�       ! BottomPanel.Static.Background#���  5�      $��35��5�      �� 5�      -   5        $��tbSearch��5�      $�� ��5�      $	��200��5�      $��20,5,0,0q�5�      5�      �� 5�      -   5        $��	chbSearch��5�      $��
240,10,0,0q�5�        CheckBox.Static.Foreground#���  5�      $'��!Учитывать регистр��5�      �� 5�      -   5        $��	btnSearch��5�      $��20��5�      $��80��5�      $��
Найти��5�      $��	380,7,0,0q�5�        Button.Style#���  5�      {�5�      �� *SelectedIndex+$ 
tabControl��# [ 5�      �� 5�      -   5        $��btnDump��5�      $��20��5�      $��80��5�      $��Сбросить��5�      $��	480,7,0,0q�5�      #���  5�      {�5�      �� *SelectedIndex+$ 
tabControl��# [ 5�      5�      5�      5�      