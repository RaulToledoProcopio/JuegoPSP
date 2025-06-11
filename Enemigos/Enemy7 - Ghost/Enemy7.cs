using Godot;
using System;

public partial class Enemy7 : CharacterBody2D
{
	public const float Speed = 100.0f;
	private AnimatedSprite2D animation;
	private Timer _riseTimer;
	private Timer _deathTimer;
	private Vector2 _movementDirection = Vector2.Zero;
	private int hp = 100;
	private bool _timerStarted = false;
	[Export] public int damage = 25;
	private AudioStreamPlayer deathSound;

	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_riseTimer = GetNode<Timer>("Timer2");
		_deathTimer = GetNode<Timer>("Timer");
		deathSound = GetNode<AudioStreamPlayer>("Dead");

		animation.Play("Rise");
		_riseTimer.Start(1.5f);
	}

	private void OnRiseTimerTimeout()
	{
		animation.Play("Walk");
		CambiarDireccion(-1);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_movementDirection != Vector2.Zero)
		{
			Vector2 velocity = Velocity;
			velocity.X = _movementDirection.X * Speed;
			Velocity = velocity;
			MoveAndSlide();
		}

		if (IsOnWall())
		{
			_movementDirection.X *= -1;
			animation.FlipH = _movementDirection.X > 0;
		}

		if (hp <= 0 && !_timerStarted)
		{
			if (animation.Animation != "Death")
			{
				animation.Play("Death");
				deathSound?.Play();
			}

			_movementDirection = Vector2.Zero;
			_deathTimer.Start(1f);
			_timerStarted = true;
		}
	}

	private void CambiarDireccion(float nuevaDireccionX)
	{
		if (_movementDirection.X != nuevaDireccionX)
		{
			_movementDirection.X = nuevaDireccionX;
			animation.FlipH = nuevaDireccionX > 0;
		}
	}

	private void OnDeathTimerTimeout()
	{
		QueueFree();
		GetParent().CallDeferred("OnEnemyDefeated", this);
	}

	private void _on_body_entered(Node body)
	{
		if (body is Player player)
		{
			player.TakeDamage(damage);
		}
	}

	public void TakeDamage(int damage)
	{
		hp -= damage;
		var player = GetNode<Player>("../Player");
		float direction = (player.Position.X < this.Position.X) ? 1.0f : -1.0f;
		this.Position = new Vector2(this.Position.X + (direction * 50), this.Position.Y);
	}
}
