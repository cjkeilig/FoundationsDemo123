# FoundationsDemo123

## Goal
Send an encrypted message to a Web API, store it, and decrypt the message using Azure Key Vault
For example say 'Alice', or a customer needs to send us a message securely and our business needs to decrypt the message. 

## **Disclaimer** 
__Not all parts of this application are secure__
Do these steps at your _own risk_. You may find it a good exercise to secure the parts of this demo that aren't so secure.

## Setup Steps
1. Create client-facing web app application to submit encrypted files.
2. Create Azure Storage Account to store the files in a Queue.
3. Create console application to read encrypted files off the queue.
4. Decrypt files with Azure Key Vault.
5. Print secret message to console.

### Encrypted Communication Setup
  Download OpenSSL for Windows.
  https://slproweb.com/products/Win32OpenSSL.html
  
  Create a Self-Signed Certificate
  
  `openssl req -x509 -sha256 -nodes -days 265 -in priv.key -out cert.crt -config "C:\OpenSSL-Win32\bin\openssl.cfg"`
  
  Create a PFX key pair to upload into Azure Key Vault
  
  `openssl pkcs12 -export -out certificate.pfx -inkey privateKey.key -in cert.crt`
  
  Have OpenSSL make us a public key to hand out to Alice
  
  `openssl rsa -in privateKey.key -pubout > DemoPub.key`
  
  Create a text file on Desktop with text 'Secret Message'.
  Encrypt the file using rsautl
  
  `openssl rsautl -encrypt -pubin -inkey DemoPub.key -in "C:\Users\cjk\Desktop\MessageToEncrypt.txt" -out "C:\Users\cjk\Desktop\secretmessage.txt"`
  
 ### Azure Storage
 We need to set up a place to store our encrypted messages that we are sending so we will create an Azure Storage Account
 With your student Microsoft Imagine License you should be able to receive a free trial with 200$ of credit to your Azure account.
 After you have made an account with Microsoft, log-in to https://portal.azure.com/article-most-common-openssl-commands
 
 I added a storage account called demoqueue which holds storage resources such as Blobs, Files, Tbles and Queues. We will be using a queue
 
 ![Screenshot](AzureStorageTypes.PNG)
 
 Click into 'Queue' and click '+ Queue'. 
 We will need to come back from this to get the Queue URL for our application.
 
 ### Sending the file 
 Now we need to create a small web app with a File Input button so we can transmit the encrypted message.
 1. Open Visual Studio 
 2. Create new Project -> ASP.NET Web Application (.NET Framework)
 3. Name the project (I am using .NET Framework 4.5.2)
 4. Choose WebApi and Ok
 
 ![Screenshot](WebApi.PNG)
 
 5. Open Views\Share\_Layout.cshtml and delete the body content, not the @Scripts
 6. Past this into the body
 7. You may need to change the port number on the layout.cshtml file depending on what Visual Studio runs your app on
 
 7. Create the file 'UploadController' in the Controllers folder
 8. Or , clone this repo
 9. You will need to either do a Nuget Restore and/or add the Nuget Packages for 
	WindowsAzure.Storage
	WindowsAzure.ConfigurationManager
 
 Run the application
 Click submit, 
 Select the file from your Desktop that you have encrypted.
 Now it should appear encrypted in the Azure Queue
 
 ![Screenshot](QueueMessage.PNG)
 
 ### Decrypt it from our side
 Create a console application that reads encrypted files off the queue and then decrypts them using Azure Key Vault,
 We will then print the secret text to the screen.
 1. Upload our key to Azure Key Vault. Any thing that needs decrypted will go to this vault, pass the data and the api call will decrypt it for us.
 The key does not move. But, we do have to make sure that our authentication into the key vault is secure.
 There is an option to use a certificate which ensures communications are coming from a certain locatino.
 But in this case, we will use a ClientId and a Secret associated with our application.
 Any application can have access to the Key Vault, so you can give another application access to the Key Vault and just use that application's credentials if you so choose.
 
 In this case, we will publish our 'Upload' Web Api project to Azure, Register it to Azure Active Directory and get the ClientId and Client Secret.
 
 ![Screenshot](PublishedAboutToTestUploadToQueue.PNG)
 
 ![Screenshot](AddAppReg.PNG)
 
 ![Screenshot](ClientSecret.PNG)
 
 Getting the client secret, copy the value to your local machine after save. We will put this in the Console application.
 The client id is here
 But before we forget, let's add our private/public key pair into the vault like so.
 
 ![Screenshot](Keys.PNG)
 
 Remember the pfx file created by OpenSSL before? Upload it to Key Vault
 ![Screenshot](AboutToCreateKey.PNG)
 
 ![Screenshot](giveaccess.PNG)
 
 The pfx will be located where the openssl executable sits. 
 
 In the portal you will need to add a Key Vault, you can do so by clicking 'Create a Resource'
 
 ![Screenshot](KeyVault.PNG)
 
 Create an app to read off the queue
 1. Open Visual Studio
 2. New Project -> Console Application
 To authenticate with Azure Storage, go to your created Queue and click access keys, paste the key into the value for the application in the Web.Config setting for "StorageConnectionString"
 You will also need to paste the hard-coded queue name when you execute the method GetQueueReference(<queue-name>)
 Now create 
 


Credits to: 

https://www.sslshopper.com/article-most-common-openssl-commands.html
https://gist.github.com/crazybyte/4142975