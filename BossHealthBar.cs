using Godot;
using System;

public partial class BossHealthBar : Node2D
{
	private ProgressBar healthBar;

	public override void _Ready()
	{
		healthBar = GetNode<ProgressBar>("ProgressBar");
		var redStyle = new StyleBoxFlat();
		redStyle.BgColor = new Color(0.525f, 0.114f, 0.114f);
		
		redStyle.CornerRadiusTopLeft = 8;
		redStyle.CornerRadiusTopRight = 8;
		redStyle.CornerRadiusBottomLeft = 8;
		redStyle.CornerRadiusBottomRight = 8;
		
		redStyle.ShadowSize = 3;
		redStyle.ShadowColor = new Color(0, 0, 0, 0.5f);
		
		healthBar.AddThemeStyleboxOverride("fill", redStyle);
	}

	public void UpdateHealth(float hp)
	{
		if (healthBar != null)
			healthBar.Value = hp;
	}
}
