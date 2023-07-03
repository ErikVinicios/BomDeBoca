using BomDeBoca.Server.model;
using BomDeBoca.Shared.Results;
using BomDeBoca.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BomDeBoca.Shared.dto;
using BomDeBoca.Shared;

namespace BomDeBoca.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly AppDBContext _context;

        public CompanyController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<List<Shared.Entities.Company>>> GetAll()
        {
            return _context.Companies.OrderBy(c => c.CreationDate).ToList();
        }

        [HttpGet("get")]
        public async Task<ActionResult<Shared.Entities.Company>> Get([FromQuery] Guid id)
        {
            var company = _context.Companies.FirstOrDefault(x => x.ID == id);
            if (company != null)
                return Ok(company);
            return NotFound();

        }

        [AllowAnonymous]
        [HttpPost("save")]
        public async Task<ActionResult<APIResult>> Save([FromBody] RegisterDTO company)
        {
            APIResult result = new APIResult();
            var exist = _context.Companies.FirstOrDefault(u => u.CNPJ == company.CpfCNPJ);
            if (exist == null)
            {
                if (!Methods.EmailIsValid(company.Email))
                {
                    result.Message = "O Email inserido é inválido.";
                    result.Result = false;
                    return result;
                }

                if (!Methods.PasswordIsValid(company.Password))
                {
                    result.Message = "A Senha inserida é inválida.";
                    result.Result = false;
                    return result;
                }

                Shared.Entities.Company _company = new Shared.Entities.Company()
                {
                    ID = Guid.NewGuid(),
                    Name = company.Name,
                    Logo = null,
                    CNPJ = company.CpfCNPJ,
                    Description = $"Hello! My name's {company.Name} and I am a BomDeBoca company.",
                    Email = company.Email,
                    Password = company.Password,
                    CreationDate = DateTime.Now
                };
                _context.Companies.Add(_company);
                await _context.SaveChangesAsync();
                result.Obj = _company;
                result.Result = true;
                return Ok(result);
            }
            result.Obj = null;
            result.Message = "Empresa já existe";
            result.Result = false;
            return NotFound(result);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Shared.Entities.Company>> Update([FromBody] Shared.Entities.Company company)
        {
            var _company = _context.Companies.FirstOrDefault(x => x.ID == company.ID);
            if (_company != null)
            {
                _company.ID = company.ID;
                _company.Name = company.Name;
                _company.Logo = company.Logo;
                _company.Email = company.Email;
                _company.CNPJ = company.CNPJ;
                _company.Description = company.Description;
                _company.Password = company.Password;
                _context.Update(_company);
                await _context.SaveChangesAsync();
                return Ok(_company);
            }
            return NotFound();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Shared.Entities.Company>> DeleteCompany([FromQuery] Guid id)
        {
            var company = _context.Companies.FirstOrDefault(x => x.ID == id);
            if (company != null)
            {
                _context.Remove(company);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}
