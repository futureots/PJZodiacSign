using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace GlobalManage
{
    [Serializable]
    public class GameSetting
    {
        public int resolutionIndex;
        public bool isFullScreen;
        public float masterVolume = 1f;
        public float bgmVolume = 0.8f;
        public float sfxVolume = 0.8f;
    }
    
    public class GameOption : Singleton<GameOption>
    {
        [Header("Audio")]
        [SerializeField] private AudioMixer mainMixer;
        
        [Header("Settings Data")]
        private GameSetting _currentSettings;
        private string _savePath;
        private List<Resolution> _resolutions = new();

        protected override void Awake()
        {
            base.Awake();
            _savePath = Path.Combine(Application.persistentDataPath, "settings.json");
            InitResolutions();
            LoadSettings();
        }

        #region Resolution
        
        /// <summary>
        /// Init Resolution
        /// </summary>
        /// <remarks>Get available resolutions with monitor (only 16:9)</remarks>
        private void InitResolutions()
        {
            _resolutions.Clear();
            Resolution[] allRes = Screen.resolutions;
            
            foreach (var res in allRes)
            {
                // 16:9 비율 필터링 (근사치 체크)
                float aspect = (float)res.width / res.height;
                if (Mathf.Abs(aspect - 1.777f) < 0.1f)
                {
                    _resolutions.Add(res);
                }
            }
        }

        /// <summary>
        /// Get List of Resolution Options
        /// </summary>
        /// <returns>list of options : string</returns>
        public List<string> GetResolutionOptions()
        {
            List<string> options = new List<string>();
            foreach (var res in _resolutions)
            {
                options.Add($"{res.width} x {res.height} ({res.refreshRateRatio.value:F0}Hz)");
            }
            return options;
        }

        /// <summary> 
        /// Set resolution
        /// </summary>
        /// <param name="index">option index in resolution list</param>
        /// <param name="isFull">fullscreen</param>
        private void SetResolution(int index, bool isFull)
        {
            if (index < 0 || index >= _resolutions.Count) return;

            Resolution res = _resolutions[index];
            Screen.SetResolution(res.width, res.height, isFull);
            
            _currentSettings.resolutionIndex = index;
            _currentSettings.isFullScreen = isFull;
            SaveSettings();
        }
        
        ///Wrapper for dynamic Event
        public void SetFullScreen(bool isFullScreen) => SetResolution(_currentSettings.resolutionIndex, isFullScreen);
        public void SetResolution(int index) => SetResolution(index, _currentSettings.isFullScreen);

        #endregion

        #region Volume

        /// <summary>
        /// Setting for Volume
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        private void SetVolume(string parameterName, float value)
        {
            // value: 0.0001f ~ 1f -> dB: -80dB ~ 0dB 변환
            float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
            
            if (mainMixer != null)
                mainMixer.SetFloat(parameterName, dB);

            switch (parameterName)
            {
                case "master": _currentSettings.masterVolume = value; break;
                case "bgm": _currentSettings.bgmVolume = value; break;
                case "sfx": _currentSettings.sfxVolume = value; break;
            }
            SaveSettings();
        }
        
        /// Wrapper for dynamic Event
        public void SetMasterVolume(float value) => SetVolume("master", value);
        public void SetBGMVolume(float value) => SetVolume("BGM", value);
        public void SetSFXVolume(float value) => SetVolume("SFX", value);

        #endregion
        
        #region Persistence

        /// <summary>
        /// Save Setting to json
        /// </summary>
        private void SaveSettings()
        {
            string json = JsonUtility.ToJson(_currentSettings);
            File.WriteAllText(_savePath, json);
        }

        /// <summary>
        /// Load Setting from json
        /// </summary>
        private void LoadSettings()
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                _currentSettings = JsonUtility.FromJson<GameSetting>(json);
                ApplyAllSettings();
            }
            else
            {
                _currentSettings = new GameSetting();
            }
        }

        /// <summary>
        /// Apply Current Setting
        /// </summary>
        private void ApplyAllSettings()
        {
            SetVolume("Master", _currentSettings.masterVolume);
            SetVolume("BGM", _currentSettings.bgmVolume);
            SetVolume("SFX", _currentSettings.sfxVolume);
            
            if (_resolutions.Count > _currentSettings.resolutionIndex)
            {
                SetResolution(_currentSettings.resolutionIndex, _currentSettings.isFullScreen);
            }
        }

        #endregion 
    }
}
