using Godot;

// Quando o personagem tiver euqipado com esse, vai valer como se tivesse sem arma
[GlobalClass]
public partial class ArmaFogo : Arma
{
    public string TipoMunicao = "Generico";
    public override void Usar(Personagem personagem, Personagem personagemAlvo)
	{
        if(personagem.Municao.Count != 0)
            if(personagem.Municao.ContainsKey(TipoMunicao) )
		        base.Usar(personagem, personagemAlvo);
	}
}