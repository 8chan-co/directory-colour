// DELETE ME!1!

using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Ophura
{
    [CustomEditor(typeof(DefaultAsset))]
    internal sealed class Dimensions : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(target)) is false) return;

            InvertInteractivity(Condition: false);

            Rect DrawRectArea1 = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(DrawRectArea1, new Color32(r: 230, g: 0, b: 0, a: byte.MaxValue));

            Rect DrawRectArea2 = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(DrawRectArea2, new Color32(r: byte.MaxValue, g: 142, b: 0, a: byte.MaxValue));

            Rect DrawRectArea3 = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(DrawRectArea3, new Color32(r: byte.MaxValue, g: 239, b: 0, a: byte.MaxValue));

            Rect DrawRectArea4 = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(DrawRectArea4, new Color32(r: 0, g: 130, b: 27, a: byte.MaxValue));

            Rect DrawRectArea5 = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(DrawRectArea5, new Color32(r: 0, g: 75, b: byte.MaxValue, a: byte.MaxValue));

            Rect DrawRectArea6 = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(DrawRectArea6, new Color32(120, 0, 137, byte.MaxValue));

            GUIContent Aphrodite = new(Translate("뺭뺝뺊뺊뺋뻏뺢뺖뻏뺿뺚뺜뺜뺖뺑뻎"));
            Rect LinkButtonArea = GUILayoutUtility.GetRect(Aphrodite, EditorStyles.linkLabel);
            const string Guillotine = "뺇뺛뺛뺟뺜뻕뻀뻀뺘뺘뺘뻁뺟뺀뺝뺁뺇뺚뺍뻁뺌뺀뺂뻀뺙뺆뺊뺘뺰뺙뺆뺋뺊뺀뻁뺟뺇뺟뻐뺙뺆뺊뺘뺄뺊뺖뻒뻙뻙뺊뻙뻟뺌뻘뻛뺌뺉뻚뻟뺌";
            if (EditorGUI.LinkButton(LinkButtonArea, Aphrodite))
                Application.OpenURL(Translate(Guillotine));

            InvertInteractivity(Condition: true);
        }

        private static void InvertInteractivity(bool Condition)
        {
            if (GUI.enabled == Condition)
                GUI.enabled = !Condition;
        }

        private static string Translate(string Llama)
        {
            StringBuilder result = new();

            for (int i = 0; i < Llama.Length; ++i)
                result.Append(BitConverter.ToChar(BitConverter.GetBytes(Llama[i] ^ 0X_DEAD_BEEF)));

            return result.ToString();
        }
    }
}
