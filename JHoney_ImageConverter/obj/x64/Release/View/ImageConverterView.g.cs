﻿#pragma checksum "..\..\..\..\View\ImageConverterView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A716750C20D808B457D5B348EB68DF51B8BE7EC3"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using GalaSoft.MvvmLight.Command;
using JHoney_ImageConverter.Converter;
using JHoney_ImageConverter.View;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace JHoney_ImageConverter.View {
    
    
    /// <summary>
    /// ImageConverterView
    /// </summary>
    public partial class ImageConverterView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 48 "..\..\..\..\View\ImageConverterView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGrid;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\View\ImageConverterView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas MainCanvas;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\..\..\View\ImageConverterView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton ToggleBinary;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\..\..\View\ImageConverterView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton ToggleGaussian;
        
        #line default
        #line hidden
        
        
        #line 185 "..\..\..\..\View\ImageConverterView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton ToggleCanny;
        
        #line default
        #line hidden
        
        
        #line 207 "..\..\..\..\View\ImageConverterView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton ToggleMedian;
        
        #line default
        #line hidden
        
        
        #line 229 "..\..\..\..\View\ImageConverterView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton ToggleRotate;
        
        #line default
        #line hidden
        
        
        #line 251 "..\..\..\..\View\ImageConverterView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton ToggleEdge;
        
        #line default
        #line hidden
        
        
        #line 293 "..\..\..\..\View\ImageConverterView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ScrollBar scrollBar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/JHoney_ImageConverter;component/view/imageconverterview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\ImageConverterView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.DataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 2:
            this.MainCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 3:
            this.ToggleBinary = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            return;
            case 4:
            this.ToggleGaussian = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            return;
            case 5:
            this.ToggleCanny = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            return;
            case 6:
            this.ToggleMedian = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            return;
            case 7:
            this.ToggleRotate = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            return;
            case 8:
            this.ToggleEdge = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            return;
            case 9:
            this.scrollBar = ((System.Windows.Controls.Primitives.ScrollBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

