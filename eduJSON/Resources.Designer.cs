﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eduJSON {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("eduJSON.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate element name &quot;{0}&quot; found..
        /// </summary>
        internal static string ErrorDuplicateElement {
            get {
                return ResourceManager.GetString("ErrorDuplicateElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid element name &quot;{0}&quot; found..
        /// </summary>
        internal static string ErrorInvalidIdentifier {
            get {
                return ResourceManager.GetString("ErrorInvalidIdentifier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; has no matching &quot;{1}&quot;..
        /// </summary>
        internal static string ErrorMissingClosingParenthesis {
            get {
                return ResourceManager.GetString("ErrorMissingClosingParenthesis", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing &quot;:&quot; before &quot;{0}&quot;..
        /// </summary>
        internal static string ErrorMissingSeparator {
            get {
                return ResourceManager.GetString("ErrorMissingSeparator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing &quot;,&quot; or &quot;{1}&quot; before &quot;{0}&quot;..
        /// </summary>
        internal static string ErrorMissingSeparatorOrClosingParenthesis {
            get {
                return ResourceManager.GetString("ErrorMissingSeparatorOrClosingParenthesis", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected trailing data &quot;{0}&quot; found..
        /// </summary>
        internal static string ErrorTrailingData {
            get {
                return ResourceManager.GetString("ErrorTrailingData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown value &quot;{0}&quot; found..
        /// </summary>
        internal static string ErrorUnknownValue {
            get {
                return ResourceManager.GetString("ErrorUnknownValue", resourceCulture);
            }
        }
    }
}
