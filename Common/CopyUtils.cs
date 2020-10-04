namespace Common
{
	public class CopyUtils
	{
		public static T GerDeepCloneOf<T>(T original)
		{
			var srzObj = Newtonsoft.Json.JsonConvert.SerializeObject(original);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(srzObj);
		}
	}
}
