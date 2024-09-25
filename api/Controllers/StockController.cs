using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Migrations;
using api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stocks")] //base
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDbContext context, IStockRepository stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }

        //get all of the stocks
        [HttpGet]
        public async Task<IActionResult> GetAll() //endpoint carries the same name as base bc we did not modify yet
        {
            var stocks = await _stockRepo.GetAllAsync();

            var stocksDto = stocks.Select(s => s.ToStockDto()); //for every stock in the dbset list, select the mapper version we created (ToStockDto), 
                                            //which returns the mappers version of the called StockDto
            return Ok(stocksDto);
        }

        //get stock by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetByIdAsync(id); //find selected over first or default

            if(stock == null)
            {
                return NotFound();
            }
            
            return Ok(stock.ToStockDto()); //return the ToStockDto which contains our selected values from the StockDto (copy of the dbSet)
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto) //takes in the values used in the Dto CreateStockRequestDto
        {
            var stockModel = stockDto.ToStockFromCreateDto(); //which then creates a var of the passed-in CreateStockRequestDto into the mapper
                                                                //which passes in the Dto into the model, mapper then returns a copy of the model
            await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto()); //returns 200OK and values for the stockModel Id created
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) //from route is the box where we enter data appended to the url
        {
            var stockModel = await _stockRepo.UpdateAsync(id, updateDto); //getting the first entry in the model where the id matches

            if(stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepo.DeleteAsync(id);

            if(stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}