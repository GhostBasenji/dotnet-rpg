using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private static List<Character> characters =
    [
        new Character(),
        new Character { Id = 1, Name = "Sam" }
    ];

    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<GetCharacterDto>> GetAllCharacters()
    {
        return _mapper.Map<List<GetCharacterDto>>(characters);
    }

    public async Task<GetCharacterDto?> GetCharacterById(int id)
    {
        var character = characters.FirstOrDefault(c => c.Id == id);
        return _mapper.Map<GetCharacterDto>(character);
    }

    public async Task<List<GetCharacterDto>> AddCharacter(AddCharacterDto newCharacter)
    {
        characters.Add(_mapper.Map<Character>(newCharacter));
        return _mapper.Map<List<GetCharacterDto>>(characters);
    }
}