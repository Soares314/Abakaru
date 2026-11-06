using Godot;

[GlobalClass]
public partial class Agressivo : Comportamento
{
	[Export]
	PackedScene AlvoCena = null;
	Personagem Alvo;
	bool Perseguindo = false;
	public override void InicializarComportamento(Personagem personagemControlado)
	{
		base.InicializarComportamento(personagemControlado);

		if(AlvoCena == null)
		{
			Jogador jogadorNaCena = personagem.GetNodeOrNull<Jogador>("/root/Jogador");
			Alvo = jogadorNaCena;
		}
		else
		{
			Alvo = AlvoCena.Instantiate<Personagem>();
		}
	}
	public override void AtiveComportamento()
	{

		if(IsInstanceValid(Alvo))
		{
			if(Perseguindo == false && personagem.Olhou(Alvo))
			{
				personagem.EmitSignal(Personagem.SignalName.ChamouAtencao);
				Perseguindo = true;

				_ = personagem.AndarAte(Alvo.GlobalPosition);

				personagem.PrepararAtaqueCurto(Alvo, personagem.Status["DanoNormal"]);
			}
			
			if(Perseguindo == true)
			{
				_ = personagem.AndarAte(Alvo.GlobalPosition);

				personagem.PrepararAtaqueCurto(Alvo, personagem.Status["DanoNormal"]);
			}
		}
	}
}