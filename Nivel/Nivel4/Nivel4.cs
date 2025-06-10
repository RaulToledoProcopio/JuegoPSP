using Godot;
using System.Collections.Generic;

public partial class Nivel4 : TileMapLayer
{
	private Area2D portal;
	private AnimatedSprite2D portalSprite;
	private List<Node> enemies;

	public override void _Ready()
	{
		portal = GetNode<Area2D>("Portal");
		portal.Visible = false;
		var collisionShape = portal.GetNode<CollisionShape2D>("CollisionShape2D");
		collisionShape.Disabled = true;

		portalSprite = portal.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		enemies = new List<Node>();
		foreach (Node enemy in GetTree().GetNodesInGroup("Enemies"))
		{
			enemies.Add(enemy);
		}
	}

	private void OnEnemyDefeated(Node enemy)
	{
		enemies.Remove(enemy);

		if (enemies.Count == 0)
		{
			GD.Print("Todos los enemigos han sido derrotados. Activando portal.");
			portal.Visible = true;
			var collisionShape = portal.GetNode<CollisionShape2D>("CollisionShape2D");
			collisionShape.Disabled = false;
			portalSprite.Play("Idle");
			var portalAudio = portal.GetNode<AudioStreamPlayer>("Portal");
			portalAudio?.Play();
		}
	}
}
