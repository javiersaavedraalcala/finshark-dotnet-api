using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using finshark.Data;
using finshark.Dtos.Comment;
using finshark.Interfaces;
using finshark.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace finshark.Controllers
{
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository repository, IStockRepository stockRepository)
        {
            _commentRepository = repository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comment = await _commentRepository.GetAllAsync();
            var commentDtos = comment.Select(comment => comment.ToCommentDto());

            return Ok(commentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequestDto commmentDto)
        {
            if (!await _stockRepository.StockExists(commmentDto.StockId.Value))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = commmentDto.ToCommentFromCreateDto();
            await _commentRepository.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            var commentModel = await _commentRepository.UpdateAsync(id, updateDto);

            if (commentModel == null)
            {
                return NotFound();
            }

            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var commentModel = await _commentRepository.DeleteAsync(id);

            if (commentModel == null)
            {
                return NotFound("Comment does not exist");
            }

            return NoContent();
        }
    }
}