using UnityEngine;
using XLua;
using LuaAPI = XLua.LuaDLL.Lua;

namespace HFGame
{
    public class XLuaManager
    {
        public static XLuaManager Instance()
        {
            if (null == _instance)
            {
                _instance = new XLuaManager();
            }

            return _instance;
        }

        public bool InitLuaEnv()
        {
            luaEnv = new LuaEnv();
            if (null == luaEnv)
            {
                Debug.LogError("Create Lua environment failed.");
                return false;
            }

            _luaEnv.AddBuildin("pb", LuaAPI.LoadLuaProtobuf);

            return true;
        }
        
        LuaEnv _luaEnv = null;
        public LuaEnv luaEnv
        {
            get { return _luaEnv; }
            private set { _luaEnv = value; }
        }

        private static XLuaManager _instance = null;
    }
}