namespace Ara.CodeGen.Ir.IR;

public record Struct(string Name, IEnumerable<StructField> Fields);
