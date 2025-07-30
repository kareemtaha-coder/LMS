using LMS.Domain.Lessons;
using LMS.Domain.Shared.ValueObjects;
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
           LessonStatus Status,
           IReadOnlyList<LessonContentResponse> Contents);



    [JsonDerivedType(typeof(RichTextContentResponse), typeDiscriminator: "RichText")]
    [JsonDerivedType(typeof(VideoContentResponse), typeDiscriminator: "Video")]
    [JsonDerivedType(typeof(ImageWithCaptionContentResponse), typeDiscriminator: "ImageWithCaption")]
    [JsonDerivedType(typeof(ExamplesGridContentResponse), typeDiscriminator: "ExamplesGrid")]
    public abstract record LessonContentResponse(
            Guid Id,
            string Title,
            int SortOrder,
            string ContentType);


    // تحديث كل DTO ليرث العنوان
    public sealed record RichTextContentResponse(
        Guid Id,
        string Title, // <-- تمت الإضافة هنا
        int SortOrder,
        string? ArabicText,
        string? EnglishText,
        NoteType NoteType) : LessonContentResponse(Id, Title, SortOrder, "RichText");

    public sealed record VideoContentResponse(
       Guid Id,
       string Title, // <-- تمت الإضافة هنا
       int SortOrder,
       string VideoUrl) : LessonContentResponse(Id, Title, SortOrder, "Video");

    // DTO for Image with Caption Content
    public sealed record ImageWithCaptionContentResponse(
        Guid Id,
        string Title, // <-- تمت الإضافة هنا
        int SortOrder,
        string ImageUrl,
        string? Caption) : LessonContentResponse(Id, Title, SortOrder, "ImageWithCaption");

    // DTO for Examples Grid Content, which contains its own items
    public sealed record ExamplesGridContentResponse(
         Guid Id,
         string Title, // <-- تمت الإضافة هنا
         int SortOrder,
         IReadOnlyList<ExampleItemResponse> ExampleItems) : LessonContentResponse(Id, Title, SortOrder, "ExamplesGrid");

    // DTO for a single item within the Examples Grid
    public sealed record ExampleItemResponse(
        Guid Id,
        string ImageUrl,
        string? AudioUrl);
}
