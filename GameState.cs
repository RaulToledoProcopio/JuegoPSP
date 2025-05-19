using Godot;

public partial class GameState : Node
{
	// Vida y munición globales persistentes
	public static float Health = 100f;
	public static int Ammo = 10;

	// Para que el UI pueda refrescarse al cambiar valores
	public static UI CurrentUI;

	public override void _Ready()
	{
		// Nada aquí, sólo almacena datos
	}

	// Métodos para cambiar valores y actualizar UI
	public static void ChangeHp(float amount)
	{
		Health = Mathf.Clamp(Health + amount, 0, 100);
		CurrentUI?.UpdateHealth(Health);
	}

	public static void ChangeAmmo(int amount)
	{
		Ammo = Mathf.Max(Ammo + amount, 0);
		CurrentUI?.UpdateAmmo(Ammo);
	}
}
