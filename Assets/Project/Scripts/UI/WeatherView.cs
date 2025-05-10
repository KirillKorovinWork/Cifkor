using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherView : MonoBehaviour, IWeatherView
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text temperature;

    public event Action OnBecameVisible;
    public event Action OnBecameInvisible;

    private void OnEnable() => OnBecameVisible?.Invoke();
    private void OnDisable() => OnBecameInvisible?.Invoke();

    public void SetData(Sprite s, string t)
    {
        icon.sprite = s;
        temperature.text = t;
    }
}
