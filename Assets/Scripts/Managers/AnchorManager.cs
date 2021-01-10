using UnityEngine.XR.ARFoundation;
using Baks.Core.Utils;

namespace Baks.Core.Managers
{
    public class AnchorManager : Singleton<AnchorManager>
    {
        public ARAnchorManager ARAnchorManager { get => _arAnchorManager; set => _arAnchorManager = value; }

        private ARAnchorManager _arAnchorManager;

        private void Awake() => _arAnchorManager = GetComponent<ARAnchorManager>();
    }
}