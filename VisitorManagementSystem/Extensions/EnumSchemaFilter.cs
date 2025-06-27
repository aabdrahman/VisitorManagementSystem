using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VisitorManagementSystem.Extensions;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if(context.Type.IsEnum)
        {
            schema.Enum = context.Type
                                .GetEnumNames()
                                .Select(x => (IOpenApiAny)new Microsoft.OpenApi.Any.OpenApiString(x))
                                .ToList();
            schema.Type = "string";
        }
    }
}
