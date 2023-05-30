#region

using System.Runtime.InteropServices;

#endregion

namespace Ara.Parsing;

[StructLayout(LayoutKind.Sequential)]
public readonly struct TsPoint
{
    public readonly Int32 Row;
    public readonly Int32 Column;
}