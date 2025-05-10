using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class RequestQueue : IRequestQueue, ITickable, IDisposable
{
    private readonly Queue<IRequest> _queue = new();
    private CancellationTokenSource _cts;

    public void Enqueue(IRequest req) => _queue.Enqueue(req);

    public void Tick()
    {
        if (_cts is { IsCancellationRequested: false } || _queue.Count == 0)
            return;

        var req = _queue.Dequeue();
        _cts = new CancellationTokenSource();
        _ = Process(req, _cts.Token);
    }

    private async UniTaskVoid Process(IRequest req, CancellationToken ct)
    {
        try { await req.ExecuteAsync(ct); }
        catch (OperationCanceledException) { /* нормально */ }
        finally { _cts.Dispose(); _cts = null; }
    }

    public void Cancel(Type t) => _queue.RemoveAll(q => q.GetType() == t);
    public void CancelActive(Type t) => _cts?.Cancel();
    public void Dispose() => _cts?.Cancel();
}

