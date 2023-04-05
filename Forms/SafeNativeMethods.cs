using System.Runtime.InteropServices;
using System.Security;

[assembly: AllowPartiallyTrustedCallers]
namespace Com.Nakasendo.Gakupetit.Forms;

// カーソル物理座標取得用
internal struct CursorPoint
{
    internal int PosX;
    internal int PosY;
}
// ネイティブメソッドを分離
static partial class SafeNativeMethods
{
    [SecurityCritical]
    [LibraryImport("user32.dll", EntryPoint = "GetPhysicalCursorPos")]
    [return: MarshalAs(UnmanagedType.U1)]
    private static partial bool GetPhysicalCursorPosPinvoke(ref CursorPoint lpPoint); // Security Critical P/Invoke

    [SecuritySafeCritical]
    internal static bool GetPhysicalCursorPos(ref CursorPoint lpPoint)
    {
        return GetPhysicalCursorPosPinvoke(ref lpPoint);
    }
}
