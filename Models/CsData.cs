using System.Collections.Generic;

using DotLiquid;

namespace ModelGenerator.CSharp
{

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

		public List<CsClass> classes { get; set; }


		[LiquidType("name", "props")]
		public class CsClass
		{
			public CsClass(string name)
			{
				this.name = name;
				props = new List<CsClassProp>();
			}

			public string name { get; private set; }
			public List<CsClassProp> props { get; private set; }

			public CsClassProp AddNewProp(string name, string type)
			{
				CsClassProp aProp = new CsClassProp(name, type);
				props.Add(aProp);

				return aProp;
			}


			public CsClassProp AddIdProp()
			{
				CsClassProp aProp = new CsClassProp("Id", "long");
				props.Insert(0, aProp);

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