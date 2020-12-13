using System;

namespace HermaFx.SimpleConfig
{
    internal class SectionIdentity
    {
        private readonly string _name;
        private readonly Type _type;
        private readonly ConfigurationSectionForInterface _section;

        public SectionIdentity(string name, Type type, ConfigurationSectionForInterface section)
        {
            _name = name;
            _type = type;
            _section = section;
        }

        public ConfigurationSectionForInterface Section
        {
            get { return _section; }
        }

        public Type Type
        {
            get { return _type; }
        }

        public bool Equals(SectionIdentity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._name, _name) && Equals(other.Type, Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SectionIdentity)) return false;
            return Equals((SectionIdentity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_name.GetHashCode() * 397) ^ Type.GetHashCode();
            }
        }
    }
}
