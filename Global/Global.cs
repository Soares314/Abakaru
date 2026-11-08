using Godot;
using Godot.Collections;

public static class Calculadora
{
    // Aumenta ou diminui a chance de um acontecimento, mantendo o espaço amostral
    public static void ModificarChance(Dictionary<string, float> poolChance, string chave, float modificador)
    {
        if (!poolChance.ContainsKey(chave) || modificador == 0) return;

        float novaChance;

        if(modificador < 0)
        {
            novaChance = poolChance[chave] * modificador;
        }
        else 
        {
            novaChance = poolChance[chave] * 1f/modificador;
        }

        // Aplicar a formula para normalizar os valores
        foreach (var essaChave in poolChance.Keys)
        {
            if (essaChave != chave)
                poolChance[essaChave] *= (1f - novaChance)/(1f - poolChance[chave]);
        }
    }
}