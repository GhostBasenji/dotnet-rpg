using System.Security.Claims;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterService _characterService;

    public CharacterController(ICharacterService characterService)
    {
        _characterService = characterService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> Get()
    {
        return Ok(await _characterService.GetAllCharacters());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSingle(int id)
    {
        return Ok(await _characterService.GetCharacterById(id));
    }

    [HttpPost]
    public async Task<IActionResult> AddCharacter(AddCharacterDto newCharacter)
    {
        return Ok(await _characterService.AddCharacter(newCharacter));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCharacter(UpdateCharacterDto updateCharacter)
    {
        var response = await _characterService.UpdateCharacter(updateCharacter);
        if (response.Data is null)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _characterService.DeleteCharacter(id);
        if (response.Data is null)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
}