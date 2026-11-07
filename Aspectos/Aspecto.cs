using Godot;

[GlobalClass]
public partial class Aspecto : Resource
{
	[Export]
	public float Amplificador = 1f;
    public Jogador jogador;
    public virtual void InicializarAspecto(Jogador jogadorQueComeu)
	{
		jogador = jogadorQueComeu;
	}

	// Controlador de comportamentos de um NPC, chamado todo frame
	public virtual void RodarAspecto()
	{
		GD.Print("Esse é o aspecto padrão com aplificação", Amplificador);
		return;
	}
}
