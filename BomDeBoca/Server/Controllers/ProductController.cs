using BomDeBoca.Shared.Entities;
using BomDeBoca.Server.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BomDeBoca.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace BomDeBoca.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDBContext _context;
        public ProductController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var result = _context.Products.OrderBy(p => p.CreationDate).ToList();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("getallbycompanyid")]
        public async Task<ActionResult<List<Product>>> GetAllByCompanyID(Guid ID)
        {
            var result = _context.Products.Where(p => p.CompanyID == ID).OrderBy(p => p.CreationDate).ToList();
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpGet("get")]
        public async Task<ActionResult<Product>> Get([FromQuery] Guid id)
        {
            var result = _context.Products.FirstOrDefault(p => p.ID == id);
            if (result != null)
                return Ok(result);
            return NotFound();
        }

        [HttpPut("update")]
        public async Task<ActionResult<Product>> Update([FromBody] Product product)
        {
            var _product = _context.Products.FirstOrDefault(p => p.ID == product.ID);
            if (_product != null)
            {
                _product.Name = product.Name;
                _product.Description = product.Description;
                _product.Price = product.Price;
                _product.Image = product.Image;

                _context.Update(_product);
                await _context.SaveChangesAsync();
                return Ok(_product);
            }
            return NotFound();
        }

        [HttpPost("save")]
        public async Task<ActionResult<Product>> Save([FromBody] Product product)
        {
            var _product = _context.Products.FirstOrDefault(p => p.ID == product.ID && p.CompanyID == product.CompanyID);
            if (_product == null)
            {
                Product _newProduct = new Product()
                {
                    ID = Guid.NewGuid(),
                    Name = product.Name,
                    CompanyID = product.CompanyID,
                    Description = product.Description,
                    Image = product.Image,
                    Price = product.Price,
                    CreationDate = DateTime.Now
                };
                _context.Add(_newProduct);
                await _context.SaveChangesAsync();
                return Ok(_newProduct);
            }
            return NotFound();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Product>> Delete([FromQuery] Guid id)
        {
            var result = _context.Products.FirstOrDefault(x => x.ID == id);
            if (result != null)
            {
                _context.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
    }
}
