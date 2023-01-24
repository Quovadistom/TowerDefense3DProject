using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneCollection", menuName = "ScriptableObjects/SceneCollection")]
public class SceneCollection : ScriptableObject
{
    [Scene]
    public string TownScene;
    [Scene]
    public string LevelScene;
}
