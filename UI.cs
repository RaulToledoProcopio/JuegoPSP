using Godot;

public partial class UI : Node2D
{
	private ProgressBar healthBar;
	private Label ammoLabel;

	public override void _Ready()
	{
		healthBar = GetNode<ProgressBar>("HealthBar");
		ammoLabel = GetNode<Label>("Ammo");

		// Referenciar este UI para que GameState lo actualice
		GameState.CurrentUI = this;

		// Mostrar valores iniciales
		UpdateHealth(GameState.Health);
		UpdateAmmo(GameState.Ammo);
	}

	public void UpdateHealth(float hp)
	{
		if (healthBar != null)
			healthBar.Value = hp;
	}

	public void UpdateAmmo(int ammo)
	{
		if (ammoLabel != null)
			ammoLabel.Text = ammo.ToString();
	}
}
