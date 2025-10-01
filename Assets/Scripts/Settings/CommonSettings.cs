using UnityEngine;

namespace Test.Settings
{
    [CreateAssetMenu(fileName = nameof(CommonSettings), menuName = nameof(CommonSettings))]
    public class CommonSettings : ScriptableObject, ICommonSettings
    {
        [field: SerializeField]
        public string GridFilename { get; private set; }

        [field: SerializeField]
        public int GridSize { get; private set; }
    }
}