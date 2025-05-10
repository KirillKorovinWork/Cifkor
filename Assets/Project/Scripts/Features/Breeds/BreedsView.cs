using UnityEngine;

public class BreedsView : MonoBehaviour
{
    [SerializeField] Transform contentRoot;
    [SerializeField] BreedButtonView buttonPrefab;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] InfoPanelView infoPanel;

    public void ShowLoading(bool v) => loadingPanel.SetActive(v);

    public BreedButtonView SpawnButton() =>
        Instantiate(buttonPrefab, contentRoot);

    public InfoPanelView InfoPanel => infoPanel;
}
