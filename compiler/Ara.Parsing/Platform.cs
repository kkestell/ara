namespace Ara.Parsing;

public static class Platform
{
#if _WINDOWS
    public const string SharedLibrary = "Ara.Parsing.Windows-x86_64.dll";
#elif _DARWIN
    public const string SharedLibrary = "Ara.Parsing.Darwin-x86_64.dylib";
#elif _LINUX
    public const string SharedLibrary = "Ara.Parsing.Linux-x86_64.so";
#endif
}
