   M S B A M L   `   `   `  ����B  >Simvars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:SimvarsSimvars  5   	   p lMicrosoft.FlightSimulator.SimConnect, Version=11.0.62651.3, Culture=neutral, PublicKeyToken=baf445ffb3a06b5c�`clr-namespace:Microsoft.FlightSimulator.SimConnect;assembly=Microsoft.FlightSimulator.SimConnect$Microsoft.FlightSimulator.SimConnect 5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel 5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5	   	       Simvars.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Simvars     5   	   rsimco`clr-namespace:Microsoft.FlightSimulator.SimConnect;assembly=Microsoft.FlightSimulator.SimConnect     5   	   Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase     5   	   6sys&clr-namespace:System;assembly=mscorlib     5	   	     � Title$  
MainWindow��5   	   $	��700��6   $	��800��6)   $	��800��65   $	��700��6D    � 	Resources 5         SimObjectTypeFromEnum S� 
MethodName S� 
ObjectType  System.Enum S� MethodParameters M� TypeName  AlphaSortedSimvarNames �� Source �� SortDescriptions+  %System.ComponentModel.SortDescription  AlphaSortedUnitNames��%:  &	        &	 |     &	 �     S� 5   
   $ 	GetValues��6;   "  6R    5      M� 5      $% simco:SIMCONNECT_SIMOBJECT_TYPE��6   5      �� 5   
    6>   �� *aSimvarNames+ 5        5      5      �� 5   
    6<   �� *
aUnitNames+ 5        5      5      5      .��5      � .+�5    
   �� $	��236��6   $��10,11,546,-11q�6    $��23��67   $
��Left=�6C   $	��Top=�6^   �� 5!      -   5         ��Name$ hl_SimConnect��5!      $<��6https://msdn.microsoft.com/en-us/library/cc526983.aspx?�6/   .�6�   SimConnect Reference5#      5$      �� 5%   
   $��10,33,546,613q�6   $	��236��6+   $	��Top=�67   $
��Left=�6O   $��23��6j   �� 5&      -   5        $ hl_SimVariables��5&      $<��6https://msdn.microsoft.com/en-us/library/cc526981.aspx?�61   .�6�    Simulation Variables Reference5(      5)      �� 5+   
   -   5        	 ��Name$	 btn_Connection��5+      $
��Left=�6v   $��	29,68,0,0q�6�   $	��Top=�6�   $	��146��6�   $��48��6�   ��6)   �� *sConnectButtonLabel+��6Q   �� *cmdToggleConnect+6�   �� 5,   
   -   5        $	 btn_LoadFile��5,      $��	Load File��6'   $
��Left=�6\   $��
29,156,0,0q�6w   $	��Top=�6�   $	��100��6�   $��23��6�   
 J� RenderTransformOrigin$
 0.423,0.841&�6�   ��6;   �� *cmdLoadFiles+6�   �� 5-   
   -   5        $	 btn_SaveFile��5-      $��	Save File��6'   $
��Left=�6[   $��148,156,0,0q�6v   $	��Top=�6�   $	��100��6�   $��23��6�   ��6;   �� *cmdSaveFile+6�   �� 5.   
   $��Save values��6   $
��Left=�6X   $��167,184,0,0q�6s   $	��Top=�6�   $
 0.597,0.489&�6�    d� 	IsChecked 6)   �� *bSaveValues+ �� Mode$ TwoWay=�6�   �� 50   
   -   5         ��Name$ 
lv_Simvars��50      $��270,10,10,10q�6�   ��6'   �� *lSimvarRequests+ �� SelectedItem 6O   �� *oSelectedSimvarRequest+$ OneWayToSource=���51      �� 52       �� 
TargetType" ��6    �� Triggers 53      �� 54       �� Binding 6&   �� *bStillPending+ �� Value True6H   .a�55      ��  �� Property � �� �� Value �	#55D3D3736;   56      58       � ContextMenu 5:      �� 5;      .@�5<      m� $��Remove��6    m� Command 6/   �� *cmdRemoveSelectedRequest+5=       �� View 5?      �� 5@      .P�5A      �� $��Simvar��6%    �� Width$	 200��65    �� DisplayMemberBinding 6A   �� *sName+�� 5B      $��Value��6%   $	 150��64    6@   �� *dValue+ �� StringFormat$ {0:F9}���� 5C      $��Units��6%   $	 150��64    6@   �� *sUnits+5D      5F      �� 5H   
   $��	SimObject��6   $
��Left=�6$   $��
10,208,0,0q�6?   $	��Top=�6S   $��23��6k   $��83��6w   �� 5I   
   -   5         ��Name$ cbb_SimObjectType��5I       �� SelectedIndex$ 0��6�   $
��Left=�6�   $��
11,231,0,0q�6�   $	��Top=�6�   $	��119��6  $��23��6  ��6.   ��  �� Source# [   �� SelectedValue 6t   �� *eSimObjectType+$ OneWayToSource=�6  �� 5J   
   $��ObjectID��6   $
��Left=�6#   $��165,208,0,0q�6>   $	��Top=�6S   $��23��6k   $��71��6w   �� 5K   
   -   5        $ cbb_ObjectIds��5K      $ 0��6�   $
��Left=�6�   $��165,231,0,0q�6�   $	��Top=�6�   $��48��6  $��23��6"  ��6*   �� *
lObjectIDs+ 6M   �� *iObjectIdRequest+$ OneWayToSource=�{�6�   �� *bObjectIDSelectionEnabled+6"  �� 5M   
   $��Simvar Name��6   $
��Left=�6&   $��
10,259,0,0q�6A   $	��Top=�6U   $��23��6m   $��83��6y   �� 5N   
   -	   5        $ cbb_SimvarNames��5N      $ 0��6�   $
��Left=�6�   $��
10,282,0,0q�6�   $	��Top=�6�   $	��238��6
  $��23��6  ��6,   �� # [  6s   �� *sSimvarRequest+$ OneWayToSource=�6  �� 5O   
   $��Index��6   $
��Left=�6    $��
10,310,0,0q�6;   $	��Top=�6O   $��23��6g   $��71��6s   �� 5P   
   -
   5        $ cbb_Indices��5P      $ 0��6�   $
��Left=�6�   $��
10,333,0,0q�6�   $	��Top=�6�   $��45��6�   $��23��6�   ��6(   �� *
aIndices+ 6I   �� *iIndexRequest+$ OneWayToSource=�6�   �� 5Q   
   $��Units��6   $
��Left=�6    $��
70,310,0,0q�6;   $	��Top=�6O   $��23��6g   $��83��6s   �� 5R   
   -   5        $ cbb_UnitNames��5R      $ 0��6�   $
��Left=�6�   $��
70,333,0,0q�6�   $	��Top=�6�   $	��178��6  $��23��6  ��6*   �� # [  6o   �� *sUnitRequest+$ OneWayToSource=�6  �� 5S   
   -   5        $	 btn_Request��5S      $��Add request >��6&   $
��Left=�6c   $��158,370,0,0q�6~   $	��Top=�6�   $��90��6�   $��23                 ��6�   ��6A   �� *cmdAddRequest+6�   �� 5V   
   $��Value��6   $
��Left=�6    $��
10,396,0,0q�6;   $	��Top=�6O   $��23��6g   $��83��6s   �� 5W   
   -   5         ��Name$ tb_SetValue��5W      $
��Left=�6�   $��23��6�   $��
10,419,0,0q�6�     �� TextWrapping$
  Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6R   �� *	sSetValue+$ OneWayToSource=�6�   �� 5X   
   -   5        $	 btn_TrySetValue��5X      $��Try set value >��6*   $
��Left=�6�   $��158,419,0,0q�6�   $	��Top=�6�   $��90��6�   $��23��6�   ��6G   �� *cmdTrySetValue+{�6j   �� *
bConnected+6�   A� 5Z   
   $
��Left=�6   $��14��6-   $��
251,10,0,0q�69   
���   �$	��Top=�6\   $��14��6t   ��5[      �� 5\      " A�6    5]      �� 5^       6&   �� *
bConnected+	 false6E   .a�5_      ��  � �� �	#FFFF503065   5`      �� 5a       6&   �� *
bConnected+ true6E   .a�5b      ��  � �� �	#FF50FF3065   5c      �� 5d       6&   �� *
bOddTick+ true6C   .a�5f      ��  � �� �	#FF45E2F465   5g      5i      5k      �� 5m   
   $��Errors��6   $
��Left=�6!   $��
10,454,0,0q�6<   $	��Top=�6P   $��23��6h   $��83��6t   �� 5o   
   $
 0.5,0.5&�69   $	��236��6Y   $
��Left=�6e   $��10,482,0,10q�6�   ��6   �� *lErrorMessages+��5p      �� 5q      " ��6   .�5r      ��  � ��
 �  ��63   5s      ��5u      �� 5v      .R�5w      $� $��Bottom=�6-   5x      5z      �� 5|   
   $��	Frequency��6   $
��Left=�6$   $��
183,61,0,0q�6?   $	��Top=�6S   $��25��6k   �� 5}   
   -   5        ! ��Name$! sl_Tick��5}      " � Value$" 10Q�6"   $
��Left=�6X   $��
183,86,0,0q�6s   $	��Top=�6�   $��63��6�   # � Minimum$# 10Q�6�   $ � Maximum$
$ 3000Q�6�   % � LargeChange$	% 100Q�6�   & � SmallChange$& 10Q�6�   ' �� IsDirectionReversed' . 6�   �� 5~   
   $&�� Use FSX-compatible configuration��6   $
��Left=�6p   $��
29,121,0,0q�6�   $	��Top=�6�   $	��217��6�    6>   �� *bFSXcompatible+$ TwoWay=�6�   5�      5�      