using LMS.Domain.Abstractions;
using LMS.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    public sealed class ExampleItem : Entity
    {
        public Guid ExamplesGridContentId { get; private set; }
        public string ImageUrl { get; private set; }
        public string? AudioUrl { get; private set; }

        private ExampleItem(Guid id, Guid examplesGridContentId, string imageUrl, string? audioUrl) : base(id)
        {
            ExamplesGridContentId = examplesGridContentId;
            ImageUrl = imageUrl;
            AudioUrl = audioUrl;
        }

        private ExampleItem()  { } // For EF Core

        internal static Result<ExampleItem> Create(Guid parentId, string imageUrl, string? audioUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return Result.Failure<ExampleItem>(LessonErrors.EmptyUrl(nameof(ExampleItem)));
            }

            var item = new ExampleItem(Guid.NewGuid(), parentId, imageUrl, audioUrl);

            // This implicitly returns Result.Success(item)
            return item;
        }
    }

}

