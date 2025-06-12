# API PSP

## Nombre del Proyecto
API de PSP para el videojuego de GODOT

## Descripción del Proyecto
1. Registrar usuarios y autenticarles desde Godot.
2. Guardar y recuperar las puntuaciones de las partidas jugadas.

### Documentos y sus Campos

1. **Usuario**

   **Usuario**
   - `id` : Identificador único generado por MongoDB.
   - `username` : Nombre del usuario.
   - `passwordHash` : Contraseña cifrada con SHA-256 y codificada en base64.
   - `email` : Correo electrónico del usuario.

   **Score**
   - `id` : Identificador único generado por MongoDB.
   - `username` : Nombre del usuario que jugó.
   - `timeSeconds` : Tiempo en segundos obtenido en la partida.
   - `playedAt` : Fecha y hora (UTC) en que se jugó la partida.

---

## DTOs

Para login
```csharp
public class LoginDto
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
}
```
Para enviar puntuaciones
```csharp
public class ScoreDto
{
    public string Username { get; set; }
    public double TimeSeconds { get; set; }
}
```
## Endpoints de la API

### **Autenticación** (`/api/auth`)

| Método | Ruta         | Descripción                              | Request Body                        | Response                                       |
| ------ | ------------ |------------------------------------------| ----------------------------------- | ---------------------------------------------- |
| POST   | `/register`  | Registra un nuevo usuario.               | `{ "username", "passwordHash", "email" }` | `200 OK` “Usuario registrado” / `400 Bad Request` |
| POST   | `/login`     | Autentica un usuario y devuelve un token | `{ "username", "passwordHash" }`    | `200 OK` “Login correcto” / `401 Unauthorized` |

### **Puntuaciones** (`/api/scores`)

| Método | Ruta               | Descripción                                                 | Request Body              | Response                        |
| ------ | ------------------ | ----------------------------------------------------------- | ------------------------- | ------------------------------- |
| POST   | `/api/scores`      | Envía una nueva puntuación.                                 | `{ "username", "timeSeconds" }`| `200 OK` “Score añadido” / `400 Bad Request` |
| GET    | `/api/scores/top10`| Recupera el Top 10 de mejores tiempos. |                           | `200 OK` List<Score>            |

---

## PRUEBAS GESTIÓN USUARIOS

### Registrar un Usuario

- En este caso vamos a registrar un usuario mediante insomnia a nivel local antes de desplegar la API

<img src="API/Images/UsuarioRegistrado.png" alt="UsuarioRegistrado" width="1000"/>


### Logear con un Usuario

- En este caso vamos a logearnos con un usuario a nivel local.

<img src="API/Images/LoginUsuario.png" alt="LoginUsuario" width="1000"/>

### Aplicación desplegada en Render

<img src="API/Images/ApiRender.png" alt="ApiRender" width="1000"/>

### Registrar un Usuario en Render

- En este caso vamos a registrar un usuario mediante insomnia con la API desplegada

<img src="API/Images/RegisterRender.png" alt="RegisterRender" width="1000"/>
<img src="API/Images/RegisterRender2.png" alt="RegisterRender2" width="1000"/>

### Logear con un Usuario en Render

- En este caso vamos a logear un usuario mediante insomnia con la API desplegada

<img src="API/Images/LoginRender.png" alt="LoginRender" width="1000"/>


## PRUEBAS EN EL JUEGO

- En este caso hemos creado una pantalla para el login y otra para el registro

### Registrar un Usuario

- Vamos a registrar a un Usuario correctamente

<img src="API/Images/RegistroCorrecto.png" alt="RegistroCorrecto" width="1000"/>
<img src="API/Images/RegistroCorrectoDB.png" alt="RegistroCorrectoDB" width="1000"/>


- Vamos a registrar a un Usuario con algún campo vacío

<img src="API/Images/RegistroIncorrecto.png" alt="RegistroIncorrecto" width="1000"/>

- Vamos a registrar a un Usuario con un Username/email que ya existe

<img src="API/Images/RegistroIncorrecto2.png" alt="RegistroIncorrecto2" width="1000"/>

### Logear con un Usuario

- Hemos logeado con un Usuario correctamente, lo que nos lleva al menú del juego.

<img src="API/Images/LoginCorrecto.png" alt="LoginCorrecto" width="1000"/>

- Vamos a logear con un Usuario vacío o que no exista

<img src="API/Images/LoginIncorrecto.png" alt="LoginIncorrecto" width="1000"/>

## Prueba de gestión de puntuaciones

### Enviar puntuaciones en Insomnia

<img src="API/Images/ScoreInsomnia.png" alt="RegistroCorrecto" width="1000"/>

### Enviar puntuaciones sin cronómetro

<img src="API/Images/ScoreInsomniaB.png" alt="RegistroCorrecto" width="1000"/>

### Traer el Top10 en Insomnia

<img src="API/Images/Top10Insomnia.png" alt="RegistroCorrecto" width="1000"/>

### Vista dentro del juego

<img src="API/Images/Top10Juego.png" alt="RegistroCorrecto" width="1000"/>
