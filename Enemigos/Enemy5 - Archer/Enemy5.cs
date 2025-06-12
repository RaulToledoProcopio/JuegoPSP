using Godot;
using System;

public partial class Enemy5 : CharacterBody2D
{
	
	[Export] public PackedScene FireBallScene;    // Referencia a la escena de flecha
	[Export] public float IdleMoveSpeed = 100f;
	[Export] public bool startFlipped = false;
	private Vector2 _movementDirection = Vector2.Zero;


	private float fireRate = 2f;       // Tiempo entre disparos
	private float timeSinceLastFire = 0; // Acumulado desde el último disparo
	private int hp = 100;                // Puntos de vida

	private AnimatedSprite2D animation;
	private Timer _deathTimer;
	private AudioStreamPlayer deathSound;
	private enum EnemyState { Idle, Attack, Dead }
	private EnemyState currentState = EnemyState.Idle;
	private bool idleDone = false; // Flag para saber que la voltereta ha terminado
	private CollisionShape2D collisionShape; // Para que no colisione muerto


	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_deathTimer = GetNode<Timer>("Timer");
		deathSound = GetNode<AudioStreamPlayer>("Dead");
		_deathTimer.Stop();
		_deathTimer.OneShot = true;  
		_deathTimer.WaitTime = 4.5f; // Duración de animación “Dead”
		collisionShape = GetNode<CollisionShape2D>("CollisionShape2D"); // <-- Aquí

		// Arrancamos en estado Idle
		currentState = EnemyState.Idle;
		idleDone = false;
		animation.Play("Idle");

		// Aplica flip y dirección según la casilla de startFlipped
		_movementDirection = startFlipped ? new Vector2(-1, 0) : new Vector2(1, 0);
		animation.FlipH = !startFlipped;
	}


	public override void _PhysicsProcess(double delta)
	{
		float d = (float)delta;
		
		// Avanzar un poco mientras dura la voltereta
		if (currentState == EnemyState.Idle)
		{
			if (!idleDone)
			{
				float dir = animation.FlipH ? -1f : 1f;
				Position += new Vector2(dir * IdleMoveSpeed * d, 0);

				// Detectamos último fotograma de “Idle” para pasar a Attack
				int lastFrameIndex = animation.SpriteFrames.GetFrameCount("Idle") - 1;
				if (animation.Animation == "Idle" && animation.Frame == lastFrameIndex)
				{
					idleDone = true;
					currentState = EnemyState.Attack;
					animation.Play("Attack");
				}
			}
			return;
		}
		
		if (currentState == EnemyState.Attack)
		{
			if (hp <= 0)
			{
				EnterDeathState();
				return;
			}

			timeSinceLastFire += d;
			if (timeSinceLastFire >= fireRate)
			{
				timeSinceLastFire = 0;
				ShootFireBall();
			}
			return;
		}
	}

	private void EnterDeathState()
	{
		if (currentState == EnemyState.Dead)
			return;

		currentState = EnemyState.Dead;
		animation.Play("Dead");
		deathSound?.Play();
		if (collisionShape != null)
		collisionShape.CallDeferred("set_disabled", true);
		_deathTimer.Start();
		_deathTimer.Connect("timeout", Callable.From(OnDeathTimerTimeout));
	}
	
	private void ShootFireBall()
	{
		Arrow arrowInstance = (Arrow)FireBallScene.Instantiate();

		// Ajusta la velocidad según la dirección del sprite
		arrowInstance.speedDagger = animation.FlipH
		? -Mathf.Abs(arrowInstance.speedDagger)
		: Mathf.Abs(arrowInstance.speedDagger);

		// Establece la posición de la flecha
		arrowInstance.Position = Position + new Vector2(animation.FlipH ? -25 : 15, -10);
		GetParent().AddChild(arrowInstance);
	}
	
	private void OnDeathTimerTimeout()
	{
		QueueFree();
		GetParent().CallDeferred("OnEnemyDefeated", this);
	}

	public void TakeDamage(int damage)
	{
		if (currentState == EnemyState.Dead)
			return;

		hp -= damage;
		if (hp <= 0)
		{
			EnterDeathState();
		}
	}
}
