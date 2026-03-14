# AiDemo

After installing LM Studio and downloading the model, you can run this demo to test the integration of LM Studio and .NET.

## LM Studio
You can download LM Studio from the official website: https://lmstudio.ai/.

Press the Server Settings, and switch the "Require Authentication" to "On".
Press "Manage Tokens", and create a new token. Copy the token, and use the as described in the next section.

## Prerequisites
dotnet user-secrets init
dotnet user-secrets set LmStudio:Token <your-lmstudio-token>

## Update appsettings.json
Update the appsettings.json LmStudio section with your local lmstudio server url and the model you want to use. For example:

## Run the demo
dotnet run