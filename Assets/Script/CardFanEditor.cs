using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardFan))]
public class CardFanEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CardFan fan = (CardFan)target;

        GUILayout.Space(10);
        GUILayout.Label("Card Control", EditorStyles.boldLabel);

        if (GUILayout.Button("Draw Card"))
        {
            fan.DrawCard();
            EditorUtility.SetDirty(fan); // 변경 사항 적용
        }

        if (GUILayout.Button("Remove Card"))
        {
            fan.RemoveCard();
            EditorUtility.SetDirty(fan); // 변경 사항 적용
        }
    }
}