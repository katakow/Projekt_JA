   M S B A M L   `   `   `  ����E  AProjekt_JA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Projekt_JA
Projekt_JA  5          Projekt_JA.MainWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      *localclr-namespace:Projekt_JA     5        � Title$,  &Katarzyna Kowalczewska Filtr Laplace'a��5	      $	��400��64   $	��600��6D   $	��800��6Q   $
��1200��6`   .��5      �  � ColumnDefinitions 5   
   �� 5      $
��398* �6   �� 5      $
��767* �6   �� 5      $	��30* �6    � RowDefinitions 5   
   �� 5      $��4* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��2��6%   $��10,10,9.6,10.4q�66   .+�5      �� .��6   Wybierz implementację:5      	� 5       	� 	GroupName$ implementation��6    d� 	IsChecked 65   ��  �� Path$! AssemblerImplementationMode� �� Mode$ TwoWay=�.��6y   	Assembler6�   	� 5      $ implementation��6    65   �� $ CSharpImplementationMode�$ TwoWay=�.��6v   C#6z   �� 5       $��0��6   .��6$   Ścieżka obrazu:5"      � 5$       5%      �� 5&      $��2* �6'   �� 5'      $��* �6'   .+�5*      �� $��0��6    �� 
IsReadOnly . ��6>   �� $ InputFilePath�$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5,      -   5        $��Find_Btn��5,      $��1��6)   $��10 0 0 0q�69   .��6c   	Znajdź6k   5-      5.      � 50   
   $��1��6   $��2��6   $��0,0,766.1,0.4q�60    51      �� 52      $��* �6#   �� 53      $��* �6#   .+�56      � $��1��6    57      �� 58      $��* �6'   �� 59      $��* �6'   .+�5<      �� -   5        $��
Filter_Btn��5<      $��0��6T   $��10q�6d   $��Right=�6q   $	��Top=�6�   $��
10 10 0 0 q�6�   {�6+   �� $ FilterEnabled�.��6�   
Filtruj!6�   �� 5>      -   5        $��Save_Btn��5>      $��1��6P   $��10q�6`   $��Right=�6m   $	��Top=�6�   $��	10 10 0 0q�6�   {�6)   �� $ SaveEnabled�.��6�   Zapisz6�   5?      �� 5A      ���	#FF4B4B8F
�������$��1q�6D   ._�5B      �� $��0��6   $��5q�6-   	 J� RenderTransformOrigin$		 1,0&�68   
�������.+�5C      �� $��Czas wykonania(ms):��6   �� 5E      ��6   �� $ ElapsedTime�$ TwoWay=�$ PropertyChanged=�6u   5F      5G      5H      �� 5K   
   $��1��6   $��0��6!   $��10.4,10,9.6,10q�6.   
���   �
�������$��1q�6q   $��2��6�   ._�5L      �� 
 � UseLayoutRounding
 . $��2��6Q   ��6-   �� $ ImageSource�6d   5M      � 5O   
   -   5        $��ProgressBar��5O      $��1��6)   $��1��69   $y�Hidden=�6F    � Minimum$ 0Q�6Z    � Maximum$ 1Q�6f   $��10.4,10,9.6,0q�6�   $	��Top=�6�   $��10��6�   $��2��6�    � Value 6r   �� $ ProgressBar�$ PropertyChanged=�6�   5P      5Q      