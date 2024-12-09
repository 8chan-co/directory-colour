using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditorInternal;
using UnityEngine;

namespace Ophélia
{
    [InitializeOnLoad]
    internal sealed class DirectoryTint : AssetModificationProcessor
    {
        private static string AssetsDirectory => InternalEditorUtility.GetAssetsFolder();

        internal const string TintDirectoryLabel = "Tint Directory";
        internal const string SuppressTingeLabel = "Suppress Tinge";
        internal const string RegisteredDirectoriesLabel = "Registered Directories";

        private const int BasePrecedence = 1200;
        internal const int TintDirectoryPrecedence = BasePrecedence + 0;
        internal const int SuppressTingePrecedence = BasePrecedence + 1;
        internal const int RegisteredDirectoriesPrecedence = BasePrecedence + 2;

        private static readonly Color32 PureDirectoryFinish = new(r: 194, g: 194, b: 194, a: byte.MaxValue);

        static DirectoryTint() => EditorApplication.projectWindowItemInstanceOnGUI += AsProjectViewAssetDemandsPaint;

        private static void AsProjectViewAssetDemandsPaint(int Identifier, Rect Area)
        {
            if (Event.current.type is not EventType.Repaint) return;

            if (RepresentsCompatibleDirectory(Identifier, out string Pathname) is false) return;

            if (TintStorage.instance.TryGetValue(Pathname, out Color32 Tinge) is false) return;

            GUI.DrawTexture(
                position: ComputeAreaAlignment(Area),
                image: DirectoryFullnessImagery(Pathname),
                scaleMode: ScaleMode.ScaleToFit,
                alphaBlend: true,
                imageAspect: 0f,
                color: Tinge,
                borderWidth: 0f,
                borderRadius: 0f
            );
        }

        private static bool RepresentsCompatibleDirectory(int Identifier, out string Pathname)
        {
            Pathname = AssetDatabase.GetAssetPath(Identifier);

            return AssetDatabase.IsValidFolder(Pathname) && string.CompareOrdinal(Pathname, AssetsDirectory) is not 0;
        }

        private static Rect ComputeAreaAlignment(Rect Area) => Area switch
        {
            { height: > 20f } => new(Area.x, Area.y, Area.width, Area.width),
            _ => new(Area.x, Area.y, Area.height, Area.height)
        };

        private static Texture DirectoryFullnessImagery(string Pathname)
        {
            Pathname = Directory.EnumerateFileSystemEntries(Pathname)
                .Any() ? EditorResources.folderIconName : EditorResources.emptyFolderIconName;

            return EditorGUIUtility.FindTexture(Pathname);
        }

        [MenuItem("Assets/Ophélia/" + TintDirectoryLabel, isValidateFunction: true, priority: TintDirectoryPrecedence)]
        private static bool PresentTingeSelectorActivation() =>
            RepresentsCompatibleDirectory(Selection.activeInstanceID, out _);

        [MenuItem("Assets/Ophélia/" + TintDirectoryLabel, isValidateFunction: false, priority: TintDirectoryPrecedence)]
        internal static void PresentTingeSelector()
        {
            string Identifier = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
            bool AppearanceBeenMutated = TintStorage.instance.ContainsKey(Identifier);
            Color32 Tinge = AppearanceBeenMutated ? TintStorage.instance[Identifier] : PureDirectoryFinish;

            TingeSelector.Present(Operation: WhilstTintAlters, InitialTinge: Tinge);
        }

        private static void WhilstTintAlters(Color Tinge)
        {
            string DirectoryIdentifier = AssetDatabase.GetAssetPath(Selection.activeInstanceID);

            if (TintStorage.instance.ContainsKey(DirectoryIdentifier))
                TintStorage.instance[DirectoryIdentifier] = Tinge;
            else TintStorage.instance.Add(DirectoryIdentifier, Tinge);

            EditorApplication.RepaintProjectWindow();
            RegisteredDirectories.Refresh();
        }

        [MenuItem("Assets/Ophélia/" + SuppressTingeLabel, isValidateFunction: true, priority: SuppressTingePrecedence)]
        private static bool SuppressTingeActivation() =>
            RepresentsCompatibleDirectory(Selection.activeInstanceID, out string Identifier) &&
            TintStorage.instance.ContainsKey(Identifier);

        [MenuItem("Assets/Ophélia/" + SuppressTingeLabel, isValidateFunction: false, priority: SuppressTingePrecedence)]
        internal static void SuppressTinge()
        {
            TintStorage.instance.Remove(AssetDatabase.GetAssetPath(Selection.activeInstanceID));
            TintStorage.instance.Save();

            EditorApplication.RepaintProjectWindow();
            RegisteredDirectories.Refresh();
        }

        internal static AssetDeleteResult OnWillDeleteAsset(string Identifier, RemoveAssetOptions _)
        {
            if (File.GetAttributes(Identifier).HasFlag(FileAttributes.Directory))
            {
                if (TintStorage.instance.ContainsKey(Identifier))
                {
                    TintStorage.instance.Remove(Identifier);
                    TintStorage.instance.Save();

                    EditorApplication.RepaintProjectWindow();
                }
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }
}
