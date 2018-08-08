using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;
using HFGame;

[System.Serializable]
public class Variable
{
    public string name;
    public GameObject value;
}

[LuaCallCSharp]
[Hotfix]
public class XLuaBehaviour : MonoBehaviour
{
    public TextAsset luaScript;
    public Variable[] variables;

    internal static LuaEnv luaEnv = null;
    internal static float lastGCTime = 0;
    internal const float GCInterval = 1;//1 second

    private Action luaAwake;
    private Action luaStart;
    private Action luaUpdate;
    private Action luaOnDestroy;

    private LuaTable scriptEnv;

    void Awake()
    {
        XLuaManager.Instance().InitLuaEnv();
        luaEnv = XLuaManager.Instance().luaEnv;

        scriptEnv = luaEnv.NewTable();

        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        scriptEnv.Set("self", this);
        foreach (var injection in variables)
        {
            scriptEnv.Set(injection.name, injection.value);
        }

        luaEnv.DoString(luaScript.text, "XLuaBehaviour", scriptEnv);

        scriptEnv.Get("Awake", out luaAwake);
        scriptEnv.Get("Start", out luaStart);
        scriptEnv.Get("Update", out luaUpdate);
        scriptEnv.Get("OnDestroy", out luaOnDestroy);

        if (null != luaAwake)
        {
            luaAwake();
        }
    }

	void Start ()
    {
        if (null != luaStart)
        {
            luaStart();
        }
	}

	void Update ()
    {
        if (null != luaUpdate)
        {
            luaUpdate();
        }

        if (Time.time - XLuaBehaviour.lastGCTime > GCInterval)
        {
            luaEnv.Tick();
            XLuaBehaviour.lastGCTime = Time.time;
        }
	}

    void OnDestroy()
    {
        if (null != luaOnDestroy)
        {
            luaOnDestroy();
        }

        luaOnDestroy = null;
        luaUpdate = null;
        luaStart = null;
        luaAwake = null;
        scriptEnv.Dispose();
        variables = null;
        
        Show();
    }

    void Show()
    {
        Debug.Log("CSharp: Show");
    }
}
