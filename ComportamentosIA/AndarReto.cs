using Godot;

[GlobalClass]
public partial class AndarReto : Comportamento
{
	private bool AndandoReto = false;
    [Export]
    public float tamanho;
    [Export]
    public Vector2 direcao;
	public override void AtiveComportamento()
	{
		if(AndandoReto == false && Ativo == true)
		{
			CaminhoAutomatizado rotaReta = personagem.CalcularReto(direcao, tamanho);
			_ = personagem.EstabelecerRota(rotaReta);
			AndandoReto = false;
		}
	}
}