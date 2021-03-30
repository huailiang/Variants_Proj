using UnityEngine;
using System.IO;
using UnityEngine.Profiling;

public class VariantsTest : MonoBehaviour
{
    private AssetBundleManifest manifest;

    private string prefix
    {
        get { return Path.Combine(Application.streamingAssetsPath, "assets"); }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 200, 100), "LoadVariants - Join"))
        {
            Profiler.BeginSample("LoadVariants");
            LoadVariants();
            LoadCube("cubemulticompile", new Vector3(2, 0, 0));
            LoadCube("cubefeature", new Vector3(-2, 0, 0));
            Profiler.EndSample();
        }

        if (GUI.Button(new Rect(20, 140, 200, 100), "LoadVariants - Mat"))
        {
            Profiler.BeginSample("LoadVariants");
            LoadVariants();
            LoadMat("mat_shaderfeature");
            LoadMat("mat_multicompile");
            LoadCube("cubemulticompile", new Vector3(2, 0, 0));
            LoadCube("cubefeature", new Vector3(-2, 0, 0));
            Profiler.EndSample();
        }
    }

    private void LoadVariants()
    {
        var pat = Path.Combine(prefix, "shader");
        var b = AssetBundle.LoadFromFile(pat);
        var svc = b.LoadAsset<ShaderVariantCollection>("MultiShaderVariants");
        svc.WarmUp();
        // svc的WarmUp就会触发相关Shader的预编译，触发预编译之后再加载Shader Asset即可
        // var shaders = b.LoadAllAssets<Shader>();
        // foreach (var shader in shaders)
        // {
        //     Debug.Log("shader: " + shader.name);
        // }

        var mats = b.LoadAllAssets<Material>();
        foreach (var mat in mats)
        {
            Debug.Log("mat: " + mat.name);
        }

        // b.Unload(false);
    }

    private void LoadMat(string mat)
    {
        var pat = Path.Combine(prefix, mat);
        var b = AssetBundle.LoadFromFile(pat);
        b.LoadAllAssets<Material>();
    }


    private void LoadCube(string name, Vector3 pos)
    {
        var pat = Path.Combine(prefix, name);
        var b = AssetBundle.LoadFromFile(pat);
        var obj = b.LoadAsset<GameObject>(name);
        var go = GameObject.Instantiate(obj);
        go.name = name + "...";
        go.transform.position = pos;
        b.Unload(false);
    }
}