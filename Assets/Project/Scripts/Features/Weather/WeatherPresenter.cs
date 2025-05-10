using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class WeatherPresenter : IInitializable, IDisposable
{
    readonly IWeatherView _view;
    CancellationTokenSource _cts;

    [Inject] public WeatherPresenter(IWeatherView view) => _view = view;

    public void Initialize()
    {
        _view.OnBecameVisible += StartUpdates;

        if ((_view as MonoBehaviour)?.isActiveAndEnabled == true)
            StartUpdates();
    }


    void StartUpdates()
    {
        _view.OnBecameInvisible -= StopUpdates;
        _view.OnBecameInvisible += StopUpdates;

        _cts?.Dispose();
        _cts = new CancellationTokenSource();
        Loop(_cts.Token).Forget();
    }

    async UniTaskVoid Loop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await UpdateWeather(ct);
            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: ct);
        }
    }

    async UniTask UpdateWeather(CancellationToken ct)
    {
        _view.ShowLoading();

        const string url = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        using var uwr = UnityWebRequest.Get(url);
        await uwr.SendWebRequest().WithCancellation(ct);
        if (uwr.result != UnityWebRequest.Result.Success) return;

        var root = JsonUtility.FromJson<Root>(uwr.downloadHandler.text);
        var p = root.properties.periods[0];
        int temp = p.temperature;
        string iconUrl = p.icon;

        Sprite sprite = null;
        using (var texReq = UnityWebRequestTexture.GetTexture(iconUrl))
        {
            await texReq.SendWebRequest().WithCancellation(ct);
            if (texReq.result == UnityWebRequest.Result.Success)
            {
                var tex = DownloadHandlerTexture.GetContent(texReq);
                var rect = new Rect(0, 0, tex.width, tex.height);
                sprite = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
            }
        }

        _view.SetData(sprite, $"Сегодня: {temp} °F");
    }

    [Serializable] class Root { public Props properties; }
    [Serializable] class Props { public Period[] periods; }
    [Serializable]
    class Period
    {
        public int temperature;
        public string icon;
    }

    void StopUpdates()
    {
        _view.OnBecameInvisible -= StopUpdates;
        if (_cts != null)
        {
            if (!_cts.IsCancellationRequested) _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    public void Dispose()
    {
        _view.OnBecameVisible -= StartUpdates;
        if (_cts != null)
        {
            if (!_cts.IsCancellationRequested) _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}
