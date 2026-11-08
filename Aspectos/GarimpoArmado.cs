using System.Runtime.Serialization.Formatters;
using Godot;

[GlobalClass]
public partial class GarimpoArmado : Aspecto
{
    public override void InicializarAspecto(Jogador jogadorQueComeu)
	{
        base.InicializarAspecto(jogadorQueComeu);

        
	}

    public override void RodarAspecto()
	{
        //Vantagem: Pode dobrar todas as munições a cada amplificador
        if(Amplificador > 0)
        {
            if (Input.IsActionJustPressed("DuplicarMunição"))
            {
                foreach(string tipoMunicao in jogador.Municao.Keys)
                    jogador.Municao[tipoMunicao] *= 2;

                Amplificador--;
            }
        }

	}

    // Perde a força, volta normal
    protected override void Dispose(bool disposing)
    {
		base.Dispose(disposing);

		if (disposing)
		{
			Amplificador--;
		}
    }

	
}