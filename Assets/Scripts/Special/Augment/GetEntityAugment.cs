using System;
using UnityEngine;

namespace Augment
{
    [CreateAssetMenu(fileName = "DebugAugment", menuName = "Augment/Get Entity")]
    public class GetEntityAugment: AugmentSO
    {
        public EntityData newEntity;
        
        public override void OnActive()
        {
            try
            {
                Agent.LocalPlayer.GetEntity(newEntity);
            }
            catch (Exception e)
            {
                EditorLogger.PrintError(e);
            }
        }
    }
}