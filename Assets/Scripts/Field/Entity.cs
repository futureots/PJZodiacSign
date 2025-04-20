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

        public bool MoveSequence(Tile tile)
        {
            var isMovable = GetMoveArea().Contains(tile) && !tile.isEmpty;
            if (isMovable)
            {
                MoveTo(tile);
            }
            return true;
        }
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

        public List<Tile> GetMoveArea()
        {
            var list = GetComponents<IMoveArea>();
            var tiles = new List<Tile>();
            foreach (var area in list)
            {
                tiles.AddRange(area.GetMoveArea(curTile));
            }
            return tiles;
        }
        public List<Tile> GetAttackArea(Tile tile)
        {
            var list = GetComponents<IAttackArea>();
            var tiles = new List<Tile>();
            foreach (var area in list)
            {
                tiles.AddRange(area.GetAttackArea(tile));
            }
            return tiles;
        }
        public List<Tile> GetAttackArea()
        {
            return GetAttackArea(curTile);
        }


        #region Input
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
        #endregion
    }
}