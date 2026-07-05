using Augment;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AugmentTable", menuName = "Scriptable Objects/AugmentTable")]
public class AugmentTable : ScriptableObject
{
    public List<AugmentSO> augmentList;
}
