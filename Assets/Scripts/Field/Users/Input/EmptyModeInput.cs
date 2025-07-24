using UnityEngine;

namespace PlayerInput
{
    public class EmptyModeInput : IModeInput
    {
        public void RemoveMode()
        {

        }

        public void SetMode()
        {
            Debug.Log("SetEmptyMode");
        }
    }
}
