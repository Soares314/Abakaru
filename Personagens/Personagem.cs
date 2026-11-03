using Godot;
using Godot.Collections;

public partial class Personagem : CharacterBody2D
{
	public RayCast2D AlcancePersonagem;

	[Export]
	public Dictionary<string, int> Status = new Dictionary<string, int>()
	{
		{ "DanoSoco", 2},
		{ "Velocidade", 300},
		{ "Cabeça", 10 },
		{ "Pernas", 10 },
		{ "Braços", 10 },
		{ "Peitoral", 10}
	};

	public Dictionary<string, float> Precisao = new Dictionary<string, float>()
	{
		{ "Cabeça", 0.15f },
		{ "Pernas", 0.2f },
		{ "Braços", 0.3f },
		{ "Peitoral", 0.35f }
	};

    public override void _Ready()
    {
        AlcancePersonagem = GetNode<RayCast2D>("Alcance Personagem");
    }

	// Tomar dano
	public void TomarDano(int dano, string membro)
	{
		Status[membro] -= dano;
	}

	public void AnimePersonagem(string tipoAnimacao)
	{
		
	}

}
