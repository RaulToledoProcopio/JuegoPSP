using Godot;
using System;

public partial class Enemy2 : CharacterBody2D
{
	[Export] public PackedScene FireBallScene;  // Referencia a la escena de la bola de fuego.
	private float fireRate = 1.5f;  // Tiempo entre disparos.
	private float timeSinceLastFire = 0.0f;  // Tiempo desde el último disparo.
	private int hp = 100; // Puntos de vida del enemigo.
	private Timer _deathTimer; // Temporizador para eliminar al enemigo después de morir.
	private bool _timerStarted = false; // Bandera para evitar que se reinicie el temporizador.
	private AnimatedSprite2D animation; // Referencia a AnimatedSprite2D para animaciones.
	
	
	public override void _Ready()
	{
		// Obtención de los nodos necesarios
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_deathTimer = GetNode<Timer>("Timer");
		_deathTimer.Stop(); // Asegurar de que el Timer no esté corriendo al principio
	}

	public override void _PhysicsProcess(double delta)
	{

		timeSinceLastFire += (float)delta; // Sumar el tiempo transcurrido desde el último cuadro.

		// Verifica si ha pasado el tiempo necesario para disparar una nueva bola de fuego.
		if (timeSinceLastFire >= fireRate)
		{
			timeSinceLastFire = 0.0f;  // Reiniciar el contador.
			ShootFireBall();  // Llamar al método para disparar la bola de fuego.
		}
		
		if (hp <= 0 && !_timerStarted)
		{
			// Si no se está ejecutando la animación la ejecuta, para evitar que se reinicie.
			if (animation.Animation != "Death")
			{
				animation.Play("Death");
			}
			// Activar el Timer de muerte.
			_deathTimer.Start(0.7f);
			_timerStarted = true;
		}
		
	}

	// Método para disparar la bola de fuego.
	private void ShootFireBall()
	{
		// Instanciamos la bola de fuego a partir de la escena.
		FireBall fireBallInstance = (FireBall) FireBallScene.Instantiate();
		// Ajusta la dirección de la velocidad de la bola de fuego según si la animación está volteada horizontalmente.
		fireBallInstance.speedDagger = (animation.FlipH ? -fireBallInstance.speedDagger : fireBallInstance.speedDagger); 
		// Hacer un cast a Node2D para poder usar la propiedad Position.
		Node2D fireBallNode = fireBallInstance as Node2D;
		// Establece la posición de la bola de fuego en relación con la posición actual del personaje.
		fireBallNode.Position = Position + new Vector2(animation.FlipH ? 15 : -15, -15);
		// Añadimos la bola de fuego al árbol de nodos del juego.
		GetParent().AddChild(fireBallInstance);
	}
	
	private void OnDeathTimerTimeout()
	{
		QueueFree(); // Eliminar al enemigo del juego.
		GetParent().CallDeferred("OnEnemyDefeated", this); // Envía la señal.
	}
	
	// Método para recibir daño.
	public void TakeDamage(int damage)
	{
		hp -= damage;
	}
}
