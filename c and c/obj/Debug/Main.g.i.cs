#pragma checksum "..\..\Main.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "A81FFC51C6BC9FF2C93B53D5375C1EE53DD8AC488DBFD66144972CC81A950E95"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CC;
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


namespace CC
{


    /// <summary>
    /// Main
    /// </summary>
    public partial class Main : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {


#line 12 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem UsersMenuButton;

#line default
#line hidden


#line 15 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock userLabel;

#line default
#line hidden


#line 16 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame frame;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CC;component/main.xaml", System.UriKind.Relative);

#line 1 "..\..\Main.xaml"
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
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.UsersMenuButton = ((System.Windows.Controls.MenuItem)(target));

#line 12 "..\..\Main.xaml"
                    this.UsersMenuButton.Click += new System.Windows.RoutedEventHandler(this.UsersMenuButton_Click);

#line default
#line hidden
                    return;
                case 2:
                    this.userLabel = ((System.Windows.Controls.TextBlock)(target));
                    return;
                case 3:
                    this.frame = ((System.Windows.Controls.Frame)(target));

#line 16 "..\..\Main.xaml"
                    this.frame.LoadCompleted += new System.Windows.Navigation.LoadCompletedEventHandler(this.frame_LoadCompleted);

#line default
#line hidden
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.MenuItem CalibMenuButton;
        internal System.Windows.Controls.MenuItem PosMenuButton;
        internal System.Windows.Controls.MenuItem NegMenuButton;
        internal System.Windows.Controls.MenuItem AntigenMenuButton;
        internal System.Windows.Controls.MenuItem ArrayMenuButton;
        internal System.Windows.Controls.MenuItem SearchBatchButton;
        internal System.Windows.Controls.MenuItem SearchCalibButton;
        internal System.Windows.Controls.MenuItem SearchNegButton;
        internal System.Windows.Controls.MenuItem SearchPosButton;
        internal System.Windows.Controls.MenuItem SearchArrayButton;
        internal System.Windows.Controls.MenuItem AssignBatchButton;
        internal System.Windows.Controls.MenuItem SearchPlateButton;
        internal System.Windows.Controls.MenuItem AddUserButton;
        internal System.Windows.Controls.MenuItem AdminMenu;
        internal System.Windows.Controls.MenuItem Report1;
        internal System.Windows.Controls.MenuItem Report2;
    }
}

