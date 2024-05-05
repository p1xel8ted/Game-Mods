using UnityEngine;

namespace Namify;

internal class PopupManager : MonoBehaviour
{
    private bool showPopup;
    private string popupMessage = "";
    private Rect popupRect = new(Screen.width / 2f - 150, Screen.height / 2f - 75, 300, 100);
    private GUIStyle? messageStyle;

    private void Start()
    {
        messageStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true,
            fontSize = 20,
            normal = {textColor = Color.white}
        };
    }

    private void OnGUI()
    {
        if (showPopup)
        {
            popupRect = GUILayout.Window(0, popupRect, DrawPopup, Plugin.PluginName);
        }
    }

    private void DrawPopup(int windowID)
    {
        // Add flexible space before the content
        GUILayout.FlexibleSpace();

        // Use the custom style in the label for the message
        GUILayout.Label(popupMessage, messageStyle);

        // Add some space between the label and the button
        GUILayout.FlexibleSpace();

        // Create a horizontal group for the button
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Add flexible space before the button

        // Create the button with a specific width
        if (GUILayout.Button("Close", GUILayout.Width(100)))
        {
            Plugin.AddName.Value = string.Empty;
            showPopup = false;
        }

        GUILayout.FlexibleSpace(); // Add flexible space after the button
        GUILayout.EndHorizontal();

        // Add flexible space after the content
        GUILayout.FlexibleSpace();
    }


    public void ShowPopup(string message)
    {
        popupMessage = message;
        showPopup = true;
    }
}