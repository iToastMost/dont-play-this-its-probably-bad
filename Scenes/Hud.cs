using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	public void ShowGameOver()
	{
		GetNode<Label>("StartGameMessage").Show();
		GetNode<Button>("StartButton").Show();
	}

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		GetNode<Label>("StartGameMessage").Hide();
		EmitSignal(SignalName.StartGame);
	}

	public void HideHud()
	{
        GetNode<Button>("StartButton").Hide();
        GetNode<Label>("StartGameMessage").Hide();
    }

	public void UpdateScore(float score)
	{
		GetNode<Label>("Score").Text = $"Score: {score}";
	}
}
