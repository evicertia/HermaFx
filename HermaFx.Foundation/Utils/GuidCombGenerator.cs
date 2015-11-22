using System;

namespace HermaFx.Utils
{
	public class GuidCombGenerator
	{
		public GuidCombGenerator()
		{
		}

		public static Guid Generate()
		{
			byte[] destinationArray = Guid.NewGuid().ToByteArray();
			DateTime time = new DateTime(1900, 1, 1);
			DateTime now = DateTime.Now;
			TimeSpan span = new TimeSpan(now.Ticks - time.Ticks);
			TimeSpan timeOfDay = now.TimeOfDay;
			byte[] bytes = BitConverter.GetBytes(span.Days);
			byte[] array = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
			Array.Reverse(bytes);
			Array.Reverse(array);
			Array.Copy(bytes, (int)bytes.Length - 2, destinationArray, (int)destinationArray.Length - 6, 2);
			Array.Copy(array, (int)array.Length - 4, destinationArray, (int)destinationArray.Length - 4, 4);
			return new Guid(destinationArray);
		}
	}
}