using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherView : MonoBehaviour, IWeatherView
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text tempText;
    [SerializeField] GameObject loadingPanel;

    public event Action OnBecameVisible;
    public event Action OnBecameInvisible;

    void OnEnable() => OnBecameVisible?.Invoke();
    void OnDisable() => OnBecameInvisible?.Invoke();

    public void ShowLoading()
    {
        loadingPanel.SetActive(true);
        icon.enabled = false;
        tempText.enabled = false;
    }

    public void SetData(Sprite s, string t)
    {
        loadingPanel.SetActive(false);
        icon.sprite = s;
        icon.enabled = s != null;
        tempText.text = t;
        tempText.enabled = true;
    }
}
