using Godot;
using System.Collections.Generic;

public partial class AudioManager : Node2D
{
	private readonly Dictionary<int, AudioStream> _tracks = new()
	{
		{ 1, ResourceLoader.Load<AudioStream>("res://BSO/2-Fase1.mp3")! },
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
		return (level == 2 || level == 3) ? 1 : 0;
	}
}
