using System;
using aulaDioApi.Data.Collections;
using aulaDioApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace aulaDioApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDTO dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }

        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDTO dto)
        {
             _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dto.DataNascimento), Builders<Infectado>.Update.Set("sexo",dto.Sexo));
            
            return Ok("Atualizado com sucesso");
        }

        [HttpDelete("{dataNasc}")]
        public ActionResult DeletarInfectado(DateTime dataNasc)
        {
             _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_ => _.DataNascimento == dataNasc));
            
            return Ok("Deletado com sucesso");
        }
    }

}