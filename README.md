# MCLiveStatus - Purity Vanilla

Observe and receive notifications about Purity Vanilla's player count.

# Table of Contents

- [Description](#description)
- [Downloads](#downloads)
- [Installation](#installation)
- [Features](#features)
- [Local Development](#local-development)

## Description

MCLiveStatus - Purity Vanilla is a desktop application for observing the player count, server capacity, and queue capacity of Purity Vanilla. If you answer yes to any of the following questions, MCLiveStatus - Purity Vanilla is the perfect application for you!

- Do you want to quickly check Purity Vanilla's status without opening Minecraft or leaving the server you are playing?
- Do you want to receive desktop notifications when the Purity Vanilla queue opens up so that you can quickly join the server without waiting?

## Downloads

### [Windows Latest](https://github.com/PineconeLP/mc-live-status-purity-vanilla/releases/download/bpv-win%2Fv1.3.0/MCLiveStatus.PurityVanilla.Setup.1.3.0.exe) | [Mac/OSX Latest](https://github.com/PineconeLP/mc-live-status-purity-vanilla/releases/download/bpv-osx%2Fv1.3.0/MCLiveStatus.PurityVanilla-1.3.0.dmg)

## Installation

1. Download the MCLiveStatus - Purity Vanilla installer for your respective OS from the [downloads](#Downloads) section

2. Run the installer

- On Windows, you may need to accept security warnings

3. Run MCLiveStatus - Purity Vanilla. That's it!

- On Mac/OSX, you may need to [configure security preferences](https://www.youtube.com/watch?v=X_VbIRSz8Fg)

Accept any of the above security warnings at your own risk. If you are unsure about the application's security, feel free to check out the source code in this repository.

## Features

- View the total player count of Purity Vanilla
- View the queue status of Purity Vanilla
- Subscribe to notifications total server capacity status
- Subscribe to notifications on server queue status
- Configure server status ping interval
- Lots more on the way!

## Local Development

### Requirements

1. [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)

2. [.NET EF Core Tools](https://dotnet.microsoft.com/download/dotnet/5.0)

3. [Docker](https://www.docker.com/products/docker-desktop)

4. [Azure Functions Core Tools V3](https://dotnet.microsoft.com/download/dotnet/5.0)

5. An Azure SignalR Connection String

6. [A Firebase Service Credential](https://firebase.google.com/docs/admin/setup#initialize-sdk)

### Microservices

Running the MC Live Status supporting microservices locally is made easy via Docker. To run all microservices locally:

1. Add an AzureSignalRConnectionString to 'pinger-functions-secrets.env'. Replace 'VALUE' with your connection string

```
cd environments
echo 'AzureSignalRConnectionString=VALUE' > pinger-functions-secrets.env
```

3. Create a [Firebase Service Credential](https://firebase.google.com/docs/admin/setup#initialize-sdk)

4. Copy the Firebase Credential JSON to 'settings-functions/firebase-credential.json'

```
cd settings-functions
cp PATH_TO_FIREBASE_CREDNETIAL_JSON firebase-credential.json
```

4. Start the docker-compose authentication database service

```
cd environments
docker-compose up authdb
```

5. With the authentication database still running, run database migrations

```
cd authentication-functions
dotnet ef database update
```

6. Stop authentication database service and start ALL docker-compose services

```
cd environments
docker-compose down
docker-compose up
```

7. (Optional) Run a few docker-compose services. Replace SERVICE1 and SERVICE2 with docker-compose service names

```
cd environments
docker-compose up SERVICE1 SERVICE2 ...
```

Example (start authentication services):

```
cd environments
docker-compose up authdb azurite authfunctions
```

### Web

To run the Blazor WASM web application:

1. Go to WASM directory

```
cd blazor-purity-vanilla-wasm
```

2. Start the application

```
dotnet run
```
