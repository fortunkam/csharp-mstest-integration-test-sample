# C# HTTP Integration Tests example

## Prerequisites
Can be run using `dotnet test`
Assumes the required API Management instance is configured correctly (one api configured to fail if parameter is passed incorrectly, one api configured with OAuthv2 authentication on Azure AD)
Environment variables need to be provided 

## Notes

Uses MSTest
For an Azure DevOps pipeline, use a variable group linked to a keyvault to get the secrets
Set the parameters as pipeline variables and run dotnet test