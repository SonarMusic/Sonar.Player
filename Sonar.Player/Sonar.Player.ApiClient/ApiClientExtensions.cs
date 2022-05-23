using System.Text;

namespace Sonar.Player.ApiClient;

public partial interface IFilesApiClient
{
    IFilesApiClient SetToken(string token);
}

public partial class FilesApiClient
{
    private string _token;

    public IFilesApiClient SetToken(string token)
    {
        _token = token;
        return this;
    }
    
    private Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string urlBuilder, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_token) && !request.Headers.Contains("Token")) request.Headers.Add("Token", _token);
        return Task.CompletedTask;
    }
    
    private Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_token) && !request.Headers.Contains("Token")) request.Headers.Add("Token", _token);
        return Task.CompletedTask;
    }
    
    private Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public partial interface IQueueApiClient
{
    IQueueApiClient SetToken(string token);
}

public partial class QueueApiClient
{
    private string _token;

    public IQueueApiClient SetToken(string token)
    {
        _token = token;
        return this;
    }
    
    private Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string urlBuilder, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_token) && !request.Headers.Contains("Token")) request.Headers.Add("Token", _token);
        return Task.CompletedTask;
    }
    
    private Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_token) && !request.Headers.Contains("Token")) request.Headers.Add("Token", _token);
        return Task.CompletedTask;
    }
    
    private Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}