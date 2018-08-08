namespace XLua.LuaDLL
{
    using System.Runtime.InteropServices;

    public partial class Lua
    {
        #region lua-protobuf
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb(System.IntPtr L);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb_conv(System.IntPtr L);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb_buffer(System.IntPtr L);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb_slice(System.IntPtr L);

        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_pb_io(System.IntPtr L);
        #endregion

        #region MonoPInvokeCallback
        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int open_pb(System.IntPtr L)
        {
            return luaopen_pb(L);
        }

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int open_pb_conv(System.IntPtr L)
        {
            return luaopen_pb_conv(L);
        }

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int open_pb_buffer(System.IntPtr L)
        {
            return luaopen_pb_buffer(L);
        }

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int open_pb_slice(System.IntPtr L)
        {
            return luaopen_pb_slice(L);
        }

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int open_pb_io(System.IntPtr L)
        {
            return luaopen_pb_io(L);
        }
        #endregion
    }
}