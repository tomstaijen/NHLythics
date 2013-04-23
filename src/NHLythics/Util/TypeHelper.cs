using System;
using System.Collections.Generic;
using System.Reflection;

namespace NHLythics.Util
{
    public class TypeHelper
    {

        /* all enums found in the assemblies */
        public Dictionary<string, Type> enumTypes = new Dictionary<string, Type>();
        /* all subclasses of Datatype found in the assemblies */
        //public List<Type> dataTypes = new List<Type>();
        /* the assemblies by name (to prevent loading more then once)*/
        private Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
        /* the types in the assemblies by name */
        private Dictionary<string, Type> types = new Dictionary<string, Type>();

        public IEnumerable<Type> Types
        {
            get { return types.Values; }
        }


        public Type getType(string name)
        {
            if (types.ContainsKey(name))
            {
                return types[name];
            }
            return null;
        }

        #region loading

        private String dllpath;

        public TypeHelper(String dllpath, string main)
        {
            this.dllpath = dllpath;
            //loadAll("NHibernate");
            //loadAll("NHibernate.Mapping.Attributes");
            loadAll(main);
        }

        /**
         * Loads the dll with the passed name and all referenced dlls.
        **/

        private void loadAll(string basename)
        {
            AssemblyName[] reffed = load(basename);
            foreach (var name in reffed)
            {
                if (name.Name.ToLower().StartsWith("force.") || name.Name.ToLower().StartsWith("fms.") ||
                    name.Name.ToLower().StartsWith("quion."))
                {
                    if (!assemblies.ContainsKey(name.Name))
                    {
                        loadAll(name.Name);
                    }
                }
            }
        }

        /**
         * Loads the dll with the given name and registers it in the assemblies by its name.
        **/

        private AssemblyName[] load(string name)
        {

            Assembly asm = System.Reflection.Assembly.LoadFrom(dllpath + "\\" + name + ".dll");
            if (!name.Equals(asm.GetName().Name))
            {
                string name2 = asm.GetName().Name;
                Console.WriteLine("Invalid assembly name used: " + name + " + instead of " + name2);
                Environment.Exit(0);
            }
            assemblies.Add(asm.GetName().Name, asm);
            AssemblyName[] reffed = asm.GetReferencedAssemblies();

            //string docpath = dllpath + "\\" + name + ".XML";
            //if (File.Exists(docpath))
            //{
            //    if( !DescriberProgram.docs.Contains(docpath))
            //    {
            //        DescriberProgram.docs.Add(docpath);
            //    }
            //}

            return reffed;
        }

        #endregion loading

        #region singleton

        private static TypeHelper singleton;

        public static TypeHelper instance
        {
            get { return singleton; }
        }

        public static TypeHelper createInstance(String dllpath, string main)
        {
            if (singleton == null)
            {
                singleton = new TypeHelper(dllpath, main);
            }
            return singleton;
        }

        #endregion singleton


        #region type resolution

        public Type getPropertyType(PropertyInfo p)
        {
            List<Type> types = new List<Type>();
            Type t = p.PropertyType;
            if (t.IsGenericType)
            {
                if (t.Name.StartsWith("Nullable") || t.Name.StartsWith("IEnumerable") ||
                    t.Name.StartsWith("ReadOnlyCollection") || t.Name.StartsWith("IList") || t.Name.StartsWith("List"))
                {
                    t = t.GetGenericArguments()[0];
                }
            }
            return t;
        }

        // stores resolved concrete types for a given property type
        private Dictionary<Type, List<Type>> concreteTypesCache = new Dictionary<Type, List<Type>>();
        private Dictionary<Type, List<Type>> subclasses = new Dictionary<Type, List<Type>>();

        private void addSubclass(Type baseType, Type subType)
        {
            subclasses[baseType].Add(subType);
        }

        public List<Type> getConcreteTypes(Type basetype)
        {
            if (!types.ContainsKey(basetype.Name))
            {
                Console.WriteLine("Type not found: " + basetype.Name);
                return new List<Type>();
            }


            List<Type> resolvedTypes;
            if (!concreteTypesCache.ContainsKey(basetype))
            {
                resolvedTypes = new List<Type>();
                // first check if basetype is abstract
                if (!basetype.IsAbstract)
                {
                    resolvedTypes.Add(basetype);
                }
                foreach (Type subtype in subclasses[basetype])
                {
                    resolvedTypes.AddRange(getConcreteTypes(subtype));
                }
                concreteTypesCache.Add(basetype, resolvedTypes);

            }
            else
            {
                resolvedTypes = concreteTypesCache[basetype];
            }
            return resolvedTypes;
        }

        #endregion type resolution
    }
}
