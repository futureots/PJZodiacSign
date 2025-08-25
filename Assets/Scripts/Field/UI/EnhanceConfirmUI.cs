using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EnhanceConfirmUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogPanel;
    public TextMeshProUGUI messageText;
    public Button confirmButton;
    public Button cancelButton;
    
    private Action onConfirm;
    private Action onCancel;
    
    private void Start()
    {
        // 버튼 이벤트 설정
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
        
        // 초기에는 숨김
        HideDialog();
    }
    
    /// <summary>
    /// 확인 다이얼로그를 표시합니다.
    /// </summary>
    /// <param name="message">표시할 메시지</param>
    /// <param name="confirmCallback">확인 시 실행할 콜백</param>
    /// <param name="cancelCallback">취소 시 실행할 콜백</param>
    public void ShowDialog(string message, Action confirmCallback = null, Action cancelCallback = null)
    {
        messageText.text = message;
        onConfirm = confirmCallback;
        onCancel = cancelCallback;
        
        dialogPanel.SetActive(true);
    }
    
    /// <summary>
    /// 확인 다이얼로그를 숨깁니다.
    /// </summary>
    public void HideDialog()
    {
        dialogPanel.SetActive(false);
        onConfirm = null;
        onCancel = null;
    }
    
    private void OnConfirm()
    {
        onConfirm?.Invoke();
        HideDialog();
    }
    
    private void OnCancel()
    {
        onCancel?.Invoke();
        HideDialog();
    }
}
