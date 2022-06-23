using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumOrder))]
public class EnumOrderDrawer : PropertyDrawer
{
    private EnumOrder EnumOrder => (EnumOrder)attribute;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Store resorted string array for the popup item names
        var items = new string[EnumOrder.Order.Length];
        items[0] = property.enumNames[0];
        for (var i = 0; i < EnumOrder.Order.Length; i++) items[i] = property.enumNames[EnumOrder.Order[i]];

        // Get selected enum based on position
        var index = -1;
        for (var i = 0; i < EnumOrder.Order.Length; i++)
            if (EnumOrder.Order[i] == property.enumValueIndex)
            {
                index = i;
                break;
            }

        if (index == -1 && property.enumValueIndex != -1)
        {
            SortingError(position, property, label);
            return;
        }

        // Display popup
        index = EditorGUI.Popup(
            position,
            label.text,
            index,
            items);
        property.enumValueIndex = EnumOrder.Order[index];

        // Default
        //EditorGUI.PropertyField(position, property, new GUIContent("*" + label.text));

        EditorGUI.EndProperty();
    }

    /// Use default enum popup, but flag label to aware user
    private void SortingError(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, new GUIContent(label.text + " (sorting error)"));
        EditorGUI.EndProperty();
    }
}