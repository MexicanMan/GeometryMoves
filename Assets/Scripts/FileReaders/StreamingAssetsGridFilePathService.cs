using System.IO;
using Test.Settings;
using UnityEngine;
using UnityEngine.Networking;

namespace Test.FileReaders
{
    public class StreamingAssetsGridFilePathService : IGridFilePathService
    {
        private const string PersistentFilename = "grid_copy.txt";

        private readonly string _gridFilepath;

        public StreamingAssetsGridFilePathService(ICommonSettings commonSettings)
        {
            _gridFilepath = Path.Combine(Application.streamingAssetsPath, commonSettings.GridFilename);

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_WEBGL)
            if (TryCopyGridFile(out var newGridPath))
                _gridFilepath = newGridPath;
#endif
        }
        public string GetGridFilePath()
        {
            return _gridFilepath;
        }

        private bool TryCopyGridFile(out string copiedFilepath)
        {
            if (_gridFilepath.StartsWith("jar") || _gridFilepath.StartsWith("http"))
            {
                var persistentFilepath = Path.Combine(Application.persistentDataPath, PersistentFilename);

                var request = UnityWebRequest.Get(_gridFilepath);
                request.SendWebRequest().GetAwaiter().OnCompleted(() =>
                    File.WriteAllBytes(persistentFilepath, request.downloadHandler.data));

                copiedFilepath = persistentFilepath;
                return true;
            }

            copiedFilepath = null;
            return false;
        }
    }
}