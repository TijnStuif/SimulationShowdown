using UnityEngine;

namespace SceneCamera
{
    /// <summary>
    /// Make the camera view incredibly trippy
    /// (seizure warning)
    /// </summary>
    public class TripView : MonoBehaviour
    {
        private Camera cam;
        private void Awake()
        {
            cam = Camera.main;
        }

        private void LateUpdate()
        {
            Matrix4x4 mat = Camera.main.projectionMatrix;
            mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
            cam.projectionMatrix = mat;
        }
    }
}
