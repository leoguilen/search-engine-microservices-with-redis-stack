namespace QueryApi.Models.Filters;

public readonly record struct PeriodRangeFilter(DateOnly? StartDate, DateOnly? EndDate);
