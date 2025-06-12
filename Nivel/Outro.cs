using Godot;
using System;
using System.Collections.Generic;

public partial class Outro : Control
{
	private TextureRect imageDisplay;
	private Label narrationLabel;
	private Timer sceneTimer;

	private List<Texture2D> images = new();
	private List<string> texts = new();
	private int currentIndex = 0;

	public override void _Ready()
	{
		// Referencias a los nodos
		imageDisplay = GetNode<TextureRect>("TextureRect");
		narrationLabel = GetNode<Label>("Label");
		sceneTimer = GetNode<Timer>("Timer");

		// Cargar imágenes y textos
		images.Add(GD.Load<Texture2D>("res://Nivel/frame1.png"));

		texts.Add("Joder! Qué ha sido eso? No vuelvo a cenar fentanilo");

		// Mostrar primera imagen y texto
		ShowFrame(currentIndex);

		// Conectar el Timer
		sceneTimer.Timeout += OnTimerTimeout;
		sceneTimer.WaitTime = 5.0;
		sceneTimer.Start();
	}

	private void ShowFrame(int index)
	{
		imageDisplay.Texture = images[index];
		narrationLabel.Text = texts[index];
	}

	private void OnTimerTimeout()
	{
		currentIndex++;
		if (currentIndex < images.Count)
		{
			ShowFrame(currentIndex);
			sceneTimer.Start();
		}
		else
		{
			
			GD.Print("Corte finalizado.");
			GetTree().ChangeSceneToFile("res://Menú/Credits.tscn");
		}
	}
}
