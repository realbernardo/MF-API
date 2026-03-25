using MF_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {

        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        // Recuperar TUDO
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Veiculos.ToListAsync();

            return Ok(model);
        }


        // Criar Veiculo
        [HttpPost]
        public async Task<ActionResult> Create(Veiculo model)
        {

            if (model.AnoFabricacao <= 0 || model.AnoModelo <= 0)
            {
                return BadRequest(new { message = "Informe Ano de Fabricação e Ano de Modelo" });
            }

            _context.Veiculos.Add(model);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new {id = model.Id}, model);
        }

        // Encontrar veiculo pelo Id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {

            var model = await _context.Veiculos.Include(t => t.Consumos).FirstOrDefaultAsync(c => c.Id == id);
           
            if(model == null)
            {
                return NotFound(new { message = "Veículo não encontrado" });
            }

            GerarLinks(model);
            return Ok(model);
        }

        // Atualizar veiculo
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Veiculo model)
        {

            if (id != model.Id)
            {
                return BadRequest(new { message = "Id do veículo não corresponde ao id da URL" });
            }

            var modeloDb = await _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if(modeloDb == null)
            {
                return NotFound(new { message = "Veículo não encontrado" });
            }

            _context.Veiculos.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        // Deletar veiculo
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Veiculos.FindAsync(id);

            if(model == null)
            {
                return NotFound(new { message = "Veículo não encontrado" });
            }

            _context.Veiculos.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        private void GerarLinks(Veiculo model)
        {
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "self", metodo: "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "update", metodo: "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "delete", metodo: "DELETE"));

        }
    }
}
