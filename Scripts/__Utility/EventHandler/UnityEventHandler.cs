using System;

namespace FatshihLib.Event
{
    public delegate void UnityEventHandler<TEventArgs>(UnityEngine.Object sender, TEventArgs e) where TEventArgs : EventArgs;
}