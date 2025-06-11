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
	private AudioStreamPlayer hitSound;
	private Vector2 _knockbackVelocity = Vector2.Zero;
	private bool _isKnockedBack = false;
	private const float KnockBackSpeed    = 200f;  // fuerza horizontal
	private const float KnockBackUpForce  = 100f;  // fuerza vertical
	private const float Gravity           = 1000f;  // gravedad

	public override void _Ready()
	{
		// Obtención de los nodos necesarios
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_deathTimer = GetNode<Timer>("Timer");
		
		animation.Play("Walk");// Reproducir la animación de caminar al inicio
		_deathTimer.Stop(); // Asegurar de que el Timer no esté corriendo al principio
		deathSound = GetNode<AudioStreamPlayer>("Dead");
		hitSound = GetNode<AudioStreamPlayer>("Hit");
	}

	public override void _PhysicsProcess(double delta)
{
	if (_isKnockedBack)
	{
		// Aplica gravedad al retroceso
		_knockbackVelocity.Y += Gravity * (float)delta;
		Velocity = _knockbackVelocity;
		MoveAndSlide();

		// Cuando tocas el suelo, terminas el knockback
		if (IsOnFloor())
		{
			_isKnockedBack = false;
			_knockbackVelocity = Vector2.Zero;
		}
		return; // no ejecutas el movimiento normal mientras haces knockback
	}

	// —— Aquí va tu movimiento normal —— 
	if (_movementDirection != Vector2.Zero)
	{
		Vector2 velocity = Velocity;
		velocity.X = _movementDirection.X * Speed;
		Velocity = velocity;
		MoveAndSlide();
	}

	// Animación y timer de muerte
	if (hp <= 0 && !_timerStarted)
	{
		if (animation.Animation != "Death")
		{
			animation.Play("Death");
			deathSound?.Play();
		}
		_movementDirection = Vector2.Zero;
		_deathTimer.Start(0.7f);
		_timerStarted = true;
	}

	// Rebote en paredes
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

	// Calcula la dirección: 1 si el player está a la izquierda, -1 si está a la derecha
	var player = GetNode<Player>("../Player");
	float direction = (player.Position.X < Position.X) ? 1f : -1f;

	hitSound?.Play();
	_knockbackVelocity = new Vector2(direction * KnockBackSpeed, -KnockBackUpForce);
	_isKnockedBack = true;
}

}
