using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LMS.Application.Features.Lessons.GetLessonById
{
    // This is the main response object for the lesson itself.
    public sealed record LessonResponse(
        Guid Id,
        string Title,
        int SortOrder,
        IReadOnlyList<LessonContentResponse> Contents);

    
    [JsonDerivedType(typeof(RichTextContentResponse), typeDiscriminator: "RichText")]
    [JsonDerivedType(typeof(VideoContentResponse), typeDiscriminator: "Video")]
    [JsonDerivedType(typeof(ImageWithCaptionContentResponse), typeDiscriminator: "ImageWithCaption")]
    [JsonDerivedType(typeof(ExamplesGridContentResponse), typeDiscriminator: "ExamplesGrid")]
    public abstract record LessonContentResponse(
       Guid Id,
       int SortOrder,
       string ContentType);


    // DTO for Rich Text Content
    public sealed record RichTextContentResponse(
        Guid Id,
        int SortOrder,
        string? ArabicText,
        string? EnglishText) : LessonContentResponse(Id, SortOrder, "RichText");

    // DTO for Video Content
    public sealed record VideoContentResponse(
        Guid Id,
        int SortOrder,
        string VideoUrl) : LessonContentResponse(Id, SortOrder, "Video");

    // DTO for Image with Caption Content
    public sealed record ImageWithCaptionContentResponse(
        Guid Id,
        int SortOrder,
        string ImageUrl,
        string? Caption) : LessonContentResponse(Id, SortOrder, "ImageWithCaption");

    // DTO for Examples Grid Content, which contains its own items
    public sealed record ExamplesGridContentResponse(
        Guid Id,
        int SortOrder,
        IReadOnlyList<ExampleItemResponse> ExampleItems) : LessonContentResponse(Id, SortOrder, "ExamplesGrid");

    // DTO for a single item within the Examples Grid
    public sealed record ExampleItemResponse(
        Guid Id,
        string ImageUrl,
        string? AudioUrl);
}
