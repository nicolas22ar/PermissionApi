using System.Threading.Tasks;
using API.Test.Infrastructure.Concrete.DbContexts;
using API.Test.Infrastructure.Concrete.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Test.Infrastructure.Concrete.Repositories
{
	public abstract class BaseRepository
	{
		protected readonly TestDbContext _context;
		protected const string DuplicatedErrorMsg = "Error while updating the database - Duplicated entity";
		protected const string ForeignConstraintErrorMsg = "Error while updating the database - Constraint validation";

		public BaseRepository(TestDbContext context)
		{
			_context = context;
		}

		public async Task SaveAsync()
		{
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				//2601 - Violation in unique index. 2627 Violation in unique constraint. 547 - Violation in foreign key
				switch (((SqlException)ex.InnerException).Number)
				{
					case 2627:
					case 2601:
						throw new DuplicatedEntityException(DuplicatedErrorMsg);
					case 547:
						throw new EntityNotFoundException(ForeignConstraintErrorMsg);
				}
			}

		}
	}
}
