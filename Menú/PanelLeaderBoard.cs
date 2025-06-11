using Godot;
using System;

public partial class PanelLeaderBoard : Panel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var style = new StyleBoxFlat();
		style.BgColor = new Color(0, 0, 0, 0.7f);
		style.SetCornerRadiusAll(6);
		style.ContentMarginLeft = 10;
		style.ContentMarginRight = 10;
		style.ContentMarginTop = 5;
		style.ContentMarginBottom = 5;

		AddThemeStyleboxOverride("panel", style);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
