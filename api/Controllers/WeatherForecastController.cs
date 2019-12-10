using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GraphQLController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public GraphQLController(ApplicationDbContext db)
		{
			_db = db;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody]GraphQLQuery query)
		{
			var inputs = query.Variables.ToInputs();

			var schema = new Schema()
			{
				Query = new EatMoreQuery(_db)
			};

			var result = await new DocumentExecuter().ExecuteAsync(_ =>
			{
				_.Schema = schema;
				_.Query = query.Query;
				_.OperationName = query.OperationName;
				_.Inputs = inputs;
			}).ConfigureAwait(false);

			if (result.Errors?.Count > 0)
			{
				return BadRequest();
			}

			return Ok(result);
		}
	}
}
