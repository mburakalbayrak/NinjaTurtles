using AutoMapper;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Core.Helpers.MailServices;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Dtos;


namespace NinjaTurtles.Business.Concrete
{
    public class SupportTaskManager : ISupportTaskService
    {
        private ISupportTaskDal _supportTaskDal;
        private IMapper _mapper;

        public SupportTaskManager(ISupportTaskDal supportTaskDal, IMapper mapper)
        {
            _supportTaskDal = supportTaskDal;
            _mapper = mapper;
        }

        public async Task<IResult> SendReport(SendReportDto dto)
        {
            var entity = _mapper.Map<Entities.Concrete.SupportTask>(dto);
            _supportTaskDal.Add(entity);

            var tpl = Messages.SendReportMailTemplate
               .Replace("{{NameSurName}}", $"{dto.NameSurName}")
               .Replace("{{Subject}}", dto.Subject)
               .Replace("{{Message}}", dto.Message)
               .Replace("{{Error}}", dto.Error)
               .Replace("{{Guid}}", dto.GuidId.ToString())
               .Replace("{{year}}", DateTime.Now.Year.ToString());

            MailWorker mail = new MailWorker();
            mail.Init(StaticVars.SupportMailUserName, StaticVars.SupportMailPassword);

            var mailBody = tpl; 
            var result = await mail.SendMailAsync(
                 StaticVars.SupportMailUserName,
                dto.Email,
                "Karekodla Destek",
                mailBody,
                $"{dto.Subject}",
                true);

            return new SuccessResult(Messages.SendReportSuccess);
        }
            
    }
}
