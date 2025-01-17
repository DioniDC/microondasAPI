using System.Text.Json;
using MicrowaveAPI.Models;

namespace MicrowaveAPI.Data
{
    public class MicrowaveData
    {
        public List<MicrowavePreset> Presets { get; set; } = new List<MicrowavePreset>();
        public List<MicrowavePreset> Custom { get; set; } = new List<MicrowavePreset>();
        private static readonly string FilePath = Path.Combine("C:\\Users\\dioni\\Microondas\\MicrowaveAPI\\MicrowaveAPI", "data.json");

        // Carregar dados do JSON
    public static MicrowaveData Load()
    {
    if (!File.Exists(FilePath))
    {
        Console.WriteLine("Arquivo data.json não encontrado. Criando um novo...");
        var initialData = new MicrowaveData();
        Save(initialData);
        return initialData;
    }

    try
    {
        Console.WriteLine("Carregando dados do arquivo data.json...");
        var json = File.ReadAllText(FilePath);
        var data = JsonSerializer.Deserialize<MicrowaveData>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignora diferenças de maiúsculas/minúsculas nos nomes das propriedades
        });

        if (data == null)
        {
            Console.WriteLine("Dados desserializados são nulos. Retornando estrutura vazia.");
            return new MicrowaveData();
        }

        return data;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao carregar os dados: {ex.Message}");
        return new MicrowaveData(); // Retorna estrutura vazia em caso de erro
    }
}


        // Salvar dados no JSON
        public static void Save(MicrowaveData data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
