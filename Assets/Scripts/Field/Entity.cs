using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Battle
{
    public class Entity : MonoBehaviour, IDamageable
    {
        public Tile curTile;

        #region status
        public int level;
        public int maxHp;
        public int curHp;
        public int energy;
        public int power;



        public void Damaged(int damage)
        {
            curHp -= damage;
            Debug.Log($"Damaged : {damage} , CurrentHp : {curHp}");
        }

        public void Dead()
        {
            Destroy(gameObject);
        }

        public void Healed(int amount)
        {
            curHp += amount;
        }

        public bool isZero()
        {
            if (curHp > 0) return false;
            return true;
        }
        #endregion
        public void MoveTo(Tile tile)
        {
            if (curTile != null)
            {
                curTile.OccupyObject();
            }
            tile.OccupyObject(gameObject);
            curTile = tile;
            transform.position = tile.transform.position;
        }

        private void OnMouseDown()
        {
            Debug.Log("Mouse DOWN");
            InputManager.Instance.OnGameObjectDown(gameObject);
        }
        private void OnMouseUp()
        {
            Debug.Log("Mouse UP");
            InputManager.Instance.OnGameObjectUp();
        }
    }
}