namespace Ara.CodeGen.IR;

public record Struct(string Name, IEnumerable<StructField> Fields);
