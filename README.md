# AuthGold is an Application for Requests Tracing.

Hello guys, I am happy to share a project with you in order to show you what I am currently studying.

## Objective.

This project aims to create a Logger for client-server requests in .NET Core APIs.

## What can I do?

I can determine the time elapsed in the request, which HTTP method was returned by the server,
which Status Code is returned, address path, and creation date and other things.

## Safety.

As for security, the application encrypts all logs with AES-128 bit encryption,
in the future I intend to make changes to AES-512 bits. Only high-level administrators
with password you can access readable information from the log files. When logging, we save
in a database and we save the encrypted data in a binary file. The admin user with a password
you can decrypt the information in the log file to perform an audit.

1. Elapsed time
2. Http Method
3. Address
4. CreatedAt
5. UpdatedAt
6. ClientCode

## Future.

The intention is to generate a Nuget package for the community as a logging option for APIs.

## Setting up

### We're here configure the filenames and filepath for encryption/decryption files

Start by setting the variables inside appsettings.json, see the example below, in it we're going to configure a 16-byte encryption key, choose a 16-byte/character key of your choice:

```json
{
    "SecuritySession": {
    "SecretKey": "50GT0FC2E5D6RNG7",
    "KeyForBookResource":  "fdsgçoaiglkajdfhd"
  }
}
```

After that, we'll configure the pathnames and files for encryption and decryption. See the example below (extension is optional, choose in honor of my name: Yuri <3):
```json
{
    "FileConfigSession": {
    "FilePathForEncrypt": "C:\\Users\\YuriMelo\\Documents",
    "FilePathForDecrypt": "C:\\Users\\YuriMelo\\Documents",
    "FileNameForEncrypt": "RequestTrace.yur",
    "FileNameForDecrypt": "DecryptedRequestTrace.yur"
  }
}
```