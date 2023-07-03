using BomDeBoca.Server.model;
using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BomDeBoca.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompanyFeedbackController : ControllerBase
    {
        private readonly AppDBContext _context;

        public CompanyFeedbackController(AppDBContext context) {
            _context = context;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<List<CompanyFeedback>>> GetAll()
        {
            var listFeedbacks = _context.CompanyFeedbacks.OrderByDescending(c => c.Date).ToList();
            if (listFeedbacks == null) return NotFound();
            return Ok(listFeedbacks);
        }

        [HttpGet("getallbyidcompany")]
        public async Task<ActionResult<List<CompanyFeedback>>> GetAllByIdCompany([FromQuery] Guid ID)
        {
            var listFeedbacks = _context.CompanyFeedbacks.Where(c => c.CompanyID == ID).OrderByDescending(c => c.Date).ToList();
            if (listFeedbacks == null) return NotFound();
            return Ok(listFeedbacks);
        }

        [HttpGet("get")]
        public async Task<ActionResult<CompanyFeedback>> Get([FromQuery] Guid ID)
        {
            APIResult result = new APIResult();
            var feedback = _context.CompanyFeedbacks.FirstOrDefault(x => x.ID == ID);
            if (feedback != null)
            {
                return Ok(feedback);
            }
            return NotFound();
        }

        [HttpPost("save")]
        public async Task<ActionResult<CompanyFeedback>> Save([FromBody]CompanyFeedback feedback)
        {
            CompanyFeedback newFeedback = new CompanyFeedback()
            {
                ID = Guid.NewGuid(),
                ClientID = feedback.ClientID,
                CompanyID = feedback.CompanyID,
                ClientName = feedback.ClientName,
                ClientPhoto = feedback.ClientPhoto,
                Description = feedback.Description,
                Date = DateTime.Now,
                Rating = feedback.Rating
            };

            _context.Add(newFeedback);
            await _context.SaveChangesAsync();
            return Ok(newFeedback);
        }

        [HttpPut("update")]
        public async Task<ActionResult<CompanyFeedback>> Update([FromBody]CompanyFeedback feedback)
        {
            APIResult result = new APIResult();
            var _feedback = _context.CompanyFeedbacks.FirstOrDefault(f =>f.ID == feedback.ID && f.ClientID == feedback.ClientID);
            if (_feedback != null)
            {
                _feedback.ID = feedback.ID;
                _feedback.ClientID = feedback.ClientID;
                _feedback.CompanyID = feedback.CompanyID;
                _feedback.Description = feedback.Description;
                _feedback.Date = DateTime.Now;
                _feedback.Rating = feedback.Rating;

                _context.Update(_feedback);
                await _context.SaveChangesAsync();
                
                return Ok(_feedback);
            }
            return NotFound();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<APIResult>> Delete([FromQuery]Guid id)
        {
            APIResult result = new APIResult();
            var feedback = _context.CompanyFeedbacks.FirstOrDefault(f => f.ID == id);
            if (feedback != null)
            {
                _context.Remove(feedback);
                await _context.SaveChangesAsync();
                result.Result = true;
                result.Message = "Avaliação excluida com sucesso.";
                return Ok(result);
            }
            result.Result = false;
            result.Message = "Avaliação não ncontrada.";
            return NotFound(result);
        }
    }
}
