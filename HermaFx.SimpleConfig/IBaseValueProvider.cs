using System.Reflection;

namespace HermaFx.SimpleConfig
{
    internal interface IBaseValueProvider
    {
        object this[PropertyInfo property] { get; }
    }
}
