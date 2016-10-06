
using NUnit.Framework;
using System;

namespace HermaFx.DataAnnotations
{
	[TestFixture]
	public class NotDefaultFixture
	{
		private class Model
		{
			[NotDefault]
			public Guid Value { get; set; }
		}

		[Test]
		public void IsNotValidWithNulls()
		{
			var model = new Model() { };
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(model));
		}

		#region Guid

		private class GuidModel
		{
			[NotDefault]
			public Guid Value { get; set; }
		}

		[Test]
		public void GuidIsValid()
		{
			var model = new GuidModel() { Value = Guid.NewGuid() };
			ExtendedValidator.EnsureIsValid(model);
		}

		[Test]
		public void GuidIsNotValid()
		{
			var model = new GuidModel() { Value = default(Guid) };
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(model));
		}

		// GUID?
		private class GuidNulableModel
		{
			[NotDefault]
			public Guid? Value { get; set; }
		}

		[Test]
		public void GuidNulableIsValid()
		{
			var model = new GuidNulableModel() { Value = Guid.NewGuid() };
			ExtendedValidator.EnsureIsValid(model);
		}

		#endregion

		#region int

		private class IntModel
		{
			[NotDefault]
			public int Value { get; set; }
		}

		[Test]
		public void IntIsValid()
		{
			var model = new IntModel() { Value = 5 };
			ExtendedValidator.EnsureIsValid(model);
		}

		[Test]
		public void IntIsNotValid()
		{
			var model = new IntModel() { Value = default(int) };
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(model));
		}

		// int?
		private class IntNulableModel
		{
			[NotDefault]
			public int? Value { get; set; }
		}

		[Test]
		public void IntNulableIsValid()
		{
			var model = new IntNulableModel() { Value = 5 };
			ExtendedValidator.EnsureIsValid(model);
		}

		#endregion

		#region short

		private class ShortModel
		{
			[NotDefault]
			public short Value { get; set; }
		}

		[Test]
		public void ShortIsValid()
		{
			var model = new ShortModel() { Value = (short)5 };
			ExtendedValidator.EnsureIsValid(model);
		}

		[Test]
		public void ShortIsNotValid()
		{
			var model = new ShortModel() { Value = default(short) };
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(model));
		}

		// short?
		private class ShortNulableModel
		{
			[NotDefault]
			public short? Value { get; set; }
		}

		[Test]
		public void ShortNulableIsValid()
		{
			var model = new ShortNulableModel() { Value = (short)5 };
			ExtendedValidator.EnsureIsValid(model);
		}

		#endregion

		#region bool

		private class BoolModel
		{
			[NotDefault]
			public bool Value { get; set; }
		}

		[Test]
		public void BoolIsValid()
		{
			var model = new BoolModel() { Value = true };
			ExtendedValidator.EnsureIsValid(model);
		}

		[Test]
		public void BoolIsNotValid()
		{
			var model = new BoolModel() { Value = default(bool) };
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(model));
		}

		// Bool?
		private class BoolNulableModel
		{
			[NotDefault]
			public bool? Value { get; set; }
		}

		[Test]
		public void BoolNulableIsValid()
		{
			var model = new BoolNulableModel() { Value = true };
			ExtendedValidator.EnsureIsValid(model);
		}

		#endregion

		#region byte

		private class ByteModel
		{
			[NotDefault]
			public byte Value { get; set; }
		}

		[Test]
		public void ByteIsValid()
		{
			var model = new ByteModel() { Value = byte.MaxValue };
			ExtendedValidator.EnsureIsValid(model);
		}

		[Test]
		public void ByteIsNotValid()
		{
			var model = new ByteModel() { Value = default(byte) };
			Assert.Throws<AggregateValidationException>(() => ExtendedValidator.EnsureIsValid(model));
		}

		// byte?
		private class byteNulableModel
		{
			[NotDefault]
			public byte? Value { get; set; }
		}

		[Test]
		public void ByteNulableIsValid()
		{
			var model = new byteNulableModel() { Value = byte.MaxValue };
			ExtendedValidator.EnsureIsValid(model);
		}

		#endregion

		#region String

		private class StringModel
		{
			[NotDefault]
			public string Value { get; set; }
		}

		[Test]
		public void StringIsValid()
		{
			var model = new StringModel() { Value = "Hello" };
			ExtendedValidator.EnsureIsValid(model);
		}

		#endregion
	}
}
