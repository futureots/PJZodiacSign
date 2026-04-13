using UnityEngine;
using UnityEngine.UI;

namespace DebugTool
{
    public class DebugEvent : MonoBehaviour
    {
        public Button button;
        public GameObject targetObject;

        public ScriptableObject targetData;

        public void TriggerEvent()
        {
            #region DebugArea

            //EntityFactory.Request(targetData as EntityData, 3);

            #endregion
        }
    }
}

