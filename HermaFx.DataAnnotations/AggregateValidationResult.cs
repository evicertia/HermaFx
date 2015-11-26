using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace HermaFx.DataAnnotations
{
	/// <summary>
	/// Class to hold multiple validation errors.
	/// </summary>
	public class AggregateValidationResult : ValidationResult
	{
		private const string DefaultErrorMessage = "Some invalid values detected.";
		private const string TypedErrorMessage = "Object of type {0} has some invalid values.";
		private const string NestedErrorMessage = "{0} has some invalid values.";

		private readonly List<ValidationResult> _results = new List<ValidationResult>();

		public IEnumerable<ValidationResult> Results
		{
			get
			{
				return _results;
			}
		}

		private AggregateValidationResult(string errorMessage, IEnumerable<ValidationResult> results) 
			: base(errorMessage)
		{
			if (results != null) _results.AddRange(results);
		}

		#region Factory Ctors
		public static AggregateValidationResult CreateFor(IEnumerable<ValidationResult> results)
		{
			return new AggregateValidationResult(DefaultErrorMessage, results);
		}

		public static AggregateValidationResult CreateFor(string errorMessage, IEnumerable<ValidationResult> results)
		{
			return new AggregateValidationResult(errorMessage, results);
		}

		public static AggregateValidationResult CreateFor<T>(T obj, IEnumerable<ValidationResult> results = null)
		{
			return CreateFor(typeof(T), results);
		}

		public static AggregateValidationResult CreateFor(Type type, IEnumerable<ValidationResult> results = null)
		{
			return new AggregateValidationResult(
				string.Format(TypedErrorMessage, type.Name),
				results
			);
		}

		public static AggregateValidationResult CreateFor(ValidationContext context, IEnumerable<ValidationResult> results = null)
		{
			return new AggregateValidationResult(
				string.Format(NestedErrorMessage, context.DisplayName ?? context.MemberName),
				results: results				
			);
		}
		#endregion

		public IEnumerable<ValidationResult> Flatten()
		{
			return Flatten(Results);
		}

		public static IEnumerable<ValidationResult> Flatten(IEnumerable<ValidationResult> results)
		{
			var aggregates = results.OfType<AggregateValidationResult>().ToArray();
			var flattens = aggregates.SelectMany(x => x.Flatten());

			return results.Except(aggregates).Union(flattens).ToArray();
		}
	}
}
