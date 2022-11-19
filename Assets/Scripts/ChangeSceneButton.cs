using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

// Should become router/state machine?
public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private Button m_button;
    private SceneCollection m_sceneCollection;

    [Inject]
    public void Construct(SceneCollection sceneCollection)
    {
        m_sceneCollection = sceneCollection;
    }

    private void Awake()
    {
        m_button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        m_button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        SceneManager.LoadScene(m_sceneCollection.LevelScene.name, LoadSceneMode.Single);
    }
}
