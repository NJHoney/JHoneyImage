   M S B A M L   `   `   `  ���� P  LJHoney_ImageConverter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:JHoney_ImageConverter.ViewJHoney_ImageConverter.View  Q-clr-namespace:JHoney_ImageConverter.ConverterJHoney_ImageConverter.Converter  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity b ^GalaSoft.MvvmLight.Platform, Version=5.4.1.0, Culture=neutral, PublicKeyToken=5f873c45e98af8a1lMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.PlatformGalaSoft.MvvmLight.Command 3    -JHoney_ImageConverter.View.ImageConverterView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :local(clr-namespace:JHoney_ImageConverter.View     H DMahApps.Metro, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  6vc-clr-namespace:JHoney_ImageConverter.Converter  XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  WcmdMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Platform  y��� *
Visibility+  � 	Resources  ;  5JHoney_ImageConverter.Converter.MultiBindingConverter   MBC��%   &	          .��� $��5q� � ColumnDefinitions �� $��* ��� $
��auto � � RowDefinitions �� $
��auto ��� $
��auto ��� $��* �.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ KeyDown��.  (System.Windows.Interactivity.TriggerBase   Actions. /  )GalaSoft.MvvmLight.Command.EventToCommand     PassEventArgsToCommand .    Command �� *,*ImageConvertViewModelKeyEvent.EventKeyDown+  $ KeyUp��.    .  �� **(ImageConvertViewModelKeyEvent.EventKeyUp+  $ KeyUp��.    .  �� .+�� $��0��$��0�� �� $
��auto ��� $
��auto �.+��� $��0��$��5q�$��Center=�.����  �� Orientation$ 
Horizontal=�$��5q�.+�&   System.Windows.Controls.DataGrid  -   	  Name$	 DataGrid��$��Center=�$��Center=����� *ImageInfoDataGridList+   $ Loaded��.    �� 
 �� Path$
 CommandLoaded�   CommandParameter ��  �� ElementName$ DataGrid�� �� Mode$ OneWay=��� $��1��$��5q�$��Center=�y��� *IsSelectRectangle+ �� 	Converter A  ;JHoney_ImageConverter.Converter.BoolenToVisibilityConverter  .���� $ 
Horizontal=�$��5q�$��Center=�$��Center=�.+��� $��ROI X : ��$��Center=�$��5q��� $��35��$��15��$��Center=�$��5q����� *StartRectPointX+�� $��ROI Y : ��$��Center=�$��5q��� $��35��$��Center=�$��5q����� *StartRectPointY+�� $��Width : ��$��Center=�$��5q��� $��35��$��Center=�$��5q����� *	RectWidth+�� $��	Height : ��$��Center=�$��5q��� $��35��$��Center=�$��5q����� *
RectHeight+�� $
��Crop��$��5q�$��Center=�$
��Crop������ *CommandCropResize+�� $��ReSize��$��5q�$��Center=�$��ReSize������ *CommandCropResize+�� $��0��$��2��$
��Auto=�$
��Auto=�.���� -    ��Name$ 
MainCanvas�� J� 	AllowDrop . ���� *ImageShow.ImageBrush+���� *Background.ImageSource.Width+$ 
MainCanvas��$ OneWay=����� *Background.ImageSource.Height+$ 
MainCanvas��$ OneWay=� � LayoutTransform V� .~���  �� ScaleX �� *ImageShow.ImageBrushScaleX+ �� ScaleY �� *ImageShow.ImageBrushScaleY+   $ Loaded��.    �� $
 CommandLoaded� �� $ 
MainCanvas��$ OneWay=�  $ MouseUp��.    .  �� $
 CanvasEventMouseUp�  $ 	MouseDown��.    .  �� $
 CanvasEventMouseDown�  $ 	MouseMove��.    .  �� $
 CanvasEventMouseMove�  $
 Drop��.    .  �� $
 CommandDropFile�  $ 
MouseWheel��.    .  �� $
 CanvasEventMouseWheel� � ContextMenu �� .@�m�  m� CommandParameter$ Normal��$��Normal�� m� Command �� *CanvasContext+ m� 	IsChecked �� *IsSelectNormal+m� $ SetROI��$��SetROI�� m� IsCheckable .  �� *CanvasContext+ �� *IsSelectRectangle+m� $ ToList��$��ToList�� �� *CanvasContext+m� $
 Save��$
��Save�� �� *CanvasContext+.+��� 
���  �� J� RenderTransformOrigin$ 0.5,0.5&�$��5������ *	RectWidth+���� *
RectHeight+y��� *IsSelectRectangle+   z�V� .~��� �� �� T�  T� X �� *StartRectPointX+ T� Y �� *StartRectPointY+�� $��1��$��0��$��3��$��5q�.���� $ Vertical=�$��5q�$��Center=�$	��Top=�.+�9� $��Single Converter�� 9� 
IsExpanded . $��5q�.���� $ Vertical=�$��Center=�$��5q�.+��� $��	GrayScale��$��5q�$��	GrayScale������ *CommandSingleConvert+�� $��Erode��$��5q�$��Erode������ *CommandSingleConvert+�� $��Dilate��$��5q�$��Dilate������ *CommandSingleConvert+�� $��Reverse��$��5q�$��Reverse������ *CommandSingleConvert+�� $	��Red��$��5q�$	��Red������ *CommandSingleConvert+�� $��Green��$��5q�$��Green������ *CommandSingleConvert+�� $
��Blue��$��5q�$
��Blue������ *CommandSingleConvert+�� $
��Test��$��5q�$
��Test������ *CommandSingleConvert+9� $��Parameter Converter�� .  $��5q�.���� $ Vertical=�$��Center=�$��5q�.+�d� -    d�Name$ ToggleBinary��$��Binary��{��� *TogleButtonEnabled[0]+   $ Checked��.    �� $
 CommandToggle� `�  `� 	Converter# [  .1��� $ ToggleBinary��  $ 	Unchecked��.    �� $
 CommandToggle� `� # [  .1��� $ ToggleBinary��d� -   $ ToggleGaussian��$��GaussianBlur��{��� *TogleButtonEnabled[1]+   $ Checked��.    �� $
 CommandToggle� `� # [  .1��� $ ToggleGaussian��  $ 	Unchecked��.    �� $
 CommandToggle� `� # [  .1��� $ ToggleGaussian��d� -   $ ToggleCanny��$��
Canny Edge��{��� *TogleButtonEnabled[2]+   $ Checked��.    �� $
 CommandToggle� `� # [  .1��� $ ToggleCanny��  $ 	Unchecked��.    �� $
 CommandToggle� `� # [  .1��� $ ToggleCanny��d� -   $ ToggleMedian��$��Median Blur��{��� *TogleButtonEnabled[3]+   $ Checked��.    �� $
 CommandToggle� `� # [  .1��� $ ToggleMedian��  $ 	Unchecked��.    �� $
 CommandToggle� `� # [  .1��� $ ToggleMedian��d� -   $ ToggleRotate��$��Rotate��{��� *TogleButtonEnabled[4]+   $ Checked��.    �� $
 CommandToggle� `� # [  .1��� $ ToggleRotate��  $ 	Unchecked��.    �� $
 CommandToggle� `� # [  .1��� $ ToggleRotate��d� -   $ 
ToggleEdge��$��Rotate��{��� *TogleButtonEnabled[5]+   $ Checked��.    �� $
 CommandToggle� `� # [  .1��� $ 
ToggleEdge��  $ 	Unchecked��.    �� $
 CommandToggle� `� # [  .1��� $ 
ToggleEdge���� $��0��$��1��$��5q�y��� *	IsToggled+   .���  �� $
��auto ��� $��* � �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$��0��$��5q�$��Center=�$��Center=����� *OptionParamText01+�� -	    ��Name$ 	scrollBar��$��0��$��1��  �� Orientation$  
Horizontal=�$	��200��$��5q�$��Stretch=�$��Center=�! � SmallChange$! 1Q�" � LargeChange$" 10Q�# � Minimum# �� *OptionParamSlider_01_Min+$ � Maximum$ �� *OptionParamSlider_01_Max+   $ ValueChanged��.    �� $
 CommandChangeSliderValue� �� *Value+$ 	scrollBar���� $��0��$��2��$��5q�$��Center=�$��Center=�% �� TextAlignment$% Right=����� *Value+$ 	scrollBar�� 4  .JHoney_ImageConverter.Converter.FloatToInteger  �� $��0��$��3��$��Confirm������ *CommandConfirmChange+