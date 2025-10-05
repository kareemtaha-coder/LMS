using LMS.Application.Features.Curriculums.GetAllCurriculums;
using LMS.Application.Features.Curriculums.GetCurriculum;
using LMS.Domain.Chapters;
using LMS.Domain.Curriculums;
using LMS.Domain.Lessons;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Common.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Curriculum, CurriculumSummaryResponse>()
              .Map(dest => dest.Title, src => src.Title.Value);

            config.NewConfig<Curriculum, CurriculumResponse>()
             .Map(dest => dest.Title, src => src.Title.Value)
             .Map(dest => dest.Introduction, src => src.Introduction.Value);

            config.NewConfig<Chapter, ChapterResponse>()
                // خذ القيمة من SortOrder.Value وضعها في SortOrder
                .Map(dest => dest.SortOrder, src => src.SortOrder.Value);

            config.NewConfig<Lesson, LessonResponse>()
                .Map(dest => dest.SortOrder, src => src.SortOrder.Value);

            // التحويلات البسيطة مثل Curriculum -> CurriculumResponse
            // لا تحتاج لتعريف، فـ Mapster ذكي كفاية ليفهمها تلقائيًا.
        }
    }
}