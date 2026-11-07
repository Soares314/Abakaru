using Godot;

public partial class Item : Area2D
{
    [Export]
    public Resource ItemNoChao;

    private void OnBodyEntered(Node2D Corpo)
    {
        if(Corpo is Jogador)
        {
            (Corpo as Jogador).NaZonaDe = ItemNoChao;
        }
    }

    private void OnBodyExited(Node2D Corpo)
    {
        if(Corpo is Jogador)
        {
            (Corpo as Jogador).NaZonaDe = null;
        }
    }
    
}
