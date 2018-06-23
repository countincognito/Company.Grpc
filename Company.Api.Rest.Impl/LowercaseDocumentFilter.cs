using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Company.Api.Rest.Impl
{
    public class LowercaseDocumentFilter
        : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            IDictionary<string, PathItem> currentPaths = swaggerDoc.Paths;

            // Generate the new keys.
            var newPaths = new Dictionary<string, PathItem>();
            var keysToRemove = new HashSet<string>();
            foreach (KeyValuePair<string, PathItem> path in currentPaths)
            {
                string oldKey = path.Key;
                string newKey = oldKey.ToLower();
                if (string.CompareOrdinal(newKey, oldKey) != 0)
                {
                    keysToRemove.Add(oldKey);
                    newPaths.Add(newKey, path.Value);
                }
            }

            // Remove the old keys.
            foreach (string oldKey in keysToRemove)
            {
                swaggerDoc.Paths.Remove(oldKey);
            }

            // Add the new keys.
            foreach (KeyValuePair<string, PathItem> newPath in newPaths)
            {
                swaggerDoc.Paths.Add(newPath.Key, newPath.Value);
            }
        }
    }
}
