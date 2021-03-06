using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;

public class CreateUTMenuItem
{
    [MenuItem("Test/Build TestAB")]
    public static void BuildTestAB()
    {
        List<AssetBundleBuild> abbuild_list = new List<AssetBundleBuild>();
        AssetBundleBuild abbuild = new AssetBundleBuild();
        abbuild.assetBundleName = "test.ab";
        abbuild.assetNames = new string[]
        {
            "Assets/Resources/sample/a.prefab"
        };
        abbuild_list.Add(abbuild);

        BuildPipeline.BuildAssetBundles(PathUtil.GetABOutPath(), abbuild_list.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}
