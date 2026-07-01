using System.Collections;
using UnityEngine;

public abstract class ExSystem : MonoBehaviour
{
    public abstract IEnumerator Init(FieldController controller, StageData stageData);
}
