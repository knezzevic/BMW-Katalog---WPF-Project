   M S B A M L   `   `   `  ����F  BBMW Katalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BMW_Katalog.ViewBMW_Katalog.View  5   	        BMW_Katalog.View.LoginUser   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   *localclr-namespace:BMW_Katalog.View  5   	   -   5          � Title$  	LoginUser��5	   	   $	��750��6   $
��1200��6(    � WindowStyle$
 None=�5
   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency$
 True��5   	    � WindowStartupLocation$ CenterScreen=�5   	   
������ .��5      � .+�5   
   ��  �� CornerRadius$ 30��6   ��5   
   �� 5       �� ImageSource$ \Assets\bg.jpg��6    j� Stretch$
 Fill=�66   ._�5   
   �� $ 30��6    J� Opacity$
 0.93Q�6#   $��5q�62   ��5      �� 5      	 �� 
StartPoint$
	 0, 0&�6*   
 �� EndPoint$

 1, 1&�6<   .��5      �  � Color$ #0066B1��6'    � Offset$ 0Q�67   � 5      $ #003C78��6'   $	 0.5Q�67   � 5      $ #E22718��6'   $ 1Q�67   5      ��5"      �� 5#      $
	 0, 0&�6&   $

 1, 1&�68   .��5$      � $ #1b1c1c��6#   $ 0Q�63   � 5%      $ Black��6#   $ 1Q�61   5&      5)      5*      #  MaterialDesignThemes.Wpf.Card  5,   
   $	��450��6   $	��500��6+      UniformCornerRadius$ 30Q�67   
���   �$��5q�6c   ��5.      �� 5/      $
	 0, 0&�6&   $

 1, 1&�68   .��50      � $ #E22718��6#   $ 0Q�63   � 51      $ #003C78��6#   $	 0.5Q�63   � 52      $ #0066B1��6#   $ 1Q�63   53      .��56      �� .+�57      �� $��20 20q�6   .+�58      �� $��\Assets\logo.png��6   $��50��66   $��50��6B   $��10q�6M   �� 59      $��
User Login��6    $��36(�62    �� TextAlignment$ Center=�6@   
�������$��
Montserrat)�6j   $
��Bold"�6�   5:      �� 5<       �� Orientation$ 
Horizontal=�6   $��30q�66   $��Center=�6B   $��Center=�6_   $	��400��6z   .+�5=      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 User=�6.   $��40��6:   $��40��6E   
�������6Q   �� 5>      -   5         ��Name$ txtUsername��5>      $
��15 0q�63   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Unesi korisnicko ime��6A   
�������$��18(�6�   $	��350��6�    �� SelectionBrush �#003C78 �� 
CaretBrush �#0066B1$��
Montserrat)�6�   5?      �� 5@      $ 
Horizontal=�6   $
��30 0q�66   $��Center=�6D   $��Center=�6a   $	��400��6|   .+�5A        $
 Lock=�6.   $��40��6:   $��40��6E   
�������6Q   G� 5B      -   5         G�Name$ txtPassword��5B   "   $
��15 0q�67   $ Unesi lozinku��6E   
�������$��18(�6�   $	��350��6�   $��
Montserrat)�6�   5C      �� 5E      $��50 30q�6   $��Center=�6,   $ 
Horizontal=�6I   .+�5F      �� -   5         ��Name$ btnLogin��5G   x   $	��170��5F      $��60��6)   $��Login��65      MaterialDesignRaisedButton#��[  6E   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 20��5G      
������ 
�������$��20(�6�   $��
Montserrat)�6�   ��5H      �� 5I      $
	 0, 0&�62   $

 1, 1&�6D   .��5J   "   � $ #0066B1��6/   $ 0Q�6?   � 5K   "   $ #003C78��6/   $	 0.5Q�6?   � 5L   "   $ #E22718��6/   $ 1Q�6?   5M      5P      �� 5Q      -   5        $ btnClose��5R      $	��170��5Q      $��60��6)   ���#E22718 � ToolTip$ Close��5R      
������ $
��Exit��6T   
�������$ 20��6v   $��20(�6�   $��20 0 0 0q�6�   $��
Montserrat)�6�   5T      5V      5W      5X      5Z      5[      