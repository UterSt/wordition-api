using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Wordition.API.OpenApi;

internal class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authenticatedSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticatedSchemes.Any(anySchemes => anySchemes.Name == "Bearer"))
        {
            var bearerSchemes = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            };
            
            document.Components ??= new OpenApiComponents();
            document.AddComponent("Bearer",  bearerSchemes);
        }
    }
}