using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.StringComparison;

namespace QModReloaded;

public class QModLoader
{
    private static readonly string QModBaseDir = Path.Combine(Environment.CurrentDirectory, "QMods");
    private static readonly string ManagedDirectory = Path.Combine(Environment.CurrentDirectory, "Graveyard Keeper_Data", "Managed");
    private static readonly string DisableMods = Path.Combine(QModBaseDir, "disable");

    public static void Patch()
    {
        CleanAndCopyHelper();
        LoadHelper();
        Logger.WriteLog("Assembly-CSharp.dll has been patched, (otherwise you wouldn't see this message.");
        if (File.Exists(DisableMods))
        {
            Logger.WriteLog("Game has been launched via Load Modless. Disabling mods.");
            return;
        }
        Logger.WriteLog("Patch method called. Attempting to load mods.");
        var dllFiles =
            Directory.EnumerateDirectories(QModBaseDir).SelectMany(
                directory => Directory.EnumerateFiles(directory, "*.dll"));

        var mods = new List<QMod>();
       // var modLoaderDomain = AppDomain.CreateDomain("QModManagerReloadedDomain");
        foreach (var dllFile in dllFiles.Where(a=>!a.ToLowerInvariant().Contains("helper")))
        {
            if (dllFile.ToLowerInvariant().EndsWith("resource.dll")) continue;
            //modLoaderDomain.a
            var directoryName = new FileInfo(dllFile).DirectoryName;
            var jsonPath = Path.Combine(directoryName!, "mod.json");
            if (!new FileInfo(jsonPath).Exists)
            {
                var created = CreateJson(dllFile);
                if (created)
                {
                    Logger.WriteLog(
                        $"No mod.json found for {dllFile}. Have created one, attempting to load. Note if I couldn't automatically determine the entry method, a best guess is used.");
                    jsonPath = Path.Combine(directoryName, "mod.json");
                }
                else
                {
                    Logger.WriteLog(
                        $"No mod.json found for {dllFile}. Failed to create one automatically. Usually indicates an issue with the DLL.", true);
                    continue;
                }
            }

            var modToAdd = QMod.FromJsonFile(jsonPath);
            if (!modToAdd.Enable)
            {
                Logger.WriteLog($"{modToAdd.DisplayName} has been disabled in config. Skipping.");
                continue;
            }
            modToAdd.LoadedAssembly = Assembly.LoadFrom(dllFile);
            modToAdd.ModAssemblyPath = dllFile;
            mods.Add(modToAdd);
        }



        mods.Sort((m1, m2) => m1.LoadOrder.CompareTo(m2.LoadOrder));

        foreach (var mod in mods)
        {
            if (mod.Description.Contains("QModHelper"))
            {
                mod.Enable = true;
            }
            if (mod.Enable)
                LoadMod(mod);
            else
                Logger.WriteLog($"{mod.DisplayName} has been disabled in config. Skipping.");
        }
    }

    private static string FixVersion(string version)
    {
        var dotCount = version.Count(c => c == '.');
        var fixedVersion = dotCount switch
        {
            0 =>
                //i.e 1
                $"{version}.0.0.0",
            1 =>
                //ie 1.0
                $"{version}.0.0",
            2 =>
                //ie 1.2.3
                $"{version}.0",
            _ => "0.0.0.0"
        };

        return fixedVersion;
    }

    private const string HelperDll = "Helper.dll";

    private static void CleanAndCopyHelper()
    {

        var currentVersionPath = Path.Combine(QModBaseDir, HelperDll);
        var destDepFile = Path.Combine(ManagedDirectory, "dep", HelperDll);
        if (!File.Exists(currentVersionPath) && File.Exists(destDepFile))
        {
            Logger.WriteLog($"No Helper found in QMod directory. Copied backup copy.", false);
            File.Copy(destDepFile, currentVersionPath, true);
        }

        Dictionary<FileInfo, Version> helpers = new();
        helpers.Clear();

        foreach (var file in Directory.GetFiles(QModBaseDir, HelperDll, SearchOption.AllDirectories))
        {
            var fi = new FileInfo(file);
            var path = Path.GetDirectoryName(fi.FullName)!.ToLowerInvariant();
            if (path.EndsWith("qmods", InvariantCultureIgnoreCase)) continue;
            var ver = Version.Parse(FixVersion(FileVersionInfo.GetVersionInfo(file).FileVersion));
            helpers.Add(fi, ver);
        }

        foreach (var file in Directory.GetFiles(ManagedDirectory, HelperDll, SearchOption.AllDirectories))
        {
            var fi = new FileInfo(file);
            //var path = Path.GetDirectoryName(fi.FullName)!.ToLowerInvariant();
            //if (path.EndsWith("qmods", InvariantCultureIgnoreCase)) continue;
            var ver = Version.Parse(FixVersion(FileVersionInfo.GetVersionInfo(file).FileVersion));
            helpers.Add(fi, ver);
        }

        var helperList = helpers.ToList();
        helperList.Sort((pair1, pair2) => string.CompareOrdinal(pair1.Value.ToString(), pair2.Value.ToString()));

        var currentVersion = new Version();
        
        var currentVersionExists = File.Exists(currentVersionPath);
        if (currentVersionExists)
        {
            currentVersion = Version.Parse(FixVersion(FileVersionInfo.GetVersionInfo(currentVersionPath).FileVersion));
        }
        var newVersion = helperList[helperList.Count - 1].Value;

        var result = currentVersion.CompareTo(newVersion);
        if (result < 0)
        {
            try
            {
                var sourceFile = helperList[helperList.Count - 1].Key.FullName;
              
                if (!string.Equals(sourceFile, destDepFile, Ordinal))
                {
                    File.Copy(sourceFile, destDepFile, true);
                }
                File.Copy(sourceFile, currentVersionPath, true);

                Logger.WriteLog($"Updated QMod Helper to {newVersion}.", false);
            }
            catch (Exception)
            {
                Logger.WriteLog($"Issue updating QMod Helper to {newVersion}. Please update manually as one/all of the mods requires it to function.", true);
            }
        }

        foreach (var helper in helperList.Where(helper => !Path.GetDirectoryName(helper.Key.FullName)!
                     .EndsWith("dep", InvariantCultureIgnoreCase)))
        {
            File.Delete(helper.Key.FullName);
        }


    }

    private static bool CreateJson(string file)
    {
        var sFile = new FileInfo(file);
        var path = new FileInfo(file).DirectoryName;
        var fileNameWithoutExt = sFile.Name.Substring(0, sFile.Name.Length - 4);
        var (namesp, type, method, found) = GetModEntryPoint(file);

        var modInfo = FileVersionInfo.GetVersionInfo(path!);

        var newMod = new QMod
        {
            DisplayName = modInfo.ProductName,
            Enable = true,
            ModAssemblyPath = path,
            Description = modInfo.FileDescription,
            AssemblyName = AssemblyName.GetAssemblyName(sFile.FullName).Name,
            Author = modInfo.CompanyName,
            NexusId = ParseNexusId(modInfo.LegalTrademarks),
            Id = fileNameWithoutExt,
            EntryMethod = found ? $"{namesp}.{type}.{method}" : $"Couldn't find a PatchAll. Not a valid mod.",
            Version = modInfo.ProductVersion
        };
        newMod.SaveJson();
        var files = new FileInfo(Path.Combine(path, "mod.json"));
        return files.Exists;
    }

    private static int ParseNexusId(string nexusId)
    {
        var success = int.TryParse(nexusId, out var id);
        if (success)
        {
            return id;
        }
        return -1;
    }


    private static void LoadHelper()
    {
        try
        {
            var path = Path.Combine(Environment.CurrentDirectory, "QMods", "Helper.dll");
            var assembly = Assembly.LoadFile(path);
            var m = GetModEntryPoint(path);
            var methodToLoad = assembly.GetType(m.namesp + "." + m.type).GetMethod(m.method);
            methodToLoad?.Invoke(m, Array.Empty<object>());
            Logger.WriteLog("Successfully invoked QMod Helper entry method.");
        }
        catch (FileNotFoundException ex)
        {
            Logger.WriteLog($"Could not find QMod Helper. {ex.Message}");
        }
        catch (Exception ex)
        {
            Logger.WriteLog($"Error invoking QMod Helper. {ex.Message}");
        }
    }

    private static (string namesp, string type, string method, bool found) GetModEntryPoint(string mod)
    {
        try
        {
            var modAssembly = AssemblyDefinition.ReadAssembly(mod);

            var toInspect = modAssembly.MainModule
                .GetTypes()
                .SelectMany(t => t.Methods
                    .Where(m => m.HasBody)
                    .Select(m => new { t, m }));

            // toInspect = toInspect.Where(x => x.m.Name is "Patch");

            foreach (var method in toInspect)
                if (method.m.Body.Instructions.Where(instruction => instruction.Operand != null)
                    .Any(instruction => instruction.Operand.ToString().Contains("PatchAll")))
                    return (method.t.Namespace, method.t.Name, method.m.Name, true);
        }
        catch (Exception ex)
        {
            Logger.WriteLog($"GetModEntryPoint(): Error, {ex.Message}", true);
        }

        return (null, null, null, false);
    }

    private static bool IsModCompatible(string mod)
    {
        try
        {
            var modAssembly = AssemblyDefinition.ReadAssembly(mod);

            var toInspect = modAssembly.MainModule
                .GetTypes()
                .SelectMany(t => t.Methods
                    .Where(m => m.HasBody)
                    .Select(m => new { t, m }));

            // toInspect = toInspect.Where(x => x.m.Name is "Patch");

            if (toInspect.Any(method => method.m.Body.Instructions.Where(instruction => instruction.Operand != null)
                    .Any(instruction => instruction.OpCode == OpCodes.Newobj && instruction.Operand.ToString().Contains("HarmonyLib.Harmony"))))
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Logger.WriteLog($"GetModEntryPoint(): Error, {ex.Message}", true);
        }

        return false;
    }

    private static void LoadMod(QMod mod)
    {
        try
        {
            MethodInfo methodToLoad;
            var jsonEntrySplit = mod.EntryMethod.Split('.');
            var m = GetModEntryPoint(mod.ModAssemblyPath);
            if (!IsModCompatible(mod.ModAssemblyPath))
            {
                Logger.WriteLog($"{mod.Id} is not Harmony2 enabled, and as such, is not compatible.", true);
                return;
            }
            var jsonEntry = $"{jsonEntrySplit[0]}.{jsonEntrySplit[1]}.{jsonEntrySplit[2]}";
            var foundEntry = $"{m.namesp}.{m.type}.{m.method}";

            if (!jsonEntry.Equals(foundEntry, Ordinal))
            {
                Logger.WriteLog(
                    $"Found entry point in {mod.AssemblyName} does not match what's in the JSON. Ignoring JSON and loading found entry method.");
                methodToLoad = mod.LoadedAssembly.GetType(m.namesp + "." + m.type).GetMethod(m.method);
            }
            else
            {
                methodToLoad = mod.LoadedAssembly.GetType($"{jsonEntrySplit[0]}.{jsonEntrySplit[1]}")
                    .GetMethod(jsonEntrySplit[2]);
            }

            methodToLoad?.Invoke(m, Array.Empty<object>());
            Logger.WriteLog($"Load order: {mod.LoadOrder}, successfully invoked {mod.DisplayName} entry method.");
        }
        catch (TargetInvocationException)
        {
            Logger.WriteLog($"Invoking the specified EntryMethod {mod.EntryMethod} failed for {mod.Id}. Is the mod Harmony2.0 compatible?", true);
        }
        catch (NullReferenceException nullEx)
        {
            Logger.WriteLog(nullEx.Message, true);
        }
        catch (Exception finalEx)
        {
            Logger.WriteLog($"LoadMod():{finalEx.Message}, Source: {finalEx.Source}, Trace: {finalEx.StackTrace}", true);
        }
    }
}