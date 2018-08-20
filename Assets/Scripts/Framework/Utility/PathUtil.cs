using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class PathUtil
{
    /* Assetbundle输入目录 */
    public const string AssetBundlesInputDir = "";
    /* Assetbundle输出目录 */
    public const string AssetBundlesOutputDir = "AssetBundles";

    /// <summary>
    /// 获取WWW协议路径
    /// </summary>
    /// <returns></returns>
    public static string GetWWWPath()
    {
        if (Application.isMobilePlatform)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "jar:file://" + GetABOutPath();
                default:
                    return null;
            }
        }
        else
        {
            return "file:///" + GetABOutPath();
        }
    }

    /// <summary>
    /// 获取AB资源（输入目录）
    /// </summary>
    /// <returns></returns>
    public static string GetABResourcesPath()
    {
        return Application.dataPath + "/" + AssetBundlesInputDir;
    }

    /// <summary>
    /// 获取AB输出路径
    /// 说明：
    ///     由两部分构成
    ///     1： 平台(PC/移动端)路径
    ///     2： 平台(PC/移动端)名称
    /// </summary>
    /// <returns></returns>
    public static string GetABOutPath()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(GetPlatformPath());
        sb.Append("/");
        sb.Append(AssetBundlesOutputDir);
        sb.Append("/");
        sb.Append(GetPlatformName());
        return sb.ToString();
    }

    /// <summary>
    /// 获取平台路径
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformPath()
    {
        if (Application.isMobilePlatform)
        {
            return Application.persistentDataPath;
        }
        else
        {
            return Application.streamingAssetsPath;
        }
    }

    /// <summary>
    /// 获取平台名称
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformName()
    {
#if UNITY_EDITOR
        return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
        return GetPlatformForAssetBundles(Application.platform);
#endif
    }

#if UNITY_EDITOR
    private static string GetPlatformForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "OSX";
            case BuildTarget.tvOS:
                return "tvOS";
            default:
                return null;
        }   
    }
#else
    private static string GetPlatformForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                return "OSX";
            case RuntimePlatform.tvOS:
                return "tvOS";
            default:
                return null;
        }
    }
#endif
}
