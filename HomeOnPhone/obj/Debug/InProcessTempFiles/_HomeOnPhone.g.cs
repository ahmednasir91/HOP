//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XamlStaticHelperNamespace {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("XamlBuildTask", "4.0.0.0")]
    internal class _XamlStaticHelper {
        
        private static System.WeakReference schemaContextField;
        
        private static System.Collections.Generic.IList<System.Reflection.Assembly> assemblyListField;
        
        internal static System.Xaml.XamlSchemaContext SchemaContext {
            get {
                System.Xaml.XamlSchemaContext xsc = null;
                if ((schemaContextField != null)) {
                    xsc = ((System.Xaml.XamlSchemaContext)(schemaContextField.Target));
                    if ((xsc != null)) {
                        return xsc;
                    }
                }
                if ((AssemblyList.Count > 0)) {
                    xsc = new System.Xaml.XamlSchemaContext(AssemblyList);
                }
                else {
                    xsc = new System.Xaml.XamlSchemaContext();
                }
                schemaContextField = new System.WeakReference(xsc);
                return xsc;
            }
        }
        
        internal static System.Collections.Generic.IList<System.Reflection.Assembly> AssemblyList {
            get {
                if ((assemblyListField == null)) {
                    assemblyListField = LoadAssemblies();
                }
                return assemblyListField;
            }
        }
        
        private static System.Collections.Generic.IList<System.Reflection.Assembly> LoadAssemblies() {
            System.Collections.Generic.IList<System.Reflection.Assembly> assemblyList = new System.Collections.Generic.List<System.Reflection.Assembly>();
            assemblyList.Add(Load("Microsoft.Phone, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1" +
                        "e"));
            assemblyList.Add(Load("Microsoft.Phone.Interop, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0d" +
                        "8c86cda1e"));
            assemblyList.Add(Load("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
            assemblyList.Add(Load("System.Core, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"));
            assemblyList.Add(Load("System.Device, Version=2.0.5.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e"));
            assemblyList.Add(Load("System, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"));
            assemblyList.Add(Load("System.Net, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"));
            assemblyList.Add(Load("System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e" +
                        ""));
            assemblyList.Add(Load("System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"));
            assemblyList.Add(Load("AsyncCtpLibrary_Phone, Version=1.1.4304.19911, Culture=neutral, PublicKeyToken=31" +
                        "bf3856ad364e35"));
            assemblyList.Add(Load("Microsoft.Phone.Controls, Version=7.0.0.0, Culture=neutral, PublicKeyToken=24eec0" +
                        "d8c86cda1e"));
            assemblyList.Add(Load("Microsoft.Phone.Controls.Maps, Version=7.0.0.0, Culture=neutral, PublicKeyToken=2" +
                        "4eec0d8c86cda1e"));
            assemblyList.Add(Load("Microsoft.Phone.Controls.Toolkit, Version=7.0.0.0, Culture=neutral, PublicKeyToke" +
                        "n=b772ad94eb9ca604"));
            assemblyList.Add(Load("Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=null"));
            assemblyList.Add(Load("protobuf-net, Version=2.0.0.480, Culture=neutral, PublicKeyToken=257b51d87d2e4d67" +
                        ""));
            assemblyList.Add(Load("Telerik.Windows.Controls.Primitives, Version=2012.2.607.2040, Culture=neutral, Pu" +
                        "blicKeyToken=5803cfa389c90ce7"));
            assemblyList.Add(Load("Telerik.Windows.Core, Version=2012.2.607.2040, Culture=neutral, PublicKeyToken=58" +
                        "03cfa389c90ce7"));
            assemblyList.Add(System.Reflection.Assembly.GetExecutingAssembly());
            return assemblyList;
        }
        
        private static System.Reflection.Assembly Load(string assemblyNameVal) {
            System.Reflection.AssemblyName assemblyName = new System.Reflection.AssemblyName(assemblyNameVal);
            byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
            System.Reflection.Assembly asm = null;
            try {
                asm = System.Reflection.Assembly.Load(assemblyName.FullName);
            }
            catch (System.Exception ) {
                System.Reflection.AssemblyName shortName = new System.Reflection.AssemblyName(assemblyName.Name);
                if ((publicKeyToken != null)) {
                    shortName.SetPublicKeyToken(publicKeyToken);
                }
                asm = System.Reflection.Assembly.Load(shortName);
            }
            return asm;
        }
    }
}
