using System;
using GraphQL.Types;

namespace api
{
	public class EatMoreQuery : ObjectGraphType
	{
		public EatMoreQuery(ApplicationDbContext db)
		{

			Field<ListGraphType<ProdutoType>>(
					"produtos",
					resolve: context =>
					{
						var produtos = db
									.Produtos;
						return produtos;
					});

		}
	}
}
