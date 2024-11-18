using System.Security.Claims;
using System.Text.Json;
using BlazorApp1.Models;
using DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp1.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
    private bool _isPrerendering = true;

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task LoginAsync(string userName, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", new LoginRequest(userName, password));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (loginResponse == null || loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            throw new Exception("Invalid response from server.");
        }

        // Store user and token in session storage
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", JsonSerializer.Serialize(loginResponse.User));
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "authToken", loginResponse.Token);

        _currentUser = CreateClaimsPrincipal(loginResponse.User, loginResponse.Token);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "currentUser");
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "authToken");

        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_isPrerendering)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var userJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        var token = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "authToken");

        if (string.IsNullOrEmpty(userJson) || string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var user = JsonSerializer.Deserialize<UserDTO>(userJson);
        _currentUser = CreateClaimsPrincipal(user, token);
        return new AuthenticationState(_currentUser);
    }

    private ClaimsPrincipal CreateClaimsPrincipal(UserDTO user, string token)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("Id", user.Id.ToString()),
            new Claim("jwt", token)
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
    }

    public async Task InitializeAsync()
    {
        _isPrerendering = false;
        NotifyAuthenticationStateChanged(Task.FromResult(await GetAuthenticationStateAsync()));
    }
}
