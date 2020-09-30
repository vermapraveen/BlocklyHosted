using System.Collections.Generic;
using DotLiquid;

namespace BlkHost.Pages
{
    public class CsTypeChecker
    {
        static Dictionary<string, string> TypeMapping = new Dictionary<string, string>() {
                { "uuid", "Guid"},
                { "integer", "int"},
                { "", "object"},
                { "string", "string"},
                { "boolean", "bool"}
            };

        public static string CheckType(string typeToCheck)
        {
            return TypeMapping[typeToCheck.Trim()];
        }
    }

    public class JsonTypeStrings
    {
        public const string Object = "object";
        public const string Null = "null";
        public const string Array = "array";
        public const string Integer = "integer";
        public const string Boolean = "boolean";
        public const string String = "string";
    }

    [LiquidType("classes")]
    public class CsData
    {
        public CsData()
        {
            classes = new List<CsClass>();
        }

        public CsClass AddNewClass(string name)
        {
            CsClass aClass = new CsClass(name);
            classes.Add(aClass);

            return aClass;
        }

        public IList<CsClass> classes { get; set; }


        [LiquidType("name", "props")]
        public class CsClass
        {
            public CsClass(string name)
            {
                this.name = name;
                props = new List<CsClassProp>();
            }

            public string name { get; private set; }
            public IList<CsClassProp> props { get; private set; }

            public CsClassProp AddNewProp(string name, string type)
            {
                CsClassProp aProp = new CsClassProp(name, type);
                props.Add(aProp);

                return aProp;
            }


            [LiquidType("name", "type")]
            public class CsClassProp
            {
                public CsClassProp(string name, string type)
                {
                    this.name = name;
                    this.type = type;
                }
                public string type { get; private set; }
                public string name { get; private set; }
            }
        }
    }
}