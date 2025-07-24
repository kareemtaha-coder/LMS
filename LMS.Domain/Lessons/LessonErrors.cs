using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    public static class LessonErrors
    {
        public static readonly Error DuplicateSortOrder = new(
            "Lesson.DuplicateSortOrder",
            "Content with this sort order already exists in the lesson.");

        public static readonly Error ContentNotFound = new(
            "Lesson.ContentNotFound",
            "The specified content was not found in this lesson.");

        public static readonly Error InvalidContentType = new(
            "Lesson.InvalidContentType",
            "The specified content is not of the expected type (e.g., not an ExamplesGrid).");

        public static readonly Error MaxExamplesReached = new(
            "ExamplesGrid.MaxExamplesReached",
            "Cannot add more than 6 examples to a single grid.");

        public static  Error EmptyUrl(string content_type) => new Error($"{content_type}.EmptyUrl", $"{content_type} URL cannot be empty.");
        public static  Error InvalidUrl(string content_type) => new Error($"{content_type}.InvalidUrl", $"{content_type} URL is not a valid URL.");

        public static  Error EmptyText = new Error("RichTextContent.Empty", "At least one text (Arabic or English) must be provided.");

    }
}
