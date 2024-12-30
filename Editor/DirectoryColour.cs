using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Ophura
{
    [InitializeOnLoad]
    internal sealed class DirectoryColour
    {
        private static string SelectedDirectoryPathname => AssetDatabase.GetAssetPath(Selection.activeInstanceID);

        static DirectoryColour() => EditorApplication.projectWindowItemInstanceOnGUI += DirectoryColourisationApplication;

        private static void DirectoryColourisationApplication(int Identifier, Rect Layout)
        {
            if (Event.current.rawType is not EventType.Repaint) return;

            string Pathname = AssetDatabase.GetAssetPath(Identifier);

            if (RepresentsCompatibleDirectory(Pathname) is false) return;

            if (ColourStorage.instance.AttemptObtaintion(Pathname, out Color Colour) is false) return;

            // TODO: Use the provided colour, otherwise; nothing will be drawn.

            //Layout.size *= 0.5f;
            /*Layout.position += new Vector2(-2f, -2f);

            GUI.DrawTexture(
                position: Layout,
                image: DirectoryFullnessImagery(Pathname),
                scaleMode: ScaleMode.ScaleToFit,
                alphaBlend: true,
                imageAspect: 0f,
                color: Colour,
                borderWidth: 0f,
                borderRadius: 0f
            );*/

            _ = Colour;
        }

        internal static bool RepresentsCompatibleDirectory(string Pathname)
        {
            if (string.IsNullOrEmpty(Pathname))
            {
                return false;
            }
            if (File.GetAttributes(Pathname).HasFlag(FileAttributes.Directory) is false)
            {
                return false;
            }

            string AssetsDirectoryPathname = InternalEditorUtility.GetAssetsFolder();
            string AssetsDirectoryName = Path.GetDirectoryName(AssetsDirectoryPathname);

            if (string.IsNullOrEmpty(AssetsDirectoryName))
            {
                return AssetsDirectoryPathname.Equals(Pathname) is false;
            }

            return AssetsDirectoryName.Equals(Pathname) is false;
        }

        [MenuItem(itemName: Consistent.ColouriseDirectoryFeature, isValidateFunction: true, priority: Consistent.ColouriseDirectoryPrecedence)]
        private static bool PresentColourSelectorActivation() => RepresentsCompatibleDirectory(SelectedDirectoryPathname);

        [MenuItem(itemName: Consistent.ColouriseDirectoryFeature, isValidateFunction: false, priority: Consistent.ColouriseDirectoryPrecedence)]
        internal static void PresentColourSelector()
        {
            string Identifier = SelectedDirectoryPathname;

            Color InitialColour = ColourStorage.instance.Incorporates(Identifier)
                ? ColourStorage.instance[Identifier]
                : Consistent.PureDirectoryColour;

            ColourSelector.Show(Operation: ColourMutationApplication, InitialColour);
        }

        private static void ColourMutationApplication(Color Colour)
        {
            string Identifier = SelectedDirectoryPathname;

            if (ColourStorage.instance.Incorporates(Identifier))
                ColourStorage.instance[Identifier] = Colour;
            else ColourStorage.instance.Affix(Identifier, Colour);

            EditorApplication.RepaintProjectWindow();
            EffectiveDirectories.Refresh();
        }

        [MenuItem(itemName: Consistent.SuppressColourFeature, isValidateFunction: true, priority: Consistent.SuppressColourPrecedence)]
        private static bool SuppressColourActivation()
        {
            string Identifier = SelectedDirectoryPathname;

            return RepresentsCompatibleDirectory(Identifier) && ColourStorage.instance.Incorporates(Identifier);
        }

        [MenuItem(itemName: Consistent.SuppressColourFeature, isValidateFunction: false, priority: Consistent.SuppressColourPrecedence)]
        internal static void SuppressColour()
        {
            ColourStorage.instance.Eliminate(SelectedDirectoryPathname);

            EditorApplication.RepaintProjectWindow();
            EffectiveDirectories.Refresh();
        }
    }
}
