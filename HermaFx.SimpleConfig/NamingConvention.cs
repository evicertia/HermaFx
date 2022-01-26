using System;
using System.Reflection;

using HermaFx.SimpleConfig.BasicExtensions;

namespace HermaFx.SimpleConfig
{
    public class NamingConvention : INamingConvention
    {
        private static INamingConvention _current = new NamingConvention();

        public static INamingConvention Current
        {
            get {
                return _current;

            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Convention must not be null","value");
                }
                
                _current = new CheckForInvalidNamesDecorator(value);
            }
        }

        public string SectionNameByInterfaceType<T>()
        {
            var interfaceType = typeof (T);
            return SectionNameByInterfaceType(interfaceType);
        }

        public string SectionNameByInterfaceOrClassType(Type classOrInterface)
        {
            if (classOrInterface.IsInterface)
            {
                return SectionNameByInterfaceType(classOrInterface);
            }else
            {
                return SectionNameByClassType(classOrInterface);
            }
        }

        public string SectionNameByClassType(Type classOrInterface)
        {
            return classOrInterface.Name;
        }

        public virtual string SectionNameByInterfaceType(Type interfaceType)
        {
            if (interfaceType.Name[0] == 'I')
            {
                return interfaceType.Name.Substring(1);
            }
            return interfaceType.Name;
        }

        public virtual string SectionNameByInterfaceTypeAndPropertyName(Type propertyType, string propertyName)
        {
            return propertyName;
        }

        public virtual string AddToCollectionElementName(Type collectionElementType, string propertyName)
        {
            return "add";
        }

        public virtual string RemoveFromCollectionElementName(Type collectionElementType, string propertyName)
        {
            return "remove";
        }

        public virtual string ClearCollectionElementName(Type collectionElementType, string propertyName)
        {
            return "clear";
        }

        public virtual string AttributeName(PropertyInfo propertyInfo)
        {
            return propertyInfo.Name;
        }

        private class CheckForInvalidNamesDecorator : INamingConvention
        {
            private readonly INamingConvention _realConvention;
            private readonly INamingConvention _defaultConvention;

            public CheckForInvalidNamesDecorator(INamingConvention realConvention)
            {
                _realConvention = realConvention;
                _defaultConvention = new NamingConvention();
            }

            public string AddToCollectionElementName(Type collectionElementType, string propertyName)
            {
                return
                    IfEmptyStringThenDefault(
                        conv => conv.AddToCollectionElementName(collectionElementType, propertyName));
            }
            public string RemoveFromCollectionElementName(Type collectionElementType, string propertyName)
            {
                return
                    IfEmptyStringThenDefault(
                        conv => conv.RemoveFromCollectionElementName(collectionElementType, propertyName));
            }

            public string ClearCollectionElementName(Type collectionElementType, string propertyName)
            {
                return
                    IfEmptyStringThenDefault(
                        conv => conv.ClearCollectionElementName(collectionElementType, propertyName));
            }

            public string SectionNameByInterfaceOrClassType(Type classOrInterface)
            {
                return IfEmptyStringThenDefault(conv => conv.SectionNameByInterfaceOrClassType(classOrInterface));
            }

            public string SectionNameByClassType(Type classOrInterface)
            {
                return IfEmptyStringThenDefault(conv => conv.SectionNameByClassType(classOrInterface));
            }


            public string SectionNameByInterfaceType(Type interfaceType)
            {
                return IfEmptyStringThenDefault(conv => conv.SectionNameByInterfaceType(interfaceType));
            }

            public string SectionNameByInterfaceTypeAndPropertyName(Type propertyType, string propertyName)
            {
                return
                    IfEmptyStringThenDefault(
                        conv => conv.SectionNameByInterfaceTypeAndPropertyName(propertyType, propertyName));
            }

            public string AttributeName(PropertyInfo propertyInfo)
            {
                return IfEmptyStringThenDefault(conv => conv.AttributeName(propertyInfo));
            }

            private string IfEmptyStringThenDefault(Func<INamingConvention, string> convention)
            {
                var propsedName = convention(_realConvention);
                if(propsedName.IsNullOrEmptyOrWhiteSpace())
                {
                    propsedName = convention(_defaultConvention);
                }

                return propsedName;
            }
        }
    }
}
