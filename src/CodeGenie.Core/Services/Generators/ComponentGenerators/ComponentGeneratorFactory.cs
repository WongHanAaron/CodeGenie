using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Generators.ComponentGenerators
{
    /// <summary> Factory method for creating component generator </summary>
    public interface IComponentGeneratorFactory
    {
        /// <summary> Create a component based on the component generator type </summary>
        IComponentGenerator Create<TComponentGenerator>() where TComponentGenerator : IComponentGenerator;
        
        /// <summary> Create a component based on the language </summary>
        IComponentGenerator Create(string language);

        /// <summary> Returns the list of supported languages </summary>
        IEnumerable<string> SupportedLanguages { get; }
    }

    public class ComponentGeneratorFactory : IComponentGeneratorFactory
    {
        private const string _expectedGeneratorSuffix = "ComponentGenerator";
        protected readonly ConcurrentDictionary<string, IComponentGenerator> _generators;
        public ComponentGeneratorFactory(IEnumerable<IComponentGenerator> componentGenerators)
        {
            _generators = new ConcurrentDictionary<string, IComponentGenerator>(componentGenerators.ToDictionary(c => ConvertToLanguageName(c), StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<string> SupportedLanguages => _generators.Keys;

        public string ConvertToLanguageName(IComponentGenerator generator)
            => generator.GetType().Name.Replace(_expectedGeneratorSuffix, "");

        public IComponentGenerator Create<TComponentGenerator>() where TComponentGenerator : IComponentGenerator
            => Create(typeof(TComponentGenerator));

        public IComponentGenerator Create(string language)
        {
            if (!_generators.ContainsKey(language)) throw new ArgumentException($"There does not exist a generator for the language '{language}'");
            return _generators[language];
        }

        protected IComponentGenerator Create(Type generatorType)
        {
            var matching = _generators.Values.FirstOrDefault(g => generatorType.IsAssignableFrom(g.GetType()));
            if (matching == null) throw new ArgumentException($"There does not exist a generator for type '{generatorType}'");
            return matching;
        }
    }
}
