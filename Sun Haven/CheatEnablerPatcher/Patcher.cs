// ReSharper disable once UnusedType.Global
namespace CheatEnablerPatcher;

public static class Patcher
{
    // List of assemblies to patch
    // ReSharper disable once UnusedMember.Global
    public static IEnumerable<string> TargetDLLs { get; } = new[] { "SunHaven.Core.dll" };

    // Patches the assemblies
    // ReSharper disable once UnusedMember.Global
    public static void Patch(AssemblyDefinition assembly)
    {
        var qcAssemblyPath = Path.Combine(Paths.ManagedPath, "QFSW.QC.dll");
        var qcAssembly = AssemblyDefinition.ReadAssembly(qcAssemblyPath);
      
        const string className = "Wish.QuantumConsoleManager";
        var targetType = assembly.MainModule.Types.FirstOrDefault(t => t.FullName == className);

        if (targetType == null)
        {
            Trace.TraceError($"Type {className} not found.");
            return;
        }
        
        var descriptionAttributeType = qcAssembly.MainModule.Types.FirstOrDefault(t => t.Name == "CommandDescriptionAttribute");

        if (descriptionAttributeType == null)
        {
            Trace.TraceError("CommandDescriptionAttribute type not found in QFSW.QC.dll.");
            return;
        }

        var descriptionConstructor = descriptionAttributeType.Methods.First(m => m.IsConstructor && m.Parameters.Count == 1 && m.Parameters[0].ParameterType.FullName == "System.String");

        foreach (var method in targetType.Methods)
        {
            var commandAttribute = method.CustomAttributes.FirstOrDefault(attr => attr.AttributeType.Name == "CommandAttribute");
            if (commandAttribute == null) continue;
            
            var alias = (string)commandAttribute.ConstructorArguments[0].Value;
            
            var key = alias;
            switch (method.Parameters.Count)
            {
                case 1:
                    key += "_single";
                    break;
                case > 1:
                    key += "_multiple";
                    break;
            }
            
            if (!CommandDescriptions.Commands.TryGetValue(key, out var description))
            {
                CommandDescriptions.Commands.TryGetValue(alias, out description);
            }

            if (description != null)
            {
                InjectCommandDescriptionAttribute(assembly.MainModule, method, descriptionConstructor, description);
            }
            else
            {
                Trace.TraceError($"No description found for command: {alias}");
            }
        }
        Trace.TraceInformation("Finished injecting descriptions to the built-in cheat commands.");
    }

    private static void InjectCommandDescriptionAttribute(ModuleDefinition module, MethodDefinition method, MethodReference descriptionConstructor, string description)
    {
        var customAttribute = new CustomAttribute(module.ImportReference(descriptionConstructor));
        customAttribute.ConstructorArguments.Add(new CustomAttributeArgument(module.ImportReference(typeof(string)), description));

        method.CustomAttributes.Add(customAttribute);
    }
}