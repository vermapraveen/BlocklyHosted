using System.Collections.Generic;
using System.Runtime.InteropServices;

using DotLiquid;

namespace ModelGenerator.CSharp
{

	[LiquidType("classes", "namespace", "db", "api", "appname")]
	public class CsData
	{
		public string @namespace { get; set; }
		public string appname { get; set; }
		public Db db { get; set; }
		public Api api { get; set; }
		public List<CsClass> classes { get; set; }

		public CsData()
		{
			db = new Db();
			api = new Api();
			classes = new List<CsClass>();
		}

		public CsClass AddNewClass(string name)
		{
			CsClass aClass = new CsClass(name);
			classes.Add(aClass);

			return aClass;
		}

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
				public string type { get; set; }
				public string name { get; set; }
			}
		}

		[LiquidType("endpoint")]
		public class Api
		{			
			public Endpoint endpoint { get; set; }

			public Api()
			{
				endpoint = new Endpoint();
			}

			[LiquidType("name", "model")]
			public class Endpoint
			{
				public string name { get; set; }
				public string model { get; set; }
			}
		}
		
		[LiquidType("connstring")]
		public class Db
		{
			public string connstring { get; set; }
		}
	}
}