using LiteDB;

namespace Backend.Models
{
    public static partial class DataContext
    {
        private static LiteDatabase _db;
        public static LiteDatabase db
            { get  {
                if (_db == null)
                    _db = new LiteDatabase(@"Database.db");
                return _db;
                }
            }
    }
}