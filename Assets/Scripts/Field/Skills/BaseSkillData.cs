using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class BaseSkillData : ScriptableObject
{
    public Sprite skillIcon;
    [SerializeField] private LocalizedString skillCombinedData; 

    public string SkillName { get; private set; } = "";
    public string SkillDescription { get; private set; } = "";

    // ScriptableObject가 로드될 때 이벤트 구독
    private void OnEnable()
    {
        // 언어가 변경되거나 텍스트가 처음 로드될 때 자동으로 OnStringChanged 함수를 실행하도록 연결(당장 비어있어도 게임 실행 시 세팅 됨)
        skillCombinedData.StringChanged += OnStringChanged;
    }

    // ScriptableObject가 언로드될 때 이벤트 해제 (메모리 누수 방지)
    private void OnDisable()
    {
        skillCombinedData.StringChanged -= OnStringChanged;
    }

    // 언어가 바뀌거나 최초 로드될 때 딱 한 번만 실행되는 함수
    private void OnStringChanged(string newText)
    {
        if (string.IsNullOrEmpty(newText)) return;

        // '|' 문자를 기준으로 앞(이름)과 뒤(설명)를 분리해서 캐시에 저장
        string[] splitText = newText.Split('|');
        if (splitText.Length >= 2)
        {
            SkillName = splitText[0].Trim();
            SkillDescription = splitText[1].Trim();
        }
        else
        {
            SkillName = newText;
            SkillDescription = string.Empty;
        }
    }

    [SerializeReference, SubclassSelector]
    public BaseSkillLogic skillLogic;
}