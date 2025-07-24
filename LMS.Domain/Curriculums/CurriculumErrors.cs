using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Curriculums
{
    public static class CurriculumErrors
    {
        // Corrected Error for duplicate CHAPTERS
        public static readonly Error DuplicateChapterTitle = new(
            "Curriculum.DuplicateChapterTitle",
            "A chapter with this title already exists in the curriculum.");

        // Corrected Error for duplicate LESSONS
        public static readonly Error DuplicateLessonTitle = new(
            "Curriculum.DuplicateLessonTitle",
            "A lesson with this title already exists in the specified chapter.");

        public static readonly Error ChapterNotFound = new(
            "Curriculum.ChapterNotFound",
            "The specified chapter was not found in this curriculum.");

        public static readonly Error LessonNotFound = new(
            "Curriculum.LessonNotFound",
            "The specified lesson was not found in this chapter.");
    }
}
