﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Conexion.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.14.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FitLife2;Integrated Security=T" +
            "rue;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False")]
        public string FitLife2ConnectionString {
            get {
                return ((string)(this["FitLife2ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=LAPTOP-5K2SBFSQ;Initial Catalog=FitLife;Integrated Security=True;Conn" +
            "ect Timeout=30;Encrypt=True;TrustServerCertificate=True")]
        public string FitLifeConnectionString {
            get {
                return ((string)(this["FitLifeConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=MCCARTHYS\\SQLEXPRESS;Initial Catalog=FitLife2;Integrated Security=Tru" +
            "e;TrustServerCertificate=True")]
        public string FitLife2ConnectionString1 {
            get {
                return ((string)(this["FitLife2ConnectionString1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=DESKTOP-7UPKFS7\\SQLEXPRESS;Initial Catalog=FitLife2;Integrated Securi" +
            "ty=True;Encrypt=True;TrustServerCertificate=True")]
        public string FitLife2ConnectionString2 {
            get {
                return ((string)(this["FitLife2ConnectionString2"]));
            }
        }
    }
}
