using Microsoft.AspNetCore.Mvc;
using MicrowaveAPI.Data;
using MicrowaveAPI.Models;

namespace MicrowaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MicrowaveController : ControllerBase
    {
        [HttpGet("presets")]
        public IActionResult GetPresets()
        {
            var data = MicrowaveData.Load();
            return Ok(new { data.Presets, data.Custom });
        }

        [HttpPost("custom")]
        public IActionResult AddOrUpdateCustom([FromBody] MicrowavePreset preset)
        {
            Console.WriteLine($"Recebido: Id={preset.Id}, Nome={preset.Nome}, Tempo={preset.Tempo}, Potência={preset.Potencia}, Strings={preset.Strings}, Instrução={preset.Instrucao}");

            if (preset.Id <= 0 || string.IsNullOrEmpty(preset.Nome))
            {
                Console.WriteLine("Erro: ID ou Nome inválidos.");
                return BadRequest("ID ou Nome inválidos.");
            }

            preset.Alimento = preset.Alimento ?? "";

            try
            {
                var data = MicrowaveData.Load();

                var existing = data.Custom.FirstOrDefault(b => b.Id == preset.Id);
                if (existing != null)
                {
                    existing.Nome = preset.Nome;
                    existing.Tempo = preset.Tempo;
                    existing.Potencia = preset.Potencia;
                    existing.Strings = preset.Strings;
                    existing.Instrucao = preset.Instrucao;
                }
                else
                {
                    if (data.Custom.Count >= 3)
                        return BadRequest("Você pode ter no máximo 3 botões customizados.");

                    data.Custom.Add(preset);
                }

                MicrowaveData.Save(data);
                return Ok(preset);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar a solicitação: {ex.Message}");
                return BadRequest($"Erro interno: {ex.Message}");
            }
        }

        [HttpDelete("custom/{id}")]
        public IActionResult DeleteCustom(int id)
        {
            var data = MicrowaveData.Load();

            var buttonToDelete = data.Custom.FirstOrDefault(b => b.Id == id);
            if (buttonToDelete == null) return NotFound();

            data.Custom.Remove(buttonToDelete);
            MicrowaveData.Save(data);
            return NoContent();
        }
        [HttpGet("check-string/{string}")]
        public IActionResult CheckStringAvailability(string @string)
        {
            var data = MicrowaveData.Load();
            var isUsed = data.Presets.Any(p => p.Strings == @string) || data.Custom.Any(c => c.Strings == @string);

            return Ok(!isUsed);
        }
    }
    
}
