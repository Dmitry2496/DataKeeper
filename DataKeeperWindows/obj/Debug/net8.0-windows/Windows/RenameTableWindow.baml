   M S B A M L   `   `   `  ����L  HDataKeeperWindows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:DataKeeperWindows.WindowsDataKeeperWindows.Windows  5   	   1    +DataKeeperWindows.Windows.RenameTableWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:DataKeeperWindows.Windows     5   	   -   5        $��window��5   	     � Title$1  +Переименование таблицы��5   	   $	��180��5	   	   $	��330��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � ShowInTaskbar$
 True��5   	    � Topmost$
 True��5   	      Window.NoResize.Style#��[  5   	   .��5      �  � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��35 �6   �� 5      $��40 �6   �� 5      $��35 �6   �� 5      $��40 �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� -   5        $��	topCanvas��5      $��0��5      ��5       �� *Width+ �� ConverterParameter$ -6��  WidthWindowConverter �� 	Converter# [  �� ElementName$ window��.+�5!      �� -   5        $��topPanel��5!        TopPanel.Style#���  5#      ��5"      �� *Width+$ 	topCanvas��5$      �� 5%        TopPanel.Icon.Style#���  6   �� 5&      $1��+Переименование таблицы��6   	 �� Left$	 30��5'        TextBlock.Static.Foreground#���  5(      �� 5)      -   5        
 ��Name$
 butClose��5)        TopButton.Close.Style#���  5*      5+      5,      �� 50   
   -   5        $��currentName��51      $'��!Текущее название: ��50      $��1��52      $
��Left=�53      $	��Top=�54      $��
10,10,10,0q�55      $
��Auto��56      $��CharacterEllipsis=�57      #��[ 58      �� 5:   
   $"��Новое название:��6   $��2��5;      $
��Left=�5<      $	��Top=�5=      $��	10,5,10,0q�5>      $
��Auto��5?      #��[ 5@      �� 5B   
   -   5        $��	inputName��5B      $��2��5C      $	��200��5D      $��25��5E      $
��Left=�5F      $	��Top=�5G      $��
110,2,10,0q�5H      5I      �� 5K   
   -   5        $��warningText��5L      $'��!!!! Пустое название��5K      $��3��5M      $��Center=�5N      $	��Top=�5O      $��0,0,0,0q�5P      $
��Auto��5Q      
���  ��5R      �� 5T   
   $��4��6    �� Orientation$ 
Horizontal=�5U      $��RightToLeft=�5V      .+�5W      �� -   5        $��buttonCancel��5W      $��20��5X      $��60��5Y        Button.Style#���  5Z      $��Отмена��5[      $��10,0,0,0q�5\      5]      �� 5^      -	   5        $��	buttonYes��5^      ${�false��5_      $��20��5`      $��60��5a      #���  5b      $
��ОК��5c      $��10,0,0,0q�5d      5e      5f      5g      5h      