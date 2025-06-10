using Godot;
using System;

public partial class Enemy3 : CharacterBody2D
{
	public const float Speed = 50.0f; // Velocidad de movimiento del enemigo.

	// Parámetros de knockback en arco
	private const float KnockBackSpeed   = 200f;  // velocidad horizontal inicial
	private const float KnockBackUpForce = 100f;  // velocidad vertical inicial
	private const float Gravity          = 1000f; // aceleración hacia abajo

	private Vector2 _knockbackVelocity = Vector2.Zero;
	private bool _isKnockedBack = false;

	private AnimatedSprite2D animation;
	private Timer _riseTimer;
	private Timer _deathTimer;
	private Vector2 _movementDirection = new Vector2(-1, 0);
	private int hp = 100;
	private bool _timerStarted = false;
	[Export] public int damage = 10;
	private AudioStreamPlayer deathSound;

	public override void _Ready()
	{
		animation   = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_riseTimer  = GetNode<Timer>("Timer");
		_deathTimer = GetNode<Timer>("Timer2");
		deathSound  = GetNode<AudioStreamPlayer>("Dead");

		animation.Play("Rise");
		_riseTimer.Start(1.0f);
	}

	private void OnRiseTimerTimeout()
	{
		animation.Play("Walk");
		_movementDirection = new Vector2(-1, 0);
	}

	public override void _PhysicsProcess(double delta)
	{
		// Si estamos en estado de knockback, aplicamos la parábola
		if (_isKnockedBack)
		{
			_knockbackVelocity.Y += Gravity * (float)delta;
			Velocity = _knockbackVelocity;
			MoveAndSlide();
			
			if (IsOnFloor())
			{
				_isKnockedBack = false;
				_knockbackVelocity = Vector2.Zero;
			}
			return;
		}

		if (_movementDirection != Vector2.Zero)
		{
			Vector2 vel = Velocity;
			vel.X = _movementDirection.X * Speed;
			Velocity = vel;
			MoveAndSlide();
		}

		if (IsOnWall())
		{
			_movementDirection.X *= -1;
			animation.FlipH = !animation.FlipH;
		}

		if (hp <= 0 && !_timerStarted)
		{
			if (animation.Animation != "Death")
			{
				animation.Play("Death");
				deathSound?.Play();
			}

			_movementDirection = Vector2.Zero;
			_deathTimer.Start(0.5f);
			_timerStarted = true;
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
			player.TakeDamage(damage);
	}

	public void TakeDamage(int damage)
	{
		hp -= damage;
		var player = GetNode<Player>("../Player");
		float direction = (player.Position.X < Position.X) ? 1f : -1f;
		_knockbackVelocity = new Vector2(direction * KnockBackSpeed, -KnockBackUpForce);
		_isKnockedBack = true;
	}
}
