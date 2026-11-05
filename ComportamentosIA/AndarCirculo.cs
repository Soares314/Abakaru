using Godot;
using System;

[GlobalClass]
public partial class AndarCirculo : Comportamento
{
	private bool AndandoCirculo = false;
	public override void AtiveComportamento()
	{
		if(AndandoCirculo == false && Ativo == true)
		{
			CaminhoAutomatizado rotaCircular = personagem.CacularPerimetro();
			_ = personagem.EstabelecerRota(rotaCircular);
			AndandoCirculo = false;
		}
	}
	public override void OnChamouAtencao()
	{
		Ativo = false;
	}
}