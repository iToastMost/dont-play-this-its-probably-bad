using Godot;
using System;
using System.Collections.Generic;

public static class SceneManager
{
	private static Dictionary<string, PackedScene> Presets = new()
	{
		{"one_jump", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/one_jump_platform_preset.tscn") },
        {"timed", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/timed_platform_preset.tscn") },
        {"vertical", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/vertical_platform_preset.tscn") },
    };

    public static PackedScene GetPreset(string key) 
    {
        return Presets.ContainsKey(key) ? Presets[key] : null;
    }

}
