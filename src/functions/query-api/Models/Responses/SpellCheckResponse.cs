namespace QueryApi.Models.Responses;

public readonly record struct SpellCheckResponse(string SearchTerm, string[] CorrectionTerms);