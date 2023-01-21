using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private ItemsMenu m_itemsMenu;
    [SerializeField] private List<TowerBoostCollection> m_towerUpgradeCollections;
    private TowerBoostService m_towerUpgradeService;
    private SerializationService m_serializationService;

    [Inject]
    public void Construct(TowerBoostService towerUpgradeService, SerializationService serializationService)
    {
        m_towerUpgradeService = towerUpgradeService;
        m_serializationService = serializationService;
    }

    [Button]
    public void SaveService()
    {
        m_serializationService.RequestSerialization();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
