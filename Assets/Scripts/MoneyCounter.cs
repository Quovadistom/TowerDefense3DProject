using TMPro;
using UnityEngine;
using Zenject;

public class MoneyCounter : MonoBehaviour
{
    public TMP_Text m_text;
    private ResourceService m_resourceService;

    [Inject]
    public void Construct(ResourceService resourceService)
    {
        m_resourceService = resourceService;
    }

    private void Awake()
    {
        UpdateText(m_resourceService.GetAvailableResourceAmount<BattleFunds>());
        m_resourceService.ResourceChanged += OnFundsChanged;
    }

    private void OnDestroy()
    {
        m_resourceService.ResourceChanged -= OnFundsChanged;
    }

    private void OnFundsChanged(object sender, ResourcesChangeEventArgs e)
    {
        if (e.Resource is BattleFunds)
        {
            UpdateText(e.NewAmount);
        }
    }

    private void UpdateText(int amount)
    {
        m_text.text = amount.ToString();
    }
}