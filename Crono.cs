using Godot;
using System;

public partial class Crono : Node
{
	private float elapsedTime = 0f;
	private bool timerRunning = false;

	public override void _Process(double delta)
	{
		// Si el cronómetro está activo, se incrementa el tiempo transcurrido.
		if (timerRunning)
		{
			elapsedTime += (float)delta;
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
