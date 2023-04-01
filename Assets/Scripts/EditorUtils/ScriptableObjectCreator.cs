#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScriptableObjectCreator
{
    [MenuItem("Assets/Create/Scriptable Object", false, 0)]
    public static void CreateScriptableObject()
    {
        Object selection = Selection.activeObject;
        var assetPath = AssetDatabase.GetAssetPath(selection.GetInstanceID());

        if (assetPath.Contains("."))
            assetPath = assetPath.Remove(assetPath.LastIndexOf('/'));

        var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(assetPath, $"{selection.name}.asset"));
        var scriptableObject = ScriptableObject.CreateInstance(selection.name);
        ProjectWindowUtil.CreateAsset(scriptableObject, uniqueFileName);
    }

    [MenuItem("Assets/Create/Scriptable Object", true)]
    public static bool CreateScriptableObjectValidate()
    {
        return Selection.activeObject is MonoScript monoScript && IsScriptableObject(monoScript.GetClass());
    }

    private static bool IsScriptableObject(System.Type type) => typeof(ScriptableObject).IsAssignableFrom(type);
}
#endif