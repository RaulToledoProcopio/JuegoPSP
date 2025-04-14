using Godot;
using System;

public partial class Crono : Node
{
	private float elapsedTime = 0f;
	private bool timerRunning = false;

	public override void _Process(double delta)
	{
		// Si el cronómetro está activo, se incrementa.
		if (timerRunning)
		{
			elapsedTime += (float)delta;

			// Formateamos el tiempo
			int minutes = (int)(elapsedTime / 60);  // Minutos
			int seconds = (int)(elapsedTime % 60);  // Segundos
			int decimas = (int)((elapsedTime % 1) * 10); // Décimas

			var tiempoLabel = GetNode<Label>("/root/CronometroUI/CronoUI");
			
			tiempoLabel.Text = $"{minutes:D2}:{seconds:D2}:{decimas:D2}";
			
		}
	}

	public void StartTimer()
	{
		elapsedTime = 0f;
		timerRunning = true;
		GD.Print("Cronómetro iniciado.");
	}

	public void StopTimer()
	{
		timerRunning = false;
		GD.Print("Cronómetro detenido: " + elapsedTime + " segundos.");
	}

	public float GetElapsedTime() => elapsedTime;
}
