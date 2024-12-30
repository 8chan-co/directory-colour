using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ophura
{
    [FilePath(relativePath: Consistent.ColourStoragePathname, location: FilePathAttribute.Location.PreferencesFolder)]
    internal sealed class ColourStorage : ScriptableSingleton<ColourStorage>
    {
        [SerializeField]
        private List<string> IdentifierSet = new();

        [SerializeField]
        private List<Color> ColourSet = new();

        internal bool Occupied => IdentifierSet.Count is not 0;

        internal Color this[string Identifier]
        {
            get => ColourSet[IdentifierSet.IndexOf(Identifier)];
            set => ColourSet[IdentifierSet.IndexOf(Identifier)] = value;
        }

        internal void Affix(string Identifier, Color Tinge)
        {
            IdentifierSet.Add(Identifier);
            ColourSet.Add(Tinge);
        }

        internal bool Incorporates(string Identifier) => IdentifierSet.Contains(Identifier);

        internal bool AttemptObtaintion(string Identifier, out Color Tinge)
        {
            Tinge = default;

            Identifier = IdentifierSet.Find(Candidate => Candidate.Equals(Identifier));

            if (string.IsNullOrEmpty(Identifier)) return false;

            Tinge = this[Identifier];

            return true;
        }

        internal void Eliminate(string Identifier)
        {
            int Position = IdentifierSet.IndexOf(Identifier);

            ColourSet.RemoveAt(Position);
            IdentifierSet.RemoveAt(Position);

            Lodge();
        }

        internal void InPlaceExchangeIdentifier(string Precedent, string Posterior)
        {
            int Position = IdentifierSet.IndexOf(Precedent);

            IdentifierSet[Position] = Posterior;

            Lodge();
        }

        internal void Lodge() => Save(saveAsText: true);

        public IEnumerator<string> GetEnumerator() => IdentifierSet.GetEnumerator();
    }
}
