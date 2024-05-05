using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpCodes = Mono.Cecil.Cil.OpCodes;

namespace QModReloadedGUI;

public partial class FrmResModifier : Form
{
    private static string _gameLocation;
    private static ILProcessor _prc;
    private static AssemblyDefinition _resAssembly;
    private readonly DataGridView _dgvLog;

    public FrmResModifier(ref DataGridView dgvLog, string gameLocation)
    {
        _dgvLog = dgvLog;
        _gameLocation = gameLocation;
        InitializeComponent();
    }

    private void BtnApply_Click(object sender, EventArgs e)
    {
        var isHeightNumber = int.TryParse(TxtRequestedMaxHeight.Text, out _);
        var isWidthNumber = int.TryParse(TxtRequestedMaxWidth.Text, out _);
        if (isHeightNumber && isWidthNumber)
        {
            UpdateResolutions();
            GetCurrentResolutions();
        }
        else
        {
            MessageBox.Show(@"Enter integers (whole numbers, no decimals) only.", @"What is that?", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void GetCurrentResolutions()
    {
        try
        {
            if (TxtMaxHeight.Text.Length <= 0)
            {
                WriteLog("[RESOLUTION]: Obtained current max in-game resolution.");
            }
            var currentHeight = Convert.ToInt32(_prc?.Body.Instructions[19].Operand.ToString());
            var currentWidth = Convert.ToInt32(_prc?.Body.Instructions[22].Operand.ToString());
            TxtMaxHeight.Text = currentHeight.ToString();
            TxtMaxWidth.Text = currentWidth.ToString();
        }
        catch (Exception ex)
        {
            WriteLog($"[RESOLUTION]: Message: {ex.Message}, Source: {ex.Source}, Trace: {ex.StackTrace}", true);
        }
    }

    private void ResModifier_Load(object sender, EventArgs e)
    {
        try
        {
            _resAssembly = AssemblyDefinition.ReadAssembly(Path.Combine(_gameLocation,
                "Graveyard Keeper_Data\\Managed\\Assembly-CSharp.dll"));
            var toInspect = _resAssembly.MainModule
                .GetTypes()
                .SelectMany(t => t.Methods
                    .Where(m => m.HasBody)
                    .Select(m => new { t, m }));

            toInspect = toInspect.Where(x =>
                x.t.Name.EndsWith("ResolutionConfig") && x.m.Name == "GetResolutionConfigOrNull");
            _prc = toInspect.FirstOrDefault()?.m.Body.GetILProcessor();
            GetCurrentResolutions();
        }
        catch (Exception ex)
        {
            WriteLog($"[RESOLUTION]: Message: {ex.Message}, Source: {ex.Source}, Trace: {ex.StackTrace}", true);
        }
    }

    private void UpdateResolutions()
    {
        try
        {
            var newHeight = _prc?.Create(OpCodes.Ldc_I4, int.Parse(TxtRequestedMaxHeight.Text));
            var newWidth = _prc?.Create(OpCodes.Ldc_I4, int.Parse(TxtRequestedMaxWidth.Text));

            _prc?.Replace(_prc?.Body.Instructions[19], newHeight);
            _prc?.Replace(_prc?.Body.Instructions[22], newWidth);

            _resAssembly.Write(Path.Combine(_gameLocation, "Graveyard Keeper_Data\\Managed\\Assembly-CSharp.dll"));
            WriteLog($"[RESOLUTION]: Successfully patched in {TxtRequestedMaxWidth.Text} x {TxtRequestedMaxHeight.Text}.");
            MessageBox.Show(@"Successfully patched in new resolution. I think.", @"Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            WriteLog($"[RESOLUTION]: Message: {ex.Message}, Source: {ex.Source}, Trace: {ex.StackTrace}", true);
        }
    }

    private void WriteLog(string message, bool error = false)
    {
        var dt = DateTime.Now;
        var rowIndex = _dgvLog.Rows.Add(dt.ToLongTimeString(), message);
        var row = _dgvLog.Rows[rowIndex];
        if (error)
        {
            row.DefaultCellStyle.BackColor = Color.LightCoral;
        }

        string logMessage;
        if (error)
        {
            logMessage = "-----------------------------------------\n";
            logMessage += dt.ToShortDateString() + " " + dt.ToLongTimeString() + " : [RESOLUTION][ERROR] : " + message + "\n";
            logMessage += "-----------------------------------------";
        }
        else
        {
            logMessage = dt.ToShortDateString() + " " + dt.ToLongTimeString() + " [RESOLUTION] : " + message;
        }
        _dgvLog.FirstDisplayedScrollingRowIndex = _dgvLog.RowCount - 1;
        Utilities.WriteLog(logMessage, _gameLocation);
    }
}