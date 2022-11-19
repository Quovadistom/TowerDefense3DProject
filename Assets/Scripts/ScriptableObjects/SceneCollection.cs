
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneCollection", menuName = "ScriptableObjects/SceneCollection")]
public class SceneCollection : ScriptableObject
{
    public SceneAsset TownScene;
    public SceneAsset LevelScene;
}
