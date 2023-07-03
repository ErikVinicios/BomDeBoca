using BomDeBoca.Shared.Entities;
using BomDeBoca.Server.model;
using BomDeBoca.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BomDeBoca.Shared.dto;
using System.ComponentModel.DataAnnotations;
using BomDeBoca.Client.Utils;
using BomDeBoca.Shared;
using System.Net;

namespace BomDeBoca.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _context;

        public UserController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<List<Shared.Entities.Client>>> GetAll() {
            return _context.Clients.OrderBy(c => c.CreationDate).ToList();
        }

        [HttpGet("get")]
        public async Task<ActionResult<Shared.Entities.Client>> Get([FromQuery] Guid id)
        {
            var client = _context.Clients.FirstOrDefault(x => x.ID == id);
            if (client != null)
                return Ok(client);
            return NotFound();

        }

        [AllowAnonymous]
        [HttpPost("save")]
        public async Task<ActionResult<APIResult>> Save([FromBody] RegisterDTO user)
        {
            APIResult result = new APIResult();
            var exist = _context.Clients.FirstOrDefault(u => u.CPF == user.CpfCNPJ);
            if (exist == null) {
                if (!Methods.EmailIsValid(user.Email))
                {
                    result.Message = "O Email inserido é inválido.";
                    result.Result = false;
                    return result;
                }

                if (!Methods.PasswordIsValid(user.Password))
                {
                    result.Message = "A Senha inserida é inválida.";
                    result.Result = false;
                    return result;
                }
                
                Shared.Entities.Client _user = new Shared.Entities.Client()
                {
                    ID = Guid.NewGuid(),
                    Name = user.Name,
                    Photo = null,
                    CPF = user.CpfCNPJ,
                    Email = user.Email,
                    Password = user.Password,
                    CreationDate = DateTime.Now
                };
                _context.Clients.Add(_user);
                await _context.SaveChangesAsync();
                result.Obj = _user;
                result.Result = true;
                return Ok(result);
            }
            result.Obj = null;
            result.Message = "Um cliente com este CPF já está cadastrado.";
            result.Result = false;
            return NotFound(result);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Shared.Entities.Client>> Update([FromBody] Shared.Entities.Client user) {
            var _user = _context.Clients.FirstOrDefault(x => x.ID == user.ID);
            if (_user != null) {
                _user.ID = user.ID;
                _user.Name = user.Name;
                _user.Email = user.Email;
                _user.Photo = user.Photo;
                _user.CPF = user.CPF;
                _user.Password = user.Password;
                _context.Update(_user);
                await _context.SaveChangesAsync();
                return Ok(_user);
            }
            return NotFound();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Shared.Entities.Client>> Delete([FromQuery] Guid id) {
            var user = _context.Clients.FirstOrDefault(x => x.ID == id);
            if (user != null) {
                _context.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}
