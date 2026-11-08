using Godot;

[GlobalClass]
public partial class MorteHumana : Comportamento
{
	// Controlador de comportamentos de um NPC, chamando suas ações relativas
	public override void AtiveComportamento()
	{
		if(personagem.StatusAtual["Cabeça"] <= 0 || personagem.StatusAtual["Peitoral"] <= 0)
        {
			personagem.Morrer();
        }
	}
}