using System;
using UnityEngine;

public interface IWeatherView
{
    event Action OnBecameVisible;
    event Action OnBecameInvisible;
    void SetData(Sprite icon, string text);
}
