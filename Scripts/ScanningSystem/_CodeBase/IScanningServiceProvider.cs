using GameServices.ScanningService;
using UnityEngine;

namespace GameServices.Interface
{
    public interface IScanningServiceProvider
    {
        Color ScannerColor { get; set; }

        void RegisterScannable(Scannable scannable);
    }
}