using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermaFx.DataAnnotations
{
	// NOTE: 
	// Originally based on: http://www.technofattie.com/2011/10/05/recursive-validation-using-dataannotations.html
	// However, this is still a WIP an quite a few enhancements are still missing.
	// You may take a look at this following docs in order to improve this code:
	//  - https://github.com/reustmd/DataAnnotationsValidatorRecursive/tree/master/DataAnnotationsValidator/DataAnnotationsValidator
	//  - https://prateektiwari.wordpress.com/tag/data-annotations/
	//  - http://www.tsjensen.com/blog/post/2011/12/23/Custom+Recursive+Model+Validation+In+NET+Using+Data+Annotations.aspx
	//
	public class ValidateObjectAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null)
			{
				return ValidationResult.Success;
			}

			var results = new List<ValidationResult>();

			if (value is IEnumerable)
			{
				var valueAsEnumerable = value as IEnumerable;
				foreach (var item in valueAsEnumerable)
				{
					var context = new ValidationContext(item, null, null);
					Validator.TryValidateObject(item, context, results, true);
				}
			}
			else
			{
				var context = new ValidationContext(value, null, null);
				Validator.TryValidateObject(value, context, results, true);
			}

			// FIXME: Inspect 'results' collection as to swap any 'ValidationResult' entry with an
			//		  'ExtendedValidationResult' one which references failed object's instance,
			//		  failing attribute, etc. (pruiz)

			if (results.Count != 0)
			{
				//_Log.DebugFormat("Validation failed for: {0} | {1} | {2}", validationContext.ObjectType, validationContext.DisplayName, validationContext.MemberName);
				//results.ForEach((i,x) => _Log.DebugFormat(" {0}: {1}", x.MemberNames.IfNotNull(m => string.Join(",", m.ToArray())), x.ErrorMessage));
				var compositeResults = new CompositeValidationResult(String.Format("Validation failed for: {0}", validationContext.DisplayName), new[] { validationContext.MemberName });
				results.ForEach(compositeResults.AddResult);

				return compositeResults;
			}

			return ValidationResult.Success;
		}
	}

	/// <summary>
	/// Class to hold multiple validation errors.
	/// </summary>
	public class CompositeValidationResult : ValidationResult
	{
		private readonly List<ValidationResult> _results = new List<ValidationResult>();

		public IEnumerable<ValidationResult> Results
		{
			get
			{
				return _results;
			}
		}

		public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
		public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
		protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

		public void AddResult(ValidationResult validationResult)
		{
			_results.Add(validationResult);
		}
	}
}
