using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    private SerializedProperty typeProperty;
    private SerializedProperty carriableTypeProperty;

    private void OnEnable()
    {
        // Lấy các SerializedProperty từ đối tượng Interactable
        typeProperty = serializedObject.FindProperty("type");
        carriableTypeProperty = serializedObject.FindProperty("carriableType");
    }

    public override void OnInspectorGUI()
    {
        // Cập nhật đối tượng serialized
        serializedObject.Update();

        // Hiển thị thuộc tính type
        EditorGUILayout.PropertyField(typeProperty);

        // Hiển thị CarriableType chỉ khi type là Carriable
        if ((Interactable.InteractableType)typeProperty.enumValueIndex == Interactable.InteractableType.Carriable)
        {
            EditorGUILayout.PropertyField(carriableTypeProperty);
        }

        // Áp dụng thay đổi
        serializedObject.ApplyModifiedProperties();
    }
}