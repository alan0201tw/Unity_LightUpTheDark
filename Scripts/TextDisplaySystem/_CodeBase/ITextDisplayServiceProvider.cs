using System;

namespace GameServices.Interface
{
    public interface ITextDisplayServiceProvider
    {
        void DisplayText(string content, float durationTime, Action onDisplayEnded = null);
        void DisplayTextImmediate(string content, float durationTime, Action onDisplayEnded = null);
    }
}