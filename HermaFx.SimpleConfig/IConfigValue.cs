using System.Reflection;

namespace HermaFx.SimpleConfig
{
    internal interface IConfigValue
    {
        object Value(PropertyInfo property);
    }
}
