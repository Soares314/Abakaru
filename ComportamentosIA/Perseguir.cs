using Godot;

[GlobalClass]
public partial class Perseguir : Comportamento
{
	public override void AtiveComportamento()
	{
        Jogador alvo = personagem.GetNodeOrNull<Jogador>("/root/Jogador");

		_ = personagem.AndarAte(alvo.GlobalPosition);

        personagem.PrepararAtaqueCurto(alvo, personagem.Status["DanoNormal"]);
	}
}