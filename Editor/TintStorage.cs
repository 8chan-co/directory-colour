using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ophélia
{
    [FilePath("DirectoryTint/TintStorage.llama", FilePathAttribute.Location.PreferencesFolder)]
    internal sealed class TintStorage : ScriptableSingleton<TintStorage>
    {
        [SerializeField]
        internal List<string> IdentifierSet = new();

        [SerializeField]
        internal List<Color32> TintSet = new();

        internal Color32 this[string Identifier]
        {
            get => TintSet[IdentifierSet.IndexOf(Identifier)];

            set => TintSet[IdentifierSet.IndexOf(Identifier)] = value;
        }

        internal void Add(string Identifier, Color32 Tinge)
        {
            IdentifierSet.Add(Identifier);

            TintSet.Add(Tinge);
        }

        internal bool ContainsKey(string Identifier) => IdentifierSet.Contains(Identifier);

        internal bool TryGetValue(string Identifier, out Color32 Tinge)
        {
            Tinge = default;

            Identifier = IdentifierSet.Find(Candidate => string.CompareOrdinal(Candidate, Identifier) is 0);

            if (string.IsNullOrEmpty(Identifier)) return false;

            Tinge = this[Identifier];

            return true;
        }

        internal void Remove(string Identifier)
        {
            int Position = IdentifierSet.IndexOf(Identifier);

            TintSet.RemoveAt(Position);

            IdentifierSet.RemoveAt(Position);
        }

        internal void Save() => Save(saveAsText: true);
    }
}
