using Godot;

public partial class GameState : Node
{
	// Vida y munición globales persistentes
	public static float Health = 100f;
	public static int Ammo = 10;
	public static bool DiedByFall = false; // Quiero controlar esto para evitar que el sonido de muerte se solape con el de caída.

	// Para que el UI pueda refrescarse al cambiar valores
	public static UI CurrentUI;

	public override void _Ready()
	{
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
	
	public static void Reset()
	{
	Health = 100f;
	Ammo = 10;
	DiedByFall = false;
	}
}
