using System;
using System.Collections.Generic;
using UnityEngine;

namespace Augment
{
    [CreateAssetMenu(fileName = "DebugAugment", menuName = "Augment/Get Entity")]
    public class GetEntityAugment: AugmentSO
    {
        public List<EntityData> newEntity;
        public readonly List<Entity> addedEntity = new();
        
        public override void OnActive()
        {
            try
            {
                foreach(var newEt in newEntity) 
                {
                    addedEntity.Add(Agent.LocalPlayer.GetEntity(newEt));
                }
            }
            catch (Exception e)
            {
                EditorLogger.PrintError(e);
            }
        }
    }
}