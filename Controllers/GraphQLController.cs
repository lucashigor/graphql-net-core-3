using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Utilities;

namespace graphql_with_external_services.Controllers
{
    [Route("[controller]")]
    public class GraphQLController : ControllerBase
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _documentExecuter;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }
        

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GraphQLQuery query)
        {
            if(query == null)
            {
                throw new ArgumentException();
            }

            var inputs = query.Variables?.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            var result = await _documentExecuter
                .ExecuteAsync(executionOptions);

            if(result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }
    }
}
