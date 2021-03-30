using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MultiCompile : MonoBehaviour
{
    private AssetBundleManifest manifest;

    private readonly Dictionary<string,Object> hash = new Dictionary<string, Object>();

    private void Start()
    {
        Analy();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(20,20,200,100),"Cube"))
        {
            LoadCube();
        }
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
        Vector3 pos = Vector3.zero;
        var obj = LoadDep("cubefeature");
        var go = GameObject.Instantiate(obj);
        go.name = "cube feature...";
        pos.x = -2;
        SetPos(go, pos);

        obj = LoadDep("cubemulticompile");
        go = GameObject.Instantiate(obj);
        go.name = "cube multi compile...";
        pos.x = 2;
        SetPos(go, pos);
    }

    private void SetPos(Object obj, Vector3 pos)
    {
        GameObject go = obj as GameObject;
        go.transform.position = pos;
    }
}