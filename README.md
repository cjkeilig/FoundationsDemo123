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
  
 ### Sending the file 
 Now we need to create a small web app with a File Input button so we can transmit the encrypted message.
 1. Open Visual Studio 
 2. Create new Project -> ASP.NET Web Application (.NET Framework)
 3. Name the project (I am using .NET Framework 4.5.2)
 4. Choose WebApi and Ok
 5. Open Views\Share\_Layout.cshtml and delete the body content, not the @Scripts
 6. Past this into the body
 `
 
 7. Create the file 'UploadController' in the Controllers folder
 


Credits to: 

https://www.sslshopper.com/article-most-common-openssl-commands.html
https://gist.github.com/crazybyte/4142975
