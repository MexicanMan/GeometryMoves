using System;
using System.Collections.Generic;
using Test.FileReaders;
using Test.Inputs;
using Test.Settings;
using UnityEngine;
using Grid = Test.GridLogic.Grid;

namespace Test
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField]
        private CommonSettings _commonSettings;

        [SerializeField]
        private InputProvider _inputProvider;

        [SerializeField]
        private GridProvider _gridProvider;

        private List<IDisposable> _disposables = new();

        protected void Awake()
        {
            var gridFilePathService = new StreamingAssetsGridFilePathService(_commonSettings);

            var fileReader = new LineSlidingWindowReader(gridFilePathService);
            _disposables.Add(fileReader);

            var grid = new Grid(_inputProvider, fileReader, _commonSettings);
            _disposables.Add(grid);

            _gridProvider.Initialize(grid, _commonSettings);
        }

        protected void OnDestroy()
        {
            foreach (var disposable in _disposables)
                disposable?.Dispose();
        }
    }
}