using Godot;
using System;

public partial class Arrow : Area2D
{
	[Export] public float speedDagger = 2500f;  // Velocidad de la flecha
	[Export] public int damage = 25;  // Daño que inflige la flecha

	private Vector2 velocity = new Vector2(1, 0);  // Dirección de movimiento de la flecha
	private AnimatedSprite2D animation;  // Referencia al AnimatedSprite2D para controlar las animaciones de la flecha
	public override void _Ready()
	{
		velocity = new Vector2(speedDagger, 0); // Establece la dirección de flecha.
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); // Buscar el AnimatedSprite2D para poder controlar la animación.
		animation.Play("Default");
	}

	public override void _Process(double delta)
	{
		Position += velocity * (float)delta; // Mueve la flecha en la dirección determinada por 'velocity'.
		// Si la flecha sale de la pantalla, la eliminamos del juego.
		if (Position.X > GetViewportRect().Size.X || Position.Y > GetViewportRect().Size.Y || Position.X < 0 || Position.Y < 0)
		{
			QueueFree();
		}
	}
	
	// Método para que la flecha haga daño.
	private void _on_body_entered(Node body)
	{
		if (body is Player player)
		{
			player.TakeDamage(damage);
			QueueFree();
		}
	}
}
