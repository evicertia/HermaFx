﻿#region LICENSE
// Source Code licensed under MS-PL.
// Derived from: MVC Foolproof Validation (http://foolproof.codeplex.com/)  
# endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HermaFx.DataAnnotations
{
	public class OperatorMetadata
	{
		public string ErrorMessage { get; set; }
		public Func<object, object, bool> IsValid { get; set; }

		static OperatorMetadata()
		{
			CreateOperatorMetadata();
		}

		private static Dictionary<Operator, OperatorMetadata> _operatorMetadata;

		public static OperatorMetadata Get(Operator @operator)
		{
			return _operatorMetadata[@operator];
		}

		private static void CreateOperatorMetadata()
		{
			_operatorMetadata = new Dictionary<Operator, OperatorMetadata>()
			{
				{
					Operator.EqualTo, new OperatorMetadata()
					{
						ErrorMessage = "equal to",
						IsValid = (value, dependentValue) => {
							if (value == null && dependentValue == null)
								return true;
							else if (value == null && dependentValue != null)
								return false;
							else if (value is Enum && dependentValue is Enum)
								// TODO: Is FlagAttribute checking needed in this case??
								return (dependentValue as Enum).HasFlag(value as Enum);

							return value.Equals(dependentValue);
						}
					}
				},
				{
					Operator.NotEqualTo, new OperatorMetadata()
					{
						ErrorMessage = "not equal to",
						IsValid = (value, dependentValue) => {
							if (value == null && dependentValue != null)
								return true;
							else if (value == null && dependentValue == null)
								return false;
							else if (value is Enum && dependentValue is Enum)
								// TODO: Is FlagAttribute checking needed in this case??
								return !(dependentValue as Enum).HasFlag(value as Enum);

							return !value.Equals(dependentValue);
						}
					}
				},
				{
					Operator.GreaterThan, new OperatorMetadata()
					{
						ErrorMessage = "greater than",
						IsValid = (value, dependentValue) => {
							if (value == null || dependentValue == null)
								return false;

							return Comparer<object>.Default.Compare(value, dependentValue) >= 1;
						}
					}
				},
				{
					Operator.LessThan, new OperatorMetadata()
					{
						ErrorMessage = "less than",
						IsValid = (value, dependentValue) => {
							if (value == null || dependentValue == null)
								return false;

								return Comparer<object>.Default.Compare(value, dependentValue) <= -1;
						}
					}
				},
				{
					Operator.GreaterThanOrEqualTo, new OperatorMetadata()
					{
						ErrorMessage = "greater than or equal to",
						IsValid = (value, dependentValue) => {
							if (value == null && dependentValue == null)
								return true;

							if (value == null || dependentValue == null)
								return false;

							return Get(Operator.EqualTo).IsValid(value, dependentValue) || Comparer<object>.Default.Compare(value, dependentValue) >= 1;
						}
					}
				},
				{
					Operator.LessThanOrEqualTo, new OperatorMetadata()
					{
						ErrorMessage = "less than or equal to",
						IsValid = (value, dependentValue) => {
							if (value == null && dependentValue == null)
								return true;

							if (value == null || dependentValue == null)
								return false;

							return Get(Operator.EqualTo).IsValid(value, dependentValue) || Comparer<object>.Default.Compare(value, dependentValue) <= -1;
						}
					}
				},
				{
					Operator.RegExMatch, new OperatorMetadata()
					{
						ErrorMessage = "a match to",
						IsValid = (value, dependentValue) => {
							return Regex.Match((value ?? "").ToString(), dependentValue.ToString()).Success;
						}
					}
				},
				{
					Operator.NotRegExMatch, new OperatorMetadata()
					{
						ErrorMessage = "not a match to",
						IsValid = (value, dependentValue) => {
							return !Regex.Match((value ?? "").ToString(), dependentValue.ToString()).Success;
						}
					}
				},
				{
					Operator.HasFlag, new OperatorMetadata()
					{
						ErrorMessage = "has flag",
						IsValid = (value, dependentValue) => {
							if (value == null && dependentValue == null)
								return true;
							else if (value == null && dependentValue != null)
								return false;

							Guard.Against<ArgumentOutOfRangeException>(!(value is Enum), "Value should be an (bit-flags) enum.");

							var dependentEnumValue = Enum.ToObject(value.GetType(), dependentValue) as Enum;
							return dependentEnumValue.HasFlag(value as Enum);
						}
					}
				},
				{
				Operator.NotHasFlag, new OperatorMetadata()
				{
					ErrorMessage = "not has flag",
					IsValid = (value, dependentValue) => {
						if (value == null && dependentValue != null)
							return true;
						else if (value == null && dependentValue == null)
							return false;

						Guard.Against<ArgumentOutOfRangeException>(!(value is Enum), "Value should be an (bit-flags) enum.");

						var dependentEnumValue = Enum.ToObject(value.GetType(), dependentValue) as Enum;
						return !dependentEnumValue.HasFlag(value as Enum);
					}
				}
				},
			};
		}
	}
}
