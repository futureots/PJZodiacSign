using GlobalManage;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class OptionEntry : MonoBehaviour
{
    private Button button;
          
    [Header("UI")]
    [SerializeField] private GameObject OptionPanel;
    [SerializeField] private Scrollbar masterBar;
    [SerializeField] private Scrollbar bgmBar;
    [SerializeField] private Scrollbar sfxBar;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button quitButton;

    public void OnButtonClick()
    {
        if (!GameOption.Instance)
        {
            return;
        }

        OptionPanel?.SetActive(true);
        RefreshUI();
        ApplyEvent();
    }

    // Dynamic Func
    private void ApplyEvent()
    {
        masterBar?.onValueChanged.AddListener(delegate { GameOption.Instance.SetMasterVolume(masterBar.value); });
        bgmBar?.onValueChanged.AddListener(delegate { GameOption.Instance.SetBGMVolume(bgmBar.value); });
        sfxBar?.onValueChanged.AddListener(delegate { GameOption.Instance.SetSFXVolume(sfxBar.value); });
        
        fullScreenToggle?.onValueChanged.AddListener(delegate { GameOption.Instance.SetFullScreen(fullScreenToggle.isOn); });
        resolutionDropdown?.onValueChanged.AddListener(delegate { GameOption.Instance.SetResolution(resolutionDropdown.value); });
        
        exitButton?.onClick.AddListener(delegate { GameManager.Instance.EndGame(); });
        quitButton?.onClick.AddListener(Application.Quit);
    }
    
    private void RefreshUI()
    {
        // Get Option
        var option = GameOption.Instance;
        if (!option) return;
        var setting = option.CurrentSettings;
        
        // Resolution
        foreach (var res in option.GetResolutionOptions())
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res));
        }
        resolutionDropdown.value = setting.resolutionIndex;
        fullScreenToggle.isOn = setting.isFullScreen;
        
        // Volume
        masterBar.value = setting.masterVolume;
        bgmBar.value = setting.bgmVolume;
        sfxBar.value = setting.sfxVolume;
    }

    public IEnumerator ChangeLanguageCoroutine(int index)
    {
        yield return LocalizationSettings.InitializationOperation;
        
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

    }
}
