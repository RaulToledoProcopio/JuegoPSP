using Godot;
using System;

public partial class Espada : Area2D
{
	[Export] public int damage = 25;  // Daño que inflige la espada.
	private bool _isActive = false;  // Para saber si el área de la espada está activa.

	public override void _Ready()
	{
		// Desactivamos el área de colisión de la espada.
		Visible = false;  // Hacemos la espada invisible al inicio.
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;  // Desactivamos la colisión de la espada al inicio.
	}

	public void ActivarEspada()
	{
		Visible = true;  // Hacemos visible la espada cuando se activa.
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;  // Activamos la colisión de la espada.
	}


	/* Para desactivar la espada creamos un método para llamarlo de manera diferida al igual que en 
	los portales porque tiraba un error flushing queries por la sincronización de las físicas*/
	public void DesactivarEspada()
	{
		CallDeferred(nameof(DeferredDesactivarEspada));
	}

	private void DeferredDesactivarEspada()
	{
		Visible = false;  // Hacemos invisible la espada.
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;  // Desactivamos la colisión de la espada.
	}

	// Método para hacer daño a los enemigos con el área de la espada.
	private void _on_body_entered(Node body)
	{
		if (body is Enemy1 enemy1)  // Si el cuerpo es un enemigo del tipo 'Enemy1'.
		{
			enemy1.TakeDamage(damage);  // Inflige daño al enemigo.
			DesactivarEspada();  // Desactiva el área de la espada después de infligir el daño.
		}
		else if (body is Enemy2 enemy2)
		{
			enemy2.TakeDamage(damage);
			DesactivarEspada();
		}
		else if (body is Enemy3 enemy3)
		{
			enemy3.TakeDamage(damage);
			DesactivarEspada();
		}
	}
}
