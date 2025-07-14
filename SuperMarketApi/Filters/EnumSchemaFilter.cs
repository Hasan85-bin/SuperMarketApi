using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Runtime.Serialization;

namespace SuperMarketApi.Filters
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                
                // Get all enum values and their string representations
                foreach (var enumValue in Enum.GetValues(context.Type))
                {
                    var enumName = Enum.GetName(context.Type, enumValue);
                    schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(enumName));
                }
                
                // Set the type to string to show enum values as strings
                schema.Type = "string";
                schema.Format = null;
            }
        }
    }
} 