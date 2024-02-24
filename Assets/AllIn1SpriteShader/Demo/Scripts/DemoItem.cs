using UnityEngine;

namespace AllIn1SpriteShader
{
    public class DemoItem : MonoBehaviour
    {
        static readonly Vector3 lookAtZ = new Vector3(0, 0, 1);

        void Update()
        {
            transform.LookAt(transform.position + lookAtZ);
        }
    }
}