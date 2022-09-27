using System.Runtime.InteropServices;

namespace Ara.Parsing;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TsPoint
{
    public readonly Int32 Row;
    public readonly Int32 Column;
}