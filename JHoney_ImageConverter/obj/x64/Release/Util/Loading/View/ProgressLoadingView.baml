   M S B A M L   `   `   `  ���� P  LJHoney_ImageConverter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:JHoney_ImageConverter.Util.Loading.View'JHoney_ImageConverter.Util.Loading.View  A    ;JHoney_ImageConverter.Util.Loading.View.ProgressLoadingView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Glocal5clr-namespace:JHoney_ImageConverter.Util.Loading.View     H DMahApps.Metro, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  y��� *
Visibility+.��� 
�������.+�� 
���   �$��10q�  � ColumnDefinitions  �� $
��auto ��� $
��auto � � RowDefinitions �� $
��auto ��� $
��auto ��� $
��auto � � 	Resources    panelMessageText �� 
TargetType �� Property �� Value  panelSubMessageText��%�  &	        &	 �     �� " ��.���  � ��$
 Left=���  � ��$ 0,11,0,0q���  � ��$ WrapWithOverflow=���  � ��$
 Bold"���  � ��$ 25(���  � ��$	 Top=���  � ��$ Arial)���  �@J�SnapsToDevicePixels . �� " ��.���  � ��$
 Left=���  � ��$	 0,5q���  � ��$
 Wrap=���  � ��$ 12(���  � ��$	 Top=���  � ��$ Arial)���  �@J�SnapsToDevicePixels . .+�)  #MahApps.Metro.Controls.ProgressRing  $
��Left=�$��0��$��0��$��20q��� $��Center=�$��1��$��0��$	��200��.+��� -    ��Name$ 
txtMessage��#��[  
����������� *LoadingText+�� -   $ txtSubMessage��#��[ ���	#FFA1C3D2���� *SubMessageText+�� $��0��$��2��$��1�� �� Orientation$ 
Horizontal=�$��Right=�$��5q�.+��� 
�������$��3q����� *Value+ �� ElementName$ progressBar���� $	�� / ��
�������$��3q��� 
�������$��3q����� *	Maximum+$ progressBar��� -   	 �Name$	 progressBar��$��0��$��2��$��2��$��12��$��Stretch=�$��10q�
 � Minimum
 �� *ProgressMin+ � Maximum �� *ProgressMax+ � Value �� *ProgressCurrent+