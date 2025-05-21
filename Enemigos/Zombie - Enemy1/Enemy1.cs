using Godot;
using System;

public partial class Enemy1 : CharacterBody2D
{
	public const float Speed = 100.0f;  // Velocidad de movimiento del enemigo.
	private AnimatedSprite2D animation;  // Referencia a AnimatedSprite2D para animaciones.
	private Timer _deathTimer;  // Temporizador para eliminar al enemigo después de morir.
	private Vector2 _movementDirection = new Vector2(-1, 0);  // Dirección inicial del movimiento (izquierda).
	private int hp = 100;  // Puntos de vida del enemigo.
	private bool _timerStarted = false;  // Bandera para evitar que se reinicie el temporizador.
	[Export] public int damage = 10; // Daño que inflinge el enemigo
	private AudioStreamPlayer deathSound;

	public override void _Ready()
	{
		// Obtención de los nodos necesarios
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_deathTimer = GetNode<Timer>("Timer");
		
		animation.Play("Walk");// Reproducir la animación de caminar al inicio
		_deathTimer.Stop(); // Asegurar de que el Timer no esté corriendo al principio
		deathSound = GetNode<AudioStreamPlayer>("Dead");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_movementDirection != Vector2.Zero) // Verifica que haya alguna dirección de movimiento .
		{
			Vector2 velocity = Velocity; // Obtiene la velocidad actual.
			velocity.X = _movementDirection.X * Speed; // Modifica X de la velocidad según la dirección de movimiento y la velocidad máxima.
			Velocity = velocity; // Asigna la nueva velocidad al objeto, incluyendo X modificada.
			MoveAndSlide();
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

			// Detener el movimiento del enemigo
			_movementDirection = Vector2.Zero;

			// Activar el Timer de muerte.
			_deathTimer.Start(0.7f);
			_timerStarted = true;
		}

		// Si el enemigo colisiona, invierte la dirección
		if (IsOnWall())
		{
			_movementDirection.X *= -1;
			animation.FlipH = !animation.FlipH;
		}
	}

	/* Este método se llamará cuando el Timer termine para eliminar al enemigo,
	y enviará una señal a la escena para que el contador de enemigo lo reste */
	private void OnDeathTimerTimeout()
	{
		QueueFree();  // Elimina al enemigo del juego
		GetParent().CallDeferred("OnEnemyDefeated", this); // Envía la señal.
	}
	
	// Colisión con el jugador para hacerle daño.
	private void _on_body_entered(Node body)
	{
		if (body is Player player)
		{
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
