   M S B A M L   `   `   `  ����F  BBMW Katalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:BMW_Katalog.View.PagesBMW_Katalog.View.Pages  5      )    #BMW_Katalog.View.Pages.DataGridPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:BMW_Katalog.View.Pages     5        N� Title$  DataGridPage��5	      .��5      � .+�5   
   ��  �� CornerRadius$ 15��6   $��1q�6#   
���   �$
}�True��6K   
���   �$��20q�6r   ._�5      &   System.Windows.Controls.DataGrid  -   5          Name$ BMWDataGrid��5      
����������#121212$
��True��6T   $
��Auto=�6y   $
��Auto=�6�      
IsReadOnly$
 True��6�      AutoGenerateColumns$ False��6�   $��5q�6	  ��6  �� *
ListOfCars+   ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value
 �����67   �� 5       � ��$ SemiBold"�67   �� 5       � ��
 �   �67   �� 5       � ��$ 22(�65   �� 5       � ��$ 10q�64   �� 5       � ��$ Center=�6G   5      	   Columns	 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$

 Name��6-      
IsReadOnly$
 True��6;      CellTemplate 5      �� 5      .R�5   "   �� �� 5   -   -   5        
 � Tag 5   O   �� .�6_   .�5   1   �� �65   �� *Name+5   .   5    #   5!      5#      0  *System.Windows.Controls.DataGridTextColumn  5$      $
 Realse Date��6)      FontSize$ 20Q�6]   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6>   �� *
RealseDate+   ElementStyle 5%      �� 5&      " ��6$   .�5'   "   ��  � ��$ Center=�6H   �� 5(   "    � ��$ 10q�6;   �� 5)   "    � ��$ 
Montserrat)�6?   5*      5,        5-      $
 Image��6-    5.      �� 5/      .R�50   "   �� $��3q�6)   $ 10��6=   $	��100��6O   $��60��6[   ��51   &   �� 52   *    �� 
StartPoint$
 0, 0&�6>    �� EndPoint$
 1, 1&�6P   .��53   .   �  � Color$ #0066B1��6;    � Offset$ 0Q�6K   � 54   .   $ #003C78��6;   $	 0.5Q�6K   � 55   .   $ #E22718��6;   $ 1Q�6K   56   +   ._�58   &   � .+�59   &   �� $
��Fill=�5:   (   $��Center=�5;   (   $��Center=�5<   (   ��59   ,   �� *UrlImg+5<   (   �� 5>   *   $��3q�61   $ 10��6E   $	��100��6W   $��60��6c   
������ $	��-10q�6�   ��5?   .   �� 5@   2   $
 0, 0&�6F   $
 1, 1&�6X   .��5A   6   � $ #0066B1��6C   $ 0Q�6S   � 5B   6   $ #003C78��6C   $	 0.5Q�6S   � 5C   6   $ #E22718��6C   $ 1Q�6S   5D   3   5F   +   5G   '   5H   #   5I      5K        5L      $

 Date��6)   $ 20Q�6P    67   �� *Date+ 5M      �� 5N      " ��6$   .�5O   "   ��  � ��$ Center=�6H   �� 5P   "    � ��$ 10q�6;   �� 5Q   "    � ��$ 
Montserrat)�6?   5R      5T        5U      $
 Select��6-   $ False��6=    5V      �� 5W      .R�5X   "   �� -   5         d� 	IsChecked 5X   +   �� *
CheckBox+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6|   5Y      5[      5]      5^      5_      5`      