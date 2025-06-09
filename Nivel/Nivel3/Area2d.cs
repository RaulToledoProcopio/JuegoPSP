using Godot;
using System;

public partial class Area2d : Area2D
{
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			GameState.ChangeHp(-GameState.Health); // Reduce toda la vida a 0
		}
	}
}
