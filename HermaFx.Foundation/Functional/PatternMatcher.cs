using System;
using System.Collections.Generic;

namespace HermaFx.DesignPatterns
{
	public class PatternMatcher<Output>
	{
		List<Tuple<Predicate<object>, Func<object, Output>>> cases = new List<Tuple<Predicate<object>, Func<object, Output>>>();

		public PatternMatcher() { }

		public PatternMatcher<Output> Case(Predicate<object> condition, Func<object, Output> function)
		{
			cases.Add(new Tuple<Predicate<object>, Func<object, Output>>(condition, function));
			return this;
		}

		public PatternMatcher<Output> Case<T>(Predicate<T> condition, Func<T, Output> function)
		{
			return Case(
				o => o is T && condition((T)o),
				o => function((T)o));
		}

		public PatternMatcher<Output> Case<T>(Func<T, Output> function)
		{
			return Case(
				o => o is T,
				o => function((T)o));
		}

		public PatternMatcher<Output> Case<T>(Predicate<T> condition, Output o)
		{
			return Case(condition, x => o);
		}

		public PatternMatcher<Output> Case<T>(Output o)
		{
			return Case<T>(x => o);
		}

		public PatternMatcher<Output> Default(Func<object, Output> function)
		{
			return Case(o => true, function);
		}

		public PatternMatcher<Output> Default(Output o)
		{
			return Default(x => o);
		}

		public Output Match(object o)
		{
			foreach (var tuple in cases)
				if (tuple.Item1(o))
					return tuple.Item2(o);
			throw new Exception("Failed to match");
		}
	}

	public class PatternMatcher<Output, TContext>
		where TContext : class
	{
		List<Tuple<Predicate<object>, Func<object, TContext, Output>>> cases = new List<Tuple<Predicate<object>, Func<object, TContext, Output>>>();

		public PatternMatcher() { }

		public PatternMatcher<Output, TContext> Case(Predicate<object> condition, Func<object, TContext, Output> function)
		{
			cases.Add(new Tuple<Predicate<object>, Func<object, TContext, Output>>(condition, function));
			return this;
		}

		public PatternMatcher<Output, TContext> Case<T>(Predicate<T> condition, Func<T, TContext, Output> function)
		{
			return Case(
				o => o is T && condition((T)o),
				(o, ctx) => function((T)o, ctx));
		}

		public PatternMatcher<Output, TContext> Case<T>(Func<T, TContext, Output> function)
		{
			return Case(
				o => o is T,
				(o, ctx) => function((T)o, ctx));
		}

		public PatternMatcher<Output, TContext> Case<T>(Predicate<T> condition, Output o)
		{
			return Case(condition, (_, __) => o);
		}

		public PatternMatcher<Output, TContext> Case<T>(Output o)
		{
			return Case<T>((_, __) => o);
		}

		public PatternMatcher<Output, TContext> Default(Func<object, TContext, Output> function)
		{
			return Case(o => true, function);
		}

		public PatternMatcher<Output, TContext> Default(Output o)
		{
			return Default((_, __) => o);
		}

		public Output Match(object o, TContext context = null)
		{
			foreach (var tuple in cases)
				if (tuple.Item1(o))
					return tuple.Item2(o, context);
			throw new Exception("Failed to match");
		}
	}
}
