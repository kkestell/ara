#region

using System.Runtime.InteropServices;

#endregion

namespace Ara.Parsing;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TsNode
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    private readonly uint[] Context;

    private readonly IntPtr Id;
    private readonly IntPtr Tree;
}