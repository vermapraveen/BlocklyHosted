using System.Collections.Generic;

namespace ModelGenerator.CSharp
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
            var key = typeToCheck.Trim();
            if (TypeMapping.ContainsKey(key))
			{
                return TypeMapping[key];
            }

            return typeToCheck;
        }
    }
}