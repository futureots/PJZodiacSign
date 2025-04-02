using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Battle
{
    public class Entity : MonoBehaviour, IDamageable
    {
        public Tile curTile;
        public void Damaged(int damage)
        {
            throw new System.NotImplementedException();
        }

        public void Dead()
        {
            Destroy(gameObject);
        }

        public void Healed(int amount)
        {
            throw new System.NotImplementedException();
        }

        public bool isZero()
        {
            return false;
        }
        public void MoveTo(Tile tile)
        {
            if (curTile != null)
            {
                curTile.OccupyObject();
            }
            tile.OccupyObject(gameObject);
            curTile = tile;
        }
    }
}