using Godot;

[GlobalClass]
public partial class Arma : Resource
{
	[Export]
	int Dano = 1;
	[Export]
	int Alcance = 100;

	public virtual void Usar(Personagem personagem, Personagem personagemAlvo)
	{
		personagem.PrepararAtaqueCurto(personagemAlvo, Dano, Alcance);
	}
}