using Godot;
using System;

public partial class Dummy : CharacterBody2D
{
	private int hp = 100; // Puntos de vida del enemigo.
	private AnimatedSprite2D animation; // Referencia a AnimatedSprite2D para animaciones.
	
	
	public override void _Ready()
	{
		// Obtención de los nodos necesarios
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animation.Play("Walk");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (hp <= 0)
		{
			OnDeathTimerTimeout();
		}
	}
	
	/* Este método se llamará cuando el Timer termine para eliminar al enemigo,
	y enviará una señal a la escena para que el contador de enemigo lo reste */
	private void OnDeathTimerTimeout()
	{
		QueueFree();  // Elimina al enemigo del juego
		GetParent().CallDeferred("OnEnemyDefeated", this); // Envía la señal.
	}
	
	// Método para recibir daño.
	public void TakeDamage(int damage)
	{
		hp -= damage;
	}
}
