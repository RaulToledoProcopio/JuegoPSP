using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

public partial class Final : Godot.Control
{
	
	// Cliente HTTP
	private readonly System.Net.Http.HttpClient _http = new System.Net.Http.HttpClient();
	private Button continueButton;

	// URL del endpoint TOP10
	private const string LeaderboardUrl = "https://api-psp-2.onrender.com/api/scores/top10";

	// Nodo para mostrar el leaderboard
	private RichTextLabel _leaderboardLabel;
	private Panel _leaderboardPanel;
	
	public override void _Ready()
	
	{
			_leaderboardLabel = GetNode<RichTextLabel>("PanelLeaderBoard/LeaderBoard");
			_leaderboardLabel.Visible = false;
			_leaderboardPanel = GetNode<Panel>("PanelLeaderBoard");
			_leaderboardPanel.Visible = false;
			continueButton = GetNode<Button>("VBoxContainer/Button");
			
			continueButton.GrabFocus();
	}

	// Cambiar de escena al pulsar el primer botón del menú
	private void _on_button_pressed(){
		GameState.Reset();
		GetTree().ChangeSceneToFile("res://Nivel/Nivel1/Nivel1.tscn");
	}
	
	private void _on_button_2_pressed(){
		GetTree().ChangeSceneToFile("res://Menú/Menu.tscn");
	}
	
	private void _on_button_3_pressed(){
		GetTree().Quit();
	}
	
	private void _on_button_4_pressed(){
		GetTree().ChangeSceneToFile("res://Menú/Incoming.tscn");
	}
	
	private void _on_button_5_pressed()
	{
		_ = LoadAndShowLeaderboardAsync();
	}
	
	private void _on_close_button_pressed()
	{
	_leaderboardPanel.Visible = false;
	_leaderboardLabel.Visible = false;
	continueButton.GrabFocus();
	}

	private async Task LoadAndShowLeaderboardAsync()
	{
		try
		{
			// Hacemos la petición GET y deserializamos la lista de ScoreDto
			var scores = await _http.GetFromJsonAsync<List<ScoreDto>>(LeaderboardUrl);
			if (scores == null || scores.Count == 0)
			{
				_leaderboardLabel.Text = "No hay puntuaciones aún.";
			}
			else
			{
				// Construimos el texto con formato
				var text = "";
				int rank = 1;
				foreach (var s in scores)
				{
					// Convertimos timeSeconds a MM:SS:DS
					int minutes = (int)(s.TimeSeconds / 60);
					int seconds = (int)(s.TimeSeconds % 60);
					int decimas = (int)((s.TimeSeconds % 1) * 10);
					text += $"{rank}. {s.Username} — {minutes:D2}:{seconds:D2}:{decimas:D2}\n";
					rank++;
				}
				_leaderboardLabel.Text = text;
			}
		}
		catch (Exception ex)
		{
			_leaderboardLabel.Text = "Error al cargar leaderboard:\n" + ex.Message;
		}

		// Mostramos el label
		_leaderboardLabel.Visible = true;
		_leaderboardPanel.Visible = true;
	}
	
	// DTO interno para los scores
	private class ScoreDto
	{
		public string Username { get; set; }
		public double TimeSeconds { get; set; }
	}
}
