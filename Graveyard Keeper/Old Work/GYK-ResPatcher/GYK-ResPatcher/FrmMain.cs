using Mono.Cecil;
using Mono.Cecil.Cil;

namespace GYKResPatcher
{
    public partial class FrmMain : Form
    {
        private static AssemblyDefinition? _resAssembly;
        private static ILProcessor? _prc;
        private const string AssemblyFile = "Assembly-CSharp.dll";

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                _resAssembly = AssemblyDefinition.ReadAssembly(AssemblyFile);
                var toInspect = _resAssembly.MainModule
                    .GetTypes()
                    .SelectMany(t => t.Methods
                        .Where(m => m.HasBody)
                        .Select(m => new {t, m}));

                toInspect = toInspect.Where(x =>
                    x.t.Name.EndsWith("ResolutionConfig", StringComparison.Ordinal) && x.m.Name == "GetResolutionConfigOrNull");
                _prc = toInspect.FirstOrDefault()?.m.Body.GetILProcessor();
                GetCurrentResolutions();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($@"Make sure this patcher file is in your ..\steamapps\common\Graveyard Keeper\Graveyard Keeper_Data\Managed directory!", $@"Doh!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error: {ex.Message}", $@"{ex.Source}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateResolutions()
        {
            try
            {
                var newHeight = _prc?.Create(OpCodes.Ldc_I4, int.Parse(TxtRH.Text));
                var newWidth = _prc?.Create(OpCodes.Ldc_I4, int.Parse(TxtRW.Text));

                _prc?.Replace(_prc?.Body.Instructions[19], newHeight);
                _prc?.Replace(_prc?.Body.Instructions[22], newWidth);

                _resAssembly?.Write(AssemblyFile);
                MessageBox.Show($@"{TxtRW.Text}x{TxtRH.Text} successfully patched!", @"Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetCurrentResolutions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error: {ex.Message}", $@"{ex.Source}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetCurrentResolutions()
        {
            try
            {
                var currentHeight = Convert.ToInt32(_prc?.Body.Instructions[19].Operand.ToString());
                var currentWidth = Convert.ToInt32(_prc?.Body.Instructions[22].Operand.ToString());
                TxtCH.Text = currentHeight.ToString();
                TxtCW.Text = currentWidth.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error: {ex.Message}", $@"{ex.Source}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnApplyResPatch_Click(object sender, EventArgs e)
        {
            UpdateResolutions();
        }

        public static void InjectNoIntros()
        {
            try
            {
                var gameAssembly = AssemblyDefinition.ReadAssembly(AssemblyFile);

                var logoScene = gameAssembly.MainModule.GetType("LogoScene");
                var awakeMethod = logoScene.Methods.First(x => x.Name == "Awake");

                //where the original instruction comes from
                var onFinishedMethod = logoScene.Methods.First(x => x.Name == "OnFinished");

                awakeMethod.Body.Method.Body = onFinishedMethod.Body.Method.Body;
                gameAssembly.Write(AssemblyFile);
                
                MessageBox.Show(@$"Intros patch injected : {onFinishedMethod.Body.Instructions[0].Operand} inserted into {awakeMethod}", @"Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error: {ex.Message}", $@"{ex.Source}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool IsNoIntroInjected()
        {
            try
            {
                var gameAssembly = AssemblyDefinition.ReadAssembly(AssemblyFile);
                var logoScene = gameAssembly.MainModule.GetType("LogoScene");

                //check for start preload instruction in awake method
                var awakeMethod = logoScene.Methods.First(x => x.Name == "Awake");

                return awakeMethod.Body.Instructions.Any(instruction =>
                    instruction.OpCode.Equals(OpCodes.Call) && instruction.Operand.ToString()!
                        .Equals("System.Void UnityEngine.SceneManagement.SceneManager::LoadScene(System.String)",
                            StringComparison.Ordinal));
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Error: {ex.Message}", $@"{ex.Source}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void BtnPatchIntros_Click(object sender, EventArgs e)
        {
            if (!IsNoIntroInjected())
            {
                InjectNoIntros();
            }
            else
            {
                MessageBox.Show("Intros already patched!");
            }
        }
    }
}