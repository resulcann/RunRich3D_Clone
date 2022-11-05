using UnityEngine;

namespace Game
{
    public class SmoothCameraFollow : MonoBehaviour
    {
        public Transform target;
        [SerializeField] private float smoothTime = 0.25f;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 velocity = Vector3.zero;

        private void Update()
        {
            var camPos = transform.position;
            var desiredPos = target.position + offset;
            transform.position = Vector3.SmoothDamp(camPos, desiredPos, ref velocity, smoothTime);
            transform.rotation = target.rotation;
        }
    }
}
