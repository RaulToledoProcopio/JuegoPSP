using Godot;
using System;

public partial class Miniboss : CharacterBody2D
{
	private enum State { Patrol, Attack, Death }
	private State _state = State.Patrol;

	[Export] public float Speed = 50f;
	[Export] public PackedScene FireballScene;
	[Export] public Vector2 FireballOffset = new Vector2(100, -25);
	[Export] public float FireRate = 1f;
	[Export] public int Damage = 20;
	[Export] public int Hp = 100;
	private bool _playerInBreath = false;
	private Vector2 _breathOriginalPosition;
	private AudioStreamPlayer deathSound;



	private AnimatedSprite2D _anim;
	private CollisionShape2D _bodyShape;
	private CollisionShape2D _breathShape;
	private Area2D _breathArea;
	private Area2D _detectionArea;
	private Player _player;
	private Timer _fireTimer;
	private Timer _deathTimer;
	private Timer _breathDamageTimer;
	private Vector2 _patrolDir = Vector2.Left;
	

	public override void _Ready()
	{
		_anim          = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_bodyShape     = GetNode<CollisionShape2D>("Body");
		_breathArea    = GetNode<Area2D>("Breath");
		_breathShape   = _breathArea.GetNode<CollisionShape2D>("Breath");
		deathSound = GetNode<AudioStreamPlayer>("Dead");
		_detectionArea = GetNode<Area2D>("DetectionArea");
		_breathArea.Connect("body_entered", new Callable(this, nameof(OnBreathBodyEntered)));
		_breathArea.Connect("body_exited", new Callable(this, nameof(OnBreathBodyExited)));
		_breathOriginalPosition = _breathArea.Position;

		// Timer para lanzar bolas de fuego
		_fireTimer = new Timer();
		_fireTimer.WaitTime = FireRate;
		_fireTimer.OneShot = false;
		_fireTimer.Timeout += () => FireFireball();
		AddChild(_fireTimer);
		_fireTimer.Start();

		// Timer de muerte (en escena)
		_deathTimer = GetNode<Timer>("Timer");

		// Timer para activar/desactivar daño del breath
		_breathDamageTimer = new Timer { WaitTime = 0.5f, OneShot = true };
		_breathDamageTimer.Timeout += () =>
		{
			_breathShape.SetDeferred("disabled", true);
			if (_playerInBreath && IsInstanceValid(_player))
			_player.TakeDamage(Damage);
			if (_state == State.Attack && _player != null)
			GetTree().CreateTimer(1f).Timeout += () => ActivateBreathDamage();
		};
AddChild(_breathDamageTimer);


		_detectionArea.Connect("body_entered", new Callable(this, nameof(OnDetectionBodyEntered)));
		_detectionArea.Connect("body_exited", new Callable(this, nameof(OnDetectionBodyExited)));

		EnterPatrol();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_state == State.Death) return;

		switch (_state)
		{
			case State.Patrol:  PatrolBehavior();  break;
			case State.Attack:  AttackBehavior();  break;
		}
	}

	private void PatrolBehavior()
	{
		Velocity = _patrolDir * Speed;
		MoveAndSlide();

		if (IsOnWall())
		{
			_patrolDir.X = -_patrolDir.X;
			GlobalPosition += new Vector2(_patrolDir.X * 5, 0);
		}

		_anim.FlipH = _patrolDir.X > 0;
	}

	private void AttackBehavior()
{
	if (_player == null || !IsInstanceValid(_player))
	{
		EnterPatrol();
		return;
	}

	// El boss se detiene cuando está en ataque
	Velocity = Vector2.Zero;
	MoveAndSlide();

	// Girar sprite mirando al jugador
	bool playerRight = _player.Position.X > Position.X;
	_anim.FlipH = playerRight;

	// Reflejar el area de aliento en tiempo real
	_breathArea.Scale = new Vector2(playerRight ? -1 : 1, 1);
}


	private void EnterPatrol()
	{
		_state = State.Patrol;
		_anim.Play("Idle");
		_bodyShape.SetDeferred("disabled", false);
		_breathShape.SetDeferred("disabled", true);
		_fireTimer.Start();
		_player = null;
	}

	private void EnterAttack()
{
	_state = State.Attack;
	_anim.Play("Attack");

	// Verifica si el jugador está a la derecha del miniboss
	bool playerRight = _player.Position.X > Position.X;
	_anim.FlipH = playerRight;

	// Refleja el área Breath horizontalmente
	_breathArea.Scale = new Vector2(playerRight ? 1 : -1, 1);

	_bodyShape.SetDeferred("disabled", false);
	_breathShape.SetDeferred("disabled", true);
	_fireTimer.Stop();

	GetTree().CreateTimer(1f).Timeout += () => ActivateBreathDamage();
}



	private void OnDetectionBodyEntered(Node body)
	{
		if (body is Player player && _state != State.Death)
		{
			_player = player;
			EnterAttack();
		}
	}

	private void OnDetectionBodyExited(Node body)
	{
		if (body == _player)
			EnterPatrol();
	}

	private void FireFireball()
	{
		if (_state != State.Patrol || FireballScene == null)
			return;

		float dir = _patrolDir.X;

		var fire = FireballScene.Instantiate<FireBlue>();
		fire.Direction = dir;
		fire.GlobalPosition = GlobalPosition + new Vector2(FireballOffset.X * dir, FireballOffset.Y);
		GetParent().AddChild(fire);
	}

	// Llama este método cuando quieras activar el daño de Breath (por ejemplo desde la animación o código)
	public void ActivateBreathDamage()
{
	_breathShape.SetDeferred("disabled", true);  // primero desactiva
	_breathShape.SetDeferred("disabled", false); // luego activa de nuevo
	_breathDamageTimer.Start();
}

	private void OnBreathBodyEntered(Node body)
{
	if (body is Player player)
		_playerInBreath = true;
}

private void OnBreathBodyExited(Node body)
{
	if (body is Player player)
		_playerInBreath = false;
}

public void TakeDamage(int damage)
	{
		if (_state == State.Death) return;

		Hp -= damage;
		var playerNode = GetNode<Player>("../Player");
		if (Hp <= 0)
		{
			_state = State.Death;
			Velocity = Vector2.Zero;
			_anim.Play("Death");
			deathSound?.Play();
			_deathTimer.Start(2f);
		}
	}

	private void OnDeathTimerTimeout()
	{
		QueueFree();
		GetParent().CallDeferred("OnEnemyDefeated", this);
	}
}
