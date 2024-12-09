using System.Linq;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

namespace Ophélia
{
    internal sealed class RegisteredDirectories : EditorWindow
    {
        internal static RegisteredDirectories Singleton;

        private Vector2 ScrollView;

        [MenuItem("Assets/Ophélia/" + DirectoryTint.RegisteredDirectoriesLabel, true, DirectoryTint.RegisteredDirectoriesPrecedence)]
        private static bool GetWindowActivation() => TintStorage.instance.IdentifierSet.Any();

        [MenuItem("Assets/Ophélia/" + DirectoryTint.RegisteredDirectoriesLabel, false, DirectoryTint.RegisteredDirectoriesPrecedence)]
        private static void GetWindow() => Singleton = GetWindow<RegisteredDirectories>(true, "Registered Directories", true);

        internal void OnGUI()
        {
            ScrollView = EditorGUILayout.BeginScrollView(ScrollView);

            foreach (string Identifier in TintStorage.instance.IdentifierSet)
            {
                Rect ButtonArea = GUILayoutUtility.GetRect(new GUIContent(Identifier), EditorStyles.linkLabel);

                GUI.Label(ButtonArea, Identifier, EditorStyles.linkLabel);

                Event CurrentEvent = Event.current;
                if (CurrentEvent.type is not EventType.MouseDown) continue;
                if (ButtonArea.Contains(CurrentEvent.mousePosition) is false) continue;

                EditorGUIUtility.PingObject(Selection.activeInstanceID = SearchUtils.GetMainAssetInstanceID(Identifier));

                if (CurrentEvent.button is 1)
                {
                    GenericMenu menu = new();
                    menu.AddItem(new(DirectoryTint.TintDirectoryLabel), false, DirectoryTint.PresentTingeSelector);
                    menu.AddItem(new(DirectoryTint.SuppressTingeLabel), false, DirectoryTint.SuppressTinge);
                    menu.ShowAsContext();
                }

                CurrentEvent.Use();
            }

            EditorGUILayout.EndScrollView();
        }

        internal static void Refresh()
        {
            if (Singleton == null) return;

            Singleton.Repaint();
        }
    }
}
