using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Inject;
using System;
using System.IO;
using System.Linq;

namespace QModReloaded;

public class Injector
{
    private const string InjectorFile = "QModReloaded.dll";
    private readonly string _loader;

    public Injector(string gamePath)
    {
        _loader = Path.Combine(gamePath, "Graveyard Keeper_Data\\Managed\\Assembly-CSharp.dll");
    }

    public (bool injected, string message) Inject()
    {
        try
        {
            if (IsInjected()) return (true, "Mod patch already applied.");

            var gameAssembly = AssemblyDefinition.ReadAssembly(_loader);
            var injectorAssembly = AssemblyDefinition.ReadAssembly(InjectorFile);
            var patchInstruction = injectorAssembly.MainModule.GetType("QModReloaded.QModLoader").Methods
                .Single(x => x.Name == "Patch");
            var logoScene = gameAssembly.MainModule.GetType("LogoScene");
            foreach (var i in logoScene.Methods)
            {
                Console.WriteLine($"Method: {i.Name}");
            }
            var awakeMethod = logoScene.Methods.First(x => x.Name == "Awake");
            var injectionDefinition = new InjectionDefinition(awakeMethod, patchInstruction, InjectFlags.None, null, null);
            injectionDefinition.Inject(awakeMethod.Body.Instructions[0], direction: InjectDirection.Before);
            gameAssembly.Write(_loader);
            return (true, $"Mod patch injected: {patchInstruction} inserted into {awakeMethod}");
        }
        catch (Exception ex)
        {
            return (false, $"Mod patch inject ERROR: {ex.Message}");
        }
    }

    public (bool injected, string message) InjectNoIntros()
    {
        try
        {
            if (IsNoIntroInjected())
                return (true,
                    "Intros patch already injected.");
            var gameAssembly = AssemblyDefinition.ReadAssembly(_loader);

            var logoScene = gameAssembly.MainModule.GetType("LogoScene");
            var awakeMethod = logoScene.Methods.First(x => x.Name == "Awake");

            //where the original instruction comes from
            var onFinishedMethod = logoScene.Methods.First(x => x.Name == "OnFinished");

            awakeMethod.Body.Method.Body = onFinishedMethod.Body.Method.Body;
            gameAssembly.Write(_loader);
            return (true,
                $"Intros patch injected : {onFinishedMethod.Body.Instructions[0].Operand} inserted into {awakeMethod}");
        }
        catch (Exception ex)
        {
            return (false, $"Intros patch injected ERROR: {ex.Message}");
        }
    }

    public bool IsInjected()
    {
        try
        {
            var gameAssembly = AssemblyDefinition.ReadAssembly(_loader);
            var logoScene = gameAssembly.MainModule.GetType("LogoScene");
            var awakeMethod = logoScene.Methods.First(x => x.Name == "Awake");
            return awakeMethod.Body.Instructions.Any(instruction =>
                instruction.OpCode.Equals(OpCodes.Call) && instruction.Operand.ToString()
                    .Equals("System.Void QModReloaded.QModLoader::Patch()"));
        }
        catch (Exception ex)
        {
            Logger.WriteLog($"IsInjected ERROR: {ex.Message}");
        }

        return false;
    }

    public bool IsNoIntroInjected()
    {
        try
        {
            var gameAssembly = AssemblyDefinition.ReadAssembly(_loader);
            var logoScene = gameAssembly.MainModule.GetType("LogoScene");

            //check for start preload instruction in awake method
            var awakeMethod = logoScene.Methods.First(x => x.Name == "Awake");

            var found = awakeMethod.Body.Instructions.Any(instruction =>
                instruction.OpCode.Equals(OpCodes.Call) && instruction.Operand.ToString()
                    .Equals("System.Void UnityEngine.SceneManagement.SceneManager::LoadScene(System.String)"));
            return found;
        }
        catch (Exception ex)
        {
            Logger.WriteLog($"IsNoIntroInjected ERROR: {ex.Message}");
        }

        return false;
    }

    public string Remove()
    {
        try
        {
            if (!IsInjected()) return ("Mod patch already removed.");
            var gameAssembly = AssemblyDefinition.ReadAssembly(_loader);
            var logoScene = gameAssembly.MainModule.GetType("LogoScene");
            var awakeMethod = logoScene.Methods.First(x => x.Name == "Awake");
            var processor = awakeMethod.Body.GetILProcessor();
            var logText = awakeMethod.Body.Instructions[0].Operand.ToString();
            processor.Remove(awakeMethod.Body.Instructions[0]);
            gameAssembly.Write(_loader);
            return ($"Mod patch removed: {logText} removed from {awakeMethod}");
        }
        catch (Exception ex)
        {
            return ($"Mod patch remove ERROR: {ex.Message}");
        }
    }
}