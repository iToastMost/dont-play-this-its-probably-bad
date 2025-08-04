using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class SceneManager
{
	private static Dictionary<string, PackedScene> Presets = new()
	{
		{"one_jump", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/one_jump_platform_preset.tscn") },
        {"timed", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/timed_platform_preset.tscn") },
        {"vertical", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/vertical_platform_preset.tscn") },
        {"around_the_blackhole", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/AroundTheBlackhole.tscn") },
        {"blackhole_weaving", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/blackhole_weaving.tscn") },
        {"easy", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/easy.tscn") },
        {"medium", ResourceLoader.Load<PackedScene>("res://Scenes/Presets/medium.tscn") }
    };

    private static Dictionary<string, PackedScene> Platforms = new()
    {
        {"platform", ResourceLoader.Load<PackedScene>("res://Scenes/Platforms/Platform.tscn") },
        {"one_jump_platform", ResourceLoader.Load<PackedScene>("res://Scenes/Platforms/one_jump_platform.tscn") },
        {"vertical_platform", ResourceLoader.Load<PackedScene>("res://Scenes/Platforms/vertical_platform.tscn") },
        {"timed_platform", ResourceLoader.Load<PackedScene>("res://Scenes/Platforms/timed_platform.tscn") },
        {"horizontal_platform", ResourceLoader.Load<PackedScene>("res://Scenes/Platforms/movingPlatform.tscn")}
    };

    private static Dictionary<string, PackedScene> Enemies = new()
    {
        {"enemy", ResourceLoader.Load<PackedScene>("res://Scenes/Enemies/enemy.tscn")},
        {"flying_enemy", ResourceLoader.Load<PackedScene>("res://Scenes/Enemies/flying_enemy.tscn")},
        {"floating_enemy", ResourceLoader.Load<PackedScene>("res://Scenes/Enemies/FloatingEnemy.tscn") }
    };

    private static Dictionary<string, PackedScene> Powerups = new()
    {
        {"spring", ResourceLoader.Load<PackedScene>("res://Scenes/PowerUps/spring.tscn") },
        {"jetpack", ResourceLoader.Load<PackedScene>("res://Scenes/PowerUps/jetpack.tscn") }
    };

    public static PackedScene GetPreset(string key) 
    {
        return Presets.ContainsKey(key) ? Presets[key] : null;
    }

    public static PackedScene GetEnemy(string key) 
    {
        return Enemies.ContainsKey(key) ? Enemies[key] : null;
    }

    public static PackedScene GetPowerup(string key) 
    {
        return Powerups.ContainsKey(key) ? Powerups[key] : null;
    }

    public static PackedScene GetPlatform(string key) 
    {
        return Platforms.ContainsKey(key) ? Platforms[key] : null;
    }

    public static PackedScene GetRandomPreset() 
    {
        //Find a workaround later for easy/medium scenes not spawning platforms. They may need their own dictionary
        //int randomRange = GD.RandRange(0, Presets.Count - 1);

        int randomRange = GD.RandRange(0, 4);
        var keyArray = Presets.Keys.ToArray();
        var randomKey = keyArray[randomRange];

        GD.Print("Spawning: " + randomKey);

        return Presets.ContainsKey(randomKey) ? Presets[randomKey] : null;
    }

}
