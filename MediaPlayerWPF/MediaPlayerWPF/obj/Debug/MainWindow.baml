   M S B A M L   `   `   `  ����  MediaPlayerWPF    MediaPlayerWPF.MainWindow   5>   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5?   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5@   	     � Title$  muziv��5A   	   $	��300��6   $	��300��6$   $	��300��5B   	    � SizeToContent$ WidthAndHeight=�6   .��5M      � -   5         � RowDefinitions 5N   
   �� 5O      $
��Auto �6   �� 5P      $��* �6   �� 5Q      $
��Auto �6   .+�5S   
   c� .@�5U      �� -   5        .��5V      �� $��/assets/folder.png��6   $��50��64   $��50��6?   5W      �� 5X      �� 5Y      -   5        .��5Z      �� $��/assets/start.png��6   $��50��64   $��50��6?   5[      �� 5]      -   5        .��5^      �� $��/assets/playpause.png��6   $��50��68   $��50��6C   5_      5e      r� 5g   
   -   5        $��mePlayer��5g      $��1��6'    r� LoadedBehavior$ Manual=�64    r� Stretch$
 None=�6L   �� 5i   
   $��2��6   ��5j      �� 5k      .R�5l      �  � ColumnDefinitions 5m      �� 5n      $
��Auto �6/   �� 5o      $��* �6/   �� 5p      $
��Auto �6/   5r      5s      .@�5u      �� �� 5v      $��1��6   $��Stretch=�6,   5w      � 5x   
   -   5        $��pbStatus��5x   t   $��Center=�6   $��41��63   $	��Top=�6?   $��94��6W   $��0,17,0,0q�6b   $��1��6�   �� 5y   
   $
��Left=�6   $
��Wrap=�6/   $	��Top=�6�    J� RenderTransformOrigin$ 2.707,5.646&�6�   $��
229,22,0,0q�6�   $��32��6�   ��6C   ��  �� ElementName$ pbStatus�� �� Path$ Value�	 �� StringFormat$	 {0:0}%��6�   5{      5|      