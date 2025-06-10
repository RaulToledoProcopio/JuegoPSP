using Godot;
using System;

public partial class Boss : CharacterBody2D
{
	private enum State { PatrolHigh, MovingDown, PatrolLow, MovingUp, Death }
	private State _state = State.PatrolHigh;

	[Export] public float Speed = 300f;
	[Export] public float VerticalSpeed = 400f;
	[Export] public float HighPatrolDuration = 15f;
	[Export] public float LowPatrolDuration = 5f;
	[Export] public float LowY = 450f;
	[Export] public PackedScene LightningScene;
	[Export] public Vector2 LightningOffset = new Vector2(-100, 175);

	private int damage = 50;
	private int hp = 100;

	private float _highY;
	private Vector2 _patrolDir = Vector2.Left;
	private AnimatedSprite2D _anim;
	private float _stateTimer = 0f;
	private AudioStreamPlayer deathSound;

	private Timer _lightningTimer;
	private Timer _deathTimer;
	private CollisionPolygon2D _collisionShape;

	public override void _Ready()
	{
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_anim.Play("Idle");

		_collisionShape = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
		_highY = GlobalPosition.Y;
		
		deathSound = GetNode<AudioStreamPlayer>("Dead");
		_lightningTimer = new Timer();
		_lightningTimer.WaitTime = 2f;
		_lightningTimer.OneShot = false;
		_lightningTimer.Timeout += () => FireLightning();
		AddChild(_lightningTimer);
		_lightningTimer.Start();

		_deathTimer = new Timer();
		_deathTimer.OneShot = true;
		_deathTimer.WaitTime = 5f;
		_deathTimer.Timeout += () => OnDeathTimerTimeout();
		AddChild(_deathTimer);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_state == State.Death)
			return;

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
					_lightningTimer.Stop();
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
					_lightningTimer.Start();
				}
				break;
		}

		if (_state != State.Death)
		{
			Velocity = vel;
			MoveAndSlide();

			if ((_state == State.PatrolHigh || _state == State.PatrolLow) && IsOnWall())
			{
				_patrolDir.X = -_patrolDir.X;
				GlobalPosition += new Vector2(_patrolDir.X * 5, 0);
			}

			_anim.FlipH = _patrolDir.X > 0;
		}
	}

	private void FireLightning()
	{
		if (LightningScene == null) return;

		var lightning = LightningScene.Instantiate<Lightning>();
		lightning.GlobalPosition = GlobalPosition + LightningOffset;
		GetParent().AddChild(lightning);
	}

	public void TakeDamage(int damage)
	{
		if (_state == State.Death) return;

		hp -= damage;
		var playerNode = GetNode<Player>("../Player");
		if (hp <= 0)
		{
			_state = State.Death;
			Velocity = Vector2.Zero;
			_anim.Play("Death");
			deathSound?.Play();
			_deathTimer.Start(2f);
		}
	}

	private void EnterDeathState()
{
	_state = State.Death;
	Velocity = Vector2.Zero;
	_lightningTimer.Stop();
	_collisionShape.SetDeferred("disabled", true);

	//_anim.Play("Death"); // Animación de muerte comentada hasta tenerla lista
	_deathTimer.Start(5f); // Arranca el timer para que desaparezca en 5 segundos
}


	private void OnDeathTimerTimeout()
{
	QueueFree();
	GetParent().CallDeferred("OnEnemyDefeated", this);
}

	private void _on_body_entered(Node body)
	{
		if (_state == State.Death)
			return;

		if (body is Player player)
		{
			player.TakeDamage(damage);
		}
	}
}
