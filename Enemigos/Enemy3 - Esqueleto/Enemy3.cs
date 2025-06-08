using Godot;
using System;

public partial class Enemy3 : CharacterBody2D
{
	public const float Speed = 50.0f; // Velocidad de movimiento del enemigo.
	private AnimatedSprite2D animation;  // Referencia a AnimatedSprite2D para animaciones.
	private Timer _riseTimer;  // Referencia al temporizador para la animación de aparecer.
	private Timer _deathTimer; // Temporizador para eliminar al enemigo después de morir.
	private Vector2 _movementDirection = new Vector2(-1, 0);   // Dirección inicial del movimiento (izquierda).
	private int hp = 100; // Puntos de vida del enemigo.
	private bool _timerStarted = false; // Bandera para evitar que se reinicie el temporizador.
	[Export] public int damage = 10; // Daño que inflinge el enemigo
	private AudioStreamPlayer deathSound;

	public override void _Ready()
	{
		// Obtención de los nodos necesarios
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_riseTimer = GetNode<Timer>("Timer");
		_deathTimer = GetNode<Timer>("Timer2");
		deathSound = GetNode<AudioStreamPlayer>("Dead");

		animation.Play("Rise"); // Reproducir la animación de Rise al inicio
		_riseTimer.Start(1.0f); // Iniciar el temporizador de Rise.
	}

	// Método llamado cuando el temporizador timeout se dispara.
	private void OnRiseTimerTimeout()
	{
		animation.Play("Walk"); // Cambiar a la animación "Walk".
		// Establecer la dirección de movimiento después de que termine el Rise.
		_movementDirection = new Vector2(-1, 0);
	}

	public override void _PhysicsProcess(double delta)
	{
		// Si el enemigo no está muerto, moverlo.
		if (_movementDirection != Vector2.Zero)
		{
			Vector2 velocity = Velocity;
			velocity.X = _movementDirection.X * Speed; // Asignar velocidad en base a la dirección.
			Velocity = velocity;
			MoveAndSlide();
		}

		// Si el enemigo colisiona, invierte la dirección
		if (IsOnWall())
		{
			_movementDirection.X *= -1;
			animation.FlipH = !animation.FlipH;
		}

		// Si la vida es 0 o menos y el temporizador no ha sido iniciado, iniciar la animación de muerte
		if (hp <= 0 && !_timerStarted)
		{
			// Si no se está ejecutando la animación la ejecuta, para evitar que se reinicie.
			if (animation.Animation != "Death")
			{
				animation.Play("Death");
				if (deathSound != null)
					deathSound.Play();
			}

			// Detener el movimiento del enemigo.
			_movementDirection = Vector2.Zero;

			// Activar el Timer de muerte.
			_deathTimer.Start(0.5f);
			_timerStarted = true;
			
		}
	}

	// Este método se llamará cuando el Timer de muerte termine.
	private void OnDeathTimerTimeout()
	{
		QueueFree(); // Eliminar al enemigo del juego.
		GetParent().CallDeferred("OnEnemyDefeated", this); // Envía la señal.
	}
	
	// Colisión con el jugador para hacerle daño.
	private void _on_body_entered(Node body){
		
		if (body is Player player){
			player.TakeDamage(damage);
		}	
	}
	
	// Método para recibir daño y aplicar retroceso
	public void TakeDamage(int damage)
	{
		hp -= damage;

		// Obtener la posición del jugador dinámicamente
		var player = GetNode<Player>("../Player"); // Asegúrate de que la ruta sea correcta

		// Calcula la dirección del retroceso en función de la posición del jugador
		float direction = (player.Position.X < this.Position.X) ? 1.0f : -1.0f;

		// Retroceder en la dirección opuesta al jugador
		this.Position = new Vector2(this.Position.X + (direction * 50), this.Position.Y);
	}
}
