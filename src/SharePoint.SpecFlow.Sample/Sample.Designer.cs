﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SharePoint.SpecFlow.Sample {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Sample : global::System.Configuration.ApplicationSettingsBase {
        
        private static Sample defaultInstance = ((Sample)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Sample())));
        
        public static Sample Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://rp2013-3:113")]
        public string SiteUri {
            get {
                return ((string)(this["SiteUri"]));
            }
            set {
                this["SiteUri"] = value;
            }
        }
    }
}