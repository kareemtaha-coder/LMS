using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    public sealed class ExamplesGridContent : LessonContent
    {
        private readonly List<ExampleItem> _exampleItems = new();
        public IReadOnlyCollection<ExampleItem> ExampleItems => _exampleItems.AsReadOnly();

        private ExamplesGridContent(Guid id, Guid lessonId, SortOrder sortOrder)
            : base(id, lessonId, sortOrder) { }

        private ExamplesGridContent() { } // For EF Core

        internal static Result<ExamplesGridContent> Create(Guid lessonId, SortOrder sortOrder)
        {
            var content = new ExamplesGridContent(System.Guid.NewGuid(), lessonId, sortOrder);
            return content;
        }

        /// <summary>
        /// This is the method that needs to be correct.
        /// </summary>
        internal Result AddExampleItem(string imageUrl, string? audioUrl)
        {
            if (_exampleItems.Count >= 6)
            {
                return Result.Failure(LessonErrors.MaxExamplesReached);
            }

            // 1. Capture the entire 'Result<ExampleItem>' object.
            var exampleItemResult = ExampleItem.Create(this.Id, imageUrl, audioUrl);

            // 2. Check the result for failure.
            if (exampleItemResult.IsFailure)
            {
                return exampleItemResult; // Return the failure and stop.
            }

            // 3. Only if successful, access .Value and add it to the list.
            _exampleItems.Add(exampleItemResult.Value);

            return Result.Success();
        }
        internal Result<ExampleItem> RemoveExampleItem(Guid itemId)
        {
            var itemToRemove = _exampleItems.FirstOrDefault(i => i.Id == itemId);
            if (itemToRemove is null)
            {
                return Result.Failure<ExampleItem>(new Error("ExampleItem.NotFound", "The specified item was not found in this grid."));
            }

            _exampleItems.Remove(itemToRemove);
            return itemToRemove; // Return the removed item
        }
    }
}