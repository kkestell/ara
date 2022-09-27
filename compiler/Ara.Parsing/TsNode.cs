using System.Runtime.InteropServices;

namespace Ara.Parsing;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TsNode
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    readonly uint[] Context;

    readonly IntPtr Id;
    readonly IntPtr Tree;
}