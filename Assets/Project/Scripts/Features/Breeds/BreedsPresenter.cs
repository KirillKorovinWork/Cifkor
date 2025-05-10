using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class BreedsPresenter : IInitializable, IDisposable
{
    readonly BreedsView _view;

    [Inject] public BreedsPresenter(BreedsView view) => _view = view;

    public void Initialize()
    {
        _view.ShowLoading(true);
        LoadList().Forget();
    }

    async UniTask LoadList()
    {
        const string url = "https://dog.ceo/api/breeds/list";

        using var uwr = UnityWebRequest.Get(url);
        await uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success) return;

        var breeds = JsonUtility.FromJson<Root>(uwr.downloadHandler.text)
                                .message.Take(10).ToArray();

        for (int i = 0; i < breeds.Length; i++)
        {
            var breedName = breeds[i];                       // локальная копия
            var btn = _view.SpawnButton();
            btn.Init(i, breedName, () => OnBreedClick(btn, breedName));
        }
        _view.ShowLoading(false);
    }

    void OnBreedClick(BreedButtonView btn, string breed) => LoadInfo(btn, breed).Forget();

    async UniTask LoadInfo(BreedButtonView btn, string breed)
    {
        var panel = _view.InfoPanel;
        panel.ShowLoading(breed);

        string url = $"https://api.thedogapi.com/v1/breeds/search?q={breed}";
        using var uwr = UnityWebRequest.Get(url);
        await uwr.SendWebRequest();

        string desc = "No data";
        if (uwr.result == UnityWebRequest.Result.Success)
        {
            var arr = JsonUtility.FromJson<ArrayWrapper>(Wrap(uwr.downloadHandler.text));
            if (arr.items.Length > 0) desc = arr.items[0].temperament;
        }

        panel.SetInfo(desc);
        btn.StopLoading();
    }

    public void Dispose() { }

    [Serializable] class Root { public string[] message; }
    [Serializable] class DogInfo { public string temperament; }
    [Serializable] class ArrayWrapper { public DogInfo[] items; }
    string Wrap(string json) => $"{{\"items\":{json}}}";
}
