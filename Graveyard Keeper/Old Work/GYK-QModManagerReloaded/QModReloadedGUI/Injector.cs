using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace QModReloadedGUI
{
    public class Injector
    {
        private readonly string _mainFilename;

        public Injector(string gamePath)
        {
            _mainFilename = Path.Combine(gamePath, "Graveyard Keeper_Data\\Managed\\Assembly-CSharp.dll");
        }

        public (bool injected, string message) InjectNoIntros()
        {
            try
            {
                if (IsNoIntroInjected())
                    return (true,
                        "Intros patch already injected.");
                var gameAssembly = AssemblyDefinition.ReadAssembly(_mainFilename);

                var logoScene = gameAssembly.MainModule.GetType("LogoScene");
                var awakeMethod = logoScene.Methods.First(x => x.Name == "Awake");

                //where the original instruction comes from
                var onFinishedMethod = logoScene.Methods.First(x => x.Name == "OnFinished");

                awakeMethod.Body.Method.Body = onFinishedMethod.Body.Method.Body;
                gameAssembly.Write(_mainFilename);
                return (true,
                    $"Intros patch injected : {onFinishedMethod.Body.Instructions[0].Operand} inserted into {awakeMethod}");
            }
            catch (Exception ex)
            {
                return (false, $"Intros patch injected ERROR: {ex.Message}");
            }
        }

        public bool IsNoIntroInjected()
        {

            try
            {
                var gameAssembly = AssemblyDefinition.ReadAssembly(_mainFilename);
                var logoScene = gameAssembly.MainModule.GetType("LogoScene");

                //check for start preload instruction in awake method
                var awakeMethod = logoScene.Methods.First(x => x.Name == "Awake");

                return awakeMethod.Body.Instructions.Any(instruction =>
                    instruction.OpCode.Equals(OpCodes.Call) && instruction.Operand.ToString()
                        .Equals("System.Void UnityEngine.SceneManagement.SceneManager::LoadScene(System.String)",
                            StringComparison.Ordinal));
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"IsNoIntroInjected ERROR: {ex.Message}");
            }

            return false;


        }
    }
}