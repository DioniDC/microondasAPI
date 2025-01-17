namespace MicrowaveAPI.Models;
public class MicrowavePreset
{
    public int Id { get; set; }

    public required string Nome { get; set; }

    public int Tempo { get; set; }

    public int Potencia { get; set; }

    public string Strings { get; set; } = string.Empty;

    public required string Instrucao { get; set; }
    public required string Alimento { get; set; } 
}
