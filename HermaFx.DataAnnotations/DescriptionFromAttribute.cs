using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using HermaFx;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class DescriptionFromAttribute : DescriptionAttribute
{
	private const BindingFlags Everything = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

	public Type MetadataType { get; }
	public string TargetPath { get; }

	//public Type MetadataType { get; private set; }
	//public string Property { get; private set; }

	public DescriptionFromAttribute(Type container, params string[] path)
		: base(GetDescription(container, path))
	{
		MetadataType = container.ThrowIfNull(nameof(container));
		TargetPath = container.Name + "." + string.Join(".", path);
	}

	public static string GetDescription(Type type, params string[] path)
	{
		type.ThrowIfNull(nameof(type));
		path.ThrowIfNull(nameof(path));
		path.ThrowIfEmpty(nameof(path), "DescriptionFromAttribute path needs at least 1 property name.");

		var originalType = type;
		var pathString = type.Name;

		object instance = null;

		foreach (var name in path)
		{
			pathString += "." + name;

			var member = type.GetMember(name, Everything)
				.SingleOrDefault();

			if (member is PropertyInfo property)
				(instance, type) = (property.GetValue(instance), property.PropertyType);
			else if (member is FieldInfo field)
				(instance, type) = (field.GetValue(instance), field.FieldType);
			else
				throw new ArgumentException($"Type '{type.Name}' does not have a property or field named '{name}' in path '{pathString}'.");
		}

		if (instance is string description)
			return description;
		throw new ArgumentException($"Type '{originalType.Name}' does not have a string property or field named '{path.Last()}' in path '{pathString}'.");
	}
}
