   M S B A M L   `   `   `  ����L  HDataKeeperWindows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:DataKeeperWindows.WindowsDataKeeperWindows.Windows  5   	   6    0DataKeeperWindows.Windows.CreateOrOpenFileWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9local'clr-namespace:DataKeeperWindows.Windows     5   	   -   5        $��window��5
   	   $	��250��5   	   $	��300��5	   	     � WindowStartupLocation$  CenterScreen=�5   	    � ShowInTaskbar$
 True��5   	    � Topmost$
 True��5   	      Window.NoResize.Style#���   5   	   .��5      �  � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��	topCanvas��5      $��0��5      ��5      �� *Width+ �� ConverterParameter$ -6��  WidthWindowConverter �� 	Converter# [  �� ElementName$ window��.+�5      �� -   5        $��topPanel��5        TopPanel.Style#���  5      ��5      �� *Width+$ 	topCanvas��5      �� 5        TopPanel.Icon.Style#���  6   �� 5      -   5         ��Name$ butClose��5        TopButton.Close.Style#���  5       5!      5"      �� 5'   
   -   5        $��
buttonOpen��5'      $��50��5(   
   $	��180��5)   
   $��1��5*   
   $��0��5+   
   $��0,15,0,0q�5,   
     Button.Style#���  5-   
   $��Center=�5/   
   $��Center=�50   
   .��51      ��  �� Orientation$ 
Horizontal=�6   $��	0,10,0,10q�62   .+�52      �� $��/Resources/ExtractTo.png��6   $��35��53      $��35��6#   $��Center=�54      $��Center=�55      �� 56      $��Открыть файл��6   $��16(�57        Button.Static.Foreground#���  58      $��15,0,0,0q�59      5:      5;      �� 5=   
   -   5        $��buttonCreate��5=      $��50��5>   
   $	��180��5?   
   $��2��5@   
   $��0��5A   
   $��0,0,0,15q�5B   
   #���  5C   
   $��Center=�5E   
   $��Center=�5F   
   .��5G      �� $ 
Horizontal=�6   $��	0,10,0,10q�62   .+�5H      �� $��/Resources/Add.png��6   $��35��5I      $��35��6   $��Center=�5J      $��Center=�5K      �� 5L      $��Создать файл��6   $��16(�5M      #���  5N      $��15,0,0,0q�5O      5P      5Q      5S      5T      