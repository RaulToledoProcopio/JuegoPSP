using Godot;
using System;

public partial class Enemy6 : CharacterBody2D
{
	private enum State { Patrol, Attack, Death }
	private State _state = State.Patrol;

	[Export] public float Speed = 100f;
	[Export] public int Damage = 10;
	[Export] public float AttackRate = 1f;
	[Export] public float DetectionRadius = 200f;
	[Export] public float Gravity = 600f;

	private AnimatedSprite2D _anim;
	private Area2D _weaponArea;
	private CollisionShape2D _weaponShape;
	private Area2D _detectionArea;
	private Timer _deathTimer;
	private Timer _attackTimer;
	private AudioStreamPlayer _deathSound;
	private Player _player;

	private Vector2 _patrolDir = Vector2.Left;
	private Vector2 _weaponOriginalPosition;

	private int _hp = 100;

	// Knockback variables (copiados tal cual de Enemy1)
	private Vector2 _knockbackVelocity = Vector2.Zero;
	private bool _isKnockedBack = false;
	private const float KnockBackSpeed = 200f;
	private const float KnockBackUpForce = 100f;
	private const float KnockbackGravity = 1000f;

	public override void _Ready()
	{
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_weaponArea = GetNode<Area2D>("Weapon");
		_weaponShape = _weaponArea.GetNode<CollisionShape2D>("CollisionShape2D");
		_detectionArea = GetNode<Area2D>("DetectionArea");
		_deathTimer = GetNode<Timer>("Timer");
		//_deathSound = GetNode<AudioStreamPlayer>("Dead");

		_weaponOriginalPosition = _weaponShape.Position;

		_attackTimer = new Timer();
		_attackTimer.WaitTime = AttackRate;
		_attackTimer.OneShot = false;
		AddChild(_attackTimer);

		_weaponArea.Monitoring = false;
		_weaponShape.Disabled = true;

		_detectionArea.Connect("body_entered", new Callable(this, nameof(OnDetectionBodyEntered)));
		_detectionArea.Connect("body_exited", new Callable(this, nameof(OnDetectionBodyExited)));

		_attackTimer.Connect("timeout", new Callable(this, nameof(OnAttackTimerTimeout)));

		_anim.Play("Walk");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isKnockedBack)
		{
			_knockbackVelocity.Y += KnockbackGravity * (float)delta;
			Velocity = _knockbackVelocity;
			MoveAndSlide();

			if (IsOnFloor())
			{
				_isKnockedBack = false;
				_knockbackVelocity = Vector2.Zero;
			}
			return;
		}

		if (_state == State.Death)
			return;

		if (_hp <= 0 && _state != State.Death)
		{
			EnterDeathState();
			return;
		}

		switch (_state)
		{
			case State.Patrol:
				PatrolBehavior();
				break;
			case State.Attack:
				AttackBehavior();
				break;
		}
	}

	private void PatrolBehavior()
	{
		Velocity = new Vector2(_patrolDir.X * Speed, Velocity.Y);
		MoveAndSlide();

		if (IsOnWall())
		{
			_patrolDir.X = -_patrolDir.X;
			_anim.FlipH = !_anim.FlipH;
		}

		if (_anim.Animation != "Walk")
			_anim.Play("Walk");

		_weaponArea.Monitoring = false;
		_weaponShape.Disabled = true;
	}

	private void AttackBehavior()
	{
		Velocity = Vector2.Zero;

		_anim.FlipH = _player.Position.X < Position.X ? false : true;

		if (_anim.FlipH)
		{
			_weaponShape.Position = new Vector2(-Mathf.Abs(_weaponOriginalPosition.X), _weaponOriginalPosition.Y);
		}
		else
		{
			_weaponShape.Position = new Vector2(Mathf.Abs(_weaponOriginalPosition.X), _weaponOriginalPosition.Y);
		}

		if (_anim.Animation != "Attack")
			_anim.Play("Attack");

		_weaponArea.Monitoring = true;
		_weaponShape.Disabled = false;
	}

	private void EnterDeathState()
	{
		_state = State.Death;
		_weaponArea.SetDeferred("monitoring", false);
		_weaponShape.SetDeferred("disabled", true);
		Velocity = Vector2.Zero;
		_anim.Play("Death");
		//_deathSound?.Play();
		_deathTimer.Start(5f);
	}

	private void OnDeathTimerTimeout()
	{
		QueueFree();
		GetParent().CallDeferred("OnEnemyDefeated", this);
	}

	private void OnDetectionBodyEntered(Node body)
	{
		if (body is Player p && _state != State.Death)
		{
			_player = p;
			_state = State.Attack;
			_attackTimer.Start();
		}
	}

	private void OnDetectionBodyExited(Node body)
	{
		if (body == _player)
		{
			_attackTimer.Stop();
			CallDeferred(nameof(DisableWeapon));
			_state = State.Patrol;
			_anim.Play("Walk");
			_player = null;
		}
	}

	private void OnAttackTimerTimeout()
	{
		if (_player != null && IsInstanceValid(_player))
		{
			_player.TakeDamage(Damage);
		}
	}

	private void _on_Weapon_body_entered(Node body)
	{
		if (_state == State.Attack && body is Player p)
		{
			p.TakeDamage(Damage);
		}
	}

	public void TakeDamage(int damage)
	{
		if (_state == State.Death)
			return;

		_hp -= damage;

		var player = GetNode<Player>("../Player");
		float direction = (player.Position.X < Position.X) ? 1f : -1f;

		_knockbackVelocity = new Vector2(direction * KnockBackSpeed, -KnockBackUpForce);
		_isKnockedBack = true;

		if (_hp <= 0)
		{
			EnterDeathState();
		}
	}

	private void DisableWeapon()
	{
		_weaponArea.Monitoring = false;
		_weaponShape.Disabled = true;
	}
}
