using Godot;
using System;

public partial class ItemAmmo : Area2D
{
	void _on_body_entered(Node body)
	{
		if (body is Player)
		{
			GameState.ChangeAmmo(5);
			QueueFree();
		}
	}
}
