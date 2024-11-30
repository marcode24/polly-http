using Polly;

class Program
{
  static async Task Main(string[] args)
  {
    Console.WriteLine("Iniciando...");

    var retryPolicy = Policy
        .Handle<HttpRequestException>()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (exception, timespan, retryCount, context) =>
            {
              Console.WriteLine($"\nIntento {retryCount}: {exception.Message} - Reintentando en {timespan.Seconds} segundos.");
            });

    var fallbackPolicy = Policy
        .Handle<Exception>()
        .FallbackAsync(
            fallbackAction: async cancellationToken =>
            {
              Console.WriteLine("Fallback ejecutado: Endpoint fallido tras 3 intentos.");
              await ActualizarBaseDeDatosPorFallback();
            },
            onFallbackAsync: async (exception) =>
            {
              Console.WriteLine($"Fallback activado debido a: {exception.Message}");
              await Task.CompletedTask;
            });

    var policyWrap = Policy.WrapAsync(fallbackPolicy, retryPolicy);

    try
    {
      await policyWrap.ExecuteAsync(async () =>
      {
        await SimulateEndpointCall();
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error inesperado: {ex.Message}");
    }

    Console.WriteLine("Proceso terminado.");
  }

  static async Task SimulateEndpointCall()
  {
    Console.WriteLine("Intentando ejecutar el endpoint...");

    var client = new HttpClient();
    var response = await client.GetAsync("http://localhost:3000/ssp");

    Console.WriteLine("Respuesta del endpoint: " + response.StatusCode);

    var statusCode = (int)response.StatusCode;
    Console.WriteLine("Código de respuesta: " + statusCode);

    if (response.IsSuccessStatusCode)
    {
      Console.WriteLine("Endpoint ejecutado con éxito.");
      return;
    }

    throw new HttpRequestException("Simulación de error en el endpoint");
  }

  static async Task ActualizarBaseDeDatosPorFallback()
  {
    Console.WriteLine("Actualizando la base de datos por fallback...");
    await Task.Delay(2000);
    Console.WriteLine("Base de datos actualizada.");
  }
}
