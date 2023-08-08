using System;

using NUnit.Framework;

namespace HermaFx.RuntimeExtensions
{
	public class EnumExtensionsTests
	{
		#region Inner Types

		[Flags]
		public enum _Flags
		{
			Unknown = 0x0,
			Animal = (0x1 << 0),
			Monkey = (0x1 << 1) | Animal,
			Person = (0x1 << 2) | Monkey,
			Unicorn = (0x1 << 3) | Animal
		}

		#endregion

		[Test]
		public void AnyFlags_Method_Returns_True_If_Has_Flag() => Assert.That(_Flags.Person.AnyFlag(_Flags.Unicorn, _Flags.Animal), Is.True);

		[Test]
		public void AnyFlags_Method_Returns_False_If_Has_No_Flag() => Assert.That(_Flags.Unicorn.AnyFlag(_Flags.Person, _Flags.Monkey), Is.False);

		[Test]
		public void AnyFlags_Throws_If_Bad_Args()
		{
			Assert.That(() => EnumExtensions.AnyFlag(null, _Flags.Animal), Throws.Exception.TypeOf<ArgumentNullException>(), "#0");
			Assert.That(() => EnumExtensions.AnyFlag(_Flags.Animal, null), Throws.Exception.TypeOf<ArgumentNullException>(), "#1");
		}
	}
}
