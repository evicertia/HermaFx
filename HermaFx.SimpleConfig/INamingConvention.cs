using System;
using System.Reflection;

namespace HermaFx.SimpleConfig
{
    public interface INamingConvention
    {        
        string SectionNameByIntefaceType(Type interfaceType);
        string SectionNameByIntefaceTypeAndPropertyName(Type propertyType, string propertyName);
        string AddToCollectionElementName(Type collectionElementType, string propertyName);
        string RemoveFromCollectionElementName(Type collectionElementType, string propertyName);
        string ClearCollectionElementName(Type collectionElementType, string propertyName);
        string SectionNameByIntefaceOrClassType(Type classOrInterface);
        string SectionNameByClassType(Type classOrInterface);
        string AttributeName(PropertyInfo propertyInfo);
    }
}
