using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Models.Repositories;

namespace NZWalks.Controllers;

// https://localhost:7103/api/walks
[Route("api/[controller]")]
[ApiController]
public class WalksController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IWalkRepository _walkRepository;

    public WalksController(IMapper mapper, IWalkRepository walkRepository)
    {
        _mapper = mapper;
        _walkRepository = walkRepository;
    }

    // Create Walks
    // POST: https://localhost:7103/api/walks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        // Map DTO to Domain Model
        var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

        await _walkRepository.CreateAsync(walkDomainModel);

        // Map Domain Model to DTO
        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    //Get ALL Walks
    //GET: https://localhost:7103/api/walks
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var walksDomainModel = await _walkRepository.GetAllAsync();

        //Map Domain Model to DTO
        return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
    }

    //Get Walk by Id
    //GET: https://localhost:7103/api/walks/{id}
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var walkDomainModel = await _walkRepository.GetByIdAsync(id);
        if (walkDomainModel == null) return NotFound();

        //Map Domain Model to DTO
        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    // Update Walk by Id
    // PUT: https://localhost:7103/api/walks/{id}
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
    {
        // Map DTO to Domain Model
        var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

        var updatedWalk = await _walkRepository.UpdateAsync(id, walkDomainModel);
        if (updatedWalk == null) return NotFound();

        // Map Domain Model to DTO
        return Ok(_mapper.Map<WalkDto>(updatedWalk));
    }
}