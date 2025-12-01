using CyberArk.AAM.NetStandardPasswordSDK;
using CyberArk.AAM.NetStandardPasswordSDK.Exceptions;
using Microsoft.Extensions.Configuration;

namespace ProductApp.Security
{
    public static class CyberArkPasswordProvider
    {
        public static string GetDatabasePassword(IConfiguration configuration)
        {
            PSDKPassword? password = null;

            try
            {
                var appId   = configuration["CyberArk:AppId"];
                var safe    = configuration["CyberArk:Safe"];
                var folder  = configuration["CyberArk:Folder"] ?? "Root";
                var @object = configuration["CyberArk:Object"];
                var reason  = configuration["CyberArk:Reason"] ?? "DB password retrieval";

                if (string.IsNullOrWhiteSpace(appId) ||
                    string.IsNullOrWhiteSpace(safe) ||
                    string.IsNullOrWhiteSpace(@object))
                {
                    throw new InvalidOperationException(
                        "Configuração CyberArk incompleta. Verifique AppId, Safe e Object em appsettings.json.");
                }

                var request = new PSDKPasswordRequest();

                // Atributos usados pelo Credential Provider / Password SDK
                request.SetAttribute("AppDescs.AppID", appId);
                request.SetAttribute("Reason", reason);
                request.SetAttribute("Query", $"Safe={safe};Folder={folder};Object={@object}");

                password = PasswordSDK.GetPassword(request);

                // Conteúdo da senha vem como char[]
                var secretChars = password.Content;
                if (secretChars == null || secretChars.Length == 0)
                {
                    throw new InvalidOperationException("CyberArk retornou senha vazia.");
                }

                // Converte char[] em string
                var secret = new string(secretChars);

                // (Opcional, mas bom: limpar o array depois de usar)
                for (int i = 0; i < secretChars.Length; i++)
                {
                    secretChars[i] = '\0';
                }

                return secret;
            }
            catch (PSDKException ex)
            {
                // Logue o Reason, não a senha
                throw new ApplicationException($"Erro ao recuperar senha do CyberArk: {ex.Reason}", ex);
            }
            // Não chama Dispose() porque PSDKPassword não implementa IDisposable
        }
    }
}

