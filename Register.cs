using Godot;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public partial class Register : Godot.Control
{
	// Instancia de HttpClient
	private readonly System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();
	private Button continueButton;
	
	// URL del endpoint de registro
	private const string RegisterUrl = "https://api-psp-2.onrender.com/api/auth/register";

	private LineEdit _usernameField;
	private LineEdit _passwordField;
	private LineEdit _emailField;
	private ErrorPanel _errorPanel;

	public override void _Ready()
	{
		_usernameField = GetNode<LineEdit>("UsernameLineEdit");
		_passwordField = GetNode<LineEdit>("PasswordLineEdit");
		_emailField = GetNode<LineEdit>("EmailLineEdit");
		_errorPanel = GetNode<ErrorPanel>("ErrorPanel");
		continueButton = GetNode<Button>("RegisterButton");

		var audioManager = GetNode<AudioManager>("/root/AudioManager");
		audioManager.PlayForLevel(0);
		
		continueButton.GrabFocus();
	}

	public async void _on_register_button_pressed()
	{
		string username = _usernameField.Text;
		string password = _passwordField.Text;
		string email = _emailField.Text;

		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
		{
			_errorPanel.ShowError("Por favor, llena todos los campos.");
			return;
		}

		var registerData = new {
			username = username,
			passwordHash = password,
			email = email
		};

		try
		{
			GD.Print("Enviando datos: " + JsonSerializer.Serialize(registerData));
			HttpResponseMessage response = await _client.PostAsJsonAsync(RegisterUrl, registerData);
			GD.Print("Código de respuesta: " + response.StatusCode);

			if (response.IsSuccessStatusCode)
			{
				GD.Print("Registro exitoso");
				_errorPanel.HideError();  // Ocultamos error si estaba visible
				_errorPanel.ShowError("Registro exitoso. Ahora puedes iniciar sesión.");
			}
			else
			{
				GD.PrintErr("Error en el registro: " + response.StatusCode);
				_errorPanel.ShowError("Error: El usuario/email ya existe o hubo un problema.");
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr("Error en la solicitud HTTP: " + ex.Message);
			_errorPanel.ShowError("Error en la conexión con el servidor.");
		}
	}

	public void _on_back_pressed()
	{
		GetTree().ChangeSceneToFile("res://login.tscn");
	}
}
