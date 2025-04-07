using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Lấy tham chiếu đến các biến Serialized
        SerializedProperty typeProp = serializedObject.FindProperty("type");
        SerializedProperty burnableOptionsProp = serializedObject.FindProperty("burnableOptions");

        EditorGUILayout.PropertyField(typeProp); // Hiển thị trường Type

        // Nếu Type == Burnable, hiển thị BurnableOptions
        Interactable.InteractableType type = (Interactable.InteractableType)typeProp.enumValueIndex;
        if (type == Interactable.InteractableType.Burnable)
        {
            EditorGUILayout.PropertyField(burnableOptionsProp, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
