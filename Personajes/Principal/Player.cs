using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
	// Estados posibles del jugador para controlar animaciones
	private enum PlayerState { Idle, Run, Jump, Crouch, CrouchWalk, Attack, Death, Hit }

	[Export] public float Speed = 200f;          // Velocidad de movimiento del personaje.
	[Export] public float JumpVelocity = -500f;  // Velocidad aplicada al personaje al saltar.
	[Export] public float Gravity = 1500f;       // Fuerza de gravedad aplicada al personaje.
	[Export] public float speedDagger = 300f;    // Velocidad a la que se lanza la daga.
	[Export] public float CrouchSpeed = 100f;    // Velocidad de movimiento cuando el personaje está agachado.
	[Export] public int MaxJumps = 2;             // Número máximo de saltos permitidos para el doble salto.

	private AnimatedSprite2D animation;          // Controla las animaciones del personaje.
	private bool isCrouching = false;            // Indica si el personaje está agachado.
	public bool isAttacking = false;             // Indica si el personaje está atacando.
	private PackedScene dagger;                  // Escena cargada para las dagas.
	private Espada _espada;                      // Referencia al nodo de la espada.
	private Timer _gameOverTimer;                // Temporizador para que nos lleve a GameOver.
	private bool _isDead = false;                // Indica si el pj está muerto.
	private ProgressBar _healthBar;              // Barra de salud en la interfaz.
	private Label _ammoLabel;                    // Etiqueta que muestra la munición.
	private int jumpCount = 0;                   // Contador de saltos para doble salto.
	private CollisionShape2D _idleCollisionShape;   // Collision para "idle"
	private CollisionShape2D _crouchCollisionShape; // Collision para "crouch"
	
	//Sonidos
	private AudioStreamPlayer jumpSoundPlayer; // Salto
	private AudioStreamPlayer throwSoundPlayer; // Daga
	private AudioStreamPlayer swordSoundPlayer; // Espada
	
	
	

	public override void _Ready()
	{

		// Referencias a los nodos necesarios
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		dagger = GD.Load<PackedScene>("res://Proyectiles/Daga/daga.tscn");
		_espada = GetNode<Espada>("Espada");
		_gameOverTimer = GetNode<Timer>("Timer");
		_gameOverTimer.Stop();  // Detiene el temporizador inicialmente.
		_healthBar = GetNode<ProgressBar>("../UI/HealthBar"); 
		_healthBar.Value = GameState.Health;  // Inicializa la barra de salud.
		_ammoLabel = GetNode<Label>("../UI/Ammo");
		_idleCollisionShape = GetNode<CollisionShape2D>("idleCollisionShape2D");
		_crouchCollisionShape = GetNode<CollisionShape2D>("crouchCollisionShape2D");
		_idleCollisionShape.Disabled = false;  // Colisión de pie activa.
		_crouchCollisionShape.Disabled = true; // Colisión agachado desactivada.
		
		jumpSoundPlayer = GetNode<AudioStreamPlayer>("Jump");
		throwSoundPlayer = GetNode<AudioStreamPlayer>("Throw");
		swordSoundPlayer = GetNode<AudioStreamPlayer>("Sword");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isDead) 
			return;  // Si el personaje está muerto, no hace nada.

		// Prioriza la animación de daño si sigue reproduciéndose
		if (animation.Animation == "Hit" && animation.IsPlaying()) 
			return;

		Vector2 velocity = Velocity;  // Velocidad actual del personaje.

		_healthBar.Value = GameState.Health;
		_ammoLabel.Text = GameState.Ammo.ToString();


		// Comprobar si está agachado
		isCrouching = Input.IsActionPressed("ui_down");
		_idleCollisionShape.Disabled = isCrouching;      // Ajusta colisiones según estado.
		_crouchCollisionShape.Disabled = !isCrouching;

		// Aplicar gravedad si está en el aire
		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;
		else
			jumpCount = 0;  // Reinicia contador al tocar suelo.

		// Entrada de movimiento horizontal
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		// Ataque con espada
		if (Input.IsActionJustPressed("ui_attack") && !isAttacking && !isCrouching)
		{
			isAttacking = true;  // Marca estado de ataque.
			Attack();            // Lanza método de ataque.
			animation.Play("Attack"); // Reproduce animación de ataque.
			swordSoundPlayer.Play();
		}

		// Ataque con daga
		if (GameState.Ammo > 0 && Input.IsActionJustPressed("ui_daga") && !isAttacking && !isCrouching)
		{
				var instDagger = (Daga)dagger.Instantiate();
				instDagger.Position = GlobalPosition + new Vector2(animation.FlipH ? -15 : 15, -15);
				instDagger.RotationDegrees = RotationDegrees;
				instDagger.speedDagger = animation.FlipH ? -speedDagger : speedDagger;
				if (animation.FlipH)
				instDagger.Scale = new Vector2(-Mathf.Abs(instDagger.Scale.X), instDagger.Scale.Y);
				GetParent().AddChild(instDagger);
				throwSoundPlayer.Play();
	
				GameState.ChangeAmmo(-1);
		}

		// Comprueba fin de animación de ataque
		if (animation.Animation == "Attack" && !animation.IsPlaying())
			isAttacking = false;

		// Doble salto
		if (Input.IsActionJustPressed("ui_accept") && jumpCount < MaxJumps)
		{
			velocity.Y = JumpVelocity; 
			jumpCount++;  // Incrementa contador de saltos.
			jumpSoundPlayer.Play();
		}

		// Ajusta velocidad horizontal según input y agachar
		velocity.X = direction.X * (isCrouching ? CrouchSpeed : Speed);

		// Aplica movimiento al cuerpo
		Velocity = velocity;
		MoveAndSlide();
		UpdateAnimation(direction);
	}

	// Decide qué animación reproducir según el estado actual.
	private void UpdateAnimation(Vector2 direction)
	{
		PlayerState state;

		if (GameState.Health <= 0 && IsOnFloor())
		{
			state = PlayerState.Death;
			if (!_isDead)
			{
				_gameOverTimer.Start(5.5f);  // Inicia temporizador de GameOver
				_isDead = true;
				SaveManager.DeleteSave();
			}
		}
		// Animación de daño
		else if (animation.Animation == "Hit" && animation.IsPlaying())
			state = PlayerState.Hit;
		// Animación de ataque
		else if (animation.Animation == "Attack" && animation.IsPlaying())
			state = PlayerState.Attack;
		// En el aire
		else if (!IsOnFloor())
			state = PlayerState.Jump;
		// Agachado
		else if (isCrouching)
			state = direction.X != 0 ? PlayerState.CrouchWalk : PlayerState.Crouch;
		// Corriendo
		else if (direction.X != 0)
			state = PlayerState.Run;
		// Quieto
		else
			state = PlayerState.Idle;

		// Voltea sprite según dirección
		if (direction.X != 0)
			animation.FlipH = direction.X < 0;

		// Ajusta posición de la espada
		_espada.Position = animation.FlipH ? new Vector2(-70, 0) : Vector2.Zero;

		// Reproduce la animación correspondiente
		switch (state)
		{
			case PlayerState.Death:
				animation.Play("Death");
				break;
			case PlayerState.Hit:
				// Dejar animación de Hit corriendo
				break;
			case PlayerState.Attack:
				// Dejar animación de Attack corriendo
				break;
			case PlayerState.Jump:
				animation.Play("Jump");
				break;
			case PlayerState.CrouchWalk:
				animation.Play("CrouchWalk");
				break;
			case PlayerState.Crouch:
				animation.Play("Crouch");
				break;
			case PlayerState.Run:
				animation.Play("Run");
				break;
			default:
				animation.Play("Idle");
				break;
		}
	}

	// Activa el área de la espada durante un ataque
	public void Attack()
	{
		_espada.ActivarEspada();
		GetTree().CreateTimer(0.1f).Timeout += () => _espada.DesactivarEspada();
	}

	// Se llama al expirar el timer de GameOver
	private void OnGameOverTimeout()
	{
		GetTree().ChangeSceneToFile("res://Menú/GameOver.tscn");
	}

	// Función de recibir daño
	public void TakeDamage(int damage)
	{
			if (_isDead || isAttacking) return;
			GameState.ChangeHp(-damage);
			animation.Play("Hit");
			Position += new Vector2(animation.FlipH ? 50 : -50, 0);
	}
}
