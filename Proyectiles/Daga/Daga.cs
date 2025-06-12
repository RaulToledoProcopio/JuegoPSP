using Godot;
using System;

public partial class Daga : Area2D
{
	[Export] public float speedDagger = 500f; // Velocidad horizontal de la daga.
	[Export] public int damage = 25; // Cantidad de daño que inflige.
	

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		// Mueve la daga horizontalmente.
		Position += new Vector2(speedDagger * (float)delta, 0.0f); // Desplazamiento en el eje X.
	}
	
	private void _on_body_entered(Node body)
	{
		
		if (body is Enemy1 enemy1) // Si colisiona con un nodo de tipo Enemy1:
		{
			enemy1.TakeDamage(damage); // Inflige daño al enemigo.
			QueueFree(); // Elimina la daga de la escena.
		}
		else if (body is Enemy2 enemy2)
		{
			enemy2.TakeDamage(damage);
			QueueFree();
		}
		else if (body is Enemy3 enemy3)
		{
			enemy3.TakeDamage(damage); 
			QueueFree(); // 
		}
		else if (body is Enemy4 enemy4)
		{
			enemy4.TakeDamage(damage); 
			QueueFree(); // 
		}
		else if (body is Enemy5 enemy5)
		{
			enemy5.TakeDamage(damage);
			QueueFree();
		}
		else if (body is Enemy6 enemy6)
		{
			enemy6.TakeDamage(damage);
			QueueFree();
		}
		else if (body is Enemy7 enemy7)
		{
			enemy7.TakeDamage(damage);
			QueueFree();
		}
		else if (body is Miniboss miniboss)
		{
			miniboss.TakeDamage(damage);
			QueueFree();
		}
		else if (body is Boss boss)
		{
			boss.TakeDamage(damage);
			QueueFree();
		}
		else if (body is Dummy dummy)
		{
			dummy.TakeDamage(damage); 
			QueueFree(); // 
		}
	}
}
