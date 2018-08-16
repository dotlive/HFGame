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

            // lua-protobuf
            _luaEnv.AddBuildin("pb", LuaAPI.open_pb);
            _luaEnv.AddBuildin("pb.conv", LuaAPI.open_pb_conv);
            _luaEnv.AddBuildin("pb.buffer", LuaAPI.open_pb_buffer);
            _luaEnv.AddBuildin("pb.slice", LuaAPI.open_pb_slice);
            _luaEnv.AddBuildin("pb.io", LuaAPI.open_pb_io);

#if HOTFIX_ENABLE
            // load patch
            _luaEnv.DoString("require 'patch.main'");
#endif

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