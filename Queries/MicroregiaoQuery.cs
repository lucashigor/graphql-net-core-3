using ExternalServices;
using GraphQL.Types;
using Types;

namespace graphql_with_external_services.Queries
{
    public class MicroregiaoQuery : ObjectGraphType
    {
        public MicroregiaoQuery(IMicroregiaoExternalService service)
        {
            Field<ListGraphType<MicroregiaoType>>(
            "microregiao",
            arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id"}),
            resolve: context => {
                var id = context.GetArgument<int?>("id");

                return id != null ? service.GetById(id.Value)
                : service.GetAllAsync();
                }) ;
        }
        
    }
}
