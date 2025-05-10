using Cysharp.Threading.Tasks;
using System.Threading;

public interface IRequest
{
    UniTask<NetworkResult> ExecuteAsync(CancellationToken ct);
}