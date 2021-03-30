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
        if (GUI.Button(new Rect(20, 20, 200, 100), "LoadVariants"))
        {
            Profiler.BeginSample("LoadVariants");
            LoadVariants();
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
        var shaders = b.LoadAllAssets<Shader>();
        foreach (var shader in shaders)
        {
            Debug.Log("shader: " + shader.name);
        }
        b.Unload(false);
        LoadCube();
    }


    private void LoadCube()
    {
        var pat = Path.Combine(prefix, "mat_multicompile" );
        var b = AssetBundle.LoadFromFile(pat);
        b.LoadAsset("mat_multicompile");
        pat = Path.Combine(prefix, "cubemulticompile" );
        b.Unload(false);
        b  = AssetBundle.LoadFromFile(pat);
        var obj = b.LoadAsset<GameObject>("cubemulticompile");
        var go = GameObject.Instantiate(obj);
        go.name = "cubemulticompile...";
        b.Unload(false);
    }
}