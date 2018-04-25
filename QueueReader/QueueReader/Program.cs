using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QueueReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type next to decrypt next message");
            while (Console.ReadLine() == "Next")
            {
                // Parse the connection string and return a reference to the storage account.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ""); // Replace this string with your Storage Account key
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueClient.GetQueueReference("demoqueue123");
                CloudQueueMessage retrievedMessage = queue.GetMessageAsync().GetAwaiter().GetResult();
                Console.WriteLine(DecryptText(retrievedMessage.AsBytes));
            }

        }

        public static string DecryptText(byte[] content)
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));

            var key = kv.GetKeyAsync("VaultURL", "NameOfKey", "VersionOfKey").GetAwaiter().GetResult(); // replace these!
            //vaultBaseUrl, string keyName, string keyVersion, string algorithm, byte[] value, CancellationToken cancellationToken = default(CancellationToken));
            var decryptedData = kv.DecryptAsync("VaultURL", "NameOfKey", "VersionOfKey", JsonWebKeyEncryptionAlgorithm.RSA15, content).GetAwaiter().GetResult();
            var decryptedText = Encoding.ASCII.GetString(decryptedData.Result);

            return decryptedText;

        }

        public static async Task<string> GetToken(string authorization, string resource, string scope)
        {
                string clientId = ""; // app id goes here, azure ad
                string clientSecret = ""; // azure ad secret key
                var authContext = new AuthenticationContext(authorization);
                ClientCredential clientCred = new ClientCredential(clientId, clientSecret);
                AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}
