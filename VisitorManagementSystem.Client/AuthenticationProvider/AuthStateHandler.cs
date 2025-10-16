
using Blazored.LocalStorage;
using Shared.DataTransferObjects;
using System.Net.Http.Headers;
using System.Text.Json;
using VisitorManagementSystem.Client.Handler.Authentication;
using VisitorManagementSystem.Client.Helpers;

namespace VisitorManagementSystem.Client.AuthenticationProvider;

public class AuthStateHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;
    private readonly RefreshTokenHandler _refreshTokenHandler;
    private TokenDto? _tokenDto;

    public AuthStateHandler(ILocalStorageService localStorageService, IHttpClientFactory httpClientFactory, RefreshTokenHandler refreshTokenHandler)
    {
        _localStorageService = localStorageService;
        _refreshTokenHandler = refreshTokenHandler;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"From Auth State Handler. Fetching Token from storage.....");
        _tokenDto = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey, cancellationToken);
        Console.WriteLine($"From Auth State Handler. Token Fetched.{JsonSerializer.Serialize(_tokenDto)}");
        if (_tokenDto is not null)
        {
            Console.WriteLine($"From Auth State Handler. Token Exists. Setting bearer as access token for authorization..");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenDto.AccessToken);
            Console.WriteLine($"From Auth State Handler. Token Exists. Setting bearer as access token for authorization successful..");
        }
        Console.WriteLine($"From Auth State Handler. Calling Endpoint to get status request.");
        var result = await base.SendAsync(request, cancellationToken);
        Console.WriteLine($"From Auth State Handler. Token Exists. Calling Endpoint to get status request successful. Status Code: {result.StatusCode.ToString()}");
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine($"From Auth State Handler. Token Exists. Calling Endpoint to get status request successful. Return response to client.");
            return result;
        }

        if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"From Auth State Handler. Token Exists. Calling Endpoint to get status request successful. Unauthorized Response Code");
            if (_tokenDto is not null)
            {
                try
                {
                    //var refreshTokenResp = await _httpClient.PostAsJsonAsync("api/authentication/refresh", token, cancellationToken);

                    //refreshTokenResp.EnsureSuccessStatusCode();

                    //var responseContent = await refreshTokenResp.Content.ReadAsStringAsync();

                    //_tokenDto = JsonSerializer.Deserialize<TokenDto>(responseContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? null;

                    Console.WriteLine($"From Auth State Handler. Token Exists. Calling Endpoint to get status request successful.Calling Refrsh token handler..");
                    await _refreshTokenHandler.Handle(); //Calls the refresh token handler. This sets the new token to the local storage.
                    Console.WriteLine($"From Auth State Handler. Token Exists. Calling Endpoint to get status request successful.Calling Refrsh token handler successful. Fetching updated Token from storage..");

                    _tokenDto = await _localStorageService.GetItemAsync<TokenDto>(ClientHelper.StorageKey, cancellationToken); //Fetches from the local strage to confirm that the refresh was successful

                    if(_tokenDto is not null)
                    {
                        Console.WriteLine($"Token not null at the auth state handler. Setting Items again to storage");
                        await _localStorageService.SetItemAsync(ClientHelper.StorageKey, _tokenDto, cancellationToken);
                        Console.WriteLine($"Token not null at the auth state handler. Calling endpoint to with updated headers");
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenDto?.AccessToken);

                        result = await base.SendAsync(request, cancellationToken);
                        Console.WriteLine($"From Auth State Handler. Token Exists. Calling Endpoint to get status request successful with updated token. Status Code: {result.StatusCode.ToString()}");
                    }
                }

                catch (HttpRequestException ex)
                {

                    throw;
                }

            }


        }

        Console.WriteLine($"Return the result to client...");

        return result;
    }
}
