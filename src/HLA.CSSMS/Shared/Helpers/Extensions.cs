using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLA.CSSMS.Shared.Helpers
{
    public static class Extensions
    {
        public static T ConvertDataTable<T>(this DataTable dataTable)
        {
            var jsonText = JsonConvert.SerializeObject(dataTable);
            var models = JsonConvert.DeserializeObject<T>(jsonText);
            if (models != null)
                return models;
            return default(T);
        }
    }
}
