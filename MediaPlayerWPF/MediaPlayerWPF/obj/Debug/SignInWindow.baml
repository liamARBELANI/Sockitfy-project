   M S B A M L   `   `   `  ����I  EMediaPlayerWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:MediaPlayerWPFMediaPlayerWPF  5   	   !    MediaPlayerWPF.SignInWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   V RMaterialDesignThemes.Wpf, Version=3.1.0.1244, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   (localclr-namespace:MediaPlayerWPF  5   	     � Title$  SignInWindow��5	   	   $	��450��6   $	��800��6+    � WindowStartupLocation$ CenterScreen=�67    � WindowStyle$
 None=�6\    � 
ResizeMode$ NoResize=�6o   .��5
      � -   5        ��5   
   �� 5       �� 
StartPoint$ 0.1,0&�6"    �� EndPoint$ 0.9,1&�65   .��5      �  � Color$
 Blue��6    � Offset$ 1Q�6,   � 5      $ 	OrangeRed��6   $ 0Q�61   5      .+�5   
   �� -   5        $	��390��5      $	��Top=�6    �� CornerRadius$ 	0 0 180 0��66   ���#100E17._�5      �� 	 �� Orientation$	 
Horizontal=�6   .+�5      �� $	��350��6   .+�5      �� $��assets/logo.png��6   $	��300��65   $	��300��6A   $	��Top=�6N   $��30q�6f   $
��Fill=�6r   6�   5      �� 5      $	��350��6   .+�5      �� $��20 40q�6!   .+�5      �� $��User Sign In��6$   $��20q�68   
�������
 �� TextAlignment$
 Center=�6W   $��38(�6n   $��Great Vibes)�6|   6�   �� 5      -   5         ��Name$ errormessage��5   ]   $��20��6$   $
��Left=�60   $��67,0,0,0q�6K   $	��Top=�6t   $	��247��6�    J� OpacityMask
 �<�����	#FFE5572C6�   �� 5      $	 
Horizontal=�6%   $��10q�6>   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 User=�66   $��25��6B   $��25��6M   
�������6n   �� 5      -   5         ��Name$ txtEmail��5   '   $
��10 0q�69   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Enter Email��6G   
�������$	��250��6�   
������� �� 
CaretBrush �	#FFD94448 �� SelectionBrush �	#FFD94448.��6�   5       5"      �� 5#      $	 
Horizontal=�6%   $��10q�6>   .+�5$        $
 Lock=�66   $��25��6B   $��25��6M   
�������6n   G� 5%      -   5         G�Name$ txtPassword��5%   +   $
��10 0q�6@   $ ********��6N   
�������$	��250��6�   
������� G� 
CaretBrush �	#FFD94448 G� SelectionBrush �	#FFD944486�   5'      �� 5(      $��10q�6%   $��Center=�61   .+�5)      �� -   5         ��Name$ btnLogin��5*   �   $	��100��5)   %   $��40��61      MaterialDesignRaisedButton#��[  6=   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 10��5*      $��2q�6M   ���#D94448���#D94448
������� � ToolTip$ Login��6�   $��Login��6�   6�   5+      5-      50      �� 51      $	��100��6   .+�52      �� -   5        $ btnExit��53   &   $��10 20q�52       & "MaterialDesignFloatingActionButton#��[ 6,   $ Close��53      ��52   h   Z� .��54        $ Close=�62   
�������$��30��6R   $��30��6]   6m   57      58      59      5:      5;      