using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelView : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text info;
    [SerializeField] GameObject loading;
    [SerializeField] Button okButton;

    void Awake() => okButton.onClick.AddListener(() => gameObject.SetActive(false));

    public void ShowLoading(string breed)
    {
        title.text = breed;
        info.enabled = false;
        loading.SetActive(true);
        gameObject.SetActive(true);
    }

    public void SetInfo(string text)
    {
        loading.SetActive(false);
        info.text = text;
        info.enabled = true;
    }
}
