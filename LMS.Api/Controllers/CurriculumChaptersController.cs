using LMS.Application.Features.Curriculums.AddChapter;
using LMS.Domain.Curriculums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [ApiController]
    // نضع الجزء المشترك من العنوان على مستوى الكلاس لتوحيد كل الـ actions القادمة
    [Route("api/curriculums/{curriculumId:guid}/chapters")]
    public class CurriculumChaptersController : ControllerBase
    {
        private readonly ISender _sender;

        public CurriculumChaptersController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Creates a new chapter within a specified curriculum.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddChapter(
            [FromRoute] Guid curriculumId,
            [FromBody] AddChapterRequest request,
            CancellationToken cancellationToken)
        {
            // 1. إنشاء الأمر من البيانات القادمة من الطلب
            var command = new AddChapterCommand(
                curriculumId,
                request.Title,
                request.SortOrder);

            // 2. إرسال الأمر إلى MediatR للمعالجة
            var result = await _sender.Send(command, cancellationToken);

            // 3. التعامل مع نتيجة العملية
            if (result.IsFailure)
            {
                // إذا كان الخطأ هو أن المنهج غير موجود، نرجع 404
                if (result.Error == CurriculumErrors.NotFound)
                {
                    return NotFound(result.Error);
                }
                // لأي خطأ آخر (مثل عنوان مكرر أو خطأ تحقق)، نرجع 400
                return BadRequest(result.Error);
            }

            // في حالة النجاح، نرجع 204 No Content، وهو مناسب للأوامر التي لا تعيد بيانات
            return NoContent();
        }
    }
}
