using Godot;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public partial class Login : Godot.Control
{
	// Instancia de HttpClient
	private readonly System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient();

	// URL del endpoint de login
	private const string LoginUrl = "https://api-psp-2.onrender.com/api/auth/login";

	// Referencias a los nodos de la UI (asegúrate de asignarlos en el editor o mediante GetNode)
	private LineEdit _usernameField;
	private LineEdit _passwordField;
	private Label _errorLabel;

	public override void _Ready()
	{
		_usernameField = GetNode<LineEdit>("UsernameLineEdit");
		_passwordField = GetNode<LineEdit>("PasswordLineEdit");
		_errorLabel = GetNode<Label>("ErrorLabel");
		var audioManager = GetNode<AudioManager>("/root/AudioManager");
		audioManager.PlayForLevel(0);
	}

	// Método que se llama al presionar el botón de login
	public async void _on_login_button_pressed()
	{
		string username = _usernameField.Text;
		string password = _passwordField.Text;

		// Crea el objeto que se enviará en el JSON.
		var loginData = new {
			username = username,
			passwordHash = password
		};

		try
		{
			// Envía la petición POST de forma asíncrona
			HttpResponseMessage response = await _client.PostAsJsonAsync(LoginUrl, loginData);
			if (response.IsSuccessStatusCode)
			{
				// Si la respuesta es exitosa, carga la escena del menú
				GD.Print("Login exitoso");
				var session = GetNode<SessionManager>("/root/SessionManager");
					session.Username = username;
				GetTree().ChangeSceneToFile("res://Menú/Menu.tscn");
			}
			else
			{
				// Si el usuario no está registrado o hubo otro error, muestra el mensaje
				GD.PrintErr("Error en el login: " + response.StatusCode);
				_errorLabel.Text = "Usuario no registrado o credenciales incorrectas.";
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr("Error en la solicitud HTTP: " + ex.Message);
			_errorLabel.Text = "Error en la conexión con el servidor.";
		}
	}
	
	private void _on_register_button_pressed()
	{
		GD.Print("Cambiando a la escena de registro...");
		GetTree().ChangeSceneToFile("res://register.tscn");
	}
}
