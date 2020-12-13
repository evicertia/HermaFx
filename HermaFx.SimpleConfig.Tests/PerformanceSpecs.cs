using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Machine.Specifications;

namespace HermaFx.SimpleConfig.Tests
{
    public class when_executing_performance_comparison
    {
        private static int _interationCount = 100000;

        private Because b =
            () =>
                {
                    _classic = MeasureTime(ClassicSection);
                    _simple = MeasureTime(SimpleSection);
                };

        private It should_not_be_much_slower_than_classic_config_section =
            () =>
                {
                    Console.WriteLine("Classic: {0}, Simple: {1}", _classic, _simple);
                    var timesSlower = _simple.Ticks/_classic.Ticks;
                    // XXX: Something makes this much slower on mono, probable GC 
                    //      collecting this so many gen1 objects.
                    //      And travis builder's seems to be even sloers. :?
                    timesSlower.ShouldBeLessThanOrEqualTo(RunningOnMono ? 30 : 2);
                };

        private static void SimpleSection()
        {
            var section = Configuration.Get<IPerformanceSection>();
            EvaluateProperties(section);
        }

        private static void ClassicSection()
        {
            var section = (ClassicSection)ConfigurationManager.GetSection("ClassicSection");
            EvaluateProperties(section);
        }

        private static void EvaluateProperties(IPerformanceSection section)
        {
            var kot = section.StringValue;
            var length = kot.Length;
            var childSection = section.Child;
            var childValue = childSection.ChildDouble;
            foreach (var el in section.Elements)
            {
                childValue = el.ChildDouble;
            }
        }

        private static TimeSpan MeasureTime(Action job)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < _interationCount; i++)
            {
                job();
            }
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private static bool RunningOnMono
        {
            get
            {
                Type t = Type.GetType("Mono.Runtime");
                if (t != null)
                    return true;

                return false;
            }
        }

        private static TimeSpan _classic;
        private static TimeSpan _simple;
    }

    public class ClassicSection : ConfigurationSection, IPerformanceSection
    {
        [ConfigurationProperty("StringValue")]
        public string StringValue
        {
            get { return (string) base["StringValue"]; }
        }

        public IChildPerformanceSection Child
        {
            get { return ChildElement; }
        }

        public IEnumerable<IChildPerformanceSection> Elements
        {
            get {
                return ElementsCollection.Cast<IChildPerformanceSection>();
            }
        }

        [ConfigurationProperty("Child")]
        public ChildPerformanceElement ChildElement
        {
            get { return (ChildPerformanceElement)base["Child"]; }
        }

        [ConfigurationProperty("Elements")]
        public ElementsCollection ElementsCollection
        {
            get { return (ElementsCollection) base["Elements"]; }
        }
    }
    public class ElementsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ChildPerformanceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Guid.NewGuid();
        }
    }


    public class ChildPerformanceElement : ConfigurationElement, IChildPerformanceSection
    {
        [ConfigurationProperty("ChildDouble")]
        public double ChildDouble
        {
            get { return (double) base["ChildDouble"]; }
        }
    }

    public interface IPerformanceSection
    {
        string StringValue { get; }
        IChildPerformanceSection Child {get;}
        IEnumerable<IChildPerformanceSection> Elements { get; }
    }

    public interface IChildPerformanceSection
    {
        double ChildDouble { get; }
    }

    public class PerformanceSection : ConfigurationSection<IPerformanceSection>
    {
    }

}
