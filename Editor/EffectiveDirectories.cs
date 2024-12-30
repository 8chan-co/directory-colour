using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

namespace Ophura
{
    [EditorWindowTitle(title = Consistent.EffectiveDirectoriesFeature)]
    internal sealed class EffectiveDirectories : EditorWindow
    {
        private GenericMenu MenuOptions;
        private Vector2 ScrollView;

        internal static EffectiveDirectories Singular;

        [MenuItem(itemName: Consistent.EffectiveDirectoriesFeature, isValidateFunction: true, priority: Consistent.ListEffectiveDirectoriesPrecedence)]
        private static bool GetWindowActivation() => ColourStorage.Singular.Occupied;

        [MenuItem(itemName: Consistent.EffectiveDirectoriesFeature, isValidateFunction: false, priority: Consistent.ListEffectiveDirectoriesPrecedence)]
        private static void GetWindow() => Singular = GetWindow<EffectiveDirectories>();

        internal static void Refresh()
        {
            if (Singular == null) return;

            Singular.Repaint();
        }

        private void OnEnable()
        {
            MenuOptions = new();

            MenuOptions.AddItem(
                content: new(text: Consistent.ColouriseDirectoryLabel),
                on: false,
                func: DirectoryColour.PresentColourSelector
            );

            MenuOptions.AddSeparator(string.Empty);

            MenuOptions.AddItem(
                content: new(text: Consistent.SuppressColourLabel),
                on: false,
                func: DirectoryColour.SuppressColour
            );
        }

        private void OnGUI()
        {
            ScrollView = EditorGUILayout.BeginScrollView(ScrollView);
            foreach (string Identifier in ColourStorage.Singular)
            {
                GUIContent Content = new(Identifier);
                Rect ButtonArea = GUILayoutUtility.GetRect(Content, EditorStyles.linkLabel);
                GUI.Label(ButtonArea, Content, EditorStyles.linkLabel);

                if (mouseOverWindow != this) continue;

                Event CurrentEvent = Event.current;
                if (CurrentEvent.rawType is not EventType.MouseDown) continue;
                if (ButtonArea.Contains(CurrentEvent.mousePosition) is false) continue;

                int InstanceIdentifier = SearchUtils.GetMainAssetInstanceID(Identifier);
                Selection.activeInstanceID = InstanceIdentifier;
                EditorGUIUtility.PingObject(InstanceIdentifier);

                if (CurrentEvent.button is not Consistent.ContextMicePress) continue;

                MenuOptions.ShowAsContext();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
