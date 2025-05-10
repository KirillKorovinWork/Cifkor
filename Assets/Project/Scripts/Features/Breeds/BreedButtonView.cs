using System;                       
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreedButtonView : MonoBehaviour
{
    [SerializeField] TMP_Text indexText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] GameObject loadingImage;

    Button _btn;
    Action _onClick;

    void Awake() => _btn = GetComponent<Button>();

    public void Init(int index, string name, Action onClick)
    {
        indexText.text = (index + 1).ToString();
        nameText.text = name;
        _onClick = onClick;

        loadingImage.SetActive(false);
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        loadingImage.SetActive(true);
        _onClick?.Invoke();
    }

    public void StopLoading() => loadingImage.SetActive(false);
}
