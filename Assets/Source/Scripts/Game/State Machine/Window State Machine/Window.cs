using System;
using UnityEngine;

namespace BikeDefied.FSM.GameWindow
{
    public abstract class Window : MonoBehaviour
    {
        public abstract Type WindowType { get; }
    }
}