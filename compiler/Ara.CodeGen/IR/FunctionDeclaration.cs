using Ara.CodeGen.IR.Types;

namespace Ara.CodeGen.IR;

public record FunctionDeclaration(string Name, IrType ReturnType, List<IrType> ParameterTypes);