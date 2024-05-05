using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace QModReloadedGUI
{
    internal class Loader : MarshalByRefObject
    {
        public void CheckVersion(FrmMain form, string file)
        {
            try
            {
                form.AssemblyLoaded = true;
                var gameAssembly = Assembly.LoadFrom(file);
                var versionType = gameAssembly.GetType("LazyConsts", true, true);
                var properties = versionType.GetProperties();
                var versionGetter = properties[0].GetGetMethod();
                var version = (float)versionGetter.Invoke(null, null);
                form.WriteLog($"Game version: v{version}");

                form.LblGameVersion.ForeColor = Color.Green;
                form.LblGameVersion.Text = $@"Game version: v{version}";
                
                if (version < 1.405f)
                {
                    form.LblGameVersion.ForeColor = Color.Red;
                    form.WriteLog("Game version is not supported! Please update to v1.405 or above. Support will not be provided for out of date versions!", true);
                    MessageBox.Show(@"Game version is not supported! Please update to v1.405 or above. Support will not be provided for out of date versions!", @"QMod Manager Reloaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception)
            {
                form.WriteLog($"Error obtaining the game's version. Please ensure its v1.405 or above.");
            }
        }

        public void Load(FrmMain form, string file)
        {
            form.ModDomainLoaded = true;
            var modAssembly = Assembly.LoadFrom(file);
            if (file.ToLowerInvariant().Contains("helper")) return;

            foreach (var type in modAssembly.GetTypes().Where(t => t.FullName == $"{t.Namespace}.Config"))
            {
                form.WriteLog($"Generating config for: {Path.GetFileName(file)}");
                var staticMethodInfo = type.GetMethod("GetOptions");
                if (staticMethodInfo == null)
                {
                    form.WriteLog(
                        $"Unable to find GetOptions method in {type.FullName}. Please run the game to generate a config or consult the author.",
                        true);
                    continue;
                }

                if (staticMethodInfo.GetParameters().Length == 1)
                {
                    form.WriteLog($"Invoking method to create config for: {Path.GetFileName(file)}");
                    staticMethodInfo.Invoke(null, new object[] {true});
                }
                else
                {
                    form.WriteLog(
                        $"GetOptions method in {type.FullName} doesn't have the correct amount of parameters. Please check for mod updates before contacting the author.",
                        true);
                }
            }
        }
    }
}