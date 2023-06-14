#region

using Ara.CodeGen.Ir.IR.Types;

#endregion

namespace Ara.CodeGen.Ir.IR;

public record FunctionDeclaration(string Name, IrType ReturnType, List<IrType> ParameterTypes);

public record ExternalFunctionDeclaration(string Name, IrType ReturnType, List<IrType> ParameterTypes);