using Godot;

[GlobalClass]
public partial class MorteHumana : Comportamento
{
	// Controlador de comportamentos de um NPC, chamando suas ações relativas
	public override void AtiveComportamento()
	{
		if(personagem.Status["Cabeca"] <= 0 || personagem.Status["Peitoral"] <= 0)
        {
            
        }
	}
}