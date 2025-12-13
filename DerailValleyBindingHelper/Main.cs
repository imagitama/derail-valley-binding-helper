using System;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using DerailValleyModToolbar;

namespace DerailValleyBindingHelper;

#if DEBUG
[EnableReloading]
#endif
public static class Main
{
    public static UnityModManager.ModEntry ModEntry;
    public static Settings settings;

    private static bool Load(UnityModManager.ModEntry modEntry)
    {
        ModEntry = modEntry;

        Harmony? harmony = null;
        try
        {
            modEntry.Logger.Log("DerailValleyBindingHelper load");

            BindingHelper.OnReady += () =>
            {
                ModEntry.Logger.Log("DerailValleyBindingHelper bindings ready");

                settings = Settings.Load<Settings>(modEntry);
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;

                ModToolbarAPI
                    .Register(modEntry)
                    .AddPanelControl(
                        label: "Bindings Helper",
                        icon: "icon.png",
                        tooltip: "Change your bindings",
                        type: typeof(BindingPanel),
                        title: "Bindings Helper",
                        width: 400)
                    .Finish();

                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                ModEntry.Logger.Log("DerailValleyBindingHelper loaded");
            };
        }
        catch (Exception ex)
        {
            ModEntry.Logger.LogException($"Failed to load {modEntry.Info.DisplayName}:", ex);
            harmony?.UnpatchAll(modEntry.Info.Id);
            return false;
        }

        modEntry.OnUnload = Unload;
        return true;
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
        settings.Draw(modEntry);
    }

    static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
        settings.Save(modEntry);
    }

    private static bool Unload(UnityModManager.ModEntry entry)
    {
        ModEntry.Logger.Log("DerailValleyBindingHelper stopped");
        return true;
    }
}
