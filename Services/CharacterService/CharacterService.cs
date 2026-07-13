using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace dotnet_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetUserId() =>
        int.Parse(_httpContextAccessor.HttpContext!.User
        .FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        var character = _mapper.Map<Character>(newCharacter);
        character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

        _context.Characters.Add(character);
        await _context.SaveChangesAsync();
        
        serviceResponse.Data = await _context.Characters
            .Where(c=> c.User!.Id == GetUserId())
            .Select(c=> _mapper.Map<GetCharacterDto>(c))
            .ToListAsync();
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        try
        {
            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId())
                ?? throw new Exception($"Персонаж с Id '{id}' не найден.");
            
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
        var dbCharacters = await _context.Characters
            .Where(c => c.User!.Id == GetUserId())
            .ToListAsync();
        serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(dbCharacters);
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        var dbCharacter = await _context.Characters
            .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
        serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _context.Characters
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id)
                ?? throw new Exception($"Персонаж с Id '{updatedCharacter.Id}' не найден.");

            if (character.User!.Id != GetUserId())
            {
                throw new Exception($"Персонаж с Id '{updatedCharacter.Id}' не найден.");
            }

            _mapper.Map(updatedCharacter, character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }   
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = ex.Message;
        }
        return serviceResponse;
    }
}