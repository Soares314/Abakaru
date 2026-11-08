using Godot;

// Quando o personagem tiver euqipado com esse, vai valer como se tivesse sem arma
[GlobalClass]
public partial class Arma : Resource
{
	[Export]
	int Dano = 1;
	[Export]
	int Alcance = 100;

	public virtual void Usar(Personagem personagem, Personagem personagemAlvo)
	{
		personagem.PrepararAtaqueCurto(personagemAlvo, Dano * personagem.StatusAtual["Força"], Alcance);
	}
}