﻿using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Property drawer for SerializableGuid
///
/// Author: Searous
/// </summary>
[CustomPropertyDrawer(typeof(SerializableGuid))]
public class SerializableGuidPropertyDrawer : PropertyDrawer
{
    private float ySep = 20;
    private float buttonSize;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Start property draw
        EditorGUI.BeginProperty(position, label, property);

        // Get property
        SerializedProperty serializedGuid = property.FindPropertyRelative("serializedGuid");
        SerializedProperty lockedProperty = property.FindPropertyRelative("m_isLocked");

        // Draw label
        position = EditorGUI.PrefixLabel(new Rect(position.x, position.y + ySep / 2, position.width, position.height), GUIUtility.GetControlID(FocusType.Passive), label);
        position.y -= ySep / 2; // Offsets position so we can draw the label for the field centered

        buttonSize = position.width / 4; // Update size of buttons to always fit perfeftly above the string representation field

        if (GUI.Toggle(new Rect(position.xMin, position.yMin, buttonSize, ySep - 2), lockedProperty.boolValue, "Lock") != lockedProperty.boolValue)
        {
            lockedProperty.boolValue = !lockedProperty.boolValue;
        }

        // Buttons
        if (!lockedProperty.boolValue && GUI.Button(new Rect(position.xMin + buttonSize, position.yMin, buttonSize, ySep - 2), "New"))
        {
            serializedGuid.stringValue = Guid.NewGuid().ToString();
            lockedProperty.boolValue = true;
        }
        if (!lockedProperty.boolValue && GUI.Button(new Rect(position.xMin + buttonSize * 2, position.yMin, buttonSize, ySep - 2), "Empty"))
        {
            serializedGuid.stringValue = Guid.Empty.ToString();
        }

        if (GUI.Button(new Rect(position.xMin + buttonSize * 3, position.yMin, buttonSize, ySep - 2), "Copy"))
        {
            EditorGUIUtility.systemCopyBuffer = serializedGuid.stringValue;
        }

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        Rect pos = new Rect(position.xMin, position.yMin + ySep, position.width, ySep - 2);
        EditorGUI.PropertyField(pos, serializedGuid, GUIContent.none);

        // End property
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Field height never changes, so ySep * 2 will always return the proper hight of the field
        return ySep * 2;
    }
}
