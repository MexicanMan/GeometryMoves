using System;
using UnityEngine;

namespace Test.Inputs
{
    public class InputProvider : MonoBehaviour, IInputProvider
    {
        public event Action<Vector2Int> OnInput;

        protected void Update()
        {
            var inputDelta = Vector2Int.zero;

            if (Input.GetKeyDown(KeyCode.W))
                inputDelta.y -= 1;
            if (Input.GetKeyDown(KeyCode.A))
                inputDelta.x -= 1;
            if (Input.GetKeyDown(KeyCode.S))
                inputDelta.y += 1;
            if (Input.GetKeyDown(KeyCode.D))
                inputDelta.x += 1;

            if (inputDelta != Vector2Int.zero)
                OnInput?.Invoke(inputDelta);
        }
    }
}