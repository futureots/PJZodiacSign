using System.Collections.Generic;
using UnityEngine;

public class LogSystem : MonoBehaviour
{
    [Header("Settings")]
    public GameObject logPrefab;
    public Transform container;
    public int poolSize = 5; // 미리 만들어둘 개수

    private Queue<LogItem> logPool = new Queue<LogItem>();

    private void Awake() {
        InitializePool();
    }

    // 1. 초기 풀 생성
    private void InitializePool() {
        for (int i = 0; i < poolSize; i++) {
            CreateNewLogItem();
        }
    }

    private LogItem CreateNewLogItem() {
        GameObject obj = Instantiate(logPrefab, container);
        LogItem item = obj.GetComponent<LogItem>();
        obj.SetActive(false); // 일단 꺼둠
        logPool.Enqueue(item);
        return item;
    }

    // 2. 외부에서 호출하는 함수
    public void ShowLog(string message, LogType type) {
        LogItem item;

        // 풀에 사용할 수 있는 객체가 있는지 확인
        if (logPool.Count > 0 && !logPool.Peek().gameObject.activeSelf) {
            item = logPool.Dequeue();
        } else {
            // 모두 사용 중이면 새로 생성하거나, 가장 오래된 것을 재활용
            item = CreateNewLogItem();
            logPool.Dequeue(); // 새로 만든걸 쓰기 위해 하나 뺌 (Queue 유지)
        }

        item.gameObject.SetActive(true);
        item.transform.SetAsLastSibling(); // 가장 최근 로그가 아래로 가게 함
        
        // 타입별 데이터 설정 (예시: 빨간색/노란색 등)
        Color targetColor = (type == LogType.Error) ? Color.red : Color.white;
        item.Setup(message, targetColor);

        // 사용한 아이템은 나중에 다시 쓸 수 있게 Queue의 뒤로 보냄
        logPool.Enqueue(item);
    }

}
