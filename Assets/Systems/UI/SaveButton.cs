using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SaveButton : MonoBehaviour
{
    [SerializeField] Button m_saveButton;
    private SerializationService m_serializationService;

    [Inject]
    public void Construct(SerializationService serializationService)
    {
        m_serializationService = serializationService;
    }

    private void Awake()
    {
        m_saveButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        m_serializationService.RequestSerialization();
    }

    private void OnDestroy()
    {
        m_saveButton.onClick.RemoveListener(OnButtonClicked);
    }
}
