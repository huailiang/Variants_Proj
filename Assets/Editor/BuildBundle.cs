using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class BuildBundle
{

    internal const string dir = "Assets/StreamingAssets/assets";
    
    static void XBuildBundle(bool mat)
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
        builds.Add(BuildShader.InnerBuildShader(mat));
        var option = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
        BuildPipeline.BuildAssetBundles(dir, builds.ToArray(), option, EditorUserBuildSettings.activeBuildTarget);
        
        FinishEditor();
    }

    [MenuItem("Assets/Build Bundle - Mat Join")]
    static void XBuildBundle1()
    {
        XBuildBundle(true);
    }
    
    [MenuItem("Assets/Build Bundle - Mat Sep")]
    static void XBuildBundle2()
    {
        XBuildBundle(false);
    }

    private static void CleanOld()
    {
        if (Directory.Exists(dir))
            Directory.Delete(dir, true);
        Directory.CreateDirectory(dir);
        AssetDatabase.ImportAsset(dir);
    }
    
    internal static void FinishEditor()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
