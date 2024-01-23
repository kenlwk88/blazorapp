namespace HLA.CSSMS.Server.Data.IMS
{
    public class SubmissionRepo
    {
        private readonly ApplicationDbContext _dbContext;
        public SubmissionRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<IMSSubmissionsDto>> GetSubmissionCases(IMSSubmissionFilter filter)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@fromDate", filter.FromDate, dbType: DbType.DateTime);
                parameters.Add("@toDate", filter.ToDate, dbType: DbType.DateTime);
                if(!string.IsNullOrEmpty(filter.RefNo))
                    parameters.Add("@refNo", filter.RefNo, dbType: DbType.String);
                if (!string.IsNullOrEmpty(filter.Status) && filter.Status != "0") 
                {
                    if (filter.Status == "2") // Completed
                    {
                        parameters.Add("@status", "Completed", dbType: DbType.String);
                    }
                    else // In Progress
                    {
                        parameters.Add("@status", "In Progress", dbType: DbType.String);
                    }
                }
                var result = await _dbContext.QueryStoredProcedureAsync<IMSSubmissionsDto>("GetSubmissionCases", parameters);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
