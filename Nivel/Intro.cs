using Godot;
using System;
using System.Collections.Generic;

public partial class Intro : Control
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
		images.Add(GD.Load<Texture2D>("res://Menú/frame1.png"));
		images.Add(GD.Load<Texture2D>("res://Menú/frame2.png"));
		images.Add(GD.Load<Texture2D>("res://Menú/frame3.png"));

		texts.Add("Era una noche tranquila, nuestro héroe dormía placidamente");
		texts.Add("Cuando ¡Un ogro apareció de un portal!");
		texts.Add("¡Y lo arrojó sin piedad al abismo!");

		// Mostrar primera imagen y texto
		ShowFrame(currentIndex);

		// Conectar el Timer
		sceneTimer.Timeout += OnTimerTimeout;
		sceneTimer.WaitTime = 5.0;
		sceneTimer.Start();
	}

	private void ShowFrame(int index)
	{
		var audioManager = GetNode<AudioManager>("/root/AudioManager");
		audioManager.PlayForLevel(98);
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
			GetTree().ChangeSceneToFile("res://Nivel/Nivel1/Nivel1.tscn");
		}
	}
}
