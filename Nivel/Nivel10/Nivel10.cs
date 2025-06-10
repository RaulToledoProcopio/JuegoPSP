using Godot;
using System.Collections.Generic;

public partial class Nivel10 : TileMapLayer
{
	private Area2D portal;       // Referencia al portal
	private AnimatedSprite2D portalSprite; // Referencia al AnimatedSprite2D del portal
	private List<Node> enemies; // Lista para rastrear enemigos

	public override void _Ready()
	{
		// Encuentra el nodo del portal y ocúltalo inicialmente
		portal = GetNode<Area2D>("Portal");
		portal.Visible = false; // Lo ocultamos inicialmente
		// Desactivar la colisión para que el portal no interactúe con el jugador
		var collisionShape = portal.GetNode<CollisionShape2D>("CollisionShape2D");
		collisionShape.Disabled = true;  // Desactivar la forma de colisión

		// Encuentra el AnimatedSprite2D dentro del portal
		portalSprite = portal.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Obtén todos los nodos en el grupo "Enemies"
		enemies = new List<Node>();
		foreach (Node enemy in GetTree().GetNodesInGroup("Enemies"))
		{
			enemies.Add(enemy);
		}
	}

	private void OnEnemyDefeated(Node enemy)
	{
		// Cuando un enemigo es eliminado, lo quitamos de la lista
		enemies.Remove(enemy);

		// Si ya no quedan enemigos, activamos el portal
		if (enemies.Count == 0)
		{
			GD.Print("Todos los enemigos han sido derrotados. Activando portal.");
			portal.Visible = true; // Mostramos el portal
			// Reactivar la colisión para que el portal pueda interactuar con el jugador
			var collisionShape = portal.GetNode<CollisionShape2D>("CollisionShape2D");
			collisionShape.Disabled = false;  // Reactivar la forma de colisión
			portalSprite.Play("Idle"); // Reproduce la animación desde el AnimatedSprite2D
			var portalAudio = portal.GetNode<AudioStreamPlayer>("Portal");
			portalAudio?.Play();
		}
	}
}
