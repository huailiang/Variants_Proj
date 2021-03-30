using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildShader
{
    [MenuItem("Assets/Build Shader")]
    internal static void XBuildShader()
    {
        var objs = Selection.objects;
        List<string> names = new List<string>();
        foreach (var it in objs)
        {
            var pat = AssetDatabase.GetAssetPath(it);
            names.Add(pat);
        }

        InnerBuildShader(names.ToArray());
        BuildBundle.FinishEditor();
    }

    internal static void InnerBuildShader()
    {
        var shaderDir = "Assets/Shaders/";
        DirectoryInfo directoryInfo = new DirectoryInfo(shaderDir);
        var files = directoryInfo.GetFiles("*.shader*");
        List<string> names = new List<string>();
        foreach (var file in files)
        {
            Debug.Log(file.Name);
            names.Add(shaderDir + file.Name);
        }

        InnerBuildShader(names.ToArray());
    }

    internal static void InnerBuildShader(string[] names)
    {
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        AssetBundleBuild build = new AssetBundleBuild {assetBundleName = "shader", assetNames = names};
        builds.Add(build);
        var option = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle;
        BuildPipeline.BuildAssetBundles(BuildBundle.dir, builds.ToArray(), option,
            EditorUserBuildSettings.activeBuildTarget);
    }
}