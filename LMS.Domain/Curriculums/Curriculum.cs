using LMS.Domain.Abstractions;
using LMS.Domain.Chapters;
using LMS.Domain.Lessons; // نحتاج للوصول لأنواع المحتوى
using LMS.Domain.Shared; // مكان الـ Result و Error
using LMS.Domain.Shared.ValueObjects;

namespace LMS.Domain.Curriculums
{
    public sealed class Curriculum :Entity 
    {
        private readonly List<Chapter> _chapters = new();
        public IReadOnlyCollection<Chapter> Chapters => _chapters.AsReadOnly();
        public Title Title { get; private set; }
        public Introduction Introduction { get; private set; }

        private Curriculum(Guid id, Introduction introduction, Title title) : base(id)
        {
            Title = title;
            Introduction = introduction;
        }

        private Curriculum() { } // For EF Core

        public static Result<Curriculum> Create(string titleValue, string introductionValue)
        {
            var titleResult = Title.Create(titleValue);
            if (titleResult.IsFailure) return Result.Failure<Curriculum>(titleResult.Error);

            var introductionResult = Introduction.Create(introductionValue);
            if (introductionResult.IsFailure) return Result.Failure<Curriculum>(introductionResult.Error);

            var curriculum = new Curriculum(Guid.NewGuid(), introductionResult.Value, titleResult.Value);

         
            return curriculum;
        }

        public Result Update(Title newTitle, Introduction newIntroduction)
        {
            // The validation for Title and Introduction is handled by their respective
            // 'Create' factory methods before this method is ever called.
            Title = newTitle;
            Introduction = newIntroduction;

            return Result.Success();
        }

        // ===================================================================================
        // <<< تحسين رقم 1: كل العمليات الآن تتم من خلال الـ Aggregate Root >>>
        // <<< تحسين رقم 2: كل العمليات الآن تعيد Result بدلاً من رمي Exception >>>
        // ===================================================================================

        public Result AddChapter(Title title, SortOrder sortOrder)
        {
            if (_chapters.Any(c => c.Title.Value == title.Value))
            {
                return Result.Failure(CurriculumErrors.DuplicateChapterTitle);
            }

            // 1. The 'Create' method returns a Result<Chapter>
            var chapterResult = Chapter.Create(title, this.Id, sortOrder);

            // 2. We MUST check for failure before trying to access the value.
            if (chapterResult.IsFailure)
            {
                // If it failed, we pass the failure result up the chain and stop.
                return chapterResult;
            }

            // 3. ONLY if we are in a success state, we can safely access '.Value'.
            // chapterResult.Value is the actual 'Chapter' object.
            _chapters.Add(chapterResult.Value);

            return Result.Success();
        }

        public Result AddLessonToChapter(Guid chapterId, Title lessonTitle, SortOrder lessonSortOrder)
        {
            var chapter = _chapters.FirstOrDefault(c => c.Id == chapterId);
            if (chapter is null)
            {
                return Result.Failure(CurriculumErrors.ChapterNotFound);
            }

            // <<< تحسين رقم 3: الـ Aggregate Root هو المسؤول عن تطبيق القواعد >>>
            // تم نقل منطق التحقق من تكرار الدرس من كلاس Chapter إلى هنا
            if (chapter.Lessons.Any(l => l.Title.Value == lessonTitle.Value))
            {
                return Result.Failure(CurriculumErrors.DuplicateLessonTitle);
            }

            // الآن، الفصل Chapter هو الذي يضيف الدرس، لكن بأمر من الـ Curriculum
            // لاحظ أن دالة AddLesson في Chapter يجب أن تكون internal
            return chapter.AddLesson(lessonTitle, lessonSortOrder);
        }

        public Result AddRichTextContentToLesson(Guid lessonId, SortOrder contentSortOrder, string? arabicText, string? englishText, NoteType noteType)
        {
            // Find the lesson anywhere within this curriculum.
            var lesson = FindLesson(lessonId);
            if (lesson is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            // Delegate the final action to the Lesson entity.
            return lesson.AddRichTextContent(contentSortOrder, arabicText, englishText, noteType);
        }

        // You can refactor other "AddToLesson" methods to use this helper too.
        private Lesson? FindLesson(Guid lessonId)
        {
            return _chapters
                .SelectMany(c => c.Lessons)
                .FirstOrDefault(l => l.Id == lessonId);
        }

        public Result AddVideoContentToLesson(Guid lessonId, SortOrder contentSortOrder, string videoUrl)
        {
            // Use the existing helper to find the lesson
            var lesson = FindLesson(lessonId);
            if (lesson is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            // Delegate the final action to the Lesson entity
            return lesson.AddVideoContent(contentSortOrder, videoUrl);
        }
        public Result AddImageWithCaptionContentToLesson(Guid lessonId, SortOrder contentSortOrder, string imageUrl, string? caption)
        {
            var lesson = FindLesson(lessonId);
            if (lesson is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            return lesson.AddImageWithCaptionContent(contentSortOrder, imageUrl, caption);
        }

         public Result AddExamplesGridToLesson(Guid lessonId, SortOrder contentSortOrder)
    {
        var lesson = FindLesson(lessonId);
        if (lesson is null)
        {
            return Result.Failure(CurriculumErrors.LessonNotFound);
        }

        return lesson.AddExamplesGridContent(contentSortOrder);
    }

        public Result AddItemToExamplesGridInLesson(Guid contentId, string imageUrl, string? audioUrl)
        {
            var lesson = _chapters
                .SelectMany(c => c.Lessons)
                .FirstOrDefault(l => l.Contents.Any(co => co.Id == contentId));

            if (lesson is null)
            {
                return Result.Failure(LessonErrors.ContentNotFound);
            }

            return lesson.AddItemToExamplesGrid(contentId, imageUrl, audioUrl);
        }

        public Result ReorderLessonContents(Guid lessonId, List<Guid> orderedContentIds)
        {
            var lesson = FindLesson(lessonId);
            if (lesson is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            return lesson.ReorderContents(orderedContentIds);
        }

        public Result UpdateRichTextContent(Guid contentId, string? newArabicText, string? newEnglishText, NoteType noteType)
        {
            var content = _chapters
                .SelectMany(c => c.Lessons)
                .SelectMany(l => l.Contents)
                .FirstOrDefault(co => co.Id == contentId);

            if (content is null)
            {
                return Result.Failure(LessonErrors.ContentNotFound);
            }

            if (content is not RichTextContent richTextContent)
            {
                return Result.Failure(LessonErrors.InvalidContentType);
            }

            // Pass the new noteType parameter to the Update method
            return richTextContent.Update(newArabicText, newEnglishText, noteType);
        }

        public Result UpdateVideoContent(Guid contentId, string newVideoUrl)
        {
            var content = _chapters
                .SelectMany(c => c.Lessons)
                .SelectMany(l => l.Contents)
                .FirstOrDefault(co => co.Id == contentId);

            if (content is null)
            {
                return Result.Failure(LessonErrors.ContentNotFound);
            }

            if (content is not VideoContent videoContent)
            {
                return Result.Failure(LessonErrors.InvalidContentType);
            }

            return videoContent.Update(newVideoUrl);
        }

        public Result UpdateImageWithCaptionContent(Guid contentId, string newImageUrl, string? newCaption)
        {
            var content = _chapters
                .SelectMany(c => c.Lessons)
                .SelectMany(l => l.Contents)
                .FirstOrDefault(co => co.Id == contentId);

            if (content is null)
            {
                return Result.Failure(LessonErrors.ContentNotFound);
            }

            if (content is not ImageWithCaptionContent imageContent)
            {
                return Result.Failure(LessonErrors.InvalidContentType);
            }

            return imageContent.Update(newImageUrl, newCaption);
        }
        public Result<ExampleItem> DeleteExampleItem(Guid itemId)
        {
            var lesson = _chapters
                .SelectMany(c => c.Lessons)
                .FirstOrDefault(l => l.Contents.OfType<ExamplesGridContent>().Any(g => g.ExampleItems.Any(i => i.Id == itemId)));

            if (lesson is null)
            {
                return Result.Failure<ExampleItem>(new Error("ExampleItem.ParentLessonNotFound", "The parent lesson for the specified item was not found."));
            }

            return lesson.DeleteExampleItem(itemId);
        }

        public Result UpdateChapterTitle(Guid chapterId, Title newTitle)
        {
            var chapter = _chapters.FirstOrDefault(c => c.Id == chapterId);
            if (chapter is null)
            {
                return Result.Failure(CurriculumErrors.ChapterNotFound);
            }

            // You might want to check if a chapter with the new title already exists
            if (_chapters.Any(c => c.Title.Value == newTitle.Value && c.Id != chapterId))
            {
                return Result.Failure(CurriculumErrors.DuplicateChapterTitle);
            }

            return chapter.UpdateTitle(newTitle);
        }

        public Result<IReadOnlyList<LessonContent>> DeleteLesson(Guid lessonId)
        {
            var chapter = _chapters.FirstOrDefault(c => c.Lessons.Any(l => l.Id == lessonId));
            if (chapter is null)
            {
                return Result.Failure<IReadOnlyList<LessonContent>>(CurriculumErrors.LessonNotFound);
            }

            return chapter.RemoveLesson(lessonId);
        }
        public Result<IReadOnlyList<LessonContent>> DeleteChapter(Guid chapterId)
        {
            var chapterToRemove = _chapters.FirstOrDefault(c => c.Id == chapterId);
            if (chapterToRemove is null)
            {
                return Result.Failure<IReadOnlyList<LessonContent>>(CurriculumErrors.ChapterNotFound);
            }

            // Collect all content from all lessons within the chapter to be deleted.
            var contentsToDelete = chapterToRemove.Lessons
                .SelectMany(l => l.Contents)
                .ToList();

            _chapters.Remove(chapterToRemove);

            return contentsToDelete;
        }
        public Result PublishLesson(Guid lessonId)
        {
            var lesson = FindLesson(lessonId);
            if (lesson is null) return Result.Failure(CurriculumErrors.LessonNotFound);
            return lesson.Publish();
        }

        public Result UnpublishLesson(Guid lessonId)
        {
            var lesson = FindLesson(lessonId);
            if (lesson is null) return Result.Failure(CurriculumErrors.LessonNotFound);
            return lesson.Unpublish();
        }
    }

}