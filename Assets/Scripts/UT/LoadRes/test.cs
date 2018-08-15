using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class test : MonoBehaviour
{
    static readonly string RES_PATH1 = "patch/main.lua";    // ext = .txt
    static readonly string RES_PATH2 = "sample/a";          // ext = .txt | .prefab

    void Start()
    {
        // Resouces - Sync
        ResourcesLoad();

        // Resouces - Async
        StartCoroutine( ResourcesLoadAsync() );

        // AssetDatabase
        ADBLoad();

        // Assetbundle
        // ...
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
}
