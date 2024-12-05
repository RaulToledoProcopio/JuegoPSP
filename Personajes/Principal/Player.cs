using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 200f; // Velocidad de movimiento del personaje.
	[Export]
	public float JumpVelocity = -500f; // Velocidad aplicada al personaje al saltar.
	[Export]
	public float Gravity = 1250f; // Fuerza de gravedad aplicada al personaje.
	[Export]
	public float speedDagger = 300f; // Velocidad a la que se lanza la daga.
	[Export]
	public float CrouchSpeed = 100f; // Velocidad de movimiento cuando el personaje está agachado.
	[Export]
	public int Ammo = 5; // Cantidad de dagas disponibles.
	[Export]
	public int MaxJumps = 2; // Número máximo de saltos permitidos para el doble salto.
	public int hp = 100; // Vida del personaje.
	private AnimatedSprite2D animation; // Controla las animaciones del personaje.
	private bool isCrouching = false; // Indica si el personaje está agachado.
	public bool isAttacking = false; // Indica si el personaje está atacando.
	private PackedScene dagger; // Escena cargada para las dagas.
	private Espada _espada; // Referencia al nodo de la espada.
	private Timer _gameOverTimer; // Temporizador para que nos lleve a la pantalla de GameOver.
	private bool _timerStarted = false; // Indica si el temporizador ha comenzado.
	private bool _isDead = false; // Indica si el pj está vivo.
	private ProgressBar _healthBar; // Barra de salud en la interfaz.
	private Label _ammoLabel; // Etiqueta que muestra la cantidad de munición disponible.
	private int jumpCount = 0; // Contador de saltos realizados para el doble salto.

	public override void _Ready()
	{
		// Referencias a los nodos necesarios
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); // Animaciones del personaje.
		dagger = GD.Load<PackedScene>("res://Proyectiles/Daga/daga.tscn"); // Daga.
		_espada = GetNode<Espada>("Espada"); // Área de colisión de la espada.
		_gameOverTimer = GetNode<Timer>("Timer"); // Timer para el Game Over.
		_gameOverTimer.Stop(); // Detiene el temporizador inicialmente.
		_healthBar = GetNode<ProgressBar>("../UI/HealthBar"); // Barra de salud.
		_healthBar.Value = hp; // Iguala el valor inicial de la barra a la vida del pj.
		_ammoLabel = GetNode<Label>("../UI/Ammo"); // Munición en la interfaz.
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isDead) return; // Si el personaje está muerto, no realiza más acciones.
		if (animation.Animation == "Hit" && animation.IsPlaying()) return; // Si está reproduciendo la animación de daño, espera.

		Vector2 velocity = Velocity; // Velocidad actual del personaje.

		_healthBar.Value = hp; // Actualiza la barra de salud.
		_ammoLabel.Text = $": {Ammo}"; // Actualiza la etiqueta de munición.

		// Aplica gravedad si el personaje no está en el suelo para el salto.
		if (!IsOnFloor())
		{
			velocity.Y += Gravity * (float)delta;
		}
		else
		{
			jumpCount = 0; // Reinicia el contador de saltos al tocar el suelo.
		}

		// Detecta el movimiento horizontal del personaje.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		// Detecta si el jugador está agachado.
		isCrouching = Input.IsActionPressed("ui_down");

		// Ataque con espada.
		if (Input.IsActionJustPressed("ui_attack") && !isAttacking && !isCrouching)
		{
			isAttacking = true; // Cambia el estado a atacando.
			Attack(); // Ejecuta el ataque con espada.
			animation.Play("Attack"); // Reproduce la animación de ataque.
		}

		// Ataque con daga.
		if (Ammo > 0) // Verifica si hay munición disponible.
		{
			if (Input.IsActionJustPressed("ui_daga") && !isAttacking && !isCrouching)
			{
				// Instancia una daga y la posiciona según la dirección del personaje.
				Daga instDagger = (Daga)dagger.Instantiate(); // Instancia una daga desde la escena cargada.
				instDagger.Position = GlobalPosition + new Vector2(animation.FlipH ? -15 : 15, -15); // Posición inicial relativa al personaje.
				instDagger.RotationDegrees = RotationDegrees; // Configura la rotación según la orientación del personaje.
				instDagger.speedDagger = animation.FlipH ? -speedDagger : speedDagger; // Velocidad según dirección.


				// Ajusta la escala de la daga según la dirección del personaje.
				if (animation.FlipH)
				{
					instDagger.Scale = new Vector2(-Mathf.Abs(instDagger.Scale.X), instDagger.Scale.Y);
				}

				GetParent().AddChild(instDagger); // Agrega la daga a la escena.
				Ammo--; // Reduce la munición disponible.
			}
		}
		else
		{
			GD.Print("No quedan dagas"); // Muestra un mensaje en la consola si no hay munición.
		}

		// Verifica si la animación de ataque ha terminado.
		if (animation.Animation == "Attack" && !animation.IsPlaying())
		{
			isAttacking = false;
		}

		// Doble salto.
		if (Input.IsActionJustPressed("ui_accept") && jumpCount < MaxJumps)
		{
			velocity.Y = JumpVelocity; 
			jumpCount++; // Incrementa el contador de saltos.
			animation.Play("Jump");
		}

		// Movimiento en el suelo.
		if (IsOnFloor())
		{
			if (isCrouching && !isAttacking) // Movimiento al agacharse.
			{
				velocity.X = direction.X * CrouchSpeed; // Aplica velocidad reducida.
				animation.FlipH = direction.X < 0; // Cambia la dirección de la animación.

				animation.Play(direction.X != 0 ? "CrouchWalk" : "Crouch"); // Animaciones de caminar agachado o estar quieto.
			}
			else if (!isAttacking) // Movimiento normal.
			{
				if (direction.X != 0)
				{
					velocity.X = direction.X * Speed; // Aplica velocidad normal.
					animation.FlipH = direction.X < 0; // Cambia la dirección de la animación.
					animation.Play("Run"); // Reproduce la animación de correr.
				}
				else
				{
					velocity.X = 0; // Detiene el movimiento horizontal.
					animation.Play("Idle"); // Reproduce la animación de estar quieto.
				}
			}
		}
		else if (!isAttacking) // Movimiento en el aire.
		{
			if (direction.X != 0)
			{
				velocity.X = direction.X * Speed; // Permite moverse horizontalmente en el aire.
				animation.FlipH = direction.X < 0; // Cambia la dirección de la animación.
			}
			animation.Play("Jump"); // Reproduce la animación de salto.
		}

		// Animación de muerte.
		if (hp <= 0 && !_isDead && IsOnFloor())
		{
			if (animation.Animation != "Death")
			{
				animation.Play("Death"); // Reproduce la animación de muerte.
				_gameOverTimer.Start(5.5f); // Inicia el temporizador para el fin del juego.
				_isDead = true; // Cambia el estado a muerto.
			}
			return;
		}

		// Actualiza la velocidad y mueve al personaje.
		Velocity = velocity;
		MoveAndSlide();
	}

	// Activa el área de la espada durante un ataque.
	public void Attack()
	{
		_espada.ActivarEspada(); // Activa el nodo del área de la espada.
		GetTree().CreateTimer(0.3f).Timeout += () => _espada.DesactivarEspada(); // Desactiva la espada después de un tiempo.
	}

	// Cambia a la escena de Game Over cuando el temporizador finaliza.
	private void OnGameOverTimeout()
	{
		GetTree().ChangeSceneToFile("res://Menú/GameOver.tscn");
	}

	// Función de recibir daño
	public void TakeDamage(int damage)
	{
		if (_isDead) return; // Si está muerto, no recibe daño.
		hp -= damage; // Reduce los puntos de vida.
		GD.Print($"Recibió {damage} de daño, vida restante: {hp}");
		animation.Play("Hit");
	}
}
