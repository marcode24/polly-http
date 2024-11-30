// public async ValueTask<ResponseGeneric<ITipoResponseWebServiceApi>> PostDataAsync<ITipoResponseWebServiceApi>(IWebServices webServices)
// {
//   List<string> pasos = new();

//   ResponseGeneric<ITipoResponseWebServiceApi> resultado = null;
//   ITipoResponseWebServiceApi value;
//   _cliente = ProxyFactory.CreateClient(ClienteWebService(webServices.TipoWebService));
//   _cliente.DefaultRequestHeaders.UserAgent.ParseAdd(".NET Core/6.0");
//   _cliente.DefaultRequestHeaders.Add("Accept", "application/json");

//   pasos.Add($"Paso 1 inicio el cliente {ClienteWebService(webServices.TipoWebService)}");

//   if (webServices.EsAutenticado)
//     _cliente.DefaultRequestHeaders.Authorization = webServices.AutenticacionToken;

//   pasos.Add($"Paso 2 se autentico {webServices.AutenticacionToken}");

//   // Crear política de reintentos y fallback
//   var retryPolicy = CrearAsyncPolicy();
//   var fallbackPolicy = Policy
//       .Handle<Exception>()
//       .FallbackAsync(
//           fallbackAction: async (cancellationToken) =>
//           {
//             pasos.Add("Fallback ejecutado: actualizando base de datos.");
//             await ActualizarBDPorFallback(webServices); // Función a implementar
//             return new ResponseGeneric<ITipoResponseWebServiceApi>(
//                   $"El servicio falló después de varios intentos. Fallback ejecutado correctamente.");
//           },
//           onFallbackAsync: async (exception, context) =>
//           {
//             pasos.Add($"Fallback activado debido a: {exception.Exception.Message}");
//             ILogErrror.LogError("Fallback ejecutado en PostDataAsync.", exception.Exception);
//             await Task.CompletedTask;
//           });

//   var policyWrap = Policy.WrapAsync(fallbackPolicy, retryPolicy);

//   try
//   {
//     return await policyWrap.ExecuteAsync(async () =>
//     {
//       pasos.Add($"Paso 3 url servicio {webServices.UrlServicio}");

//       using var response = await _cliente.PostAsync(webServices.UrlServicio, webServices.Datos);
//       using HttpContent content = response.Content;
//       var contenResponse = await content.ReadAsStringAsync();

//       pasos.Add($"Paso 4 respuestaIsSuccess {response.IsSuccessStatusCode}");

//       pasos.Add($"Paso 5 StatusCode {(int)response.StatusCode}");

//       if (response.IsSuccessStatusCode)
//       {
//         var responseCopy = JsonConvert.DeserializeObject<SspReponse>(contenResponse);
//         if (!string.IsNullOrEmpty(responseCopy.contenido_pdf))
//         {
//           responseCopy.contenido_pdf = null;
//         }
//         string jsonResponse = JsonConvert.SerializeObject(responseCopy, Formatting.None, new JsonSerializerSettings
//         {
//           NullValueHandling = NullValueHandling.Ignore
//         });
//         value = JsonConvert.DeserializeObject<ITipoResponseWebServiceApi>(contenResponse);
//         resultado = new ResponseGeneric<ITipoResponseWebServiceApi>(value);
//         pasos.Add($"Paso 6 respuesta {jsonResponse}");
//       }
//       else
//       {
//         resultado = new ResponseGeneric<ITipoResponseWebServiceApi>($"Codigo : {response.StatusCode} Mensaje ${contenResponse}");
//         ILogErrror.LogAPI($"PostDataAsync {ClienteWebService(webServices.TipoWebService)}", await webServices.Datos.ReadAsStringAsync(), pasos.ToArray());
//       }

//       ILogErrror.LogAPI($"PostDataAsync {ClienteWebService(webServices.TipoWebService)}", await webServices.Datos.ReadAsStringAsync(), pasos.ToArray());
//       return resultado;
//     });
//   }
//   catch (HttpRequestException ex)
//   {
//     _cliente.Dispose();
//     ILogErrror.LogAPI($"PostDataAsync {ClienteWebService(webServices.TipoWebService)}", await webServices.Datos.ReadAsStringAsync(), pasos.ToArray());
//     ILogErrror.LogError($"Error al ejecutar el web service PostDataAsync {ClienteWebService(webServices.TipoWebService)}", ex);
//     throw new ArgumentException($"Ocurrio un error en el web service PostDataAsync {ClienteWebService(webServices.TipoWebService)}");
//   }
//   catch (Exception ex)
//   {
//     _cliente.Dispose();
//     ILogErrror.LogAPI($"PostDataAsync {ClienteWebService(webServices.TipoWebService)}", webServices.Datos.ToString(), pasos.ToArray());
//     ILogErrror.LogError($"Error crítico en la clase {nameof(ProxyWebAPI)}", ex);
//     throw new ArgumentException($"Ocurrio un error en el proceso del web services PostDataAsync {ClienteWebService(webServices.TipoWebService)}");
//   }
// }

// // Declaración de la función de fallback (implementar después)
// private async Task ActualizarBDPorFallback(IWebServices webServices)
// {
//   // Implementa la lógica para actualizar la base de datos.
//   await Task.CompletedTask;
// }
