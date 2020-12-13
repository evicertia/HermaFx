namespace HermaFx.SimpleConfig
{
    public class ConfigurationSection<T> : ConfigurationSectionForInterface
    {
        protected ConfigurationSection()
            : base(typeof (T))
        {
        }
    }
}
