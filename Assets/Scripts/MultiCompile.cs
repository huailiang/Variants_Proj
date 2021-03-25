using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MultiCompile : MonoBehaviour
{
    private AssetBundleManifest manifest;

    private Dictionary<string,Object> hash = new Dictionary<string, Object>();

    private void Start()
    {
        Analy();
        LoadCube();
    }

    private void Analy()
    {
        var pat = Path.Combine(Application.streamingAssetsPath, "assets/assets");
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(pat);
        manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

        var assets = manifest.GetAllAssetBundles();
        int len = assets.Length;
        Debug.Log("assets count: " + len);
        for (int i = 0; i < len; i++)
            Debug.Log("it" + i + ": " + assets[i]);

        string[] depends = manifest.GetAllDependencies("cube");
        len = depends.Length;
        Debug.Log("depends: " + len);
        for (int i = 0; i < len; i++)
            Debug.Log(i + ": " + depends[i]);
    }

    private Object LoadDep(string p)
    {
        string[] depends = manifest.GetAllDependencies(p);
        for (int i = 0; i < depends.Length; i++)
            LoadDep(depends[i]);
        
        if (hash.ContainsKey(p))
            return hash[p];

        var pat = Path.Combine(Application.streamingAssetsPath, "assets/" + p);
        var b = AssetBundle.LoadFromFile(pat);
        Debug.Log("load: " + pat);
        hash.Add(p, b);
        return b.LoadAsset(p);
    }

    private void LoadCube()
    {
        var obj = LoadDep("cube");
        var go = GameObject.Instantiate(obj);
        go.name = "cube...";
    }
}