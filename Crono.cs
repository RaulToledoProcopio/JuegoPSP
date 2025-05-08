using Godot;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public partial class Crono : Node
{
	private float elapsedTime = 0f;
	private bool timerRunning = false;
	private bool canSubmitScore = false;  // Controla si se puede enviar la puntuación

	private readonly System.Net.Http.HttpClient _http = new System.Net.Http.HttpClient();

	public override void _Process(double delta)
	{
		if (timerRunning)
		{
			elapsedTime += (float)delta;

			// Formateamos el tiempo en MM:SS:DS
			int minutes = (int)(elapsedTime / 60);
			int seconds = (int)(elapsedTime % 60);
			int decimas = (int)((elapsedTime % 1) * 10);

			var tiempoLabel = GetNode<Label>("/root/CronometroUI/CronoUI");
			if (tiempoLabel != null)
				tiempoLabel.Text = $"{minutes:D2}:{seconds:D2}:{decimas:D2}";
		}
	}

	public void StartTimer()
	{
		elapsedTime = 0f;
		timerRunning = true;
		canSubmitScore = false;  // No se puede enviar puntuación aún
		GD.Print("Cronómetro iniciado.");
	}

	public void StopTimer(bool submitScore = false)
	{
		timerRunning = false;
		if (submitScore && canSubmitScore)  // Solo enviar puntuación si se debe
		{
			GD.Print("Cronómetro detenido: " + elapsedTime + " segundos.");
			_ = SubmitScoreAsync(elapsedTime);
		}
	}
	
	public void ResetTimer()
	{
		elapsedTime = 0f;
		GD.Print("Cronómetro reiniciado.");

		var tiempoLabel = GetNode<Label>("/root/CronometroUI/CronoUI");
		if (tiempoLabel != null)
		{
			tiempoLabel.Text = "00:00:00";
		}
	}

	public void HideTimer()
	{
		var tiempoLabel = GetNode<Label>("/root/CronometroUI/CronoUI");
		if (tiempoLabel != null)
		{
			tiempoLabel.Visible = false;
		}
		GD.Print("Cronómetro ocultado.");
	}

	public void SetCanSubmitScore(bool value)
	{
		canSubmitScore = value;  // Activar o desactivar el envío de puntuaciones
	}

	public float GetElapsedTime() => elapsedTime;

	private async Task SubmitScoreAsync(float timeSeconds)
	{
		// Recuperar el username del Autoload SessionManager
		var session = GetNode<SessionManager>("/root/SessionManager");
		string username = session?.Username;
		if (string.IsNullOrEmpty(username))
		{
			GD.PrintErr("SessionManager.Username vacío, no se envía puntuación.");
			return;
		}

		// Construir el DTO anónimo y enviarlo
		var scoreDto = new
		{
			username = username,
			timeSeconds = timeSeconds
		};

		try
		{
			var url = "https://api-psp-2.onrender.com/api/scores"; 
			var resp = await _http.PostAsJsonAsync(url, scoreDto);
			if (resp.IsSuccessStatusCode)
				GD.Print("Puntuación enviada OK.");
			else
				GD.PrintErr($"Error al enviar score: {resp.StatusCode}");
		}
		catch (Exception ex)
		{
			GD.PrintErr("Excepción al enviar score: " + ex.Message);
		}
	}
}
