using System.Collections.Generic;

namespace CodeGenerator.CSharp
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
}