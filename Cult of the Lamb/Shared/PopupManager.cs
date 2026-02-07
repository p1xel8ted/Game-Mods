using System;
using UnityEngine;

/// <summary>
/// IMGUI popup manager supporting informational and confirmation dialogs.
/// Attach to a DontDestroyOnLoad GameObject. Call Show() or ShowConfirmation() to display.
/// </summary>
internal class PopupManager : MonoBehaviour
{
    internal bool ShowPopup;

    /// <summary>
    /// Default title shown when no title is passed to ShowPopupDlg.
    /// Set this to your plugin name during Awake().
    /// </summary>
    internal string Title { get; set; } = "";

    private string _title = "";
    private string _popupMessage = "";
    private Rect _popupRect = new(Screen.width / 2f - 250, Screen.height / 2f - 100, 500, 300);
    private GUIStyle _messageStyle;
    private GUIStyle _windowStyle;
    private GUIStyle _titleStyle;
    private bool _dontShowAgainThisSession;
    private bool _ignoreDontShow;
    private bool _isConfirmation;
    private Action _onConfirm;

    private void Start()
    {
        _titleStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true,
            fontSize = 22,
            normal = { textColor = Color.black }
        };

        _messageStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true,
            fontSize = 20,
            normal = { textColor = Color.black }
        };

        _windowStyle = new GUIStyle
        {
            normal = { background = Texture2D.whiteTexture, textColor = Color.black },
            richText = true,
            padding = new RectOffset(10, 10, 10, 10),
        };
    }

    private void OnGUI()
    {
        if (ShowPopup)
        {
            _popupRect = GUI.ModalWindow(0, _popupRect, DrawPopup, string.Empty, _windowStyle);
        }
    }

    private void DrawPopup(int windowID)
    {
        GUILayout.FlexibleSpace();
        GUI.Label(new Rect(10, 10, _popupRect.width - 20, 30), _title, _titleStyle);
        GUILayout.FlexibleSpace();
        GUILayout.Label(_popupMessage, _messageStyle);
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (_isConfirmation)
        {
            if (GUILayout.Button("Confirm", GUILayout.Width(120), GUILayout.Height(40)))
            {
                ShowPopup = false;
                _onConfirm?.Invoke();
                _onConfirm = null;
            }
            if (GUILayout.Button("Cancel", GUILayout.Width(120), GUILayout.Height(40)))
            {
                ShowPopup = false;
                _onConfirm = null;
            }
        }
        else
        {
            if (GUILayout.Button("OK", GUILayout.Width(100), GUILayout.Height(40)))
            {
                ShowPopup = false;
            }
            if (!_ignoreDontShow)
            {
                if (GUILayout.Button("Close & Don't Show Again This Session", GUILayout.Width(250), GUILayout.Height(40)))
                {
                    _dontShowAgainThisSession = true;
                    ShowPopup = false;
                }
            }
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
    }

    /// <summary>
    /// Shows an informational popup using the default Title.
    /// </summary>
    public void ShowPopupDlg(string message, bool showCloseAndDontShowAgain)
    {
        ShowPopupDlg(Title, message, showCloseAndDontShowAgain);
    }

    /// <summary>
    /// Shows an informational popup with OK and optional "Don't Show Again" buttons.
    /// </summary>
    public void ShowPopupDlg(string title, string message, bool showCloseAndDontShowAgain)
    {
        _ignoreDontShow = !showCloseAndDontShowAgain;
        if (_dontShowAgainThisSession && showCloseAndDontShowAgain)
        {
            return;
        }
        _title = title;
        _popupMessage = message;
        _isConfirmation = false;
        _onConfirm = null;
        ShowPopup = true;
    }

    /// <summary>
    /// Shows a confirmation popup with Confirm/Cancel buttons.
    /// </summary>
    public void ShowConfirmation(string title, string message, Action onConfirm)
    {
        _title = title;
        _popupMessage = message;
        _isConfirmation = true;
        _onConfirm = onConfirm;
        ShowPopup = true;
    }
}
