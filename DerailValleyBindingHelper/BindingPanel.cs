using UnityEngine;
using DerailValleyModToolbar;
using UnityModManagerNet;

namespace DerailValleyBindingHelper;

public class BindingPanel : MonoBehaviour, IModToolbarPanel
{
    public UnityModManager.ModEntry? selectedModEntry;

    public void Window(Rect rect)
    {
        foreach (var kv in BindingsAPI.AllBindings)
        {
            var modEntry = kv.Key;
            var bindings = kv.Value;

            if (GUILayout.Button($"<b>Mod '{modEntry.Info.DisplayName}' {(selectedModEntry == modEntry ? "▼" : "▶")}</b>", GUI.skin.label))
            {
                if (selectedModEntry == modEntry)
                {
                    selectedModEntry = null;
                }
                else
                {
                    selectedModEntry = modEntry;
                }
            }

            if (selectedModEntry == modEntry)
            {
                for (var i = 0; i < bindings.Count; i++)
                    BindingHelperUI.DrawBinding(bindings[i], index: i, OnUpdated: () => modEntry.OnSaveGUI(modEntry));
            }
        }
    }
}