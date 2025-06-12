using Godot;
using System.Collections.Generic;

public partial class AudioManager : Node2D
{
	private readonly Dictionary<int, AudioStream> _tracks = new()
	{
		{ 1, ResourceLoader.Load<AudioStream>("res://BSO/1-Intro.mp3")! },
		{ 2, ResourceLoader.Load<AudioStream>("res://BSO/2-Fase1.mp3")! },
		{ 3, ResourceLoader.Load<AudioStream>("res://BSO/3-Fase2.mp3")! },
		{ 4, ResourceLoader.Load<AudioStream>("res://BSO/11-Fase10.mp3")! },
		{ 5, ResourceLoader.Load<AudioStream>("res://BSO/4-Fase3.mp3")! },
		{ 6, ResourceLoader.Load<AudioStream>("res://BSO/10-Fase9.mp3")! },
		{ 7, ResourceLoader.Load<AudioStream>("res://BSO/12-Créditos.mp3")! },
		{ 8, ResourceLoader.Load<AudioStream>("res://BSO/Cinemática.mp3")! },
		
	};

	private int _currentGroup = 0;
	private AudioStreamPlayer _player = null!;

	public override void _Ready()
	{
		_player = GetNode<AudioStreamPlayer>("Audio");
	}

	public void PlayForLevel(int level)
	{
		int group = GetGroupForLevel(level);
		if (group == _currentGroup)
			return;

		_currentGroup = group;
		if (_tracks.TryGetValue(group, out var stream))
		{
			_player.Stream = stream;
			_player.Play();
		}
	}
	
	public void StopMusic()
	{
			_currentGroup = 0;
			_player.Stop();
	}


	private int GetGroupForLevel(int level)
	{
		if (level == 0)
		return 1;
		
		if (level == 2 || level == 3)
		return 2;

		if (level == 4 || level == 5)
		return 3;

		if (level == 6 || level == 7)
		return 4;

		if (level == 8 || level == 9)
		return 5;

		if (level == 10)
		return 6;
		
		if (level == 98)
		return 8;
		
		if (level == 99)
		return 7;

		return 0;
	}
	
	public float VolumeDb
	{
		get => _player.VolumeDb;
		set => _player.VolumeDb = value;
	}

	public bool Muted
	{
		get => _player.VolumeDb <= -80f;
		set => _player.VolumeDb = value ? -80f : 0f;
	}
}
