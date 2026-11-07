using Godot;

[GlobalClass]
public partial class Garimpo : Aspecto
{
	public override void InicializarAspecto(Jogador jogadorQueComeu)
	{
		base.InicializarAspecto(jogadorQueComeu);

		
	}

    // Controlador de comportamentos de um NPC, chamando suas ações relativas
    protected override void Dispose(bool disposing)
    {
		if (disposing)
		{
			
		}
    }

	
}
