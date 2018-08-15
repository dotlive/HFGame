using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class test : MonoBehaviour
{
    enum CaseMask
    {
        ResourcesSync = 1 << 0,
        ResourcesAsync = 1 << 1,
        AssetDatabase = 1 << 2,
        AssetBundle = 1 << 3,
        All = 0XFF,
    }

    static readonly string RES_PATH1 = "patch/main.lua";    // ext = .txt
    static readonly string RES_PATH2 = "sample/a";          // ext = .txt | .prefab
    static readonly CaseMask mask = CaseMask.AssetBundle;

    void Start()
    {
        // Resouces - Sync
        if (BitAdd(CaseMask.ResourcesSync))
        {
            ResourcesLoad();
        }

        // Resouces - Async
        if (BitAdd(CaseMask.ResourcesAsync))
        {
            StartCoroutine(ResourcesLoadAsync());
        }

        // AssetDatabase
        if (BitAdd(CaseMask.AssetDatabase))
        {
            ADBLoad();
        }

        // Assetbundle
        if (BitAdd(CaseMask.AssetBundle))
        {
            StartCoroutine(AssetBundleLoad());
        }
    }

    bool BitAdd(CaseMask val)
    {
        return (val & mask) != 0;
    }

    void ResourcesLoad()
    {
        /*
         * 1. 路径为相对于"Resources"目录的路径 (不包括Resources目录);
         * 2. 路径中不能有扩展名;
         * 3. 同名文件, 加载的时候需要指定文件类型;
         */

        TextAsset res_1 = Resources.Load(RES_PATH1) as TextAsset;   // 这个版本不适合同名文件的加载 (RES_PATH2 load failed)
        if (null != res_1)
        {
            Debug.Log(res_1.text);
        }

        TextAsset res_2 = Resources.Load<TextAsset>(RES_PATH2) as TextAsset;
        if (null != res_2)
        {
            Debug.Log(res_2.text);
        }

        TextAsset res_3 = Resources.Load(RES_PATH2, typeof(TextAsset)) as TextAsset;
        if (null != res_3)
        {
            Debug.Log(3);
        }
    }

    IEnumerator ResourcesLoadAsync()
    {
        /*
         * 规则与同步方式一致;
         */
        ResourceRequest rr = Resources.LoadAsync<TextAsset>(RES_PATH2);
        yield return rr;
        if (null != rr)
        {
            Debug.Log( (rr.asset as TextAsset).text );
        }
    }

    void ADBLoad()
    {
        /*
         * 1. 路径为"Assets"目录的相对路径 (包括Assets目录);
         * 2. 路径中必须有扩展名;
         * 3. 同名文件, 加载的时候需要指定文件类型;
         */

#if UNITY_EDITOR
        TextAsset res_1 = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/UT/Resources/" + RES_PATH2 + ".txt") as TextAsset;
        if (null != res_1)
        {
            Debug.Log(res_1.text);
        }

        GameObject res_2 = AssetDatabase.LoadAssetAtPath("Assets/Scripts/UT/Resources/" + RES_PATH2 + ".prefab", typeof(GameObject)) as GameObject;
        if (null != res_2)
        {
            Debug.Log(5);
        }

        TextAsset res_3 = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Scripts/UT/Resources/" + RES_PATH1 + ".txt") as TextAsset;   // 不带扩展名会失败
        if (null != res_3)
        {
            Debug.Log(res_3.text);
        }
#endif
    }

    IEnumerator AssetBundleLoad()
    {
        string strWWWPath = PathUtil.GetWWWPath();
        strWWWPath += "/";
        strWWWPath += PathUtil.GetPlatformName();
        Debug.Log(strWWWPath);
        WWW www = new WWW(strWWWPath);
        yield return www;

        AssetBundle bundle = www.assetBundle;
        AssetBundleManifest main_ab = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        string[] sub_abs = main_ab.GetAllAssetBundles();
        foreach (string sub_ab in sub_abs)
        {
            strWWWPath = PathUtil.GetWWWPath();
            strWWWPath += "/";
            strWWWPath += sub_ab;
            Debug.Log(strWWWPath);
            WWW www2 = new WWW(strWWWPath);
            yield return www2;

            AssetBundle sub_bundle = www2.assetBundle;
            AssetBundleRequest abrequest = sub_bundle.LoadAssetAsync<GameObject>("Assets/Scripts/UT/Resources/sample/a.prefab");
            yield return abrequest;

            GameObject go = abrequest.asset as GameObject;
            if (null != go)
            {
                Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
            }

            www2.Dispose();
        }

        www.Dispose();
    }
}
