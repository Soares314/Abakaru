using Godot;

[GlobalClass]
public partial class Garimpo : Aspecto
{
	public override void InicializarAspecto(Jogador jogadorQueComeu)
	{
		
		base.InicializarAspecto(jogadorQueComeu);

		// Vantagem: Ganha força
		jogador.Status["Força"] += 2; 

		// Desvantagem: Fica sonso
		Calculadora.ModificarChance(jogador.Precisao, "Cabeça", -0.5f);
	}

    // Perde a força, volta normal
    protected override void Dispose(bool disposing)
    {
		base.Dispose(disposing);

		if (disposing)
		{
			jogador.Status["Força"] -= 2;
			Calculadora.ModificarChance(jogador.Precisao, "Cabeça", 0.5f);
		}
    }

	
}
