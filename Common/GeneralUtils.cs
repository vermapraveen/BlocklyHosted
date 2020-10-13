using System;
using System.Threading;

namespace Common
{
	public static class GeneralUtils
	{
		static ulong max = 9223372036854775807;
		static ulong min = 8223372036854775807;

		public static ulong GetNextLong(this Random rand)
		{
			ulong uRange = max - min;
			ulong ulongRand;
			do
			{
				byte[] buf = new byte[8];
				rand.NextBytes(buf);
				ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
			} while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

			return ulongRand % uRange + min;
		}

		private static long lastTimeStamp = DateTime.UtcNow.Ticks;
		public static long GetNextLong2
		{
			get
			{
				long original, newValue;
				do
				{
					original = lastTimeStamp;
					long now = DateTime.UtcNow.Ticks;
					newValue = Math.Max(now, original + 1);
				} while (Interlocked.CompareExchange
							 (ref lastTimeStamp, newValue, original) != original);

				return newValue;
			}
		}
	}
}
