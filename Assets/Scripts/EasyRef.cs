using Cinemachine;
using UnityEngine;

namespace Voxhull
{
    public class EasyRef : Chernobog.Studio.Common.Singleton<EasyRef>
    {
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private CinemachineCameraOffset cameraOffset;
        public CinemachineVirtualCamera PlayerCamera => playerCamera;
        public CinemachineCameraOffset CameraOffset => cameraOffset;
    }
}