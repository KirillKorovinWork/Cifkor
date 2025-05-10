using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [SerializeField] CanvasGroup weatherPanel;   
    [SerializeField] CanvasGroup breedsPanel;    
    [SerializeField] Button weatherBtn;
    [SerializeField] Button breedsBtn;
    [SerializeField] float fadeTime = .25f;

    void Awake()
    {
        weatherBtn.onClick.AddListener(ShowWeather);
        breedsBtn.onClick.AddListener(ShowBreeds);
        ShowWeather();                          
    }

    void ShowWeather()
    {
        if (!weatherBtn.interactable) return;

        breedsPanel.DOKill();
        weatherPanel.DOKill();

        breedsPanel.DOFade(0, fadeTime)
                   .OnComplete(() => breedsPanel.gameObject.SetActive(false));

        weatherPanel.gameObject.SetActive(true);
        weatherPanel.alpha = 0;
        weatherPanel.DOFade(1, fadeTime);

        weatherBtn.interactable = false;
        breedsBtn.interactable = true;
    }

    void ShowBreeds()
    {
        if (!breedsBtn.interactable) return;

        weatherPanel.DOKill();
        breedsPanel.DOKill();

        weatherPanel.DOFade(0, fadeTime)
                    .OnComplete(() => weatherPanel.gameObject.SetActive(false));

        breedsPanel.gameObject.SetActive(true);
        breedsPanel.alpha = 0;
        breedsPanel.DOFade(1, fadeTime);

        weatherBtn.interactable = true;
        breedsBtn.interactable = false;
    }
}
