using NaughtyAttributes;
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
    [SerializeField][Scene] private string m_scene;

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
        SceneManager.LoadScene(m_scene, LoadSceneMode.Single);
    }
}
