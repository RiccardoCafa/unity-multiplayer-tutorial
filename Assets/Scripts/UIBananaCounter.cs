using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIBananaCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textBananaCounter;

    private void Start()
    {
        GameManager.Instance.BananaCounter.OnValueChanged += OnBananaCounterChanged;
    }

    private void OnBananaCounterChanged(int prev, int next)
    {
        SetBananaCounterTo(next);
    }

    public void SetBananaCounterTo(int counter)
    {
        _textBananaCounter.text = counter.ToString() + " bananas";
    }
}
