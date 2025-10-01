using System;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Settings
{
    [CreateAssetMenu(fileName = nameof(ColorSettings), menuName = nameof(ColorSettings))]
    public class ColorSettings : ScriptableObject
    {
        [field: SerializeField]
        public List<ColorEntry> Colors { get; private set; } = new();
    }

    [Serializable]
    public class ColorEntry
    {
        [field: SerializeField]
        public int Char { get; private set; }

        [field: SerializeField]
        public Color Color { get; private set; }
    }
}