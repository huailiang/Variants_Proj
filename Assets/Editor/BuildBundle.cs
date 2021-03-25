using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class BuildBundle
{

    internal const string dir = "Assets/StreamingAssets/assets";

    [MenuItem("Assets/Build Bundle")]
    static void XBuildBundle()
    {
        CleanOld();

        var objs = Selection.objects;
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        foreach (var it in objs)
        {
            AssetBundleBuild build = new AssetBundleBuild();
            var pat = AssetDatabase.GetAssetPath(it);
            build.assetBundleName = it.name;
            build.assetNames = new string[] { pat };
            builds.Add(build);
        }

        var option = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
        BuildPipeline.BuildAssetBundles(dir, builds.ToArray(), option, EditorUserBuildSettings.activeBuildTarget);
        EditorUtility.ClearProgressBar();
        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void CleanOld()
    {
        if (Directory.Exists(dir))
            Directory.Delete(dir, true);
        Directory.CreateDirectory(dir);
        AssetDatabase.ImportAsset(dir);
    }

}
