using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculateUI : MonoBehaviour
{
    [SerializeField] private ValueUI valuePrefab;
    [SerializeField] private Transform teamValueTransform;
    [SerializeField] private Transform opponentValueTransform;
    
    [SerializeField] private TextMeshProUGUI teamValueText;
    [SerializeField] private TextMeshProUGUI opponentValueText;
    [SerializeField] private Button endButton;
    [SerializeField] private TextMeshProUGUI resultText;
    public Color red;
    public Color green;
    

    public IEnumerator Calculate(InputManager inputManager)
    {
        Dictionary<EntityData, int> myValues = new();
        Dictionary<EntityData, int> opponentValues = new();

        var field = StageManager.Instance.field;
        // 필드에 있는 모든 기물을 가져와서 값 계산
        foreach (var entity in field.GetEntities())
        {
            if (entity.team.teamNumber == inputManager.agent.id)
            {
                var value = 0; 
                if (myValues.TryGetValue(entity.baseData, out var v))
                {
                    value += v;
                }
                myValues[entity.baseData] = value + entity.Level + 1;
            }
            else
            {
                var value = 0; 
                if (opponentValues.TryGetValue(entity.baseData, out var v))
                {
                    value += v;
                }
                opponentValues[entity.baseData] = value + entity.Level + 1;
            }
        }

        StartCoroutine(SetValue(teamValueText,teamValueTransform,myValues));
        StartCoroutine(SetValue(opponentValueText,opponentValueTransform,opponentValues));

        yield return null;
        // 가운데 버튼을 활성화 하면서 승리인지 패배인지 표시, 해당 버튼 누르면 스테이지 종료 UI 표시
        
        if (myValues.Sum(p => p.Value * p.Key.normalPrice) > opponentValues.Sum(p => p.Value * p.Key.normalPrice))
        {
            resultText.text = $"승리";
            endButton.colors = new ColorBlock()
            {
                normalColor =  green,
                highlightedColor = green,
                selectedColor =  green,
                pressedColor =  Color.black,
                fadeDuration = 0.1f,
                colorMultiplier = 1
            };
            endButton.onClick.AddListener(()=> inputManager.agent.fieldController.EndStage(inputManager.agent.id));
        }
        else
        {
            resultText.text = $"패배";
            endButton.colors = new ColorBlock()
            {
                normalColor =  red,
                highlightedColor = red,
                selectedColor =  red,
                pressedColor =  Color.black,
                fadeDuration = 0.1f,
                colorMultiplier = 1
            };
            endButton.onClick.AddListener(()=> inputManager.agent.fieldController.EndStage(PlayerID.None));
        }
    }

    IEnumerator SetValue(TextMeshProUGUI text,Transform parent,Dictionary<EntityData,int> values)
    {
        var sum = 0;
        foreach (var entity in values)
        {
            var v = Instantiate(valuePrefab, parent);
            v.Init(entity.Key.icon,entity.Value);
            sum += entity.Value * entity.Key.normalPrice;
            text.text = $"{sum}";
            yield return new WaitForSeconds(0.2f);
        }
    }
}
