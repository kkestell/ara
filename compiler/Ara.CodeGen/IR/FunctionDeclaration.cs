#region

using Ara.CodeGen.IR.Types;

#endregion

namespace Ara.CodeGen.IR;

public record FunctionDeclaration(string Name, IrType ReturnType, List<IrType> ParameterTypes);

public record ExternalFunctionDeclaration(string Name, IrType ReturnType, List<IrType> ParameterTypes);