using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherRequest : IRequest
{
    private const string Url = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

    public async UniTask<NetworkResult> ExecuteAsync(CancellationToken ct)
    {
        using var uwr = UnityWebRequest.Get(Url);
        await uwr.SendWebRequest().WithCancellation(ct);
        return new NetworkResult(uwr.result, uwr.downloadHandler.text);
    }
}

