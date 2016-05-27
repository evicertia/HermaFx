using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using NUnit.Framework;

namespace HermaFx.ComponentModel
{
	public class StringArrayConverterTests
	{
		[Test]
		public void CanConvertFromAssertions()
		{
			var converter = new StringArrayConverter(",", StringSplitOptions.RemoveEmptyEntries);

			Assert.That(converter.CanConvertFrom(typeof(string)), Is.True);
			Assert.That(converter.CanConvertFrom(typeof(int)), Is.False);
		}

		[Test]
		public void CanConvertToAssertions()
		{
			var converter = new StringArrayConverter(",", StringSplitOptions.RemoveEmptyEntries);

			Assert.That(converter.CanConvertTo(typeof(string)), Is.True);
			Assert.That(converter.CanConvertTo(typeof(int)), Is.False);
		}

		[Test]
		public void ConvertFromString()
		{
			var converter = new StringArrayConverter(",", StringSplitOptions.RemoveEmptyEntries);

			Assert.That(converter.ConvertFrom("one, two"), Has.Length.EqualTo(2));
			Assert.That(converter.ConvertFrom("one, two"), Is.EquivalentTo(new[] { "one", "two" }));
		}

		[Test]
		public void GenericCanConvertFromAssertions()
		{
			var converter = new StringArrayConverter<MemberTypes>(",", StringSplitOptions.RemoveEmptyEntries);

			Assert.That(converter.CanConvertFrom(typeof(string)), Is.True);
			Assert.That(converter.CanConvertFrom(typeof(int)), Is.False);
		}

		[Test]
		public void GenericCanConvertToAssertions()
		{
			var converter = new StringArrayConverter<MemberTypes>(",", StringSplitOptions.RemoveEmptyEntries);

			Assert.That(converter.CanConvertTo(typeof(string)), Is.True);
			Assert.That(converter.CanConvertTo(typeof(int)), Is.False);
		}

		[Test]
		public void GenericConvertFromString()
		{
			var converter = new StringArrayConverter(",", StringSplitOptions.RemoveEmptyEntries);

			Assert.That(converter.ConvertFrom("Custom, Field"), Has.Length.EqualTo(2));
			Assert.That(converter.ConvertFrom("Custom, Field"), Is.EquivalentTo(new[] { MemberTypes.Custom, MemberTypes.Field }));
		}

	}
}
