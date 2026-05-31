using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg.Services.CharacterService;

public interface ICharacterService
{
    Task<List<GetCharacterDto>> GetAllCharacters();
    Task<GetCharacterDto?> GetCharacterById(int id);
    Task<List<GetCharacterDto>> AddCharacter(AddCharacterDto newCharacter);
}