using JetBrains.Annotations;
using UnityEditor;

namespace Ophura
{
    internal sealed class AssetActivitySupervisor : AssetModificationProcessor
    {
        [UsedImplicitly]
        private static AssetDeleteResult OnWillDeleteAsset(string Identifier, RemoveAssetOptions _)
        {
            if (DirectoryColour.RepresentsCompatibleDirectory(Identifier))
            {
                if (ColourStorage.Singular.Incorporates(Identifier))
                {
                    ColourStorage.Singular.Eliminate(Identifier);
                    EditorApplication.RepaintProjectWindow();
                }
            }

            return AssetDeleteResult.DidNotDelete;
        }

        [UsedImplicitly]
        private static AssetMoveResult OnWillMoveAsset(string OriginPathname, string EndpointPathname)
        {
            if (DirectoryColour.RepresentsCompatibleDirectory(OriginPathname))
            {
                if (ColourStorage.Singular.Incorporates(OriginPathname))
                {
                    ColourStorage.Singular.InPlaceExchangeIdentifier(OriginPathname, EndpointPathname);
                    EditorApplication.RepaintProjectWindow();
                }
            }

            return AssetMoveResult.DidNotMove;
        }
    }
}
