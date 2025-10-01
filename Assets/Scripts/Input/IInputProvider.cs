using System;
using UnityEngine;

namespace Test.Inputs
{
    public interface IInputProvider
    {
        event Action<Vector2Int> OnInput;
    }
}