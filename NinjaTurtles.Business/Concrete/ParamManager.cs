using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Core.Utilities.Results;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.Entities.Dtos;


namespace NinjaTurtles.Business.Concrete
{
    public class ParamManager : IParamService
    {
        private IParamItemDal _paramItemDal;

        public ParamManager(IParamItemDal paramItemDal)
        {
            _paramItemDal = paramItemDal;
        }

        public IDataResult<List<SelectDto>> List(int paramId)
        {
            var list = _paramItemDal.GetList(c => ((int)c.ParamId) == paramId).Select(c => new SelectDto() { ID = c.Id, Name = c.Name }).ToList();
            return new SuccessDataResult<List<SelectDto>>(list);
        }
    }
}
