using UnityModManagerNet;

namespace DerailValleyBindingHelper;

public class Settings : UnityModManager.ModSettings, IDrawable
{
    private static UnityModManager.ModEntry.ModLogger Logger => Main.ModEntry.Logger;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
        Save(this, modEntry);
    }

    public void OnChange()
    {
    }
}
