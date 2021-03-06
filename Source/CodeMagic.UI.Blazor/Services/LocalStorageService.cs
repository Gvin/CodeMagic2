using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Services
{
    public interface ILocalStorageService
    {
        ValueTask SetAsync<T>(string key, T value);

        Task<T?> GetAsync<T>(string key);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<LocalStorageService> _logger;

        public LocalStorageService(IJSRuntime jsRuntime, ILogger<LocalStorageService> logger)
        {
            _jsRuntime = jsRuntime;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var value = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
                if (string.IsNullOrEmpty(value))
				{
                    return default(T);
				}

                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting key {Key} from local storage", key);
                throw;
            }
        }

        public async ValueTask SetAsync<T>(string key, T value)
        {
            try
            {
                var valueString = JsonConvert.SerializeObject(value);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, valueString);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while setting key {Key} in local storage", key);
                throw;
            }
        }
    }
}
