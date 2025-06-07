using Godot;
using System;

public partial class Lightning : Area2D
{
	[Export] public int Damage = 100;
	private AnimatedSprite2D _animation;

	public override void _Ready()
	{
		_animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animation.Play("Idle");
		_animation.AnimationFinished += OnAnimationFinished;
		
	}

	private void OnAnimationFinished()
	{
		QueueFree();
	}

	private void _on_body_entered(Node body)
{
	GD.Print($"Colisión con nodo: {body.Name}, tipo: {body.GetType()}");

	if (body is Player player)
	{
		GD.Print("Jugador detectado, aplicando daño.");
		player.TakeDamage(Damage);
		QueueFree();
	}
	else
	{
		GD.Print("El nodo colisionado NO es Player.");
	}
}

}
