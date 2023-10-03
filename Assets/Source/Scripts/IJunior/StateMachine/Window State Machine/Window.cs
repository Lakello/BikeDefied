using System;
using UnityEngine;

namespace IJunior.StateMachine
{
    public abstract class Window : MonoBehaviour
    {
        public abstract Type WindowType { get; }
    }
}