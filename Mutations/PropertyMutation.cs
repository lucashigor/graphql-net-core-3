using Dominio;
using ExternalServices;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Types;

namespace graphql_with_external_services.Mutations
{
    public class PropertyMutation : ObjectGraphType
    {
        public PropertyMutation(IMicroregiaoExternalService service)
        {
            Field<MicroregiaoType>(
                "addMicroregia",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<MicroregiaoInputType>> { Name = "microregiao" }),
                resolve: context =>
                {
                    var entity = context.GetArgument<Microregiao>("microregiao");
                    return service.AddAsync(entity);
                });
        }
    }
}
