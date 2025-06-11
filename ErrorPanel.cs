using Godot;
using System;

public partial class ErrorPanel : PanelContainer
{
	private Label _label;

	public override void _Ready()
	{
		var style = new StyleBoxFlat();
		style.BgColor = new Color(0, 0, 0, 0.4f);
		style.SetCornerRadiusAll(6);
		style.ContentMarginLeft = 10;
		style.ContentMarginRight = 10;
		style.ContentMarginTop = 5;
		style.ContentMarginBottom = 5;

		AddThemeStyleboxOverride("panel", style);

		_label = GetNode<Label>("CenterContainer/ErrorLabel");
		_label.HorizontalAlignment = HorizontalAlignment.Center;
		_label.VerticalAlignment = VerticalAlignment.Center;
		_label.SizeFlagsHorizontal = SizeFlags.ExpandFill;

		Visible = false; // Oculto al inicio
	}

	public void ShowError(string message)
	{
		_label.Text = message;
		Visible = true;
	}

	public void HideError()
	{
		_label.Text = "";
		Visible = false;
	}
}
