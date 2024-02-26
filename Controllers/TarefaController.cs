using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var tarefa = _context.Tarefas.Find(id);

                return Ok(tarefa);
            } 
            catch(Exception ex)
            {
                if (ex is InvalidOperationException || ex is NullReferenceException)
                {
                    return StatusCode(404);
                }
                return StatusCode(500);
            }
        }

        [HttpGet("AllTasks")]
        public IActionResult AllTasks()
        {
            try
            {
                var tarefas = _context.Tarefas;
                return Ok(tarefas);
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
                
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefas = _context.Tarefas.Where(x => x.Titulo.Contains(titulo)).ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            try
            {
                var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
                return Ok(tarefa);
            }
            catch(Exception ex)
            {
                if (ex is InvalidOperationException || ex is NullReferenceException)
                    return StatusCode(404);
                
                return StatusCode(500);
            }
            
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            try
            {
                var tarefa = _context.Tarefas.Where(x => x.Status == status);
                return Ok(tarefa);
            }
            catch(Exception ex)
            {
                if (ex is InvalidOperationException || ex is NullReferenceException)
                {
                    return StatusCode(404);
                }
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            try
            {
                if (tarefa.Data == DateTime.MinValue)
                    return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

                _context.Add(tarefa);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = tarefa.Id }, tarefa);
            }
            catch
            {
                return StatusCode(500);
            }   
            
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                var tarefaBanco = _context.Tarefas.Find(id);

                if (tarefaBanco == null)
                    return NotFound();

                _context.Tarefas.Remove(tarefaBanco);
                _context.SaveChanges();

                return NoContent();
            }
            catch(Exception ex)
            {
                if (ex is InvalidOperationException || ex is NullReferenceException)
                {
                    return StatusCode(404);
                }
                return StatusCode(500);
            }
        }
    }
}
