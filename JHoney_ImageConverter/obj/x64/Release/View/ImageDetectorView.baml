   M S B A M L   `   `   `  ���� P  LJHoney_ImageConverter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:JHoney_ImageConverter.ViewJHoney_ImageConverter.View  Q-clr-namespace:JHoney_ImageConverter.ConverterJHoney_ImageConverter.Converter  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity b ^GalaSoft.MvvmLight.Platform, Version=5.4.1.0, Culture=neutral, PublicKeyToken=5f873c45e98af8a1lMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.PlatformGalaSoft.MvvmLight.Command 2    ,JHoney_ImageConverter.View.ImageDetectorView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :local(clr-namespace:JHoney_ImageConverter.View     H DMahApps.Metro, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  6vc-clr-namespace:JHoney_ImageConverter.Converter  XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  WcmdMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Platform  y��� *
Visibility+.���   � ColumnDefinitions  �� $
��auto ��� $��* ��� $
��auto � � RowDefinitions �� $
��auto ��� $��* ��� $
��auto �.+��� $��1��$��1��$
��Auto=�$
��Auto=�.���� -    ��Name$ 
MainCanvas�� J� 	AllowDrop . ���� *ImageShow.ImageBrush+���� *Background.ImageSource.Width+ �� ElementName$ 
MainCanvas�� �� Mode$ OneWay=����� *Background.ImageSource.Height+$ 
MainCanvas��$ OneWay=� � LayoutTransform V� .~���  �� ScaleX �� *ImageShow.ImageBrushScaleX+ �� ScaleY �� *ImageShow.ImageBrushScaleY+.  (System.Windows.Interactivity.Interaction	   Triggers	 /  )System.Windows.Interactivity.EventTrigger  
   	EventName$
 Loaded��.  (System.Windows.Interactivity.TriggerBase   Actions. /  )GalaSoft.MvvmLight.Command.EventToCommand     Command ��  �� Path$ CommandLoaded�   CommandParameter �� $ 
MainCanvas��$ OneWay=�  $
 MouseUp��.      PassEventArgsToCommand .  �� $ CanvasEventMouseUp�  $
 	MouseDown��.    .  �� $ CanvasEventMouseDown�  $
 	MouseMove��.    .  �� $ CanvasEventMouseMove�  $

 Drop��.    .  �� $ CommandDropFile�  $
 
MouseWheel��.    .  �� $ CanvasEventMouseWheel� � ContextMenu �� .@�m�  m� CommandParameter$ Normal��$��Normal�� m� Command �� *CanvasContext+ m� 	IsChecked �� *IsSelectNormal+m� $ SetROI��$��SetROI�� m� IsCheckable .  �� *CanvasContext+ �� *IsSelectRectangle+.+��� 
���  �� J� RenderTransformOrigin$ 0.5,0.5&�$��5������ *	RectWidth+���� *
RectHeight+y��� *IsSelectRectangle+ �� 	Converter A  ;JHoney_ImageConverter.Converter.BoolenToVisibilityConverter  z�V� .~��� �� �� T�  T� X �� *StartRectPointX+ T� Y �� *StartRectPointY+�� $��2��$��1��$��2��$��Detect Options��$��5q�.����  �� Orientation$ Vertical=�$��5q�.+��� $ Vertical=�$��5q�.+��� -    ��Name$ itemsControl������ *
WorkList+���� .R��� $ Vertical=����� .R�	�  	� 	GroupName$ CommandSelect������ *	.Header+���� *	Command+$ DummyBtn���� -    ��Name$ DummyBtn��$y�	Collapsed=����� *CommandConvertSelect+� 