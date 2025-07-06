using Godot;
using System;
using System.Reflection;

public partial class Main : Node
{
    [Export]
    public PackedScene PlatformScene { get; set; }

    [Export]
    public PackedScene EasyScene { get; set; }

    [Export]
    public PackedScene MediumScene { get; set; }

    [Export]
    public PackedScene MovingPlatformScene { get; set; }

    PackedScene platforms;

    private float _nextLevelY = -720;
    private Node2D levelToGo;
    private Node2D previousLevel;
    private Node2D currentLevel;


    Player player;

    private float _topHeightReached = 0f;
    private float _score = 0f;
    private float _regularPlatformChance = 100;

    public override void _Process(double delta)
    {
        if (player.Position.Y < _topHeightReached)
        {
            _topHeightReached = player.GlobalPosition.Y;
            _score = Mathf.Floor(-_topHeightReached);
            UpdateScore(_score);
        }
    }

    public override void _Ready()
    {
        
        player = GetNode<Player>("Player");
        var playerPosition = GetNode<Player>("Player").Position;
        platforms = PlatformScene;

        NewGame();
    }
    public void NewGame()
    {
        ResetStats();
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);
        var hud = GetNode<Hud>("HUD");
        hud.HideHud();
        CallDeferred(nameof(SpawnLevel));

        //GetTree().Paused = true;
    }

    private void GameOver()
    {
        var hud = GetNode<Hud>("HUD");
        hud.ShowGameOver();
        //GetTree().Paused = true;
        
    }

    private void SpawnLevel()
    {
        if(levelToGo != null)
        {
            levelToGo.QueueFree();       
        }
        
        Node2D easy = EasyScene.Instantiate<Easy>();
       
        easy.Position = new Vector2(0, _nextLevelY);

        GD.Print("Level Spawned");

        AddChild(easy);

        SpawnPlatforms(easy);

        if(previousLevel != null)
        {
            levelToGo = previousLevel;
        }

        if(currentLevel != null)
        {
            previousLevel = currentLevel;
        }

        currentLevel = easy;

        var spawnAreaPosition = GetNode<Area2D>("Area2D");
        spawnAreaPosition.CallDeferred("set_position", new Vector2(spawnAreaPosition.Position.X, _nextLevelY));


        player.deathHeight = _nextLevelY + 2000;

        _nextLevelY -= 720;
        _regularPlatformChance -= 2;
        GD.Print("Moving platform spawn chance is now: " + _regularPlatformChance.ToString());

    }

    private void SpawnLevelMedium()
    {
        if (levelToGo != null)
        {
            levelToGo.QueueFree();
        }

        Node2D medium = MediumScene.Instantiate<Node2D>();

        medium.Position = new Vector2(0, _nextLevelY);

        GD.Print("Level Spawned");

        AddChild(medium);

        SpawnPlatforms(medium);

        if (previousLevel != null)
        {
            levelToGo = previousLevel;
        }

        if (currentLevel != null)
        {
            previousLevel = currentLevel;
        }

        currentLevel = medium;

        var spawnAreaPosition = GetNode<Area2D>("Area2D");
        spawnAreaPosition.CallDeferred("set_position", new Vector2(spawnAreaPosition.Position.X, _nextLevelY));


        player.deathHeight = _nextLevelY + 2000;

        _nextLevelY -= 720;
        _regularPlatformChance -= 2;
        GD.Print("Moving platform spawn chance is now: " + _regularPlatformChance.ToString());
    }

    private void SpawnPlatforms(Node parentScene)
    {
        var children = parentScene.GetNode<Node>("PlatformPaths").GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            var platformSpawnLocation = parentScene.GetNode<PathFollow2D>("PlatformPaths/"+ children[i].Name + "/SpawnPath" + i.ToString());
            platformSpawnLocation.ProgressRatio = GD.Randf();

            Vector2 spawnPos = platformSpawnLocation.GlobalPosition;
            spawnPos.Y += _nextLevelY;

            int roll = GD.RandRange(0, 100);
            if(roll <= _regularPlatformChance) 
            {
                Platform platform = PlatformScene.Instantiate<Platform>();

                platform.GlobalPosition = spawnPos;

                AddChild(platform);
            }
            else
            {
                MovingPlatform platform = MovingPlatformScene.Instantiate<MovingPlatform>();

                platform.GlobalPosition = spawnPos;

                AddChild(platform);
            }
            

            //GD.Print("Spawning Platform from Easy Scene" + children[i].Name +" at location: " + platform.Position.Y.ToString());

            
        }
    }

    private void OnArea2DBodyEntered(Node2D body)
    {
        //GD.Print("Moving area2d");
        if(_score < 1000) 
        {
            CallDeferred(nameof(SpawnLevel));
        }
        else
        {
            CallDeferred(nameof(SpawnLevelMedium));
        }
        
    }

    private void ResetStats()
    {
        if(GetNode<Node>(".") != null) 
        {
        var children = GetNode<Node>(".").GetChildren();
        foreach (var item in children)
        {
            if(item is Platform platform)
            {
                platform.QueueFree();
            }
        }
        }
        

        _nextLevelY = -720;
        _score = 0;
        _topHeightReached = 0;
        _regularPlatformChance = 100;
        var spawnAreaPosition = GetNode<Area2D>("Area2D");
        spawnAreaPosition.Position = new Vector2(spawnAreaPosition.Position.X, _nextLevelY);
        if (levelToGo != null) 
        {
            levelToGo.QueueFree();
            levelToGo = null;
        }
            
        if (previousLevel != null) 
        {
            previousLevel.QueueFree();
            previousLevel = null;
        }

        if (currentLevel != null) 
        {
            currentLevel.QueueFree();
            currentLevel = null;
        }
            
        

        player.gameOver = false;
    }

    private void UpdateScore(float score)
    {
        GetNode<Hud>("HUD").UpdateScore(score);
    }
}
