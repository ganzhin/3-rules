using UnityEngine;

namespace Assets.Gameplay
{
    public class Controls : MonoBehaviour
    {
        public float XAxis => Input.GetAxis("Horizontal");
        public float YAxis => Input.GetAxis("Vertical");
        public bool LMB => Input.GetMouseButton(0);
        public bool RMB => Input.GetMouseButton(1);
        public bool Shift => Input.GetKeyDown(KeyCode.LeftShift);
        public bool Q => Input.GetKeyDown(KeyCode.Q);
        public bool E => Input.GetKeyDown(KeyCode.E);
        public bool F => Input.GetKeyDown(KeyCode.F);
        public bool R => Input.GetKeyDown(KeyCode.R);
        
        public Vector2 Direction => new Vector2(XAxis, YAxis);

    }
}