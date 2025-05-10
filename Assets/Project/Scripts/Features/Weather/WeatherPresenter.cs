using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

public class WeatherPresenter : IInitializable, IDisposable
{
    private readonly IWeatherView _view;
    private readonly IRequestQueue _queue;
    private readonly WeatherRequest _request = new();
    private CancellationTokenSource _cts;

    [Inject]
    public WeatherPresenter(IWeatherView view, IRequestQueue queue)
    {
        _view = view;
        _queue = queue;
    }

    public void Initialize()
    {
        _view.OnBecameVisible += StartUpdates;
    }

    private void StartUpdates()
    {
        _view.OnBecameInvisible += StopUpdates;
        _cts = new CancellationTokenSource();
        Loop(_cts.Token).Forget();
    }

    private async UniTaskVoid Loop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            _queue.Enqueue(_request);
            await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: ct);
        }
    }

    private void StopUpdates()
    {
        _view.OnBecameInvisible -= StopUpdates;
        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
        _queue.CancelActive(typeof(WeatherRequest));
        _queue.Cancel(typeof(WeatherRequest));
    }

    public void Dispose()
    {
        _view.OnBecameVisible -= StartUpdates;
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
