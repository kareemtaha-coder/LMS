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

        public Result AddRichTextContentToLesson(Guid chapterId, Guid lessonId, SortOrder contentSortOrder, string? arabicText, string? englishText)
        {
            // البحث عن الفصل ثم الدرس
            var chapter = _chapters.FirstOrDefault(c => c.Id == chapterId);
            if (chapter is null) return Result.Failure(CurriculumErrors.ChapterNotFound);

            var lesson = chapter.Lessons.FirstOrDefault(l => l.Id == lessonId);
            if (lesson is null) return Result.Failure(CurriculumErrors.LessonNotFound);

            // تفويض إضافة المحتوى للدرس
            return lesson.AddRichTextContent(contentSortOrder, arabicText, englishText);
        }

        public Result AddVideoContentToLesson(Guid chapterId, Guid lessonId, SortOrder contentSortOrder, string videoUrl)
        {
            var chapter = _chapters.FirstOrDefault(c => c.Id == chapterId);
            if (chapter is null)
            {
                return Result.Failure(CurriculumErrors.ChapterNotFound);
            }

            var lesson = chapter.Lessons.FirstOrDefault(l => l.Id == lessonId);
            if (lesson is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            // Delegate the final action to the Lesson entity
            return lesson.AddVideoContent(contentSortOrder, videoUrl);
        }

        public Result AddImageWithCaptionContentToLesson(Guid chapterId, Guid lessonId, SortOrder contentSortOrder, string imageUrl, string? caption)
        {
            var chapter = _chapters.FirstOrDefault(c => c.Id == chapterId);
            if (chapter is null)
            {
                return Result.Failure(CurriculumErrors.ChapterNotFound);
            }

            var lesson = chapter.Lessons.FirstOrDefault(l => l.Id == lessonId);
            if (lesson is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            // Delegate the final action to the Lesson entity
            return lesson.AddImageWithCaptionContent(contentSortOrder, imageUrl, caption);
        }

        public Result AddExamplesGridToLesson(Guid chapterId, Guid lessonId, SortOrder contentSortOrder)
        {
            var chapter = _chapters.FirstOrDefault(c => c.Id == chapterId);
            if (chapter is null)
            {
                return Result.Failure(CurriculumErrors.ChapterNotFound);
            }

            var lesson = chapter.Lessons.FirstOrDefault(l => l.Id == lessonId);
            if (lesson is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            // Delegate the final action to the Lesson entity
            return lesson.AddExamplesGridContent(contentSortOrder);
        }

        public Result AddItemToExamplesGridInLesson(Guid chapterId, Guid lessonId, Guid contentId, string imageUrl, string? audioUrl)
        {
            var chapter = _chapters.FirstOrDefault(c => c.Id == chapterId);
            if (chapter is null)
            {
                return Result.Failure(CurriculumErrors.ChapterNotFound);
            }

            var lesson = chapter.Lessons.FirstOrDefault(l => l.Id == lessonId);
            if (lesson is null)
            {
                return Result.Failure(CurriculumErrors.LessonNotFound);
            }

            // Delegate the final, more complex action to the Lesson entity
            return lesson.AddItemToExamplesGrid(contentId, imageUrl, audioUrl);
        }
    }

}