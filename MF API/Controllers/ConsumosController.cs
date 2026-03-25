using MF_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumosController : ControllerBase
    {


        private readonly AppDbContext _context;

        public ConsumosController(AppDbContext context)
        {
            _context = context;
        }

        // Recuperar TUDO
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Consumos.ToListAsync();

            return Ok(model);
        }


        // Criar Consumo
        [HttpPost]
        public async Task<ActionResult> Create(Consumo model)
        {

            _context.Consumos.Add(model);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = model.Id }, model);
        }

        // Encontrar Consumo pelo Id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {

            var model = await _context.Consumos.FirstOrDefaultAsync(c => c.Id == id);

            if (model == null)
            {
                return NotFound(new { message = "Consumo não encontrado" });
            }

            GerarLinks(model);
            return Ok(model);
        }

        // Atualizar Consumo
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Consumo model)
        {

            if (id != model.Id)
            {
                return BadRequest(new { message = "Id do Consumo não corresponde ao id da URL" });
            }

            var modeloDb = await _context.Consumos.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (modeloDb == null)
            {
                return NotFound(new { message = "Consumo não encontrado" });
            }

            _context.Consumos.Update(model);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        // Deletar Consumo
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Consumos.FindAsync(id);

            if (model == null)
            {
                return NotFound(new { message = "Consumo não encontrado" });
            }

            _context.Consumos.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        private void GerarLinks(Consumo model)
        {
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "self", metodo: "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "update", metodo: "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), rel: "delete", metodo: "DELETE"));

        }


    }
}
