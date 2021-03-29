using System.Collections.Generic;
using UnityEditor;

public class BuildShader 
{

    [MenuItem("Assets/Build Shader")]
    static void XBuildShader()
    {
        var objs = Selection.objects;
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = "shader";
        List<string> names = new List<string>();
        foreach (var it in objs)
        {
            var pat = AssetDatabase.GetAssetPath(it);
            names.Add(pat);
        }

        build.assetNames = names.ToArray();
        builds.Add(build);

        var option = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
        BuildPipeline.BuildAssetBundles(BuildBundle.dir, builds.ToArray(), option, EditorUserBuildSettings.activeBuildTarget);
        EditorUtility.ClearProgressBar();
        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
