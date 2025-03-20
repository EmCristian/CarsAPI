namespace CarsAPI.Sorting
{
    public enum SortField
    {
        Price,
        Year,
        Mileage
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public static class SortOptionExtensions
    {
        public static string ToQueryString(this SortField field, SortDirection direction)
        {
            return $"{field.ToString().ToLower()}_{(direction == SortDirection.Ascending ? "asc" : "desc")}";
        }

        public static (SortField? Field, SortDirection Direction) FromQueryString(string sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
                return (null, SortDirection.Ascending);

            var parts = sortBy.Split('_');
            if (parts.Length != 2)
                return (null, SortDirection.Ascending);

            var fieldString = parts[0];
            var directionString = parts[1];

            if (!Enum.TryParse<SortField>(fieldString, true, out var field))
                return (null, SortDirection.Ascending);

            var direction = directionString.ToLower() == "asc" ?
                SortDirection.Ascending : SortDirection.Descending;

            return (field, direction);
        }
    }
}
