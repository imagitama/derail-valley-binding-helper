# Derail Valley Binding Helper

A framework mod for the game [Derail Valley](https://store.steampowered.com/app/588030/Derail_Valley/) that lets modders easily add new button bindings.

## TODO

- support axis

## How it works

1. Bindings are stored with your mod in your mod settings. You define a single binding per setting or a List.

2. Wait for the Binding system to be ready. Either in your mod's Main or before you check if a binding is pressed.

3. Check if your binding is pressed!

## Setup

Once a user has installed this mod, you can use it in your own mod:

`Directory.Build.props`:

```xml
<Project>
  <PropertyGroup>
    <ReferencePath>
      $(ReferencePath);
      C:\SteamLibrary\steamapps\common\Derail Valley\DerailValley_Data\Managed\;
      C:\SteamLibrary\steamapps\common\Derail Valley\Mods\DerailValleyBindingHelper\;
    </ReferencePath>
  </PropertyGroup>
</Project>
```

`Project.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
	<!-- Mods -->
	<ItemGroup>
		<Reference Include="DerailValleyBindingHelper" />
	</ItemGroup>
</Project>
```

Define some actions and some actual bindings:

```cs
public static class Actions
{
    public static int JumpOrSomething = 12345;
    public static int RearJatoActivate = 150;
    public static int FrontJatoActivate = 151;
}

public class Settings : UnityModManager.ModSettings
{
    public BindingInfo MyCoolBinding = new BindingInfo(
        "Jump Or Something",
        Actions.JumpOrSomething,
        KeyCode.Space
    ) {
        DisableDefault = true
    };

    // NOTE: UnityModManager will not update this List to ensure it matches your default
    public List<BindingInfo> Bindings = new List<BindingInfo>()
    {
        new BindingInfo("Rear JATO", Actions.RearJatoActivate, KeyCode.LeftShift),
        new BindingInfo("Front JATO", Actions.FrontJatoActivate, KeyCode.LeftControl),
    };
}
```

Now you are free to check your bindings!

## Showing your bindings in the Binding Helper panel

In your mod's main:

```cs
public static class Main
{
    public static Settings settings;

    private static bool Load(UnityModManager.ModEntry modEntry)
    {
        settings = Settings.Load<Settings>(modEntry);

        // call this
        BindingsAPI.RegisterBindings(Main.ModEntry, settings.Bindings);

        harmony = new Harmony(modEntry.Info.Id);
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}
```

## Getting binding values

**You must wait for the game's input system to be ready!**

### Wait in Main

```cs
public static class Main
{
    public static Settings settings;

    private static bool Load(UnityModManager.ModEntry modEntry)
    {
        // wait
        BindingHelper.OnReady += () =>
        {
            settings = Settings.Load<Settings>(modEntry);

            // safe to check

            harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
```

### Or wait before checking

```cs
void Update()
{
    if (!BindingHelper.IsReady)
        return;

    // check
}
```

### Now safe to check

From an `Update()` loop get a binding value with:

```cs
if (BindingHelper.GetIsPressed(Main.settings.MyRearJatoBinding))
{
    // BOOST!
}
```

or:

```cs
if (BindingHelper.GetIsPressed(Main.settings.Bindings, Actions.RearJatoActivate))
{
    // BOOST!
}
```

or if you registered your bindings:

```cs
if (BindingsAPI.GetIsPressed(Actions.RearJatoActivate))
{
    // BOOST!
}
```

## Outputting bindings

You can draw the editor in your mod settings:

```cs
public static class Main
{
    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
        settings.Draw(modEntry);

        // add this
        BindingHelperUI.DrawBindings(
            settings.Bindings,
            OnUpdated: () =>
                // don't forget this!
                BindingHelper.ApplyBindingDisables(settings.Bindings
        ));
    }
}
```

You can also draw all your bindings inside any GUI:

```cs
public MyComponent : MonoBehavior
{
    void OnGUI()
    {
        void OnUpdate()
        {
            BindingHelper.ApplyBindingDisables(settings.Bindings);
            ModEntry.OnSaveGUI();
        }

        BindingHelperUI.DrawBindings(settings.Bindings, OnUpdate);
    }
}
```

Or a single one too:

**Ensure you pass in an index if you are rendering it as part of a list so the "record" feature works properly.**

```cs
public MyComponent : MonoBehavior
{
    void OnGUI()
    {
        void OnUpdate()
        {
            BindingHelper.ApplyBindingDisables(myAwesomeBinding);
            ModEntry.OnSaveGUI();
        }

        BindingHelperUI.DrawBinding(myAwesomeBinding, OnUpdate);
    }
}
```

Instead of applying binding disables each time you can do it in your settings (watch out as OnChange is called frequently with sliders)

```cs

public class Settings : UnityModManager.ModSettings, IDrawable
{
    public void OnChange()
    {
        BindingHelper.ApplyBindingDisables(bindings);
    }
}
```

## Install

Download the zip and use Unity Mod Manager to install it.

## Development

Template from https://github.com/derail-valley-modding/template-umm

Created in VSCode (with C# and C# Dev Kit extensions) and MSBuild.

1. Run `msbuild` in root to build

## Publishing

1. Run `.\package.ps1`
