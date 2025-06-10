using Godot;
using System;

public partial class FireBlue : Area2D
{
	[Export] public float speedDagger = 500;
	[Export] public int damage = 25;
	[Export] public float Direction = -1f;

	private Vector2 _velocity;
	private AnimatedSprite2D _animation;

	public override void _Ready()
	{
		_velocity = new Vector2(speedDagger * Direction, 0);
		_animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animation.Play("Idle");
		_animation.FlipH = Direction < 0;
	}

	public override void _Process(double delta)
	{
		Position += _velocity * (float)delta;

		var rect = GetViewportRect();
		if (Position.X > rect.Size.X || Position.X < 0 || Position.Y > rect.Size.Y || Position.Y < 0)
			QueueFree();
	}

	private void _on_body_entered(Node body)
	{
		if (body is Player player)
		{
			player.TakeDamage(damage);
			QueueFree();
		}
	}
}
