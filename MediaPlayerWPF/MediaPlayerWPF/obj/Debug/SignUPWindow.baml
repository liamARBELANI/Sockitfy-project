   M S B A M L   `   `   `  ����I  EMediaPlayerWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:MediaPlayerWPFMediaPlayerWPF  5   	   !    MediaPlayerWPF.SignUPWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   V RMaterialDesignThemes.Wpf, Version=3.1.0.1244, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      (localclr-namespace:MediaPlayerWPF  5   	     � Title$  SignUPWindow��5
   	   $	��450��6   $	��790��6+    � WindowStartupLocation$ CenterScreen=�67    � WindowStyle$
 None=�6\    � 
ResizeMode$ NoResize=�6o   .��5      � $
��0,10q�6   ��5   
   �� 5       �� 
StartPoint$ 0.1,0&�6"    �� EndPoint$ 0.9,1&�65   .��5      �  � Color$
 blue��6    � Offset$ 1Q�6,   � 5      $ 	OrangeRed��6   $ 0Q�61   5      .+�5   
   �� -   5        $��textBlockHeading��5   S   $��47��6   $��Center=�6    $��274,10,406,0q�6=   $��Sign Up��6k   $	��Top=�6z   $	��110��6�   $��30(�6�   $��ExtraCondensed&�6�   �� 5   
   -   5        $��572,10,-110,283q�5      $��Login��6*    � Cursor$
 Hand��6:   ��5      �� 5      	 �� 
TargetType"	 ��6"   .R�5      �� $��	Underline{�6    �� 5      5      5      ��5      �� 5      
 �� 
TargetType"
 ��6   .�5      ��  �� Property � �� �� Value
 ��  �63    �� Triggers 5      P� 5        P� Property �@J�IsMouseOver P� Value . . �5!      ��  � ��
 �  ��6;   5"      5$      5&      � 5(   
   $��40,0,154,39q�6   
�������$	��329��67   $��Bottom=�6D    � RowDefinitions 5)      �� 5*      $
��252* �6    .+�5-      �� -   5         ��Name$ errormessage��5-   Q   $��20��6   $
��Left=�6$   $��67,0,0,0q�6?   $	��Top=�6h   $	��247��6�    J� OpacityMask
 �<�����	#FFE5572C6�   '  !MaterialDesignThemes.Wpf.PackIcon  5/         Kind$
 User=�6&   $��25��62   $
��Auto��6=   
���   �$��Bottom=�6^   $��136,0,0,284q�6y    J� RenderTransformOrigin$ -0.54,-11.08&�6�     50      $
 User=�6&   $��25��62   $��25��6=   
���   �$ 3.94,2.7&�6\   $��
136,50,0,0q�6}   6�     51      $ Email=�6&   $��25��64   $��25��6?   
���   �$��
136,80,0,0q�6^   6t     52      $
 Lock=�6&   $��25��62   $��25��6=   
���   �$��136,103,0,0q�6\   6s     53      $
 Lock=�6&   $��25��62   $��25��6=   
���   �$��136,134,0,0q�6\   6s   �� 54      -   5        $��textBoxFirstName��54   �   $��23��6   $
��Left=�6"   $��
183,20,0,0q�6=   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Enter UserFname ��6Q   
���   �$	��Top=�6�   $	��222��6�   6�   �� 55      -   5        $��textBoxLastName��55   �   $��23��6   $
��Left=�6"   $��
183,50,0,0q�6=   $ Enter UserLname��6Q   $	��Top=�6�   $	��222��6�   6�   �� 56      -   5        $��textBoxEmail��56   �   $��27��6   $
��Left=�6"   $��
183,80,0,0q�6=   
���   �$ Enter Email��6e   $	��Top=�6�   $	��222��6�   G� 57      -   5        $��passwordBox��57   �   $��23��6   $
��Left=�6&   $��183,107,0,0q�6A   $ ********��6V   
���   �$	��Top=�6�   $	��222��6�   G� 59      -   5        $��passwordBoxConfirm��59   �   $��23��6   $
��Left=�6&   $��183,136,0,0q�6A   $ Enter the same password ��6V   
���   �$	��Top=�6�   $	��222��6�   �� 5;      -	   5        $��Submit��5;   b   $��Submit��6   $��23��6&   $
��Left=�62   $��246,208,0,0q�6M   $	��Top=�6p   $��89��6�   6�   �� 5<      -
   5        $��button2��5<   a   $��Reset��6   $��23��6%   $
��Left=�61   $��164,208,0,0q�6L   $	��Top=�6p   $��70��6�   $ 0.814,0.457&�6�   �� 5=      -   5        $��button3��5=   c   $��Cancel��6   $��23��6&   $��Right=�62   $��0,208,155,0q�6N   $	��Top=�6r   $��82��6�   $ 
-0.921,0.5&�6�   5>      5?      5@      