using Godot;
using System;

public partial class ItemHeal : Area2D
{
	void _on_body_entered(Node body)
	{
		if (body is Player)
		{
			if (GameState.Health < 100)
				GameState.ChangeHp(25f);

			QueueFree();
		}
	}
}
