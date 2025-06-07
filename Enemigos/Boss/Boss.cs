using Godot;
using System;

public partial class Boss : CharacterBody2D
{
	private enum State { PatrolHigh, MovingDown, PatrolLow, MovingUp }
	private State _state = State.PatrolHigh;

	[Export] public float Speed = 300f;
	[Export] public float VerticalSpeed = 400f;
	[Export] public float HighPatrolDuration = 15f;
	[Export] public float LowPatrolDuration = 5f;
	[Export] public float LowY = 450f;
	[Export] public PackedScene LightningScene; // Rayo
	[Export] public Vector2 LightningOffset = new Vector2(-100, 175); // Posición relativa al boss
	private int damage = 50; // Daño que hace el enemigo

	private float _highY;
	private Vector2 _patrolDir = Vector2.Left;
	private AnimatedSprite2D _anim;
	private float _stateTimer = 0f;

	private Timer _lightningTimer;

	public override void _Ready()
	{
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_anim.Play("Idle");
		_highY = GlobalPosition.Y;
		_stateTimer = 0f;

		// Timer para disparar rayos
		_lightningTimer = new Timer();
		_lightningTimer.WaitTime = 2f;
		_lightningTimer.OneShot = false;
		_lightningTimer.Autostart = false;
		_lightningTimer.Timeout += () => FireLightning();
		AddChild(_lightningTimer);
		_lightningTimer.Start();
	}

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;
		_stateTimer += dt;

		Vector2 vel = Vector2.Zero;

		switch (_state)
		{
			case State.PatrolHigh:
				vel = new Vector2(_patrolDir.X * Speed, 0);
				if (_stateTimer >= HighPatrolDuration)
				{
					_state = State.MovingDown;
					_stateTimer = 0f;
					_lightningTimer.Stop(); // Dejar de disparar rayos
				}
				break;

			case State.MovingDown:
				vel = new Vector2(0, VerticalSpeed);
				if (GlobalPosition.Y >= LowY)
				{
					GlobalPosition = new Vector2(GlobalPosition.X, LowY);
					_state = State.PatrolLow;
					_stateTimer = 0f;
				}
				break;

			case State.PatrolLow:
				vel = new Vector2(_patrolDir.X * Speed, 0);
				if (_stateTimer >= LowPatrolDuration)
				{
					_state = State.MovingUp;
					_stateTimer = 0f;
				}
				break;

			case State.MovingUp:
				vel = new Vector2(0, -VerticalSpeed);
				if (GlobalPosition.Y <= _highY)
				{
					GlobalPosition = new Vector2(GlobalPosition.X, _highY);
					_state = State.PatrolHigh;
					_stateTimer = 0f;
					_lightningTimer.Start(); // Reanuda rayos al volver a patrullar arriba
				}
				break;
		}

		// Movimiento horizontal
		if (_state == State.PatrolHigh || _state == State.PatrolLow)
		{
			Velocity = vel;
			MoveAndSlide();

			if (IsOnWall())
			{
				_patrolDir.X = -_patrolDir.X;
				GlobalPosition += new Vector2(_patrolDir.X * 5, 0);
			}

			_anim.FlipH = _patrolDir.X > 0;
		}
		else
		{
			Velocity = vel;
			MoveAndSlide();
		}
	}

	private void FireLightning()
	{
		if (LightningScene == null)
			return;

		var lightning = LightningScene.Instantiate<Lightning>();
		lightning.GlobalPosition = GlobalPosition + LightningOffset;
		GetParent().AddChild(lightning);
	}
	
	private void _on_body_entered(Node body)
	{
		if (body is Player player)
		{
			player.TakeDamage(damage);
		}
	}
}
