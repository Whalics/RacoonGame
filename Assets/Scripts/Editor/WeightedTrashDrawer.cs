using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WeightedTrash), true)]
public class WeightedTrashDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
    {
        var weightProp = property.FindPropertyRelative("Weight");
        var spriteProp = property.FindPropertyRelative("Trash");

        float fourth = pos.width * 0.25f - 4;
        
        var weightRect = new Rect(pos.x, pos.y, fourth, pos.height - 2);
        EditorGUI.PropertyField(weightRect, weightProp, GUIContent.none);
        
        var spriteRect = new Rect(pos.x + fourth + 4, pos.y, 3 * fourth, pos.height - 2);
        EditorGUI.PropertyField(spriteRect, spriteProp, GUIContent.none);
    }
}
