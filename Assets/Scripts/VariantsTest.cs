using UnityEngine;
using System.IO;
using UnityEngine.Rendering;

public class VariantsTest : MonoBehaviour
{
    private AssetBundleManifest manifest;

    private void Start()
    {
        Analy();
        LoadCube();
    }

    private void Analy()
    {
        Debug.Log("hello world");
        var pat = Path.Combine(Application.streamingAssetsPath, "assets/assets");
        AssetBundle manifestBundle = AssetBundle.LoadFromFile(pat);
        manifest = (AssetBundleManifest) manifestBundle.LoadAsset("AssetBundleManifest");

        var assets = manifest.GetAllAssetBundles();
        int len = assets.Length;
        Debug.Log("assets count: " + len);
        for (int i = 0; i < len; i++)
            Debug.Log("it" + i + ": " + assets[i]);
    }


    private void LoadCube()
    {
        var pat = Path.Combine(Application.streamingAssetsPath, "assets/shader");
        var b = AssetBundle.LoadFromFile(pat);

        var svc = b.LoadAsset<ShaderVariantCollection>("MultiShaderVariants");
        svc.WarmUp();
        Debug.Log("svc: " + svc.name);
        var shader = b.LoadAsset<Shader>("MultiCompile");
        Debug.Log("shader: " + shader.name);

        string[] keywords = new[] {"DB_ON _CL_R", "DB_ON _CL_G"};
        svc.Add(new ShaderVariantCollection.ShaderVariant(shader, PassType.Vertex, keywords));
    }
}