using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Migrations;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stocks")] //base
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get all of the stocks
        [HttpGet]
        public IActionResult GetAll() //endpoint carries the same name as base bc we did not modify yet
        {
            var stocks = _context.Stock.ToList()
            .Select(s => s.ToStockDto()); //for every stock in the dbset list, select the mapper version we created (ToStockDto), 
                                            //which returns the mappers version of the called StockDto
            return Ok(stocks); 
        }

        //get stock by id
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stock.Find(id); //find selected over first or default

            if(stock == null)
            {
                return NotFound();
            }
            
            return Ok(stock.ToStockDto()); //return the ToStockDto which contains our selected values from the StockDto (copy of the dbSet)
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto) //takes in the values used in the Dto CreateStockRequestDto
        {
            var stockModel = stockDto.ToStockFromCreateDto(); //which then creates a var of the passed-in CreateStockRequestDto into the mapper
                                                                //which passes in the Dto into the model, mapper then returns a copy of the model
            _context.Stock.Add(stockModel); //add to the model context
            _context.SaveChanges(); 
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto()); //returns 200OK and values for the stockModel Id created
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) //from route is the box where we enter data appended to the url
        {
            var stockModel = _context.Stock.FirstOrDefault(x => x.Id == id); //getting the first entry in the model where the id matches

            if(stockModel == null)
            {
                return NotFound();
            }

            //directly updating the table using our UpdateStockRequestDto copy that has values passed-in from the body in the endpoint
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDividend = updateDto.LastDividend;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();

            return Ok(stockModel.ToStockDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stockModel = _context.Stock.FirstOrDefault(x => x.Id == id);

            if(stockModel == null)
            {
                return NotFound();
            }

            _context.Stock.Remove(stockModel);

            _context.SaveChanges();

            return NoContent();
        }

    }
}