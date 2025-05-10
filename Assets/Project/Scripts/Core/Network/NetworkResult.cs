using UnityEngine;
using UnityEngine.Networking;

public readonly struct NetworkResult
{
    public NetworkResult(UnityWebRequest.Result status, string text, long responseCode = 0)
    {
        Status = status;
        Text = text;
        ResponseCode = responseCode;
    }

    public UnityWebRequest.Result Status { get; }
    public string Text { get; }
    public long ResponseCode { get; }

    public bool IsSuccess => Status == UnityWebRequest.Result.Success && ResponseCode >= 200 && ResponseCode < 300;

    public override string ToString() => $"[{Status} | {ResponseCode}] {Text?.Substring(0, Mathf.Min(Text.Length, 80))}";
}

