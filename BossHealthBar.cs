using Godot;
using System;

public partial class BossHealthBar : Node2D
{
	private ProgressBar healthBar;

	public override void _Ready()
	{
		healthBar = GetNode<ProgressBar>("ProgressBar");
		StyleBoxFlat redStyle = new StyleBoxFlat();
		redStyle.BgColor = new Color(1, 0, 0); // Color de la barra
		healthBar.AddThemeStyleboxOverride("fill", redStyle);
	}

	public void UpdateHealth(float hp)
	{
		if (healthBar != null)
			healthBar.Value = hp;
	}
}
