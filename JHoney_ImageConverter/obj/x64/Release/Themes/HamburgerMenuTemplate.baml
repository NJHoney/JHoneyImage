   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  P LJHoney_ImageConverter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:JHoney_ImageConverter.ThemesJHoney_ImageConverter.Themes R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089H DMahApps.Metro, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null  �� MergedDictionaries �� Source -  )HamburgerMenuSelectionIndicatorThemeWidth . *HamburgerMenuSelectionIndicatorThemeHeight   BooleanToVisibilityConverter  HamburgerButtonStyle  ChromelessButtonStyle �� BasedOn �� 
TargetType �� Property  
BlackBrush �� Value �� 
TargetType �Name J� Opacity	 ��Name
 J� SnapsToDevicePixels �� Triggers P� Property P� Value �� 
TargetName0   *System.Windows.Media.Animation.QuinticEase 
 Freeze  ScrollBarEaseInOut7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode  HamburgerScrollViewerStyle 	 MetroScrollViewer � ColumnDefinitions ��Name � RowDefinitions ��Name �� CanContentScroll �� CanHorizontallyScroll �� CanVerticallyScroll ��Name4  .System.Windows.Automation.AutomationProperties   AutomationId � Cursor �� ScrollableHeight � Maximum � Minimum �� ViewportHeight �� ViewportSize* �� #ComputedVerticalScrollBarVisibility  � Value! �� Mode" �� RelativeSource# �� ScrollableWidth$ �� Orientation% �� ViewportWidth,& �� %ComputedHorizontalScrollBarVisibility/  )MahApps.Metro.Controls.ScrollViewerHelper' ^� 
Conditions( �� Binding) �� Path* �� Value+ �� AncestorType&   MahApps.Metro.Controls.SplitView, N� EnterActions- W� EasingFunction. �� 
TargetName/ �� TargetProperty	0 W� To1 g� Duration2 N� ExitActions3 g� 	BeginTime (
 $HamburgerMenuItemFocusVisualTemplate4 � UseLayoutRounding  HamburgerMenuItemStyle  MetroListBoxItem'  !MahApps.Metro.Controls.ItemHelper  AccentColorBrush*  $MahApps.Metro.Controls.HamburgerMenu5 �� FallbackValue6 �� FallbackValue7 �� ClearTypeHint8 ��Name9 �� 	Converter: ��Name; ]� 
Conditions< �� Property  HamburgerListBoxItemStyle  HamburgerMenuListStyle  MetroListBox"=   VerticalScrollBarOnLeftSide  HamburgerListBoxStyle  HamburgerMenuTemplate>  Name?   CompactPaneLength@   CompactPaneLengthA   DisplayModeB   DisplayModeC   OpenPaneLengthD   OpenPaneLengthE   PaneBackgroundF   PaneBackgroundG   PaneForegroundH   PaneForegroundI   PanePlacementJ   PanePlacementK   
IsPaneOpenL   PaneM ]�NameN   HamburgerHeightO   HamburgerWidthP ��Name"Q   HamburgerMenuHeaderTemplate"R   VerticalScrollBarOnLeftSideS   NameT   ItemContainerStyleU   ItemTemplateV   ItemTemplateSelectorW   ItemsSourceX �� SelectionModeY �� SelectedIndexZ �� SelectedItem [   OptionsItemContainerStyle\   OptionsItemTemplate"]   OptionsItemTemplateSelector^   OptionsItemsSource_   Content8  2MahApps.Metro.Controls.TransitioningContentControl`  Namea �� ContentStringFormat'b    RestartTransitionOnContentChangec   ContentTransitiond   
Transitione ��Namef   HamburgerMarging   HamburgerMenuTemplateh   HamburgerVisibility�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     ;Controls,http://metro.mahapps.com/winfx/xaml/controls  Bmc;http://schemas.openxmlformats.org/markup-compatibility/2006  MoptionsAhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/options  1system&clr-namespace:System;assembly=mscorlib  4local*clr-namespace:JHoney_ImageConverter.Themes    �� $Q Kpack://application:,,,/MahApps.Metro;component/Styles/Controls.Buttons.xaml?��� $Q Kpack://application:,,,/MahApps.Metro;component/Styles/Controls.ListBox.xaml?��� $T Npack://application:,,,/MahApps.Metro;component/Styles/Controls.Scrollbars.xaml?�%�  &	        &	      &	      &	      7  &	 �    &	 �    7 	 7  7  7  7  &	
 @	    &	 n	    7  7  &	 M    7  &	 \    7  7  &	 �    7  &	 �    7  7  7  X�6Q�X�24Q��� �� 8   " ��.���  � ��# �  ��  � ��$ 0q���  � ��$ 0q���  � ��$ 0q���  � �� �� " ��.R�� 
������ .+�� $ PART_Background��#��z��$ 0Q��� $	 PART_ContentPresenter��#��z��#��z��#��z��$
 0.75Q���. #
 z
  P�  �@J�IsMouseOver . . ��� $ PART_Background�� �@J�Opacity$	 0.3Q��� $ PART_ContentPresenter�� �@J�Opacity$ 1Q�P�  �@J�IsMouseOver .  . ��� $ PART_Background�� �@J�Opacity$ 0Q��� $ PART_ContentPresenter�� �@J�Opacity$
 0.75Q�P�  � {� .  . ���  � �� �#ADADAD��  � ��$ Center=�   4true $ 	EaseInOut=��� 8   " ��.���  � �� �� " ��.R�� $
 Grid��#��z�� �� $ 
leftColumn��$��* ��� $ rightColumn��$
��Auto � �� $��* ��� $
��Auto �.+��� $! PART_ScrollContentPresenter��$��0��$��0��#��z��# z�� .   .  #��z��#��z���� $ PART_VerticalScrollBar��$��0��$��0��$��2��$��Right=�$ VerticalScrollBar��$ Arrow��# z $ 0Q�$ 0Q�# z #y�z   �� *VerticalOffset+$! OneWay=�" �� *TemplatedParent+�� $ PART_HorizontalScrollBar��$��0��$��2��$��0��$��Bottom=�$ HorizontalScrollBar��$ Arrow��# z# $ 0Q�$ 0Q�$$ 
Horizontal=�# z% #y�z&   �� *HorizontalOffset+$! OneWay=�" �� *TemplatedParent+ P� # �@ VerticalScrollBarOnLeftSide . . ��� $ PART_HorizontalScrollBar�� � ��$ 1���� $! PART_ScrollContentPresenter�� � ��$ 1���� $ PART_VerticalScrollBar�� � ��$
 Left=��� $ 
leftColumn�� � ��$
 Auto ��� $ rightColumn�� � ��$ * �^� ' �� ( �� " �� *Self+$) 	IsVisible�* True�� ( �� " �� *Self+$) IsMouseOver�* True�� ( �� " �� *Self+$)) #ComputedVerticalScrollBarVisibility�* Visible�� ( �� " �� *FindAncestor+"+  $) 
IsPaneOpen�* True, �� .���� .��W� 8-  $. PART_VerticalScrollBar��$/ Opacity�$0 1X�$1 0:0:0.2D�W� 8-  $. PART_HorizontalScrollBar��$/ Opacity�$0 1X�$1 0:0:0.2D�2 �� .���� .��W� $3 0:0:1X�8-  $. PART_VerticalScrollBar��$/ Opacity�$0 0X�$1 0:0:0.5D�W� $3 0:0:1X�8-  $. PART_HorizontalScrollBar��$/ Opacity�$0 0X�$1 0:0:0.5D��� .R��� 
 . 
���   �$��2��4 . �� 8   " ��.���  � ��
 ���� ��  � ��$ 0q���  �@ SelectedBackgroundBrush# �  ��  �@�FocusVisualStyle �� " �� *FindAncestor+"+  $) ItemFocusVisualStyle�$! OneWay=�5 Z� ��  � �� �� " �� *FindAncestor+"+ ��$) 
Foreground�$! OneWay=���  � ��$ Stretch=���  � {� �� *	IsEnabled+$! OneWay=�$
6 true����  � �� �� " �� *FindAncestor+"+  $) 	IsTabStop�$! OneWay=�$
6 True����  � ��$ 0q���  � ��$ 0����  � ��$ 0q���  � �� �� " ��.R�� #��z��#7 z7 .+��� $8 Border��#��z��#��z��#��z��#
 z
 � #��z��.+�� $
��Left=�$��Center=�y��� " �� *FindAncestor+"+  $) ShowSelectionIndicator�$! OneWay=�89  .+��� $: SelectionIndicator��#���   #���  #��z��|�.  $	 0.0Q��� #��z��#��z��#��z��#
 z
  P�  �@��
IsSelected . . ���  �� " �� *Self+$3) -(Controls:ItemHelper.SelectedForegroundBrush)�$! OneWay=� � ���� $ Border�� �� " �� *TemplatedParent+$3) -(Controls:ItemHelper.SelectedBackgroundBrush)�$! OneWay=� � ���� $ SelectionIndicator�� �@J�Opacity$	 1.0Q�]� ; �� < �@��
IsSelected* . �� < �@��IsSelectionActive* . ./���  �� " �� *Self+$:) 4(Controls:ItemHelper.ActiveSelectionForegroundBrush)�$! OneWay=� � ���� $ Border�� �� " �� *TemplatedParent+$:) 4(Controls:ItemHelper.ActiveSelectionBackgroundBrush)�$! OneWay=� � ��]� ; �� < �@J�IsMouseOver* . �� < �@��
IsSelected* . ./��� $ Border�� �� " �� *TemplatedParent+$8) 2(Controls:ItemHelper.HoverSelectedBackgroundBrush)�$! OneWay=� � ��]� ; �� < �@J�IsMouseOver* . �� < �@��
IsSelected* .  ./��� $ Border�� �� " �� *TemplatedParent+$0) *(Controls:ItemHelper.HoverBackgroundBrush)�$! OneWay=� � ��P�  � {� .  . ���  �� " �� *Self+$3) -(Controls:ItemHelper.DisabledForegroundBrush)�$! OneWay=� � ���� $ Border�� �� " �� *Self+$3) -(Controls:ItemHelper.DisabledBackgroundBrush)�$! OneWay=� � ��]� ; �� < � {�* .  �� < �@��
IsSelected* . ./���  �� " �� *Self+$;) 5(Controls:ItemHelper.DisabledSelectedForegroundBrush)�$! OneWay=� � ���� $ Border�� �� " �� *Self+$;) 5(Controls:ItemHelper.DisabledSelectedBackgroundBrush)�$! OneWay=� � ����  � ��$ Stretch=��� 8   " ���� 8   " ��.���  � ��
 ���� ��  � ��$ 0q���  � ��$ Stretch=���  � �� .  ��  � ��$ Disabled=���  � �� �� " ��.R��� $��Border��#��z��#��z��#��z��#
 z
 ._��� #��z��#= z= #��z��|�.  #��z��#
 z
 8�� #��z��.���� #
 z
  P�  �@��
IsGrouping .  . ���  � �� .  ��  �@%�IsVirtualizing .  �� 8   " ���� "  .R�� .+�  $> MainSplitView��#@ z? #B zA ��.  #D zC #F zE #H zG #J zI K �� *
IsPaneOpen+$! TwoWay=�" �� *TemplatedParent+L � $ PaneGrid�� �� $
��Auto ��� $��* ��� $
��Auto �.+�]� $M 
PaneHeader��$��0��#��zN $��0 0 0 8q���. .+�� $ PaneHeaderGap��#��zO $
��Left=��� $P PaneHeaderContent��#��zQ ��.  �� $��ButtonsListView��$��1��#��zC #= zR $S 
Menu items��#��zG #��zT #��zU #��zV #��zW $
��Auto=�$X Single=�8��  $��1��Y �� " �� *TemplatedParent+$) SelectedIndex�$! TwoWay=�Z �� " �� *TemplatedParent+$) SelectedItem�$! TwoWay=��� $��OptionsListView��$��2��#��zC $��0 20 0 8q�$��Bottom=�$S Option items��#��zG #��z[ #��z\ #��z] #��z^ $��Disabled=�$X Single=�8�� $��2��Y �� " �� *TemplatedParent+$) SelectedOptionsIndex�$! TwoWay=�Z �� " �� *TemplatedParent+$) SelectedOptionsItem�$! TwoWay=�._   $` ContentPart��#��z��#��z��#��z��$S Content��#��z��#a za #��z��#��z��b . #
 z
 #d zc �� $e HamburgerButton��#��zO #��zN $��0 0 0 8q�#��zf $
��Left=�$	��Top=�$S Main button��#��zg #��zG #��z��8�� $��0��#y�zh  P�  �@ PanePlacement$ Right=�. ��� $ HamburgerButton�� � ��$ Right=��� $ PaneHeaderGap�� � ��$ Right=�