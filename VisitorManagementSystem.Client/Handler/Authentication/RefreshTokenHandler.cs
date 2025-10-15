using Blazored.LocalStorage;
using Shared.DataTransferObjects;
using System.Net.Http.Json;
using System.Text.Json;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.Handler.Authentication;

public class RefreshTokenHandler
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IHttpClientFactory _httpClientFactory;
    private TokenDto? _tokenDto;

    public RefreshTokenHandler(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory)
    {
        _localStorageService = localStorageService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task Handle()
    {
        try
        {
            _tokenDto = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey);

            if( _tokenDto == null )
            {
                return;
            }

            var getRefreshTokenResp = await _httpClientFactory.CreateClient(ClientHelper.SecureClientKey).PostAsJsonAsync("api/authentication/refresh", _tokenDto);

            getRefreshTokenResp.EnsureSuccessStatusCode();

            if(getRefreshTokenResp.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return;
            }

            var getRefreshTokenResponseContent = await getRefreshTokenResp.Content.ReadAsStringAsync();

            _tokenDto = JsonSerializer.Deserialize<TokenDto>(getRefreshTokenResponseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            if(_tokenDto == null)
            {
                return;
            }

            await _localStorageService.RemoveItemAsync(ClientHelper.StorageKey);

            await _localStorageService.SetItemAsync(ClientHelper.StorageKey, _tokenDto);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
