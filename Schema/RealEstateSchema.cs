using GraphQL;
using graphql_with_external_services.Mutations;
using graphql_with_external_services.Queries;

namespace graphql_with_external_services.Schema
{
    public class RealEstateSchema : GraphQL.Types.Schema
    {
        public RealEstateSchema(IDependencyResolver resolver) :base(resolver)
        {
            Query = resolver.Resolve<MicroregiaoQuery>();
            Mutation = resolver.Resolve<PropertyMutation>();
        }
    }
}
