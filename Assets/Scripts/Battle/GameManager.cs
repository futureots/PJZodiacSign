using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public EntityController[] controllers;
    public Field field;
    private void Awake()
    {
        field.CreateField();
        var data = PartyData.LoadPartyData("CurrentPlayerParty");
        for(int i = 0; i < controllers.Length; i++)
        {
            controllers[i].SetEntities(i+1, data);
        }
        GameStart();
    }
    public void GameStart()
    {
        InputManager.Instance.inputMode = InputManager.InputMode.Set;
    }
    public void TurnStart()
    {
        InputManager.Instance.inputMode = InputManager.InputMode.Move;
    }
    public IEnumerator TurnEndCo()
    {
        bool isFirstTurnEnd = false;
        if (InputManager.Instance.inputMode == InputManager.InputMode.Set) isFirstTurnEnd = true;  
        InputManager.Instance.inputMode = InputManager.InputMode.None;
        if (!isFirstTurnEnd)
        {
            //이동 행동
            Dictionary<Entity, Tile> operations = new Dictionary<Entity, Tile>();
            foreach (EntityController controller in controllers)
            {
                operations.AddRange(controller.OperActivate());
            }
            //해당 값으로 구성된 그룹을 생성
            var groupedMoves = operations.GroupBy(x => x.Value);
            foreach (var group in groupedMoves)
            {
                //그룹에 속해있는 엔티티 반환
                var pieces = group.Select(x => x.Key).ToList();
                //기물 순서로 정렬 후 첫번째 반환
                Entity highestPiece = pieces.OrderBy(p => p.jodiacType).First();
                //이기는 속성으로 정렬
                if (ElementInList(pieces) ==2)
                {
                    Element dominant = Element.Empty;
                    foreach (var piece in pieces)
                    {
                        if(dominant == Element.Empty || IsDominantType(piece.elementType, dominant))
                        {
                            dominant = piece.elementType;
                            highestPiece = piece;
                        }
                    }
                }
                highestPiece.MoveToTile(group.Key);
            }
        }
        yield return new WaitForSeconds(1f);
       
        TurnStart();
    }
    public void TurnEnd()
    {
        StartCoroutine(TurnEndCo());
        
    }
    public void SaveCurrentState(int team)
    {
        List<PartyEntity> list = new List<PartyEntity>();
        foreach (var i in field.tiles)
        {
            foreach (var j in i)
            {
                var entity = j.GetEntity();
                if (entity != null)
                {
                    if (entity.TeamNum != team) continue;
                    PartyEntity temp = new PartyEntity(entity.name);
                    list.Add(temp);
                }
            }
        }
        PartyData data = new PartyData();
        data.Entities = list.ToArray();
        data.SavePartyData("CurrentPlayerParty");
    }
    public static int ElementInList(List<Entity> list)
    {
        List<Element> element = new List<Element>();

        foreach(var entity in list)
        {
            if (!element.Contains(entity.elementType))
            {
                element.Add(entity.elementType);
            }
        }
        return element.Count;
    }
    public static bool IsDominantType(Element element, Element compare)
    {
        if (element == Element.Empty) return false;
        if (compare == Element.Empty) return true;
        /*
         * 목1
         * 화2
         * 토3
         * 금4
         * 수5
         * 승 1 또는 -2
         */
        var gap = (element - compare +5)%5;
        if (gap == 1 || gap == 3)
        {
            return true;
        }
        return false;
    }

}
